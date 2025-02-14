using System;
using System.Threading.Tasks;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class GameplayScreen : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button openDebugPopupButton;
    [SerializeField] private LeanLocalizedTextMeshProUGUI localizedLevelText;

    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private GameSetting gameSetting;

    public static event Action<ScreenRoute> switchRouteEvent;
    public static event Action nextLevelEvent;
    public static event Action prevLevelEvent;

    void Awake()
    {
        localizedLevelText.textTranslatedEvent += OnLevelTextTranslated;
        LevelLoader.setLevelNumberEvent += SetLevelText;

        pauseButton.onClick.AddListener(Pause);
        openDebugPopupButton.onClick.AddListener(OpenDebugPopupButton);

        if (!gameSetting.IsDebug)
        {
            openDebugPopupButton.gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        localizedLevelText.textTranslatedEvent -= OnLevelTextTranslated;
        LevelLoader.setLevelNumberEvent -= SetLevelText;
    }

    private async void Pause()
    {
        switchRouteEvent?.Invoke(ScreenRoute.Pause);
    }

    private void OpenDebugPopupButton()
    {
        switchRouteEvent?.Invoke(ScreenRoute.Debug);
    }

    private void OnLevelTextTranslated()
    {
        localizedLevelText.UpdateTranslationWithParameter(GameConstants.LEVEL_PARAMETER, $"{GetDisplayedLevel()}");
    }

    private void SetLevelText()
    {
        localizedLevelText.UpdateTranslationWithParameter(GameConstants.LEVEL_PARAMETER, $"{GetDisplayedLevel()}");
    }

    private int GetDisplayedLevel()
    {
        return currentLevel.Value;
    }
}
