#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using MenuComponents.DynamicSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TextDynamicFont : MonoBehaviour
{
    [SerializeField] private FontDynamicData fontData;

    public void OnValidate()
    {
        if(fontData == null) return;
        
        GetComponent<TMP_Text>().font = fontData.font;
    }
}
#endif