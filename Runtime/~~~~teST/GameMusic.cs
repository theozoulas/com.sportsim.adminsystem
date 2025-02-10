using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameMusic : MonoBehaviour
{
    [SerializeField] private float fadeOutSpeed = 0.1f;

    public static GameMusic instance;

    private AudioSource audioSource;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);

        audioSource = GetComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    public void PlayMusic()
    {
        audioSource.Play();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutMusic());
    }

    private IEnumerator FadeOutMusic()
    {
        var startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeOutSpeed;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}