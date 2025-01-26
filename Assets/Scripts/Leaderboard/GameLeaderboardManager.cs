using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;

public class GameLeaderboardManager : MonoBehaviour
{
    [SerializeField] private bool isAdd;

    private void Awake()
    {
        GooglePlayGamesSignInManager.userAuthenticatedEvent += AddPlayerScoreToLeaderboard;
    }

    private void OnDestroy()
    {
        GooglePlayGamesSignInManager.userAuthenticatedEvent -= AddPlayerScoreToLeaderboard;
    }

    private async void GetLeaderboardData()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync("SAMPLE");

        foreach (var item in scoresResponse.Results)
        {
            Debug.Log(item.PlayerId + "/" + item.PlayerName + "/" + item.Score);
        }
    }

    private async void AddPlayerScoreToLeaderboard()
    {
        if (isAdd)
        {
            GetLeaderboardData();

            return;
        }

        await LeaderboardsService.Instance.AddPlayerScoreAsync("SAMPLE", Random.Range(1, 1000));
    }
}
