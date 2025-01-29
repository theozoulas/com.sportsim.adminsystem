using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using MenuComponents.DataInput;
using UnityEngine;
using UnityEngine.UI;

public class ToggleValidatorManager : Validator
{
    [SerializeField] private bool mandatory;
    
    [SerializeField] private GameObject documentPagePrefab;
    [SerializeField] private GameObject documentPanel;
    [SerializeField] private Sprite[] documentPages;

    private Toggle _toggle;
    private ToggleValidationSprite _toggleValidationSprite;

    public bool IsOn => _toggle.isOn;
    
    
    private void Awake()
    {
        _toggle = GetComponentInChildren<Toggle>();
        _toggleValidationSprite = GetComponentInChildren<ToggleValidationSprite>();
        
        _toggle.onValueChanged.AddListener(OnToggleValueChange);
    }

    /// <summary>
    /// Editor Button to fill documents pages.
    /// </summary>
    [Button(ButtonSizes.Medium)]
    private void FillDocumentPages()
    {
        documentPanel.GetComponent<AddPagesToPanel>().AddPages(documentPages, documentPagePrefab);
    }

    /// <summary>
    /// Toggle Sprite on Toggle Value Change.
    /// </summary>
    /// <param name="isOn"></param>
    private void OnToggleValueChange(bool isOn)
    {
        if(!mandatory) return;

        _toggleValidationSprite.ToggleSprite(isOn);
    }

    /// <summary>
    /// Toggle Validation.
    /// </summary>
    /// <returns></returns>
    public override bool Validate()
    {
        return !mandatory || _toggle.isOn;
    }
}
