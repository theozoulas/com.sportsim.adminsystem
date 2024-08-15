using System;
using System.Globalization;
using System.Linq;
using MenuComponents.SaveSystem;
using MenuComponents.Utility;
using UnityEngine;

namespace MenuComponents.DataInput
{
    /// <summary>
    /// Class <c>PlayerDataInputManager</c> Manages player's data input.
    /// </summary>
    public class PlayerDataInputManager : MonoBehaviour
    {
        [SerializeField] private DataField[] dataFields;
        
        [SerializeField] private Validator[] otherValidators;
        
        /// <summary>
        /// Method <c>LoopThroughInputs</c> Loops through all the inputs validating them.
        /// </summary>
        public void LoopThroughInputs()
        {
            if(ValidateDataFields()) CreatePlayerDataAndStartGame();
        }

        private bool ValidateDataFields()
        {
            var valid = dataFields.Length > 0;
            
            foreach (var dataField in dataFields)
            {
                if (!dataField.Validate()) valid = false;
            }
            
            foreach (var otherValidator in otherValidators)
            {
                if (!otherValidator.Validate()) valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Method <c>CreatePlayerDataAndStartGame</c> Create player data with player's input
        /// and then start the game.
        /// </summary>
        private void CreatePlayerDataAndStartGame()
        {
            var timeRegistered = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            var dataFieldValues =
                dataFields.Select(dataField => new DataFieldValue(dataField.DataName, dataField.DataValue));

            var newPlayerData = new PlayerData(
                dataFieldValues.ToArray(),
                timeRegistered);
            
            SaveManager.currentPlayerData = newPlayerData;
            
            StartGame();
        }
        
        /// <summary>
        /// Method <c>StartGame</c> Starts the game.
        /// </summary>
        private static void StartGame()
        {
            SceneChangeManager.Instance.ChangeToSceneForward();
        }
    }
}