using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;


public class LeaderBoardGetter : MonoBehaviour
{

    public List<int> scores = new List<int>();

    async void Start()
    {
        await GetScores();
    }

    public async Task GetScores()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync("Race_Game");
        LeaderboardResult leaderboardData = JsonConvert.DeserializeObject<LeaderboardResult>(JsonConvert.SerializeObject(scoresResponse));
        Debug.Log(leaderboardData.limit);
        Debug.Log(leaderboardData.total);
        foreach (MyPlayer user in leaderboardData.results)
        {
            Debug.Log(user.playerName);
            Debug.Log(user.score);
        }
    }

}

public class LeaderboardResult
{
    public int limit { get; set; }
    public int total { get; set; }
    public MyPlayer[] results { get; set; }
}

public class MyPlayer
{
    public string playerId { get; set; }
    public string playerName { get; set; }
    public int rank { get; set; }
    public double score { get; set; }
}
