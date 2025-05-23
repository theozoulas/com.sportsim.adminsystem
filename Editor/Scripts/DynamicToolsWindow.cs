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
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 800);
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
                { "Data Entry/UI Settings/Default", DefaultDataEntryItemTree.Instance },
                { "Data Entry/UI Settings/Custom", CustomDataEntryItemTree.Instance },
                { "Data Entry/Data Entry Inputs/Default", DefaultDataEntryInputTree.Instance },
                { "UI Dynamic Data", BaseDynamicMenu.Instance, EditorIcons.GridBlocks },
                { "UI Dynamic Data/Default", DefaultMenuItemTree.Instance },
                { "UI Dynamic Data/Custom", CustomMenuItemTree.Instance },
                { "Text Dynamic Data", BaseDynamicMenu.Instance, SdfIconType.Type },
                { "Text Dynamic Data/Default", DefaultTextItemTree.Instance },
                { "Text Dynamic Data/Custom", CustomTextItemTree.Instance },
                { "Logo Dynamic Data", BaseDynamicMenu.Instance, EditorIcons.Image },
                { "Logo Dynamic Data/Default", DefaultLogoTree.Instance },
                { "Logo Dynamic Data/Custom", CustomLogoTree.Instance },
                { "Leaderboard", LeaderboardDynamicMenu.Instance, EditorIcons.List },
                { "Score", ScoreDynamicMenu.Instance, EditorIcons.FinnishBanner },
                { "Score/Custom", ExtraScoreDataMenu.Instance }
            };
            
            
            


            return tree;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (CustomMenuItemTree.Instance.newCustomMenuItemData == null) return;
            
            DestroyImmediate(CustomMenuItemTree.Instance.newCustomMenuItemData);
        }
    }
}
#endif