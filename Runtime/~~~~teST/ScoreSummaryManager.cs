using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MenuComponents.SaveSystem.SaveManager;

public class ScoreSummaryManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    
    [SerializeField] private TMP_Text reactionSpeedText;


    private void Start()
    {
        scoreText.text = StaticScoreData.GetAsStringFormatted();
        
        if(!TryGetExtraScoreFromId("ReactionSpeed", out var reactionSpeed)) return;
        
        Debug.Log(reactionSpeed.Value);

        reactionSpeedText.text = reactionSpeed.GetAsStringFormatted();
    }
}
