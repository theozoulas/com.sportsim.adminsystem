using TMPro;
using UnityEngine;
using static MenuComponents.SaveSystem.SaveManager;

[RequireComponent(typeof(TMP_Text))]
public class MainScoreDisplay : MonoBehaviour
{
     private TMP_Text _text;


     private void Awake()
     {
         _text = GetComponent<TMP_Text>();
     }

     private void Start()
    {
        _text.text = StaticScoreData.GetAsStringFormatted();
    }
}
