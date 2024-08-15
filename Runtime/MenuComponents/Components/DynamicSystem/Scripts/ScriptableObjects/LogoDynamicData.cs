#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MenuComponents.DynamicSystem
{
    [CreateAssetMenu(fileName = "LogoData", menuName = "ScriptableObjects/Logo Data", order = 1)]  
    public class LogoDynamicData : BaseDynamicData
    {
        public Sprite logo;
    }
}
#endif

