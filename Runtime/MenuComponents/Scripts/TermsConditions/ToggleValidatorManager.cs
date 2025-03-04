using System;
using System.Collections;
using System.Collections.Generic;
using MenuComponents.Components.Keyboard;
using Sirenix.OdinInspector;
using MenuComponents.DataInput;
using UnityEngine;
using UnityEngine.UI;

public class ToggleValidatorManager : Validator
{
    [SerializeField] private bool mandatory;

    [SerializeField] private Navigation navigation = new()
    {
        mode = Navigation.Mode.Explicit
    };

    [SerializeField] private bool useKeyboardAsOnSelectDown;
    
    [SerializeField] private GameObject documentPagePrefab;
    [SerializeField] private GameObject documentPanel;
    [SerializeField] private Sprite[] documentPages;

    private Toggle _toggle;
    private ToggleValidationSprite _toggleValidationSprite;

    private ControllerNavigationToggle _controllerNavigationToggle;

    private TermsConditionsButton _termsConditionsButton;

    public bool IsOn => _toggle.isOn;
    
    
    private void Awake()
    {
        _toggle = GetComponentInChildren<Toggle>();
        _toggleValidationSprite = GetComponentInChildren<ToggleValidationSprite>();
        _controllerNavigationToggle = GetComponentInChildren<ControllerNavigationToggle>();
        _termsConditionsButton = GetComponentInChildren<TermsConditionsButton>();
        
        _toggle.onValueChanged.AddListener(OnToggleValueChange);
    }

    private void Start()
    {
        ConstructNavigation();
    }

    private void ConstructNavigation()
    {
        var controllerNavigationButton = _controllerNavigationToggle.Button;
        var termsConditionsButton = _termsConditionsButton.Button;
        
        controllerNavigationButton.onClick.AddListener(SetToggle);
        
        var buttonNavigation = controllerNavigationButton.navigation;
        var termsConditionsNavigation = termsConditionsButton.navigation;
        
        buttonNavigation.selectOnLeft = navigation.selectOnLeft;
        buttonNavigation.selectOnDown = navigation.selectOnDown;
        buttonNavigation.selectOnUp = navigation.selectOnUp;
        
        termsConditionsNavigation.selectOnRight = navigation.selectOnLeft;
        termsConditionsNavigation.selectOnDown = navigation.selectOnDown;
        termsConditionsNavigation.selectOnUp = navigation.selectOnUp;

        controllerNavigationButton.navigation = buttonNavigation;
        termsConditionsButton.navigation = termsConditionsNavigation;
        
        if(!useKeyboardAsOnSelectDown) return;

        var topButton = StaticManager.Keyboard.TopButton;
        
        buttonNavigation.selectOnDown = topButton;
        termsConditionsNavigation.selectOnDown = topButton;
        
        controllerNavigationButton.navigation = buttonNavigation;
        termsConditionsButton.navigation = termsConditionsNavigation;
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
    /// Set Toggle using controller navigation button, so it can be used by either mouse, keyboard or controller. 
    /// </summary>
    private void SetToggle()
    {
        _toggle.isOn = !IsOn;
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
