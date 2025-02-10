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
        public int NumberOfPlays { get; set; }
        public float Score { get; set; }

        public string ScoreFormatted { get; set; }
        public Guid Guid { get; private set; }

        /// <summary>
        /// Constructor <c>PlayerData</c> Constructs player data with passed params. 
        /// </summary>
        /// <param name="dataFieldValues"></param>
        /// <param name="timeRegistered"></param>
        public PlayerData(DataFieldValue[] dataFieldValues, string timeRegistered)
        {
            DataFieldValues = dataFieldValues;
            TimeRegistered = timeRegistered;
            Guid = Guid.NewGuid();
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

        /// <summary>
        /// TryGet Method <c>TryGetDataFieldValueByName</c> Try Get Data Field Value by the name passed.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="result"></param>
        /// <returns>Returns true if found data field by name as <c>bool</c></returns>
        public bool TryGetDataFieldValueByName(string name, out string result)
        {
            result = DataFieldValues.FirstOrDefault(d => d.dataName.Equals(name))?.dataValue;
            
            return result != null;
        }
    }
}