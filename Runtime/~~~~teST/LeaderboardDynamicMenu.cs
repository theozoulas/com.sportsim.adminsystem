using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

[GlobalConfig("Assets/Resources/AdminSystem/ConfigFiles/")]
public class LeaderboardDynamicMenu : GlobalConfig<LeaderboardDynamicMenu>
{
    

    [EnumToggleButtons] public LeaderboardDataProviders dataProviderType;

    [ShowIf("dataProviderType", LeaderboardDataProviders.Online)]
    public string url;

    public string[] extraDataEntriesToShow;
}

public enum LeaderboardDataProviders
{
    SaveSystem,
    Online
}