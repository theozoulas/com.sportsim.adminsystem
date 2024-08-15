using UnityEngine;
using UnityEngine.UI;

namespace MenuComponents.DataInput
{
    public class ToggleValidationSprite : MonoBehaviour
    {
        [SerializeField] private Sprite errorSprite;
        
        private Image _image;
        private Sprite _defaultSprite;
        
        private void Awake()
        {
            _image = transform.GetComponent<Image>();
            _defaultSprite = _image.sprite;
        }
        
        /// <summary>
        /// Toggle error sprite if not valid or sprite will be reset to default.
        /// </summary>
        /// <param name="valid"></param>
        public void ToggleSprite(bool valid)
        {
            _image.sprite = valid ? _defaultSprite : errorSprite;
        }
    }
}
