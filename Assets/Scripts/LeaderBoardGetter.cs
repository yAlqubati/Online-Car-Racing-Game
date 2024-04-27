using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;


public class LeaderBoardGetter : MonoBehaviour
{

    async void Start()
    {
        await GetScores();
    }

    public async Task GetScores()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync("Race_Game");
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }
}
