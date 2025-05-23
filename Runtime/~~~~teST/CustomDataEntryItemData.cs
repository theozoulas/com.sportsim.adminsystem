#if UNITY_EDITOR

using System.Collections;
using System.Linq;
using Editor.Scripts;
using MenuComponents.Utility;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


using static UnityEditor.AssetDatabase;


[InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
public class CustomDataEntryItemData : ScriptableObject
{
    [ReadOnly] [HideInInlineEditors] public string key;

    [TitleGroup("$key")] [TabGroup("$key/Item", "Settings", SdfIconType.GearFill)]
    public Color colour = Color.white;

    [TabGroup("$key/Item", "Settings", SdfIconType.GearFill)]
    public bool useCustomSprite;

    [TabGroup("$key/Item", "Settings", SdfIconType.GearFill)][HideIf("useCustomSprite")] public bool useDefaultSprite;

    [TabGroup("$key/Item", "Settings", SdfIconType.GearFill)]
    [OnInspectorGUI("DrawDefaultPreview", append: true)]
    [ValueDropdown("GetDefaultSprites")]
    [HideIf("@!this.useDefaultSprite || this.useCustomSprite")]
    [HideLabel]
    [Title("Sprite Settings")]
    public Sprite defaultSprite;

    [TabGroup("$key/Item", "Settings", SdfIconType.GearFill)]
    [OnValueChanged("ErrorColourChanged")] [HideIf("@this.useCustomSprite || !this.useDefaultSprite")]
    public Color errorColour = Color.red;

    [TabGroup("$key/Item", "Settings", SdfIconType.GearFill)]
    [ShowIf("useCustomSprite")]
    public Sprite customSprite;

    [TabGroup("$key/Item", "Settings", SdfIconType.GearFill)]
    [ShowIf("useCustomSprite")] [OnInspectorGUI("DrawCustomPreview", append: true)]
    public Sprite customErrorSprite;

    [TabGroup("$key/Item", "Settings", SdfIconType.GearFill)]
    [OnValueChanged("ErrorColourChanged")] [HideIf("@!this.useCustomSprite")]
    public Color customErrorColour = Color.red;

    [TabGroup("$key/Item", "Settings", SdfIconType.GearFill)]
    [ReadOnly]
    [Title("Size Settings")]
    [ShowIf("@(this.useDefaultSprite || this.useCustomSprite) && !this.IsCustomSpriteNull")]
    public Vector2 spriteSize;

    [TabGroup("$key/Item", "Settings", SdfIconType.GearFill)]
    [ShowIf("@this.useDefaultSprite || this.useCustomSprite")]
    public bool useCustomSize;

    [TabGroup("$key/Item", "Settings", SdfIconType.GearFill)]
    [ShowIf("@(this.useDefaultSprite || this.useCustomSprite) && this.useCustomSize")]
    public Vector2 customSpriteSize;

    [TabGroup("$key/Item", "Settings", SdfIconType.GearFill)]
    public Sprite DefaultErrorSprite { get; private set; }

    private const string GUIPath =
        "Packages/com.sportsim.adminsystem/Runtime/MenuComponents/Components/DynamicSystem/GUI/Universal/DataEntry";

    private Texture2D _previewTexture;
    private Texture2D _previewErrorTexture;

    private bool _errorColourChanged;

    private bool IsCustomSpriteNull => customSprite == null;
    
    private Sprite GetDefaultErrorSprite()
    {
        if (defaultSprite == null) return null;

        var errorSprites =
            from asset in FindAssets("t:Sprite", new[] { GUIPath + "/Error" })
            select GUIDToAssetPath(asset)
            into assetPath
            select LoadAssetAtPath<Sprite>(assetPath);

        return errorSprites.FirstOrDefault(s => s.name.Contains(defaultSprite.name));
    }

    private IEnumerable GetDefaultSprites()
    {
        return (from asset in FindAssets("t:Sprite", new[] { GUIPath + "/Normal" })
            select GUIDToAssetPath(asset)
            into assetPath
            select LoadAssetAtPath<Sprite>(assetPath)
            into sprite
            let groupPath = GetValueDropdownGroup(
                sprite.name,
                new[] { "Small", "Large" },
                new[] { "Outline", "Filled" })
            select new ValueDropdownItem(groupPath + sprite.name, sprite)).Cast<object>();
    }
    
     private string GetValueDropdownGroup(string valueName, params string[][] group)
    {
        return (from groupArray in @group
            from groupName in groupArray
            where valueName.Contains(groupName)
            select groupName).Aggregate("", (current, groupName) => current + $"{groupName}/");
    }

    private void DrawDefaultPreview()
    {
        if (defaultSprite == null) return;

        var defaultTexture = defaultSprite.texture;
        var customTexture = !IsCustomSpriteNull ? customSprite.texture : null;

        spriteSize =
            useCustomSize
                ? customSpriteSize
                : useCustomSprite && customTexture != null
                    ? new Vector2Int(customTexture.width, customTexture.height)
                    : new Vector2Int(defaultTexture.width, defaultTexture.height);

        DrawSpritePreviewGUI(defaultTexture, "Default Data Entry Box");
        DrawDefaultErrorPreview();
    }
    
    private void DrawDefaultErrorPreview()
    {
        DefaultErrorSprite = GetDefaultErrorSprite();
        
        if (DefaultErrorSprite == null) return;

        var errorTexture = DefaultErrorSprite.texture;

        if (_previewErrorTexture == null || _previewErrorTexture.name != errorTexture.name || _errorColourChanged)
            _previewErrorTexture = CreateErrorPreviewTexture(defaultSprite.texture, errorTexture, errorColour);

        DrawSpritePreviewGUI(_previewErrorTexture, "Default Data Entry Error Outline");
    }
    
    private void DrawCustomErrorPreview()
    {
        if (customErrorSprite == null || customSprite == null) return;

        var errorTexture = customErrorSprite.texture;

        if (_previewErrorTexture == null || _previewErrorTexture.name != errorTexture.name || _errorColourChanged)
            _previewErrorTexture = CreateErrorPreviewTexture(customSprite.texture, errorTexture, customErrorColour);

        DrawSpritePreviewGUI(_previewErrorTexture, "Custom Data Entry Error Outline");
    }
    
    private Texture2D CreateErrorPreviewTexture(Texture2D defaultTexture, Texture2D errorTexture, Color colourForError)
    {
        _errorColourChanged = false;

        errorTexture.SetTextureToReadable();
        defaultTexture.SetTextureToReadable();
        
        var texture = new Texture2D(defaultTexture.width, defaultTexture.height)
        {
            name = errorTexture.name
        };

        for (var x = 0; x < defaultTexture.width; x++)
        {
            for (var y = 0; y < defaultTexture.height; y++)
            {
                var defaultPixel = defaultTexture.GetPixel(x, y);
                var errorPixel = errorTexture.GetPixel(x, y) * colourForError;

                texture.SetPixel(x, y, errorPixel.a > 0 ? errorPixel : defaultPixel);
            }
        }

        texture.Apply();
        
        return texture;
    }

    private void DrawCustomPreview()
    {
        if (IsCustomSpriteNull) return;

        var customTexture = customSprite.texture;
        var defaultTexture = defaultSprite.texture;
        
        spriteSize = useCustomSize
            ? customSpriteSize
            : useCustomSprite
                ? new Vector2Int(customTexture.width, customTexture.height)
                : new Vector2Int(defaultTexture.width, defaultTexture.height);

        DrawSpritePreviewGUI(customTexture, "Custom Data Entry Box");
        DrawCustomErrorPreview();
    }
    
    private void ErrorColourChanged()
    {
        _errorColourChanged = true;
    }
    
   

    private void DrawSpritePreviewGUI(Texture2D customTexture, string title)
    {
        var constrainedSize = spriteSize.CalculateConstrainedSize(100, 500);

        if (_previewTexture == null || _previewTexture.name != customTexture.name) 
            _previewTexture = customTexture;

        UpdatePreviewTexture(customTexture, constrainedSize);

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label(title);
        GUILayout.Label(_previewTexture,
            GUILayout.Height(constrainedSize.y + 5),
            GUILayout.Width(constrainedSize.x + 5));
        GUILayout.EndVertical();
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

#endif