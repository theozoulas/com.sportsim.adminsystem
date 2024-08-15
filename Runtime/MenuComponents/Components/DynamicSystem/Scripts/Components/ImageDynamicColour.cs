#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MenuComponents.DynamicSystem
{
    public class ImageDynamicColour : MonoBehaviour
    {
        [FormerlySerializedAs("colourPallet")] [SerializeField] private ColourDynamicData colourDynamicData;
    
        public void OnValidate()
        {
            if(colourDynamicData == null) return;
        
            GetComponent<Image>().color = colourDynamicData.colour;
        }

    }
}
#endif