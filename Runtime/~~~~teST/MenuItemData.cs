using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Examples;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEditor.AssetDatabase;


[CreateAssetMenu(fileName = "dsfsdfs", menuName = "ScriptableObjects/dsfdsfsd", order = 1)] 
[InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
public class MenuItemData : ScriptableObject
{
    [TitleGroup("$key")] [TabGroup("$key/Item", "Colour", SdfIconType.Eyedropper)]
    public Color color;

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

    [TabGroup("$key/Item", "Sprite", SdfIconType.Image)] [ReadOnly] [Title("Size Settings")]
    public Vector2 spriteSize;

    [TabGroup("$key/Item", "Sprite", SdfIconType.Image)]
    public bool useCustomSize;

    [TabGroup("$key/Item", "Sprite", SdfIconType.Image)] [ShowIf("useCustomSize")]
    public Vector2 customSpriteSize;

    [HideInInlineEditors] public bool noDefaultSprite;

    protected const string GUIPath =
        "Packages/com.sportsim.adminsystem/Runtime/MenuComponents/Components/DynamicSystem/GUI/Universal/";

    [HideInInlineEditors] public string key;


    public MenuItemData(string key, Color color, bool noDefaultSprite)
    {
        this.key = key;
        this.color = color;
        this.noDefaultSprite = noDefaultSprite;
    }

    protected virtual IEnumerable GetDefaultSprites()
    {
        return (from asset in FindAssets("t:Sprite", new[] { GUIPath + "Button" })
            select GUIDToAssetPath(asset)
            into assetPath
            select LoadAssetAtPath<Sprite>(assetPath)
            into sprite
            let groupPath = GetValueDropdownGroup(
                sprite.name,
                new[] { "Large", "Small" }, new[] { "Outline", "Filled" })
            select new ValueDropdownItem(groupPath + sprite.name, sprite)).Cast<object>();
    }

    protected string GetValueDropdownGroup(string valueName, params string[][] group)
    {
        return (from groupArray in @group
            from groupName in groupArray
            where valueName.Contains(groupName)
            select groupName).Aggregate("", (current, groupName) => current + $"{groupName}/");
    }

    private void DrawDefaultPreview()
    {
        if (defaultSprite == null) return;

        spriteSize = useCustomSize
            ? customSpriteSize
            : useCustomSprite
                ? new Vector2Int(customSprite.texture.width, customSprite.texture.height)
                : new Vector2Int(defaultSprite.texture.width, defaultSprite.texture.height);


        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label(defaultSprite.texture, GUILayout.MaxHeight(100));
        GUILayout.EndVertical();
    }

    private void DrawCustomPreview()
    {
        if (customSprite == null) return;

        spriteSize = useCustomSize
            ? customSpriteSize
            : useCustomSprite
                ? new Vector2Int(customSprite.texture.width, customSprite.texture.height)
                : new Vector2Int(defaultSprite.texture.width, defaultSprite.texture.height);

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label(customSprite.texture, GUILayout.MaxHeight(100));
        GUILayout.EndVertical();
    }
}