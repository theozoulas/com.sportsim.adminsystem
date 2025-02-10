using System;
using System.Collections;
using System.Collections.Generic;
using MenuComponents.SaveSystem;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public TMP_Text scoreText;
    
    public float Score;

    private void Start()
    {
        SaveManager.ResetStaticScore();
    }

    public void AddToScore(int score)
    {
        Score += score;

        scoreText.text = Score.ToString();
    }
}
