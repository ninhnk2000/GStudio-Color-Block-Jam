using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lean.Localization;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class WinPopup : BasePopup
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button returnHomeButton;
    [SerializeField] private RectTransform coinContainer;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private LeanLocalizedTextMeshProUGUI localizedLevelCompleted;

    [SerializeField] private ParticleSystem fireworkFx;

    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private UserResourcesObserver userResourcesObserver;
    [SerializeField] private LevelBoosterObserver levelBoosterObserver;
    [SerializeField] private LevelDataContainer levelDataContainer;

    [Header("CUSTOMIZE")]
    [SerializeField] private float timeCollectCoin;

    public static event Action<int> goLevelEvent;
    public static event Action nextLevelEvent;
    public static event Action<Vector3, int> collectCoinEvent;


    protected override void RegisterMoreEvent()
    {
        GameStateWin.winLevelEvent += OnLevelWinAsync;
        DebugPopup.winLevelEvent += OnLevelWinAsync;

        continueButton.onClick.AddListener(Continue);
        returnHomeButton.onClick.AddListener(ReturnHome);
    }

    protected override void UnregisterMoreEvent()
    {
        GameStateWin.winLevelEvent -= OnLevelWinAsync;
        DebugPopup.winLevelEvent -= OnLevelWinAsync;
    }

    private async void OnLevelWinAsync()
    {
        coinText.text = $"{0}";
        continueButton.transform.localScale = Vector3.zero;

        Show();

        fireworkFx.Play();

        SoundManager.Instance.PlaySoundWin();

        await Task.Delay(500);

        SetLevelCompletedText();

        CollectCoin(10);

        TrackLevelWin();

        if (currentLevel.Value - UserData.NumLevelLoop >= levelDataContainer.MaxLevel)
        {
            await UserData.SetNumLevelLoop(UserData.NumLevelLoop + 1);
        }

        currentLevel.Value++;

        currentLevel.Save();
    }

    public void SetLevelCompletedText()
    {
        // localizedLevelCompleted.UpdateTranslationWithParameter(GameConstants.LEVEL_PARAMETER, $"{currentLevel.Value}");
    }

    private void CollectCoin(int numCoin)
    {
        int _currentCoin = 0;

        coinText.text = $"{_currentCoin}";

        int deltaCoin = (int)(numCoin / 10f);

        _tweens.Add(Tween.Custom(0, numCoin, duration: timeCollectCoin, onValueChange: newVal =>
        {
            coinText.text = $"{(int)newVal}";
        })
        .OnComplete(() =>
            {
                // userResourcesObserver.UserResources.CoinQuantity += numCoin;

                // userResourcesObserver.Save();

                _tweens.Add(Tween.Scale(continueButton.transform, 1, duration: timeCollectCoin));
            })
        );

        // while (_currentCoin < numCoin)
        // {
        //     _currentCoin += deltaCoin;

        //     coinText.text = $"{_currentCoin}";

        //     await Task.Delay(100);
        // }
    }

    private async void Continue()
    {
        // collectCoinEvent?.Invoke(coinContainer.localPosition - new Vector3(0, coinContainer.sizeDelta.y), 500);

        // await Task.Delay(3000);

        AdmobAdsMax.Instance.ShowInterstitial(isShowImmediatly: true, actionIniterClose: OnInterstitialAdCompleted);
    }

    private void OnInterstitialAdCompleted()
    {
        _tweens.Add(Tween.Delay(1).OnComplete(() =>
        {
            HideImmediately();
        }));

        goLevelEvent?.Invoke(currentLevel.Value);
    }

    private void ReturnHome()
    {
        Addressables.LoadSceneAsync(GameConstants.MENU_SCENE);
    }

    private async void TrackLevelWin()
    {
        bool isFirstWinLevel = DataUtility.LoadAsync($"{GameConstants.IS_FIRST_WIN_LEVEL}_{currentLevel.Value}", false);

        if (isFirstWinLevel)
        {
            SaferioTracking.TrackLevelFirstWin(currentLevel.Value, levelBoosterObserver);

            DataUtility.SaveAsync($"{GameConstants.IS_FIRST_WIN_LEVEL}_{currentLevel.Value}", false);
        }

        SaferioTracking.TrackLevelWin(currentLevel.Value, levelBoosterObserver);
    }
}
