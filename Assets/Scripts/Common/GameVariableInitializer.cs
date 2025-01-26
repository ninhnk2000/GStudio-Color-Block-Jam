using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameVariableInitializer : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;

    CanvasScaler canvasScaler;
    [SerializeField] private Vector2Variable canvasSize;
    [SerializeField] private Vector2Variable canvasSizeAfterCanvasScaler;
    [SerializeField] private Vector2Variable canvasScale;

    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private UserResourcesObserver userResourcesObserver;
    [SerializeField] private GameSetting gameSetting;

    private Vector2 _cachedCanvasSize;

    public static event Action currentLevelFetchedEvent;
    public static event Action gameSettingLoadedEvent;

    private void Awake()
    {
        ScriptableObjectInitializer.getCachedCanvasSizeEvent += GetCachedCanvasSize;

        canvasSize.Value = canvas.sizeDelta;

        GamePersistentVariable.canvasSize = canvas.sizeDelta;

        // canvasSize.Save();

        currentLevel.Load();
        userResourcesObserver.Load();

        LoadGameSetting();

        currentLevelFetchedEvent?.Invoke();

        _cachedCanvasSize = canvas.sizeDelta;
    }

    private void Start()
    {
        canvasSizeAfterCanvasScaler.Value = canvas.sizeDelta;
        canvasScale.Value = canvas.localScale;

        GamePersistentVariable.canvasScale = canvasScale.Value;
    }

    void OnDestroy()
    {
        ScriptableObjectInitializer.getCachedCanvasSizeEvent -= GetCachedCanvasSize;
    }

    private async void LoadGameSetting()
    {
        await gameSetting.LoadAsync();

        gameSettingLoadedEvent?.Invoke();
    }

    private void GetCachedCanvasSize()
    {
        canvasSize.Value = _cachedCanvasSize;
    }
}
