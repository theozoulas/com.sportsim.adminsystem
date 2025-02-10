using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action GameStart = () => { };
    public static event Action GameOver = () => { };


    public static void StartGame()
    {
        GameStart();
        GameMusic.instance.PlayMusic();
    }

    public static void EndGame()
    {
        GameOver();
    }
}
