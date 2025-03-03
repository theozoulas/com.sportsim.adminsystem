#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MenuComponents.DynamicSystem
{
    public class TextDynamicController : MonoBehaviour
    {
        [ValueDropdown("DefaultTextDataKeys")] public string defaultDataKey;

        [ShowIf("CustomDataExits")] public bool useCustomData;

        [ShowIf("@this.useCustomData && this.CustomDataExits")] [ValueDropdown("CustomTextDataKeys")]
        public string customData;

        [Title("Bitmask Enum")] [EnumToggleButtons] [HideLabel]
        public DisableTextDynamicFeatureOptions disableTextDynamicFeatures;

        private IEnumerable<string> DefaultTextDataKeys
            => DefaultTextItemTree.Instance.defaultItemData.Select(cd => cd.key);

        private IEnumerable<string> CustomTextDataKeys
            => CustomMenuItemTree.Instance.customMenuItemDynamicData.Select(cd => cd.key);

        private bool CustomDataExits => CustomTextDataKeys.Any();

        [Flags]
        public enum DisableTextDynamicFeatureOptions
        {
            DisableColour = 1 << 1,
            DisableFont = 1 << 2,
            DisableSize = 1 << 3,
            DisableMiddleAlignment = 1 << 4,
            DisableAll = DisableColour | DisableFont | DisableSize | DisableMiddleAlignment
        }

        public void OnValidate()
        {
            if (!CustomTextDataKeys.Any()) customData = "";

            var text = GetComponent<TMP_Text>();

            if (!TrySetDefaultData(text)) TrySetCustomData(text);
        }

        private bool TrySetDefaultData(TMP_Text text)
        {
            if (useCustomData && customData != "") return false;

            var defaultMenuItemDataDic = DefaultTextItemTree.Instance.DefaultItemDataDic;

            if (!defaultMenuItemDataDic.TryGetValue(defaultDataKey, out var defaultData))
            {
                defaultDataKey = "";
                return false;
            }

            if (!disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableColour))
                text.color = defaultData.color;

            if (!disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableMiddleAlignment))
                text.alignment = GetMiddleAlignment(defaultData);

            if (!disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableSize))
            {
                if (defaultData.fontSize > 0)
                {
                    text.fontSize = defaultData.fontSize;
                }

                if (defaultData.autoSize)
                {
                    text.enableAutoSizing = defaultData.autoSize;
                    text.fontSizeMin = defaultData.autoSizeData.minSize;
                    text.fontSizeMax = defaultData.autoSizeData.maxSize;
                }
            }

            if (disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableFont)) return true;

            if (defaultData.useCustomFont && defaultData.customFont != null)
            {
                text.font = defaultData.customFont;
                return true;
            }

            if (defaultData.defaultFont == null) return false;
            text.font = defaultData.defaultFont;

            return true;
        }

        private static TextAlignmentOptions GetMiddleAlignment(TextItemData data)
        {
            return data.middleAlignmentOptions switch
            {
                TextItemData.MiddleAlignment.Middle => TextAlignmentOptions.Center,
                TextItemData.MiddleAlignment.Baseline => TextAlignmentOptions.Baseline,
                TextItemData.MiddleAlignment.Midline => TextAlignmentOptions.Midline,
                TextItemData.MiddleAlignment.Capline => TextAlignmentOptions.Capline,
                _ => TextAlignmentOptions.Center
            };
        }

        private void TrySetCustomData(TMP_Text image)
        {
            /*var customMenuItemDataDic = CustomMenuItemTree.Instance.CustomMenuItemDataDic;

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
            image.sprite = data.defaultSprite;*/
        }
    }
}
#endif