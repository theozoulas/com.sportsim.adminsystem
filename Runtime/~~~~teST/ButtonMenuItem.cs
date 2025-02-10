using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Examples;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEditor.AssetDatabase;


public class ButtonMenuItem : MenuItemData
{
    public ButtonMenuItem(string key, Color color, bool noDefaultSprite) : base(key, color, noDefaultSprite)
    {
    
    }

    protected override IEnumerable GetDefaultSprites()
    {
        return (from asset in FindAssets("t:Sprite", new[] { GUIPath + "Button" })
            select GUIDToAssetPath(asset)
            into assetPath
            select LoadAssetAtPath<Sprite>(assetPath)
            into sprite
            let groupPath = GetValueDropdownGroup(
                sprite.name,
                new[] { "Large", "Small" }, new[] { "Outline", "Filled" })
            select new ValueDropdownItem(groupPath + sprite.name, sprite)).Cast<object>();
    }
}

public class DataEntryItem : MenuItemData
{
    public DataEntryItem(string key, Color color, bool noDefaultSprite) : base(key, color, noDefaultSprite)
    {
    }
    
    protected override IEnumerable GetDefaultSprites()
    {
        return (from asset in FindAssets("t:Sprite", new[] { GUIPath + "DataEntry" })
            select GUIDToAssetPath(asset)
            into assetPath
            select LoadAssetAtPath<Sprite>(assetPath)
            into sprite
            let groupPath = GetValueDropdownGroup(
                sprite.name,
                new[] { "Large", "Small" }, new[] { "Outline", "Filled" })
            select new ValueDropdownItem(groupPath + sprite.name, sprite)).Cast<object>();
    }
}

public class LeaderboardEntryItem : MenuItemData
{
    public LeaderboardEntryItem(string key, Color color, bool noDefaultSprite) : base(key, color, noDefaultSprite)
    {
    }
    
    protected override IEnumerable GetDefaultSprites()
    {
        return (from asset in FindAssets("t:Sprite", new[] { GUIPath + "Leaderboard" })
            select GUIDToAssetPath(asset)
            into assetPath
            select LoadAssetAtPath<Sprite>(assetPath)
            into sprite
            let groupPath = GetValueDropdownGroup(
                sprite.name,
                new[] { "Large", "Small" }, new[] { "Outline", "Filled" })
            select new ValueDropdownItem(groupPath + sprite.name, sprite)).Cast<object>();
    }
}
