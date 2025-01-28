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

    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private GameSetting gameSetting;

    public static event Action<ScreenRoute> switchRouteEvent;
    public static event Action nextLevelEvent;
    public static event Action prevLevelEvent;

    void Awake()
    {
        pauseButton.onClick.AddListener(Pause);
        openDebugPopupButton.onClick.AddListener(OpenDebugPopupButton);

        if (!gameSetting.IsDebug)
        {
            openDebugPopupButton.gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {

    }

    private void Pause()
    {
        switchRouteEvent?.Invoke(ScreenRoute.Pause);
    }

    private void OpenDebugPopupButton()
    {
        switchRouteEvent?.Invoke(ScreenRoute.Debug);
    }

    private int GetDisplayedLevel()
    {
        return currentLevel.Value;
    }
}
