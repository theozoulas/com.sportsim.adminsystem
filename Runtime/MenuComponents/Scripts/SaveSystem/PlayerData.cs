using System;
using System.Linq;

namespace MenuComponents.SaveSystem
{
    /// <summary>
    /// Class <c>PlayerData</c> Used to store player data.
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        public DataFieldValue[] DataFieldValues { get; }
        public string TimeRegistered { get; }
        public int Score { get; set; }

        /// <summary>
        /// Constructor <c>PlayerData</c> Constructs player data with passed params. 
        /// </summary>
        /// <param name="dataFieldValues"></param>
        /// <param name="timeRegistered"></param>
        public PlayerData(DataFieldValue[] dataFieldValues, string timeRegistered)
        {
            DataFieldValues = dataFieldValues;
            TimeRegistered = timeRegistered;
        }

        /// <summary>
        /// Get Method <c>GetDataFieldValueByName</c> Get Data Field Value by the name passed.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Returns data value as <c>string</c></returns>
        public string GetDataFieldValueByName(string name)
        {
            return DataFieldValues.FirstOrDefault(d => d.dataName.Equals(name))?.dataValue;
        }
    }
}