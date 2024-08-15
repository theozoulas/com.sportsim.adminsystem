using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MenuComponents.Utility
{
    /// <summary>
    /// Class <c>InitializeButtonAudio</c> Used to assign button click to all button in a scene
    /// without the tag "NoSound".
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class InitializeButtonAudio : MonoBehaviour
    {
        private AudioSource _audioSource;
        
        private int CurrentBuildIndex => gameObject.scene.buildIndex;
        
        public float AudioLength { get; private set; }

        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            AudioLength = _audioSource.clip.length;

            var allButtons =
                Resources.FindObjectsOfTypeAll<Selectable>()
                    .Where
                    (res => res.gameObject.scene.buildIndex == CurrentBuildIndex &&
                            !res.CompareTag("NoSound"))
                    .ToArray();
            
            FindAllSelectables(allButtons);
        }

        /// <summary>
        /// Method <c>FindAllSelectables</c> Find all selectables int the scene and check.
        /// </summary>
        /// <param name="allButtons"></param>
        private void FindAllSelectables(IEnumerable<Selectable> allButtons)
        {
            foreach (var selectable in allButtons)
            {
                CheckSelectableComponent(selectable);
            }
        }

        /// <summary>
        /// Method <c>CheckSelectableComponent</c> Check the passed component and assign appropriate callback.
        /// </summary>
        /// <param name="selectable"></param>
        private void CheckSelectableComponent(Component selectable)
        {
            if (selectable.TryGetComponent(typeof(Button), out var component))
            {
                component.GetComponent<Button>()
                    .onClick.AddListener(PlayButtonClick);
            }
            else if (selectable.TryGetComponent(typeof(Toggle), out component))
            {
                component.GetComponent<Toggle>()
                    .onValueChanged.AddListener(delegate { PlayButtonClick(); });
            }
            else if (selectable.TryGetComponent(typeof(TMP_InputField), out component))
            {
                component.GetComponent<TMP_InputField>()
                    .onSelect.AddListener(delegate { PlayButtonClick(); });
            }
            else if (selectable.TryGetComponent(typeof(TMP_Dropdown), out component))
            {
                SetupDropDownTrigger(component);
            }
        }

        /// <summary>
        /// Method <c>SetupDropDownTrigger</c> For setting up dropdown trigger.
        /// </summary>
        /// <param name="component"></param>
        private void SetupDropDownTrigger(Component component)
        {
            if (component.transform.GetComponent<EventTrigger>() != null)
            {
                var eventTrigger = component.transform.GetComponent<EventTrigger>();
                var entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerClick
                };
                
                entry.callback.AddListener(delegate { PlayButtonClick(); });
                eventTrigger.triggers.Add(entry);
            }
            
            component.GetComponent<TMP_Dropdown>()
                .onValueChanged.AddListener(delegate { PlayButtonClick(); });
        }

        /// <summary>
        /// Method <c>PlayButtonClick</c> Play button click audio clip.
        /// </summary>
        private void PlayButtonClick() => _audioSource.Play();
    }
}
