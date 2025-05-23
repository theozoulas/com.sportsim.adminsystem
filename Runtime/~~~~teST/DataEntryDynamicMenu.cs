using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

[GlobalConfig("Assets/Resources/AdminSystem/ConfigFiles/")]
public class DataEntryDynamicMenu : GlobalConfig<DataEntryDynamicMenu>
{
    [Title("View Player Data Settings")]
    
    public bool enableExtraField1;
    [ShowIf("enableExtraField1")]
    public string extraField1;
    
    public bool enableExtraField2;
    [ShowIf("enableExtraField2")]
    public string extraField2;
    
    public IEnumerable<string> GetExtraFields()
    {
        var extraFields = new List<string>();
        
        if(enableExtraField1) extraFields.Add(extraField1);
        if(enableExtraField2) extraFields.Add(extraField2);

        return extraFields.ToArray();
    }
}
