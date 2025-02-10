#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MenuComponents.DynamicSystem
{
    public class ImageDynamicColour : MonoBehaviour
    {
        [ValueDropdown("DefaultColourDataKeys")]
        public string defaultDataKey;

        public bool useCustomData;

        [ShowIf("useCustomData")] [ValueDropdown("CustomColourDataKeys")]
        public string customData;

        private IEnumerable<string> DefaultColourDataKeys
            => DefaultMenuItemTree.Instance.defaultMenuItemData.Select(cd => cd.key);

        private IEnumerable<string> CustomColourDataKeys
            => CustomMenuItemTree.Instance.customMenuItemDynamicData.Select(cd => cd.key);
        
        public void OnValidate()
        {
            if (!CustomColourDataKeys.Any()) customData = "";

            var image = GetComponent<Image>();
            
            if (!TrySetDefaultData(image)) TrySetCustomData(image);
        }
        
        private bool TrySetDefaultData(Image image)
        {
            if (useCustomData) return false;
            
            var defaultData = DefaultMenuItemTree.Instance.DefaultMenuItemDataDic[defaultDataKey];
            image.color = defaultData.color;

            if (defaultData.spriteSize != Vector2.zero)
            {
                image.GetComponent<RectTransform>().sizeDelta = defaultData.spriteSize;
            }
                
            if (defaultData.useCustomSprite && defaultData.customSprite != null)
            {
                image.sprite = defaultData.customSprite;
                return true;
            }
            
            if (defaultData.noDefaultSprite) return true;

            if (defaultData.defaultSprite == null) return false;
            image.sprite = defaultData.defaultSprite;
                
            return true;
        }

        private void TrySetCustomData(Image image)
        {
            var customMenuItemDataDic = CustomMenuItemTree.Instance.CustomMenuItemDataDic;

            if (!customMenuItemDataDic.TryGetValue(customData, out var data)) return;
            
            image.color = data.colour;

            if (data.useCustomSprite && data.customSprite != null)
            {
                image.sprite = data.customSprite;
                return;
            }

            if (!data.useDefaultSprite || data.defaultSprite == null) return;
            image.sprite = data.defaultSprite;
        }
    }
}
#endif