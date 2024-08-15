using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MenuComponents.DynamicSystem
{
    [CreateAssetMenu(fileName = "ColourPallet", menuName = "ScriptableObjects/Colour Pallet", order = 1)]
    public class ColourDynamicData : BaseDynamicData
    {
        public Color colour;
    }
}