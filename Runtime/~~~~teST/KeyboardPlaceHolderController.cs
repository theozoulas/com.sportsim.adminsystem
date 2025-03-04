using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MenuComponents.Components.Keyboard;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class KeyboardPlaceHolderController : MonoBehaviour
{
    [SerializeField] private TMP_InputField firstSelectedInput;

    [SerializeField] private NavigationUpDown navigationUpDown;


    private void Awake()
    {
        GetComponent<Image>().enabled = false;
    }

    private void Start()
    {
        StaticManager.SetFirstInputSelected(firstSelectedInput);
        StaticManager.Keyboard.SetNavigationUpDown(navigationUpDown);
        StaticManager.Keyboard.SetSpawnPosition(transform.position);
    }
}