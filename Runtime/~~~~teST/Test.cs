using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => scoreManager.AddToScore(1));
    }
}
