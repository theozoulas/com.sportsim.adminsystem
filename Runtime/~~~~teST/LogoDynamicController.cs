#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LogoDynamicController : MonoBehaviour
{
    [ValueDropdown("DefaultLogoDataKeys")] public string defaultDataKey;

    [ShowIf("CustomDataExits")] public bool useCustomData;

    [ShowIf("@this.useCustomData && this.CustomDataExits")] [ValueDropdown("CustomLogoDataKeys")]
    public string customData;

    [Title("Dynamic Settings")] [EnumToggleButtons] [HideLabel]
    public DisableImageDynamicFeatureOptions disableImageDynamicFeatures;

    private IEnumerable<string> DefaultLogoDataKeys
        => DefaultLogoTree.Instance.defaultLogoData.Select(cd => cd.key);

    private IEnumerable<string> CustomLogoDataKeys
        => CustomLogoTree.Instance.customLogoDynamicData.Select(cd => cd.key);

    private bool CustomDataExits => CustomLogoDataKeys.Any();

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

        if (!CustomLogoDataKeys.Any()) customData = "";

        var image = GetComponent<Image>();

        if (!TrySetDefaultData(image)) TrySetCustomData(image);
    }

    private bool TrySetDefaultData(Image image)
    {
        if (useCustomData && customData != "") return false;

        var defaultMenuItemDataDic = DefaultLogoTree.Instance.DefaultLogoDataDic;

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
            if (defaultData.logoSize != Vector2.zero)
            {
                rectTransform.sizeDelta = defaultData.logoSize;
            }

            if (defaultData.useCustomSize)
            {
                rectTransform.sizeDelta = defaultData.customLogoSize;
            }
        }

        if (disableImageDynamicFeatures.HasFlag(DisableImageDynamicFeatureOptions.DisableSprite)) return true;

        if (defaultData.useCustomLogo && defaultData.customLogo != null)
        {
            image.sprite = defaultData.customLogo;
            return true;
        }

        if (defaultData.defaultLogo == null) return false;
        image.sprite = defaultData.defaultLogo;

        return true;
    }
    
    private void TrySetCustomData(Image image)
    {
        var customLogoDataDic = CustomLogoTree.Instance.CustomLogoDataDic;

        if (!customLogoDataDic.TryGetValue(customData, out var data))
        {
            customData = "";
            return;
        }

        if (!disableImageDynamicFeatures.HasFlag(DisableImageDynamicFeatureOptions.DisableColour))
            image.color = data.colour;

        var rectTransform = image.GetComponent<RectTransform>();

        if (!disableImageDynamicFeatures.HasFlag(DisableImageDynamicFeatureOptions.DisableSize))
        {
            if (data.logoSize != Vector2.zero &&
                (data.useDefaultLogo || (data.useCustomLogo && data.customLogo != null)))
            {
                rectTransform.sizeDelta = data.logoSize;
            }

            if (data.useCustomSize)
            {
                rectTransform.sizeDelta = data.customLogoSize;
            }
        }

        if (disableImageDynamicFeatures.HasFlag(DisableImageDynamicFeatureOptions.DisableSprite)) return;

        if (data.useCustomLogo && data.customLogo != null)
        {
            image.sprite = data.customLogo;
            return;
        }

        if (!data.useDefaultLogo || data.defaultLogo == null) return;
        image.sprite = data.defaultLogo;
    }
}
#endif