using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] protected int maxTime = 20;

    private int _time;

    private float _clockRateStartTime;
    
    private readonly WaitForFixedUpdate _waitForFixedUpdate = new();

    public static Action<int> TimeTick; 

    
    private void Awake()
    {
        GameManager.GameStart += StartTimer;
    }

    private void OnDestroy()
    {
        GameManager.GameStart -= StartTimer;
    }

    protected virtual void Start()
    {
        _time = maxTime;
        
        timerText.text = $"{_time / 60:00}:{_time % 60:00}";
    }

    private void StartTimer()
    {
        _clockRateStartTime = Time.time;
        _time--;
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            if (Time.time - _clockRateStartTime >= 1f)
            {
                timerText.text = $"{_time / 60:00}:{_time % 60:00}";
                _clockRateStartTime = Time.time;
                _time--;

                TimeTick(_time);

                if (_time < 0)
                {
                    GameManager.EndGame();
                    break;
                }
            }

            yield return _waitForFixedUpdate;
        }
    }
}