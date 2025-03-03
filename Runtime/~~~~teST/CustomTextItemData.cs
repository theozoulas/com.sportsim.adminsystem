using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MenuComponents.DynamicSystem;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using static UnityEditor.AssetDatabase;

[InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
public class CustomTextItemData : ScriptableObject
{
    [ReadOnly]
    [HideInInlineEditors]
    public string key;
    
   [TitleGroup("$key")] [TabGroup("$key/Item", "Colour", SdfIconType.Eyedropper)]
    public Color color;

    [TabGroup("$key/Item", "Font")]
    [OnInspectorGUI("DrawDefaultPreview", append: true)]
    [ValueDropdown("GetDefaultFonts")]
    [HideIf("@this.useCustomFont")]
    [OnValueChanged("ResetNoPreviewFontBool")]
    [InfoBox(
        "Could Not Find Associated Font from TMP_FontAsset, Make sure That the Original Font File Is With the TMP_FontAsset!",
        InfoMessageType.Warning, VisibleIf = "_noPreviewFontFound")]
    [HideLabel]
    [Title("Default Sprite Settings")]
    public TMP_FontAsset defaultFont;

    [TabGroup("$key/Item", "Font", SdfIconType.Type)] [Title("Custom Sprite Settings")]
    public bool useCustomFont;

    [TabGroup("$key/Item", "Font")] [ShowIf("useCustomFont")]
    [InfoBox(
        "Could Not Find Associated Font From TMP_FontAsset, Make Sure That The Original Font File Exits And Has The Same Name As the TMP_FontAsset Or Use Manual Font Preview!",
        InfoMessageType.Warning, VisibleIf = "_noPreviewFontFound")]
    [OnValueChanged("ResetNoPreviewFontBool")][OnInspectorGUI("DrawCustomPreview", append: true)]
    public TMP_FontAsset customFont;

    [ShowIf("useCustomFont")]
    [TabGroup("$key/Item", "Font")]
    public bool useManualPreviewFont;

    [ShowIf("useManualPreviewFont")]
    [TabGroup("$key/Item", "Font")]
    public Font manualPreviewFont;

    [TabGroup("$key/Item", "Font")] [HideIf("autoSize")] [Title("Size Settings")] [MinValue(1f)]
    public float fontSize = 30;

    [TabGroup("$key/Item", "Font")] [Title("Size Settings")] [ShowIf("autoSize")] [HideLabel]
    public AutoSizeData autoSizeData;

    [TabGroup("$key/Item", "Font")] public bool autoSize;
    
    [EnumToggleButtons] [HideLabel] [Title("Middle Alignment Settings")]
    [TabGroup("$key/Item", "Font")] 
    [InfoBox("Due to inconsistent vertical center alignment when using TextMeshPro with a custom font, try each of these settings for better centering on the vertical axis.")]
    public MiddleAlignment middleAlignmentOptions = MiddleAlignment.Capline;

    [HideInInlineEditors] public GroupData[] groupData;

    private const string GUIPath =
        "Packages/com.sportsim.adminsystem/Runtime/MenuComponents/Components/DynamicSystem/GUI/Universal/Fonts";

    private Font _previewFont;
    private bool _noPreviewFontFound;

    private bool _wasUsingCustomFont;
    

    public enum MiddleAlignment
    {
        Middle,
        Baseline,
        Midline,
        Capline
    }

    protected virtual IEnumerable GetDefaultFonts()
    {
        return (from asset in FindAssets("t:TMP_FontAsset", new[] { GUIPath })
            select GUIDToAssetPath(asset)
            into assetPath
            select LoadAssetAtPath<TMP_FontAsset>(assetPath)
            into fontAsset
            let groupPath = GetValueDropdownGroup(
                fontAsset.name,
                groupData)
            select new ValueDropdownItem(groupPath + fontAsset.name, fontAsset)).Cast<object>();
    }

    private static string GetValueDropdownGroup(string valueName, IEnumerable<GroupData> group)
    {
        return (from groupArray in @group
            from groupName in groupArray.groups
            where valueName.Contains(groupName)
            select groupName).Aggregate("", (current, groupName) => current + $"{groupName}/");
    }

    private void ResetNoPreviewFontBool()
    {
        _noPreviewFontFound = false;
    }
 private void DrawDefaultPreview()
    {
        if (_wasUsingCustomFont)
        {
            _noPreviewFontFound = false;
            _wasUsingCustomFont = false;
        }
        
        if (defaultFont == null || _noPreviewFontFound) return;

        Font font;

        if (_previewFont != null && defaultFont.name.Contains(_previewFont.name))
        {
            font = _previewFont;
        }
        else
        {
            if (!TryGetFontFromTmpFontAsset(defaultFont, out font))
            {
                _noPreviewFontFound = true;
                return;
            }

            _previewFont = font;
        }

        CreateFontPreviewGUI(font);
    }
    
    private void DrawCustomPreview()
    {
        if (customFont == null || (_noPreviewFontFound && !useManualPreviewFont)) return;
        
        _wasUsingCustomFont = true;

        Font font;

        if (!useManualPreviewFont)
        {
            if (_previewFont != null && customFont.name.Contains(_previewFont.name))
            {
                font = _previewFont;
            }
            else
            {
                if (!TryGetFontInAssetDataBase(customFont.name, out font))
                {
                    _noPreviewFontFound = true;
                    return;
                }

                _previewFont = font;
            }
        }
        else
        {
            if(manualPreviewFont == null) return;

            font = manualPreviewFont;
        }
        
        CreateFontPreviewGUI(font);
    }
    
    private static void CreateFontPreviewGUI(Font font)
    {
        const string exampleText = "The quick brown fox jumps over the lazy dog. 0123456789";

        var previewRect = EditorGUILayout.GetControlRect(false, 100);
        EditorGUI.DrawRect(previewRect, Color.black);

        var textRect = previewRect;
        textRect.x += 10;
        textRect.width -= 20;
        textRect.y += 10;
        textRect.height -= 20;

        var displayFontSize = Mathf.Clamp(textRect.height - 2, 2, 100);

        var textLength = exampleText.Length;
        if (textLength > 0)
        {
            var charSizeEstimate = textRect.width * 1.6f / (textLength);
            displayFontSize = Mathf.Min(displayFontSize, charSizeEstimate);
        }

        displayFontSize = Mathf.Clamp(displayFontSize, 2, 100);

        var style = new GUIStyle
        {
            font = font,
            fontSize = Mathf.RoundToInt(displayFontSize),
            normal =
            {
                textColor = Color.white
            },
            wordWrap = false,
            alignment = TextAnchor.MiddleCenter
        };

        EditorGUI.LabelField(textRect, exampleText, style);
    }

    private static bool TryGetFontFromTmpFontAsset(Object tmpFontAsset, out Font font)
    {
        font = FindAssets("t:Font", new[] { GUIPath })
            .Select(GUIDToAssetPath).Select(LoadAssetAtPath<Font>)
            .FirstOrDefault(f => tmpFontAsset.name.Contains(f.name));

        return font != null;
    }

    private static bool TryGetFontInAssetDataBase(string tmpFontName, out Font font)
    {
        var fontName = tmpFontName.Replace("SDF", "");
        
        var fontGuids = FindAssets(fontName + " t:Font");

        font = null;
        
        if (fontGuids.Length > 0)
        {
            var fontPath = GUIDToAssetPath(fontGuids[0]);
            font = LoadAssetAtPath<Font>(fontPath);
        }
        else return false;

        return true;
    }

    
}