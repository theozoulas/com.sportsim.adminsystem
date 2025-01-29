using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;


public class LeaderboardDynamicMenu : GlobalConfig<LeaderboardDynamicMenu>
{
    public enum LeaderboardDataProviders
    {
        SaveSystem,
        Online
    }

    [EnumToggleButtons] public LeaderboardDataProviders dataProviderType;

    [ShowIf("dataProviderType", LeaderboardDataProviders.Online)]
    public string url;

    public string[] extraDataEntriesToShow;
}