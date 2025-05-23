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

        [Title("Dynamic Settings")] [EnumToggleButtons] [HideLabel]
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
            DisableFontStyle = 1 << 5,
            DisableFontCase = 1 << 6,
            DisableSpacing = 1 << 7,
            DisableAlignment = 1 << 8,

            DisableAll = DisableColour |
                         DisableFont |
                         DisableSize |
                         DisableFontStyle |
                         DisableFontCase |
                         DisableSpacing |
                         DisableAlignment
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
                text.color = defaultData.colour;

            if (!disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableAlignment))
                text.alignment = GetAlignment(defaultData.alignment, defaultData.middleAlignmentOptions);

            if (!disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableFontStyle))
                text.fontStyle = GetFontStyle(defaultData.fontStyle, defaultData.fontCase);

            if (!disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableSpacing))
            {
                var spacingOptions = defaultData.spacingOptions;

                text.characterSpacing = spacingOptions.character;
                text.lineSpacing = spacingOptions.line;
                text.wordSpacing = spacingOptions.word;
                text.paragraphSpacing = spacingOptions.paragraph;
            }

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

        private void TrySetCustomData(TMP_Text text)
        {
            var customTextItemDataDic = CustomTextItemTree.Instance.CustomTextItemDataDic;

            if (!customTextItemDataDic.TryGetValue(customData, out var data))
            {
                customData = "";
                return;
            }

            if (!disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableColour))
                text.color = data.colour;

            if (!disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableAlignment))
                text.alignment = GetAlignment(data.alignment, data.middleAlignmentOptions);

            if (!disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableFontStyle))
                text.fontStyle = GetFontStyle(data.fontStyle, data.fontCase);

            if (!disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableSpacing))
            {
                var spacingOptions = data.spacingOptions;

                text.characterSpacing = spacingOptions.character;
                text.lineSpacing = spacingOptions.line;
                text.wordSpacing = spacingOptions.word;
                text.paragraphSpacing = spacingOptions.paragraph;
            }

            if (!disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableSize))
            {
                if (data.fontSize > 0)
                {
                    text.fontSize = data.fontSize;
                }

                if (data.autoSize)
                {
                    text.enableAutoSizing = data.autoSize;
                    text.fontSizeMin = data.autoSizeData.minSize;
                    text.fontSizeMax = data.autoSizeData.maxSize;
                }
            }

            if (disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableFont)) return;

            if (data.useCustomFont && data.customFont != null)
            {
                text.font = data.customFont;
                return;
            }

            if (data.defaultFont == null) return;
            text.font = data.defaultFont;
        }

        private TextAlignmentOptions GetAlignment(Alignment alignment, MiddleAlignment middleAlignment)
        {
            return alignment switch
            {
                Alignment.Left when middleAlignment == MiddleAlignment.Top =>
                    TextAlignmentOptions.TopLeft,
                Alignment.Left when middleAlignment == MiddleAlignment.Middle =>
                    TextAlignmentOptions.Left,
                Alignment.Left when middleAlignment == MiddleAlignment.Bottom =>
                    TextAlignmentOptions.BottomLeft,
                Alignment.Left when middleAlignment == MiddleAlignment.Baseline =>
                    TextAlignmentOptions.BaselineLeft,
                Alignment.Left when middleAlignment == MiddleAlignment.Midline =>
                    TextAlignmentOptions.MidlineLeft,
                Alignment.Left when middleAlignment == MiddleAlignment.Capline =>
                    TextAlignmentOptions.CaplineLeft,

                Alignment.Center when middleAlignment == MiddleAlignment.Top =>
                    TextAlignmentOptions.Top,
                Alignment.Center when middleAlignment == MiddleAlignment.Middle =>
                    TextAlignmentOptions.Center,
                Alignment.Center when middleAlignment == MiddleAlignment.Bottom =>
                    TextAlignmentOptions.Bottom,
                Alignment.Center when middleAlignment == MiddleAlignment.Baseline =>
                    TextAlignmentOptions.Baseline,
                Alignment.Center when middleAlignment == MiddleAlignment.Midline =>
                    TextAlignmentOptions.Midline,
                Alignment.Center when middleAlignment == MiddleAlignment.Capline =>
                    TextAlignmentOptions.Capline,

                Alignment.Right when middleAlignment == MiddleAlignment.Top =>
                    TextAlignmentOptions.TopRight,
                Alignment.Right when middleAlignment == MiddleAlignment.Middle =>
                    TextAlignmentOptions.Right,
                Alignment.Right when middleAlignment == MiddleAlignment.Bottom =>
                    TextAlignmentOptions.BottomRight,
                Alignment.Right when middleAlignment == MiddleAlignment.Baseline =>
                    TextAlignmentOptions.BaselineRight,
                Alignment.Right when middleAlignment == MiddleAlignment.Midline =>
                    TextAlignmentOptions.MidlineRight,
                Alignment.Right when middleAlignment == MiddleAlignment.Capline =>
                    TextAlignmentOptions.CaplineRight,

                Alignment.Justified when middleAlignment == MiddleAlignment.Top =>
                    TextAlignmentOptions.TopJustified,
                Alignment.Justified when middleAlignment == MiddleAlignment.Middle =>
                    TextAlignmentOptions.Justified,
                Alignment.Justified when middleAlignment == MiddleAlignment.Bottom =>
                    TextAlignmentOptions.BottomJustified,
                Alignment.Justified when middleAlignment == MiddleAlignment.Baseline =>
                    TextAlignmentOptions.BaselineJustified,
                Alignment.Justified when middleAlignment == MiddleAlignment.Midline =>
                    TextAlignmentOptions.MidlineJustified,
                Alignment.Justified when middleAlignment == MiddleAlignment.Capline =>
                    TextAlignmentOptions.CaplineJustified,

                Alignment.Flush when middleAlignment == MiddleAlignment.Top =>
                    TextAlignmentOptions.TopFlush,
                Alignment.Flush when middleAlignment == MiddleAlignment.Middle =>
                    TextAlignmentOptions.Flush,
                Alignment.Flush when middleAlignment == MiddleAlignment.Bottom =>
                    TextAlignmentOptions.BottomFlush,
                Alignment.Flush when middleAlignment == MiddleAlignment.Baseline =>
                    TextAlignmentOptions.BaselineFlush,
                Alignment.Flush when middleAlignment == MiddleAlignment.Midline =>
                    TextAlignmentOptions.MidlineFlush,
                Alignment.Flush when middleAlignment == MiddleAlignment.Capline =>
                    TextAlignmentOptions.CaplineFlush,

                Alignment.GeometryCenter when middleAlignment == MiddleAlignment.Top =>
                    TextAlignmentOptions.TopGeoAligned,
                Alignment.GeometryCenter when middleAlignment == MiddleAlignment.Middle =>
                    TextAlignmentOptions.CenterGeoAligned,
                Alignment.GeometryCenter when middleAlignment == MiddleAlignment.Bottom =>
                    TextAlignmentOptions.BottomGeoAligned,
                Alignment.GeometryCenter when middleAlignment == MiddleAlignment.Baseline =>
                    TextAlignmentOptions.BaselineGeoAligned,
                Alignment.GeometryCenter when middleAlignment == MiddleAlignment.Midline =>
                    TextAlignmentOptions.MidlineGeoAligned,
                Alignment.GeometryCenter when middleAlignment == MiddleAlignment.Capline =>
                    TextAlignmentOptions.CaplineGeoAligned,

                _ => TextAlignmentOptions.Center
            };
        }

        private TMPro.FontStyles GetFontStyle(FontStyles fontStyle, FontCases fontCase)
        {
            var tempFontStyle = TMPro.FontStyles.Normal;

            if (fontStyle.HasFlag(FontStyles.Bold)) tempFontStyle |= TMPro.FontStyles.Bold;
            if (fontStyle.HasFlag(FontStyles.Italic)) tempFontStyle |= TMPro.FontStyles.Italic;
            if (fontStyle.HasFlag(FontStyles.Strikethrough)) tempFontStyle |= TMPro.FontStyles.Strikethrough;
            if (fontStyle.HasFlag(FontStyles.Underline)) tempFontStyle |= TMPro.FontStyles.Underline;

            if (disableTextDynamicFeatures.HasFlag(DisableTextDynamicFeatureOptions.DisableFontCase)) return tempFontStyle;

            switch (fontCase)
            {
                case FontCases.Default:
                    break;
                case FontCases.Lowercase:
                    tempFontStyle |= TMPro.FontStyles.LowerCase;
                    break;
                case FontCases.Uppercase:
                    tempFontStyle |= TMPro.FontStyles.UpperCase;
                    break;
                case FontCases.Smallcaps:
                    tempFontStyle |= TMPro.FontStyles.SmallCaps;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return tempFontStyle;
        }
    }
}
#endif