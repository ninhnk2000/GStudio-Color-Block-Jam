using System;
using UnityEngine;

public class ScriptableObjectInitializer : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;

    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private Vector2Variable canvasSize;
    [SerializeField] private GameSetting gameSetting;
    [SerializeField] private UserResourcesObserver userResourcesObserver;

    public static event Action gameSettingLoadedEvent;
    public static event Action getCachedCanvasSizeEvent;

    void Awake()
    {
        currentLevel.Load();
        userResourcesObserver.Load();

        LoadGameSetting();

        getCachedCanvasSizeEvent?.Invoke();
    }

    private async void LoadGameSetting()
    {
        await gameSetting.LoadAsync();

        gameSettingLoadedEvent?.Invoke();
    }
}
