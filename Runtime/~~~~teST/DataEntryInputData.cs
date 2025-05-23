using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MenuComponents.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.AssetDatabase;
#endif    

[CreateAssetMenu(fileName = "Data Entry Input", menuName = "ScriptableObjects/DataEntryInput", order = 1)]
[InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
public class DataEntryInputData : ScriptableObject
{
    [HideInInlineEditors] public string key;

    [HideInInlineEditors] public bool isRequired;

    [TitleGroup("$GetTitle")] [TabGroup("$GetTitle/Item", "Settings", SdfIconType.GearFill)] [HideIf("@!_enabled && !isRequired")]
    public bool overwriteInputName;
    
    [TitleGroup("$GetTitle")] 
    [TabGroup("$GetTitle/Item", "Settings", SdfIconType.GearFill)] 
    [HideIf("@(!_enabled && !isRequired) || !overwriteInputName")]
    public string customInputName;
    
    [TitleGroup("$GetTitle")] [TabGroup("$GetTitle/Item", "Settings", SdfIconType.GearFill)] [HideIf("@!_enabled && !isRequired")]
    public bool overwritePlaceholderText;
    
    [TitleGroup("$GetTitle")] 
    [TabGroup("$GetTitle/Item", "Settings", SdfIconType.GearFill)] 
    [HideIf("@(!_enabled && !isRequired) || !overwritePlaceholderText")]
    public string placeholderText;
    
    [TitleGroup("$GetTitle")] [TabGroup("$GetTitle/Item", "Settings", SdfIconType.GearFill)] [HideIf("@(!_enabled && !isRequired) || !CustomDataExits")]
    public bool useCustomUI;
    
    [TabGroup("$GetTitle/Item", "Settings", SdfIconType.GearFill)]
    [ValueDropdown("GetCustomUiKeys")]
    [HideIf("@!this.useCustomUI || (!_enabled && !isRequired) || !CustomDataExits")]
    [HideLabel]
    public string uiCustomKey;

    [TitleGroup("$GetTitle")] [TabGroup("$GetTitle/Item", "Settings", SdfIconType.GearFill)] [HideIf("@(!_enabled && !isRequired)")]
    public bool isMandatory;

    [TabGroup("$GetTitle/Item", "Settings", SdfIconType.GearFill)]
    [ValueDropdown("GetDefaultValidations")]
    [HideIf("@this.useCustomValidation || !isMandatory || (!_enabled && !isRequired)")]
    [HideLabel]
    [Title("Default Validation")]
    public Validation defaultValidation;

    [TabGroup("$GetTitle/Item", "Settings", SdfIconType.GearFill)]
    //[ValueDropdown("GetCustomValidations")]
    [HideIf("@!this.useCustomValidation || !isMandatory || (!_enabled && !isRequired)")]
    [HideLabel]
    [Title("Custom Validation")]
    public Validation customValidation;

    [TabGroup("$GetTitle/Item", "Settings", SdfIconType.GearFill)] [HideIf("@!isMandatory || (!_enabled && !isRequired)")]
    public bool useCustomValidation;

    private const string Path =
        "Packages/com.sportsim.adminsystem/Runtime/MenuComponents/ScriptableObjects";

    private bool _enabled;
    
    private IEnumerable<string> GetCustomUiKeys
        => CustomDataEntryItemTree.Instance.customDataEntryItemDynamicData.Select(cd => cd.key);
    
    private bool CustomDataExits => GetCustomUiKeys.Any();


    [TitleGroup("$GetTitle")]
    [Button("$GetToggleButtonLabel", ButtonSizes.Large), GUIColor("$GetToggleButtonColour")]
    [HideIf("isRequired")]
    [PropertyOrder(-1)]
    public void Toggle()
    {
        _enabled = !_enabled;
    }

    private Color GetToggleButtonColour()
    {
        return _enabled ? Color.green : Color.red;
    }

    private string GetToggleButtonLabel()
    {
        return _enabled ? "Toggle (Enabled)" : "Toggle (Disabled)";
    }

    private string GetTitle()
    {
        return isRequired ? key + " (Is Required)" : key;
    }

#if UNITY_EDITOR
    private IEnumerable GetDefaultValidations()
    {
        return (from asset in FindAssets("t:ScriptableObject", new[] { Path + "/Validation" })
            select GUIDToAssetPath(asset)
            into assetPath
            select LoadAssetAtPath<Validation>(assetPath)
            into validation
            select new ValueDropdownItem(validation.name, validation)).Cast<object>();
    }
    
#endif    
}