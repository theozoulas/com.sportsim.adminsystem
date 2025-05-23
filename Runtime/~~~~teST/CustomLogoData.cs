using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MenuComponents.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using static UnityEditor.AssetDatabase;
#endif    

[InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
public class CustomLogoData : ScriptableObject
{
    [ReadOnly] [HideInInlineEditors] public string key;

    [TabGroup("$key/Item", "Logo", SdfIconType.Image)]
    public bool useCustomLogo;

    [TabGroup("$key/Item", "Logo", SdfIconType.Image)] [HideIf("useCustomLogo")]
    public bool useDefaultLogo;
    
    [TabGroup("$key/Item", "Logo")]
    [OnInspectorGUI("DrawDefaultPreview", append: true)]
    [ValueDropdown("GetDefaultSprites")]
    [HideIf("@!this.useDefaultLogo || this.useCustomLogo")]
    [HideLabel]
    [Title("SportSim Logo Settings")]
    public Sprite defaultLogo;

    [TabGroup("$key/Item", "Logo")] [HideIf("@!this.useDefaultLogo || this.useCustomLogo")] [EnumToggleButtons]
    public PreviewBackgrounds previewBackground;

    [TabGroup("$key/Item", "Logo", SdfIconType.Image)]
    [ShowIf("useCustomLogo")]
    [OnInspectorGUI("DrawCustomPreview", append: true)]
    public Sprite customLogo;

    [TabGroup("$key/Item", "Logo")] [ShowIf("useCustomLogo")] [LabelText("Preview Background")] [EnumToggleButtons]
    public PreviewBackgrounds customLogoPreviewBackground;

    [TabGroup("$key/Item", "Logo", SdfIconType.Image)]
    [ReadOnly]
    [Title("Size Settings")]
    [ShowIf("@(this.useDefaultLogo || this.useCustomLogo) && !IsCustomLogoNull")]
    public Vector2 logoSize;

    [TabGroup("$key/Item", "Logo", SdfIconType.Image)] [ShowIf("@this.useDefaultLogo || this.useCustomLogo")]
    public bool useCustomSize;

    [TabGroup("$key/Item", "Logo", SdfIconType.Image)]
    [ShowIf("useCustomSize")]
    [MinValue(1f)]
    [ShowIf("@(this.useDefaultLogo || this.useCustomLogo) && this.useCustomSize")]
    public Vector2 customLogoSize;

    [TitleGroup("$key")] [TabGroup("$key/Item", "Colour", SdfIconType.Eyedropper)]
    public Color colour= Color.white;

    [HideInInlineEditors] public bool noDefaultLogo;

    [HideInInlineEditors] public string menuItemType;

    [HideInInlineEditors] [SerializeField] private GroupData[] groupData;

    private const string GUIPath =
        "Packages/com.sportsim.adminsystem/Runtime/MenuComponents/Components/DynamicSystem/GUI/Universal/Logo";

    private Texture2D _previewTexture;

    private bool IsCustomLogoNull => customLogo == null;


    public enum PreviewBackgrounds
    {
        Black,
        White
    }
    
#if UNITY_EDITOR

    private IEnumerable GetDefaultSprites()
    {
        return (from asset in FindAssets("t:Sprite", new[] { GUIPath })
            select GUIDToAssetPath(asset)
            into assetPath
            select LoadAssetAtPath<Sprite>(assetPath)
            into sprite
            let groupPath = GetValueDropdownGroup(
                sprite.name,
                new[] { "Big" })
            select new ValueDropdownItem(groupPath + sprite.name, sprite)).Cast<object>();
    }
#endif    

    private string GetValueDropdownGroup(string valueName, params string[][] group)
    {
        return (from groupArray in @group
            from groupName in groupArray
            where valueName.Contains(groupName)
            select groupName).Aggregate("", (current, groupName) => current + $"{groupName}/");
    }

    private void DrawDefaultPreview()
    {
        if (defaultLogo == null) return;

        var defaultTexture = defaultLogo.texture;
        var customTexture = !IsCustomLogoNull ? customLogo.texture : null;

        logoSize =
            useCustomSize
                ? customLogoSize
                : useCustomLogo && customTexture != null
                    ? new Vector2Int(customTexture.width, customTexture.height)
                    : new Vector2Int(defaultTexture.width, defaultTexture.height);

        DrawLogoPreviewGUI(defaultTexture, previewBackground);
    }

    private void DrawCustomPreview()
    {
        if (IsCustomLogoNull) return;

        var customTexture = customLogo.texture;
        var defaultTexture = defaultLogo.texture;

        logoSize = useCustomSize
            ? customLogoSize
            : useCustomLogo
                ? new Vector2Int(customTexture.width, customTexture.height)
                : new Vector2Int(defaultTexture.width, defaultTexture.height);

        DrawLogoPreviewGUI(customTexture, customLogoPreviewBackground);
    }

    private void DrawLogoPreviewGUI(Texture2D customTexture, PreviewBackgrounds backgroundSelection)
    {
        var constrainedSize = logoSize.CalculateConstrainedSize(100, 500);

        if (_previewTexture == null) _previewTexture = customTexture;
        else if (_previewTexture.name != customTexture.name) _previewTexture = customTexture;

        UpdatePreviewTexture(customTexture, constrainedSize);

        var box = new GUIStyle(GUI.skin.box)
        {
            normal =
            {
                background = GetPreviewBackground(backgroundSelection)
            }
        };

        GUILayout.BeginVertical(box);
        GUILayout.Label(_previewTexture,
            GUILayout.Height(constrainedSize.y + 5),
            GUILayout.Width(constrainedSize.x + 5));
        GUILayout.EndVertical();
    }

    private static Texture2D GetPreviewBackground(PreviewBackgrounds backgroundSelection)
    {
        var backgroundTexture = new Texture2D(1, 1);
        var colour = backgroundSelection == PreviewBackgrounds.Black ? Color.black : Color.white;

        backgroundTexture.SetPixel(1, 1, colour);

        backgroundTexture.Apply();
        return backgroundTexture;
    }

    private void UpdatePreviewTexture(Texture2D texture, Vector2Int size)
    {
        if (!useCustomSize)
        {
            _previewTexture = texture;
            return;
        }

        if (_previewTexture.GetSizeAsVector2Int() == size) return;

        if (size.y <= 0 || size.x <= 0) return;

        _previewTexture = texture.ResizeTexture(size);
    }
}