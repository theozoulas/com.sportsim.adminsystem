using TMPro;
using UnityEngine;

namespace MenuComponents.DynamicSystem
{
    [CreateAssetMenu(fileName = "FontData", menuName = "ScriptableObjects/Font Data", order = 1)] 
    public class FontDynamicData : BaseDynamicData
    {
        public TMP_FontAsset font;
    }
}
