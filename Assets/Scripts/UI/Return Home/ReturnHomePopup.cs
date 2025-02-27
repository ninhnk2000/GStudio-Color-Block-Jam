using System;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static GameEnum;

public class ReturnHomePopup : BasePopup
{
    [SerializeField] private Button homeButton;

    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private GameSetting gameSetting;
    [SerializeField] private LevelObserver levelObserver;
    [SerializeField] private LevelBoosterObserver levelBoosterObserver;

    public static event Action<int> changeLivesNumberEvent;
    public static event Action<ScreenRoute> switchRouteEvent;

    protected override void MoreActionInAwake()
    {
        homeButton.onClick.AddListener(ReturnHome);
    }

    private void ReturnHome()
    {
        if (GamePersistentVariable.livesData.CurrentLives <= 0)
        {
            switchRouteEvent?.Invoke(ScreenRoute.LivesShop);

            return;
        }

        // avoid spamming
        homeButton.interactable = false;

        changeLivesNumberEvent?.Invoke(-1);

        SaferioTracking.TrackLevelLose(currentLevel.Value, levelObserver.Progress, levelBoosterObserver, EndLevelReason.ReturnHome.ToString());

        Addressables.LoadSceneAsync(GameConstants.MENU_SCENE);
    }
}
