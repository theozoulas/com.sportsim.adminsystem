using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using static MenuComponents.SaveSystem.SaveManager;

[RequireComponent(typeof(TMP_Text))]
[InfoBox("Could not find any Extra Score Data, ensure you have created a Extra Score Data in the Admin System's control panel!", InfoMessageType.Warning, VisibleIf = "@!ExtraDataExist")]
public class ExtraScoreDisplay : MonoBehaviour
{
    private TMP_Text _text;
    
    [ValueDropdown("ExtraScoreDataKeys")]
    [ShowIf("ExtraDataExist")] 
    public string extraScoreDataKey;

    private IEnumerable<string> ExtraScoreDataKeys
        => ExtraScoreDataMenu.Instance.ExtraScoreData.Select(cd => cd.key);
    
    private bool ExtraDataExist => ExtraScoreDataKeys.Any();


    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        if(!ExtraDataExist) return;
        
        if (!TryGetExtraScoreFromId(extraScoreDataKey, out var reactionSpeed))
        {
            Debug.LogError($"Could Not Find Extra Score With Key:{extraScoreDataKey}");
            return;
        }

        _text.text = reactionSpeed.GetAsStringFormatted();
    }
    
}
