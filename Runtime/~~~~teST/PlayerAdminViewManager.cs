using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MenuComponents.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class PlayerAdminViewManager : MonoBehaviour
{
    [SerializeField] private GameObject playerDataEntryPrefab;
    [SerializeField] private GameObject[] extraFieldTitles;

    private RectTransform _rectTransform;
    private float _entryHeight;


    private void Awake()
    {
        _rectTransform = transform.GetComponent<RectTransform>();
        _entryHeight = playerDataEntryPrefab.GetComponent<RectTransform>().sizeDelta.y;
    }

    private void Start()
    {
        UpdateScrollView();
    }

    private void UpdateScrollView()
    {
        Clear();

        var playerDataList = SaveManager.LoadPlayerData();

        if (playerDataList == null) return;

        playerDataList = playerDataList.OrderByDescending(p => p.Score).ToList();

        var sizeDelta = _rectTransform.sizeDelta;
        sizeDelta = new Vector2(sizeDelta.x, _entryHeight * playerDataList.Count);
        _rectTransform.sizeDelta = sizeDelta;

        foreach (var playerData in playerDataList)
        {
            var entry = Instantiate(playerDataEntryPrefab, transform);

            FillEntry(entry.transform, playerData);
        }
    }

    private void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var extraFieldTitle in extraFieldTitles)
        {
            extraFieldTitle.SetActive(false);
        }
    }

    private void FillEntry(Transform entry, PlayerData playerData)
    {
        entry.GetChild(0).GetComponent<TextMeshProUGUI>().text
            = playerData.GetDataFieldValueByName("Name");

        var extraFields = GetExtraFields(playerData).ToArray();
        
        for (var i = 0; i < extraFields.Length; i++)
        {
            entry.GetChild(1 + i).GetComponent<TextMeshProUGUI>().text
                = extraFields[i];
            
            extraFieldTitles[i].SetActive(true);
        }
        
        entry.GetChild(3).GetComponent<TextMeshProUGUI>().text
            = playerData.NumberOfPlays.ToString();

        entry.GetChild(4).GetComponent<TextMeshProUGUI>().text
            = playerData.TimeRegistered;

        entry.GetChild(5).GetComponent<TextMeshProUGUI>().text
            = playerData.ScoreFormatted;
    }

    private static IEnumerable<string> GetExtraFields(PlayerData playerData)
    {
        var extraFieldNames = DataEntryDynamicMenu.Instance.GetExtraFields();

        foreach (var fieldName in extraFieldNames)
        {
            var foundExtraField
                = playerData.TryGetDataFieldValueByName(fieldName, out var extraField);
        
            if(foundExtraField) yield return extraField;
        }
    }
}