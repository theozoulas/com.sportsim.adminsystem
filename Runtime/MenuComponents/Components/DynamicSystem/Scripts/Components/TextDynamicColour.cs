#if UNITY_EDITOR
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace MenuComponents.DynamicSystem
{
    public class TextDynamicColour : MonoBehaviour
    {
        [FormerlySerializedAs("colourPallet")] [SerializeField] private ColourDynamicData colourDynamicData;

        public void OnValidate()
        {
            if(colourDynamicData == null) return;
        
            GetComponent<TMP_Text>().color = colourDynamicData.colour;
        }
    }
}
#endif
