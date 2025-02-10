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
    public class CustomMenuItemData : BaseDynamicData
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
        [OnInspectorGUI("DrawPreview", append: true)]
        [ValueDropdown("GetDefaultSprites")]
        [HideIf("@!this.useDefaultSprite || this.useCustomSprite")]
        [HideLabel]
        [Title("Sprite Settings")]
        public Sprite defaultSprite;

        [TabGroup("$key/Item", "Sprite", SdfIconType.Image)] [ShowIf("useCustomSprite")]
        public Sprite customSprite;
        
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

        private void DrawPreview()
        {
            if (defaultSprite == null) return;

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(defaultSprite.texture, GUILayout.MaxHeight(100));
            GUILayout.EndVertical();
        }
    }
}