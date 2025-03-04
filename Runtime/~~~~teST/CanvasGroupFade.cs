using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFade : MonoBehaviour
{
    [SerializeField] private bool hideAtStart;
    [SerializeField] private bool playAtStart;

    [SerializeField] private float fadeInDuration = 1;
    [SerializeField] private float fadeOutDuration = 1;
    [SerializeField] private float fadeShowDelay = 1;

    private CanvasGroup _canvasGroup;

    private WaitForSeconds _waitForFadeIn;
    private WaitForSeconds _waitForFadeOut;
    private WaitForSeconds _waitForShowDelay;


    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _waitForFadeIn = new WaitForSeconds(fadeInDuration);
        _waitForFadeOut = new WaitForSeconds(fadeOutDuration);
        _waitForShowDelay = new WaitForSeconds(fadeShowDelay);
    }

    private void Start()
    {
        if (hideAtStart) _canvasGroup.alpha = 0;
        if (playAtStart) StartCoroutine(FadeInOut());
    }

    /// <summary>
    /// Coroutine <c>FadeInOut</c> Routine to fade in and out.
    /// </summary>
    [UsedImplicitly]
    public IEnumerator FadeInOut()
    {
        _canvasGroup.alpha = 0;

        yield return StartCoroutine(FadeIn());
        yield return _waitForShowDelay;
        yield return StartCoroutine(FadeOut());
    }

    /// <summary>
    /// Coroutine <c>FadeIn</c> Fades Text in.
    /// </summary>
    [UsedImplicitly]
    public IEnumerator FadeIn()
    {
        yield return _canvasGroup.DOFade(1.0f, fadeInDuration);
        yield return _waitForFadeIn;
    }

    /// <summary>
    /// Coroutine <c>FadeOut</c> Fades Text out.
    /// </summary>
    [UsedImplicitly]
    public IEnumerator FadeOut()
    {
        yield return _canvasGroup.DOFade(0.0f, fadeOutDuration);
        yield return _waitForFadeOut;
    }
}