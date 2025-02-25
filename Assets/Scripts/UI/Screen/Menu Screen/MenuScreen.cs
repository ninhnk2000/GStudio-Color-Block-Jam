using System;
using System.Threading.Tasks;
using Lean.Localization;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static GameEnum;

public class MenuScreen : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Image startGameButtonImage;
    [SerializeField] private TMP_Text startGameButtonText;
    [SerializeField] private Button openRemoveAdPopupButton;
    [SerializeField] private RectTransform openRemoveAdPopupButtonRT;
    [SerializeField] private Image openRemoveAdPopupButtonImage;
    [SerializeField] private Button luckyWheelButton;
    [SerializeField] private Button weeklyTaskButton;
    [SerializeField] private LeanLocalizedTextMeshProUGUI localizedLevelText;
    [SerializeField] private Image topBarDivider;

    [SerializeField] private Sprite[] startButtonSprites;
    [SerializeField] private Material[] textMaterials;

    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private Vector2Variable canvasSize;

    #region PRIVATE FIELD
    private float _initialOpenRemoveAdPopupButtonPositionY;
    private Tween _openRemoveAdPopupButtonTween;
    private Tween _topBarDividerTween;
    #endregion

    public static event Action<ScreenRoute> switchRouteEvent;
    public static event Action<bool> showMenuBlackScreenEvent;
    public static event Action<int> changeLivesNumberEvent;

    private void Awake()
    {
        GameVariableInitializer.currentLevelFetchedEvent += UpdateCurrentLevelText;
        BottomBarItem.switchRouteEvent += OnScreenRouteSwitched;
        ResourceEarnPopup.removeAdsEvent += OnAdsRemoved;
        SaferioIAPManager.removeAdsEvent += OnAdsRemoved;

        localizedLevelText.textTranslatedEvent += OnLevelTextTranslated;

        startGameButton.onClick.AddListener(StartGame);
        openRemoveAdPopupButton.onClick.AddListener(OpenRemoveAdPopup);
        luckyWheelButton.onClick.AddListener(OpenLuckyWheelScreen);
        weeklyTaskButton.onClick.AddListener(OpenWeeklyTaskScreen);

        _initialOpenRemoveAdPopupButtonPositionY = openRemoveAdPopupButtonRT.localPosition.y;
        topBarDivider.color = ColorUtil.WithAlpha(topBarDivider.color, 0);

        if (UserData.IsRemoveAds)
        {
            openRemoveAdPopupButton.gameObject.SetActive(false);
        }

        currentLevel.Load();

        // COLOR BASED ON DIFFICULTY
        int modulusLevel = currentLevel.Value % 5;

        int spriteIndex;

        if (modulusLevel >= 1 && modulusLevel <= 3)
        {
            spriteIndex = 0;
        }
        else if (modulusLevel == 4)
        {
            spriteIndex = 2;
        }
        else
        {
            spriteIndex = 1;
        }

        startGameButtonImage.sprite = startButtonSprites[spriteIndex];
        startGameButtonText.fontMaterial = textMaterials[spriteIndex];
    }

    private void Start()
    {
        bool isFirstTimeOpenApp = DataUtility.Load(GameConstants.IS_FIRST_TIME_OPEN_APP, true);

        showMenuBlackScreenEvent?.Invoke(isFirstTimeOpenApp);

        if (UserData.IsFirstTimeOpenApp)
        {
            StartGame();

            DataUtility.Save(GameConstants.IS_FIRST_TIME_OPEN_APP, false);
        }
    }

    void OnDestroy()
    {
        GameVariableInitializer.currentLevelFetchedEvent -= UpdateCurrentLevelText;
        BottomBarItem.switchRouteEvent -= OnScreenRouteSwitched;
        ResourceEarnPopup.removeAdsEvent -= OnAdsRemoved;
        SaferioIAPManager.removeAdsEvent -= OnAdsRemoved;

        localizedLevelText.textTranslatedEvent -= OnLevelTextTranslated;
    }

    private void OnLevelTextTranslated()
    {
        localizedLevelText.UpdateTranslationWithParameter(GameConstants.LEVEL_PARAMETER, $"{GetDisplayedLevel()}");
    }

    private void StartGame()
    {
        Addressables.LoadSceneAsync(GameConstants.GAMEPLAY_SCENE);
        
        // if (DataUtility.Load<LivesData>(GameConstants.USER_LIVES_DATA, new LivesData()).CurrentLives > 0)
        // {
        //     changeLivesNumberEvent?.Invoke(-1);

        //     Addressables.LoadSceneAsync(GameConstants.GAMEPLAY_SCENE);
        // }
        // else
        // {
        //     switchRouteEvent?.Invoke(ScreenRoute.LivesShop);
        // }
    }

    private void OpenRemoveAdPopup()
    {
        switchRouteEvent?.Invoke(ScreenRoute.RemoveAd);
    }

    private void OpenLuckyWheelScreen()
    {
        switchRouteEvent?.Invoke(ScreenRoute.LuckyWheel);
    }

    private void OpenWeeklyTaskScreen()
    {
        switchRouteEvent?.Invoke(ScreenRoute.WeeklyTask);
    }

    private void PlayClickSound()
    {
        AudioSource clickSound = ObjectPoolingEverything.GetFromPool<AudioSource>(GameConstants.CLICK_SOUND);

        clickSound.Play();
    }

    private void UpdateCurrentLevelText()
    {
        localizedLevelText.UpdateTranslationWithParameter(GameConstants.LEVEL_PARAMETER, $"{GetDisplayedLevel()}");
    }

    private void OnScreenRouteSwitched(ScreenRoute screenRoute)
    {
        ShowOpenRemoveAdPopupButtonOnScreenRouteSwitch(screenRoute);
        ShowTopBarDivider(screenRoute);
    }

    private void OnAdsRemoved()
    {
        openRemoveAdPopupButton.gameObject.SetActive(false);
    }

    private void ShowOpenRemoveAdPopupButtonOnScreenRouteSwitch(ScreenRoute screenRoute)
    {
        if (UserData.IsRemoveAds)
        {
            return;
        }

        CommonUtil.StopTween(_openRemoveAdPopupButtonTween);

        if (screenRoute != ScreenRoute.IAPShop)
        {
            _openRemoveAdPopupButtonTween = Tween.Alpha(openRemoveAdPopupButtonImage, 1, 0.3f);
            // _openRemoveAdPopupButtonTween = Tween.LocalPositionY(openRemoveAdPopupButtonRT, _initialOpenRemoveAdPopupButtonPositionY, 0.3f);
        }
        else
        {
            _openRemoveAdPopupButtonTween = Tween.Alpha(openRemoveAdPopupButtonImage, 0, 0.3f);
            // _openRemoveAdPopupButtonTween = Tween.LocalPositionY(openRemoveAdPopupButtonRT, openRemoveAdPopupButtonRT.localPosition.y + 0.3f * canvasSize.Value.y, 0.3f);
        }
    }

    private void ShowTopBarDivider(ScreenRoute screenRoute)
    {
        CommonUtil.StopTween(_topBarDividerTween);

        if (screenRoute == ScreenRoute.IAPShop)
        {
            _topBarDividerTween = Tween.Alpha(topBarDivider, 1, 0.3f);
        }
        else
        {
            _topBarDividerTween = Tween.Alpha(topBarDivider, 0, 0.3f);
        }
    }

    private int GetDisplayedLevel()
    {
        return currentLevel.Value;
    }
}
