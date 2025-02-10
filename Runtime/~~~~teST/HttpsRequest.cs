using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using MenuComponents.SaveSystem;
using UnityEngine;
using UnityEngine.Networking;

public static class HttpsRequest
{
    private const string URL = "https://www.sportsim.com.au/game/bat-flick-rugby/";

    private static List<LeaderboardPlayerData> GetXmlData(string response)
    {
        var leaderboardXMLData = new List<XmlNode>();
        
        try
        {
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(new StringReader(response));
            
            leaderboardXMLData =
            xmlDocument.GetElementsByTagName("Player").Cast<XmlNode>().ToList();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<LeaderboardPlayerData>();
        }

        var leaderboardData = new List<LeaderboardPlayerData>();
        
        if(leaderboardXMLData != null) leaderboardData = leaderboardXMLData.Select(player => new LeaderboardPlayerData(
            player.ChildNodes[0].InnerText,
            int.Parse(player.ChildNodes[2].InnerText))).ToList();

        return leaderboardData;
    }

    public static IEnumerator Post(LeaderboardPlayerData playerData)
    {
        if (playerData == null) yield break;

        var form = new WWWForm();
        form.AddField("Name", playerData.Name);
        form.AddField("Score", (int)playerData.Score);

        using (var www = UnityWebRequest.Post(URL + "postScore.asp", form))
        {
            yield return www.SendWebRequest();

            if (!string.IsNullOrWhiteSpace(www.error))
            {
                Debug.LogError($"Error {www.responseCode} - {www.error}");
            }
        }
    }

    public static IEnumerator Get(Action<List<LeaderboardPlayerData>> responseCallback)
    {
        const string uri = URL + "leaderboard.xml";

        using (var webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            var pages = uri.Split('/');
            var page = pages.Length - 1;

            if (!string.IsNullOrWhiteSpace(webRequest.error))
            {
                Debug.LogError($"Error {webRequest.responseCode} - {webRequest.error}");
                responseCallback(new List<LeaderboardPlayerData>());
                yield break;
            }

            responseCallback(GetXmlData(webRequest.downloadHandler.text));
        }
    }
}