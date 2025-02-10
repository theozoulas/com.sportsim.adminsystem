#if UNITY_EDITOR
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace MenuComponents.DynamicSystem
{
    public class TextDynamicColour : MonoBehaviour
    {
        [FormerlySerializedAs("menuItemDynamicData")] [FormerlySerializedAs("colourDynamicData")] [FormerlySerializedAs("colourPallet")] [SerializeField] private CustomMenuItemData customMenuItemData;

        public void OnValidate()
        {
            if(customMenuItemData == null) return;
        
            GetComponent<TMP_Text>().color = customMenuItemData.colour;
        }
    }
}
#endif
