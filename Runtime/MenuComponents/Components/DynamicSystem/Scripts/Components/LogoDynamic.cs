#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MenuComponents.DynamicSystem
{
    public class LogoDynamic : MonoBehaviour
    {
        [FormerlySerializedAs("logoDynamicDataSo")] [FormerlySerializedAs("logoDyanmicDataSo")] [FormerlySerializedAs("logoDataScriptableObject")] [FormerlySerializedAs("logoData")] [SerializeField] private LogoDynamicData logoDynamicData;

        public void OnValidate()
        {
            if(logoDynamicData == null) return;
        
            GetComponent<Image>().sprite = logoDynamicData.logo;
        }
    }
}
#endif
