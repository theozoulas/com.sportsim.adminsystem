#if UNITY_EDITOR
using System;
using System.IO;
using Editor.Scripts;
using MenuComponents.Utility;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace MenuComponents.DynamicSystem
{
    public class DynamicToolsWindow : OdinMenuEditorWindow
    {
        private const string ScriptableObjectsPath =
            "Assets/Resources/AdminSystem";


        /// <summary>
        /// Opens the Admin System's control panel and center aligns it. 
        /// </summary>
        [MenuItem("AdminSystem/Control Panel")]
        private static void OpenWindow()
        {
            var window = GetWindow<DynamicToolsWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        /// <summary>
        /// Builds out the menu tree.
        /// </summary>
        /// <returns>Returns the Odin Menu Tree</returns>
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                { "Home", this, EditorIcons.House },
                { "Data Entry", DataEntryDynamicMenu.Instance, EditorIcons.File },
                { "UI Dynamic Data", BaseDynamicMenu.Instance, EditorIcons.GridBlocks },
                { "UI Dynamic Data/Default", DefaultMenuItemTree.Instance },
                { "UI Dynamic Data/Advanced", CustomMenuItemTree.Instance },
                { "Text Colour", BaseDynamicMenu.Instance, EditorIcons.SpeechBubbleRound },
                { "Text Font", BaseDynamicMenu.Instance, EditorIcons.PenAdd },
                { "Logos", BaseDynamicMenu.Instance, EditorIcons.Image },
                { "Leaderboard", LeaderboardDynamicMenu.Instance, EditorIcons.List },
                { "Score", ScoreDynamicMenu.Instance, EditorIcons.FinnishBanner }
            };

            tree.AddScriptableObjectsAtPath("Text Colour",
                Resources.LoadAll<ScriptableObject>("AdminSystem/FontColourPallets"));

            tree.AddScriptableObjectsAtPath("Text Font", Resources.LoadAll<ScriptableObject>("AdminSystem/FontData"));

            tree.AddScriptableObjectsAtPath("Logos", Resources.LoadAll<ScriptableObject>("AdminSystem/LogoImages"));


            return tree;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (CustomMenuItemTree.Instance.newCustomMenuItemData == null) return;
            
            DestroyImmediate(CustomMenuItemTree.Instance.newCustomMenuItemData);
        }

        [Button(50)]
        private void Setup()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            if (!AssetDatabase.IsValidFolder("Assets/Resources/AdminSystem"))
                AssetDatabase.CreateFolder("Assets/Resources", "AdminSystem");

            foreach (var folder in AssetDatabase.GetSubFolders(
                         "Packages/com.sportsim.adminsystem/Runtime/MenuComponents/Components/DynamicSystem/DynamicScriptableObjects"))
            {
                var folderName = Path.GetFileName(folder);

                if (!AssetDatabase.IsValidFolder($"Assets/Resources/AdminSystem/{folderName}"))
                    AssetDatabase.CreateFolder("Assets/Resources/AdminSystem", folderName);

                foreach (var asset in AssetDatabase.FindAssets("t:ScriptableObject",
                             new[]
                             {
                                 $"Packages/com.sportsim.adminsystem/Runtime/MenuComponents/Components/DynamicSystem/DynamicScriptableObjects/{folderName}"
                             }))
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(asset);

                    AssetDatabase.CopyAsset(assetPath,
                        $"Assets/Resources/AdminSystem/{folderName}/{Path.GetFileName(assetPath)}");
                }
            }

            if (!AssetDatabase.IsValidFolder("Assets/Resources/AdminSystem/ColourPallets"))
                AssetDatabase.CreateFolder("Assets/Resources/AdminSystem", "ColourPallets");
        }
    }
}
#endif