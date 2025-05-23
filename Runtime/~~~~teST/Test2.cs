using System;
using System.Collections;
using System.Collections.Generic;
using MenuComponents.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Test2 : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener( () =>
        {
            SaveManager.UpdateStaticScore(scoreManager.Score);
            SaveManager.UpdateExtraScoreData("Reaction Speed", 30f /scoreManager.Score);
            SceneManager.LoadScene(gameObject.scene.buildIndex + 1);
        });
    }
}
