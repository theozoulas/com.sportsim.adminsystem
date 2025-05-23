using System.Collections;
using System.Linq;
using MenuComponents.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.AssetDatabase;
#endif   


[CreateAssetMenu(fileName = "dsfsdfs", menuName = "ScriptableObjects/dsfdsfsd", order = 1)]
[InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
public class MenuItemData : ScriptableObject
{
    [TitleGroup("$key")] [TabGroup("$key/Item", "Colour", SdfIconType.Eyedropper)]
    public Color colour = Color.white;

    [TabGroup("$key/Item", "Sprite")]
    [OnInspectorGUI("DrawDefaultPreview", append: true)]
    [ValueDropdown("GetDefaultSprites")]
    [HideIf("@this.noDefaultSprite || this.useCustomSprite")]
    [HideLabel]
    [Title("Default Sprite Settings")]
    public Sprite defaultSprite;

    [TabGroup("$key/Item", "Sprite", SdfIconType.Image)] [Title("Custom Sprite Settings")]
    public bool useCustomSprite;

    [TabGroup("$key/Item", "Sprite", SdfIconType.Image)]
    [ShowIf("useCustomSprite")]
    [OnInspectorGUI("DrawCustomPreview", append: true)]
    public Sprite customSprite;

    [TabGroup("$key/Item", "Sprite", SdfIconType.Image)]
    [ReadOnly]
    [Title("Size Settings")]
    [HideIf("@(this.noDefaultSprite && !this.useCustomSprite) || this.IsCustomSpriteNull")]
    public Vector2 spriteSize;

    [TabGroup("$key/Item", "Sprite", SdfIconType.Image)]
    public bool useCustomSize;

    [TabGroup("$key/Item", "Sprite", SdfIconType.Image)] [ShowIf("useCustomSize")]
    [MinValue(1f)]
    public Vector2 customSpriteSize;

    [HideInInlineEditors] public string key;

    [HideInInlineEditors] public bool noDefaultSprite;

    [HideInInlineEditors] public string menuItemType;

    [HideInInlineEditors] [SerializeField] private GroupData[] groupData;

    private const string GUIPath =
        "Packages/com.sportsim.adminsystem/Runtime/MenuComponents/Components/DynamicSystem/GUI/Universal/";

    private Texture2D _previewTexture;
    
    private bool IsCustomSpriteNull => customSprite == null;

#if UNITY_EDITOR
    protected virtual IEnumerable GetDefaultSprites()
    {
        return (from asset in FindAssets("t:Sprite", new[] { GUIPath + menuItemType })
            select GUIDToAssetPath(asset)
            into assetPath
            select LoadAssetAtPath<Sprite>(assetPath)
            into sprite
            let groupPath = GetValueDropdownGroup(
                sprite.name,
                groupData)
            select new ValueDropdownItem(groupPath + sprite.name, sprite)).Cast<object>();
    }
#endif   

    private string GetValueDropdownGroup(string valueName, GroupData[] group)
    {
        return (from groupArray in @group
            from groupName in groupArray.groups
            where valueName.Contains(groupName)
            select groupName).Aggregate("", (current, groupName) => current + $"{groupName}/");
    }

    private void DrawDefaultPreview()
    {
        if (defaultSprite == null) return;

        var defaultTexture = defaultSprite.texture;
        var customTexture = !IsCustomSpriteNull? customSprite.texture : null;

        spriteSize =
            useCustomSize
                ? customSpriteSize
                : useCustomSprite && customTexture != null
                    ? new Vector2Int(customTexture.width, customTexture.height)
                    : new Vector2Int(defaultTexture.width, defaultTexture.height);

        DrawSpritePreviewGUI(defaultTexture);
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

        DrawSpritePreviewGUI(customTexture);
    }

    private void DrawSpritePreviewGUI(Texture2D customTexture)
    {
        var constrainedSize = spriteSize.CalculateConstrainedSize(100, 500);

        if (_previewTexture == null) _previewTexture = customTexture;
        else if (_previewTexture.name != customTexture.name) _previewTexture = customTexture;

        UpdatePreviewTexture(customTexture, constrainedSize);

        GUILayout.BeginVertical(GUI.skin.box);
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