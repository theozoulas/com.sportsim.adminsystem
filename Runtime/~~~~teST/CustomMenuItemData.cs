using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEditor.AssetDatabase;

namespace MenuComponents.DynamicSystem
{
    [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
    public class CustomMenuItemData : ScriptableObject
    {
        [ReadOnly]
        [HideInInlineEditors]
        public string key;

        [TitleGroup("$key")] [TabGroup("$key/Item", "Colour", SdfIconType.Eyedropper)]
        public Color colour = Color.white;
        
        [TabGroup("$key/Item", "Sprite", SdfIconType.Image)]
        public bool useCustomSprite;
        
        [TabGroup("$key/Item", "Sprite", SdfIconType.Image)]
        [HideIf("useCustomSprite")]
        public bool useDefaultSprite;
        
        [TabGroup("$key/Item", "Sprite")]
        [OnInspectorGUI("DrawDefaultPreview", append: true)]
        [ValueDropdown("GetDefaultSprites")]
        [HideIf("@!this.useDefaultSprite || this.useCustomSprite")]
        [HideLabel]
        [Title("Sprite Settings")]
        public Sprite defaultSprite;

        [TabGroup("$key/Item", "Sprite", SdfIconType.Image)]
        [ShowIf("useCustomSprite")]
        [OnInspectorGUI("DrawCustomPreview", append: true)]
        public Sprite customSprite;

        [TabGroup("$key/Item", "Sprite", SdfIconType.Image)] [ReadOnly] [Title("Size Settings")]
        [ShowIf("@this.useDefaultSprite || this.useCustomSprite")]
        public Vector2 spriteSize;

        [TabGroup("$key/Item", "Sprite", SdfIconType.Image)]
        [ShowIf("@this.useDefaultSprite || this.useCustomSprite")]
        public bool useCustomSize;

        [TabGroup("$key/Item", "Sprite", SdfIconType.Image)] 
        [ShowIf("@(this.useDefaultSprite || this.useCustomSprite) && this.useCustomSize")]
        public Vector2 customSpriteSize;
        
        private bool _noDefaultSprite;
        
        private const string GUIPath =
            "Packages/com.sportsim.adminsystem/Runtime/MenuComponents/Components/DynamicSystem/GUI/Universal";
        
        private IEnumerable GetDefaultSprites()
        {
            return (from asset in FindAssets("t:Sprite", new[] { GUIPath })
                select GUIDToAssetPath(asset)
                into assetPath
                select LoadAssetAtPath<Sprite>(assetPath)
                into sprite
                let groupPath = GetValueDropdownGroup(
                    sprite.name,
                    new[] { "Button", "DataEntry" },
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
            if (defaultSprite == null || !useDefaultSprite) return;

            spriteSize = useCustomSize
                ? customSpriteSize
                    : new Vector2(defaultSprite.texture.width, defaultSprite.texture.height);


            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(defaultSprite.texture, GUILayout.MaxHeight(100), GUILayout.MaxWidth(555));
            GUILayout.EndVertical();
        }
        
        private void DrawCustomPreview()
        {
            if (customSprite == null)
            {
                spriteSize = Vector2.zero;
                return;
            }

            spriteSize = useCustomSize
                ? customSpriteSize
                :  new Vector2(customSprite.texture.width, customSprite.texture.height);

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(customSprite.texture, GUILayout.MaxHeight(100), GUILayout.MaxWidth(300));
            GUILayout.EndVertical();
        }
    }
}