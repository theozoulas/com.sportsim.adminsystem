
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
#if UNITY_EDITOR
        [ValueDropdown("DefaultMenuItemDataKeys")]
        public string defaultDataKey;

        [ShowIf("CustomDataExits")] public bool useCustomData;

        [ShowIf("@this.useCustomData && this.CustomDataExits")] [ValueDropdown("CustomMenuItemDataKeys")]
        public string customData;

        [Title("Dynamic Settings")] [EnumToggleButtons] [HideLabel]
        public DisableImageDynamicFeatureOptions disableImageDynamicFeatures;

        private IEnumerable<string> DefaultMenuItemDataKeys
            => DefaultMenuItemTree.Instance.defaultItemData.Select(cd => cd.key);

        private IEnumerable<string> CustomMenuItemDataKeys
            => CustomMenuItemTree.Instance.customMenuItemDynamicData.Select(cd => cd.key);

        private bool CustomDataExits => CustomMenuItemDataKeys.Any();

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
            if (defaultDataKey == null) return;

            if (!CustomMenuItemDataKeys.Any()) customData = "";

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
                image.color = defaultData.colour;

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

            if (!disableImageDynamicFeatures.HasFlag(DisableImageDynamicFeatureOptions.DisableColour))
                image.color = data.colour;

            var rectTransform = image.GetComponent<RectTransform>();

            if (!disableImageDynamicFeatures.HasFlag(DisableImageDynamicFeatureOptions.DisableSize))
            {
                if (data.spriteSize != Vector2.zero &&
                    (data.useDefaultSprite || (data.useCustomSprite && data.customSprite != null)))
                {
                    rectTransform.sizeDelta = data.spriteSize;
                }

                if (data.useCustomSize)
                {
                    rectTransform.sizeDelta = data.customSpriteSize;
                }
            }

            if (disableImageDynamicFeatures.HasFlag(DisableImageDynamicFeatureOptions.DisableSprite)) return;

            if (data.useCustomSprite && data.customSprite != null)
            {
                image.sprite = data.customSprite;
                return;
            }

            if (!data.useDefaultSprite || data.defaultSprite == null) return;
            image.sprite = data.defaultSprite;
        }
#endif   
    }
}
