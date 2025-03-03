#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MenuComponents.DynamicSystem
{
    public class ImageDynamicController : MonoBehaviour
    {
        [ValueDropdown("DefaultColourDataKeys")]
        public string defaultDataKey;

        [ShowIf("CustomDataExits")] public bool useCustomData;

        [ShowIf("@this.useCustomData && this.CustomDataExits")] [ValueDropdown("CustomColourDataKeys")]
        public string customData;

        [Title("Bitmask Enum")] [EnumToggleButtons] [HideLabel]
        public DisableImageDynamicFeatureOptions disableImageDynamicFeatures;

        private IEnumerable<string> DefaultColourDataKeys
            => DefaultMenuItemTree.Instance.defaultItemData.Select(cd => cd.key);

        private IEnumerable<string> CustomColourDataKeys
            => CustomMenuItemTree.Instance.customMenuItemDynamicData.Select(cd => cd.key);

        private bool CustomDataExits => CustomColourDataKeys.Any();

        [Flags]
        public enum DisableImageDynamicFeatureOptions
        {
            DisableColour = 1 << 1,
            DisableSprite = 1 << 2,
            DisableSize = 1 << 3,
            DisableAll = DisableColour | DisableSprite | DisableSize
        }

        public void OnValidate()
        {
            if (!CustomColourDataKeys.Any()) customData = "";

            var image = GetComponent<Image>();

            if (!TrySetDefaultData(image)) TrySetCustomData(image);
        }

        private bool TrySetDefaultData(Image image)
        {
            if (useCustomData && customData != "") return false;

            var defaultMenuItemDataDic = DefaultMenuItemTree.Instance.DefaultItemDataDic;

            if (!defaultMenuItemDataDic.TryGetValue(defaultDataKey, out var defaultData))
            {
                defaultDataKey = "";
                return false;
            }

            if (!disableImageDynamicFeatures.HasFlag(DisableImageDynamicFeatureOptions.DisableColour))
                image.color = defaultData.color;

            var rectTransform = image.GetComponent<RectTransform>();

            if (!disableImageDynamicFeatures.HasFlag(DisableImageDynamicFeatureOptions.DisableSize))
            {
                if (defaultData.spriteSize != Vector2.zero && !defaultData.noDefaultSprite)
                {
                    rectTransform.sizeDelta = defaultData.spriteSize;
                }

                if (defaultData.noDefaultSprite && defaultData.useCustomSize)
                {
                    rectTransform.sizeDelta = defaultData.customSpriteSize;
                }
            }
            
            if (disableImageDynamicFeatures.HasFlag(DisableImageDynamicFeatureOptions.DisableSprite)) return true;

            if (defaultData.useCustomSprite && defaultData.customSprite != null)
            {
                image.sprite = defaultData.customSprite;
                return true;
            }

            if (defaultData.noDefaultSprite)
            {
                image.sprite = null;
                return true;
            }

            if (defaultData.defaultSprite == null) return false;
            image.sprite = defaultData.defaultSprite;

            return true;
        }

        private void TrySetCustomData(Image image)
        {
            var customMenuItemDataDic = CustomMenuItemTree.Instance.CustomMenuItemDataDic;

            if (!customMenuItemDataDic.TryGetValue(customData, out var data))
            {
                customData = "";
                return;
            }

            image.color = data.colour;

            if (data.spriteSize != Vector2.zero)
            {
                image.GetComponent<RectTransform>().sizeDelta = data.spriteSize;
            }

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