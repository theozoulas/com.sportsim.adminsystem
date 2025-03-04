using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TermsConditionsButton : MonoBehaviour
{
    [SerializeField] private GameObject termsConditionsPanel;

    public Button Button => GetComponent<Button>(); 

        
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ShowPanel);
    }

    private void ShowPanel()
    {
        termsConditionsPanel.SetActive(true);
    }
}