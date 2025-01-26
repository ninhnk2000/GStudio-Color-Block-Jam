// using System;
// using com.unity3d.mediation;
// using UnityEngine;
// using UnityEngine.UI;

// public class LevelPlayManager : MonoBehaviour
// {
//     [SerializeField] private string appKey;

//     [SerializeField] private Button showAdButton;

//     private void Awake()
//     {
//         InitLevelPlay();

//         RegisterRewardedAdEvent();

//         showAdButton.onClick.AddListener(ShowRewardedAd);
//     }

//     private void OnApplicationPause(bool pauseStatus)
//     {
//         IronSource.Agent.onApplicationPause(pauseStatus);
//     }

//     private void InitLevelPlay()
//     {
//         LevelPlayAdFormat[] legacyAdFormats = new[] { LevelPlayAdFormat.REWARDED };

//         LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
//         LevelPlay.OnInitFailed += SdkInitializationFailedEvent;

//         IronSource.Agent.setMetaData("is_test_suite", "enable");
//         LevelPlay.Init(appKey, "UserId", legacyAdFormats);
//     }

//     private void RegisterRewardedAdEvent()
//     {
//         IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
//         IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
//         IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
//         IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
//         IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
//         IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
//         IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
//     }

//     private void SdkInitializationFailedEvent(LevelPlayInitError error)
//     {
//         Debug.Log(error);
//     }

//     private void SdkInitializationCompletedEvent(LevelPlayConfiguration configuration)
//     {
//         Debug.Log("Initialized");
//         IronSource.Agent.launchTestSuite();
//     }





//     private void ShowRewardedAd()
//     {
//         bool available = IronSource.Agent.isRewardedVideoAvailable();

//         Debug.Log(available);

//         if (available)
//         {
//             IronSource.Agent.showRewardedVideo();
//         }
//     }







//     void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
//     {
//     }
//     // Indicates that no ads are available to be displayed
//     // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
//     void RewardedVideoOnAdUnavailable()
//     {
//     }
//     // The Rewarded Video ad view has opened. Your activity will loose focus.
//     void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
//     {
//     }
//     // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
//     void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
//     {
//     }
//     // The user completed to watch the video, and should be rewarded.
//     // The placement parameter will include the reward data.
//     // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
//     void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
//     {
//     }
//     // The rewarded video ad was failed to show.
//     void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
//     {
//     }
//     // Invoked when the video ad was clicked.
//     // This callback is not supported by all networks, and we recommend using it only if
//     // it’s supported by all networks you included in your build.
//     void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
//     {
//     }
// }
