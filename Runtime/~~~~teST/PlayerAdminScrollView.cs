using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MenuComponents.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAdminScrollView : MonoBehaviour
{
    [SerializeField] private GameObject playerDataEntryPrefab;

    private RectTransform rect;
    private float entryHeight;


    private void Awake()
    {
        rect = transform.GetComponent<RectTransform>();
        entryHeight = playerDataEntryPrefab.GetComponent<RectTransform>().sizeDelta.y;
    }

    private void Start()
    {
        UpdateScrollView();
    }

    public void UpdateScrollView()
    {
        Clear();

        var playerDataList = SaveManager.LoadPlayerData();

        if (playerDataList == null) return;

        playerDataList = playerDataList.OrderByDescending(p => p.Score).ToList();

        var sizeDelta = rect.sizeDelta;
        sizeDelta = new Vector2(sizeDelta.x, entryHeight * playerDataList.Count);
        rect.sizeDelta = sizeDelta;

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
    }

    private static void FillEntry(Transform entry, PlayerData playerData)
    {
        entry.GetChild(0).GetComponent<TextMeshProUGUI>().text
            = playerData.GetDataFieldValueByName("Name");
        
        
        entry.GetChild(2).GetComponent<TextMeshProUGUI>().text
            = playerData.NumberOfPlays.ToString();

        entry.GetChild(3).GetComponent<TextMeshProUGUI>().text
            = playerData.TimeRegistered;
        
        entry.GetChild(4).GetComponent<TextMeshProUGUI>().text
            = playerData.Score.ToString(CultureInfo.InvariantCulture);
    }
}