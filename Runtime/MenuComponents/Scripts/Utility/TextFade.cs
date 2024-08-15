using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace MenuComponents.Utility
{
    /// <summary>
    /// Class <c>TextFade</c> Component to Fades TextMeshPro asset in and then out.
    /// </summary>
    public class TextFade : MonoBehaviour
    {
        [SerializeField] private bool hideAtStart;
        [SerializeField] private bool playAtStart;
    
        [SerializeField] private float fadeInDuration = 1;
        [SerializeField] private float fadeOutDuration = 1;
        [SerializeField] private float fadeShowDelay = 1;
    
        private TMP_Text _text;
        
        private WaitForSeconds _waitForFadeIn;
        private WaitForSeconds _waitForFadeOut;
        private WaitForSeconds _waitForShowDelay;


        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            
            _waitForFadeIn = new WaitForSeconds(fadeInDuration);
            _waitForFadeOut = new WaitForSeconds(fadeOutDuration);
            _waitForShowDelay = new WaitForSeconds(fadeShowDelay);
        }

        private void Start()
        {
            if(hideAtStart) _text.alpha = 0;
            if(playAtStart) StartCoroutine(FadeInOut());
        }

        /// <summary>
        /// Coroutine <c>FadeInOutSetText</c> Routine to fade in then out while
        /// displaying the text passed through.
        /// </summary>
        /// <param name="text"></param>
        public IEnumerator FadeInOutSetText(string text)
        {
            _text.SetText(text);
            
            yield return StartCoroutine(FadeInOut());
        }

        /// <summary>
        /// Coroutine <c>FadeInSetText</c> Only fades in the passed text.
        /// </summary>
        /// <param name="text"></param>
        [UsedImplicitly]
        public IEnumerator FadeInSetText(string text)
        {
            _text.SetText(text);
            
            yield return StartCoroutine(FadeIn());
        }

        /// <summary>
        /// Coroutine <c>FadeInOut</c> Routine to fade in and out.
        /// </summary>
        public IEnumerator FadeInOut()
        {
            _text.alpha = 0;
            
            yield return StartCoroutine(FadeIn());
            yield return _waitForShowDelay;
            yield return StartCoroutine(FadeOut());
        }

        /// <summary>
        /// Coroutine <c>FadeIn</c> Fades Text in.
        /// </summary>
        private IEnumerator FadeIn()
        {
            yield return _text.DOFade(1.0f, fadeInDuration);
            yield return _waitForFadeIn;
        }

        /// <summary>
        /// Coroutine <c>FadeOut</c> Fades Text out.
        /// </summary>
        private IEnumerator FadeOut()
        {
            yield return _text.DOFade(0.0f, fadeOutDuration);
            yield return _waitForFadeOut;
        }
    }
}
