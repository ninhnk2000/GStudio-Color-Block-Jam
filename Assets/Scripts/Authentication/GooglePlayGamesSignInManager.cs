using System;
using System.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class GooglePlayGamesSignInManager : MonoBehaviour
{
    public string Token;
    public string Error;

    public static event Action userAuthenticatedEvent;

    void Awake()
    {
        // PlayGamesPlatform.Activate();

        // LoginGooglePlayGames();
    }

    public void LoginGooglePlayGames()
    {
        // PlayGamesPlatform.Instance.Authenticate((success) =>
        // {
        //     Debug.Log("SAFERIO - " + success);

        //     if (success == SignInStatus.Success)
        //     {
        //         Debug.Log("Login with Google Play games successful.");

        //         PlayGamesPlatform.Instance.RequestServerSideAccess(true, async code =>
        //         {
        //             Token = code;

        //             await SignInWithGooglePlayGamesAsync(code);
        //         });
        //     }
        //     else
        //     {
        //         Error = "Failed to retrieve Google play games authorization code";
        //     }
        // });
    }

    async Task SignInWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            await UnityServices.InitializeAsync();

            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);

            userAuthenticatedEvent?.Invoke();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }
}
