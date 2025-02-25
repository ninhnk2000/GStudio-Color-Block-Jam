using System;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class CoinContainerUI : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform iconRT;

    [SerializeField] private Vector2Variable canvasSize;
    [SerializeField] private Vector2Variable canvasScale;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Button addButton;

    [SerializeField] private UserResourcesObserver userResourcesObserver;

    [Header("CUSTOMIZE")]
    [SerializeField] private bool isPreventCollectCoin;

    #region PRIVATE FIELD
    private List<Tween> _tweens;
    private bool _isInTransition;
    #endregion

    #region EVENT
    public static event Action<Vector3, int> collectCoinEvent;
    public static event Action goToIAPShopEvent;
    #endregion

    private void Awake()
    {
        // SaferioIAPManager.iapProductPurchasedCompletedEvent += OnCoinPurchased;
        LevelLoader.startLevelEvent += OnLevelStarted;
        UserResourcesManager.updateCoinTextEvent += UpdateCoinText;
        BuyBoosterPopup.updateCoinTextEvent += UpdateCoinText;
        SaferioIAPManager.collectCoinEvent += CollectCoin;
        ResourceEarnPopup.collectCoinEvent += CollectCoin;

        // testCollectCoinButton.onClick.AddListener(OnCoinPurchased);
        addButton.onClick.AddListener(ShowIAPShop);

        _tweens = new List<Tween>();

        userResourcesObserver.Load();

        coinText.text = $"{userResourcesObserver.UserResources.CoinQuantity}";
    }

    void OnEnable()
    {
        userResourcesObserver.Load();

        UpdateCoinText(userResourcesObserver.UserResources.CoinQuantity);
    }

    private void OnDestroy()
    {
        // SaferioIAPManager.iapProductPurchasedCompletedEvent -= OnCoinPurchased;
        LevelLoader.startLevelEvent -= OnLevelStarted;
        UserResourcesManager.updateCoinTextEvent -= UpdateCoinText;
        BuyBoosterPopup.updateCoinTextEvent -= UpdateCoinText;
        SaferioIAPManager.collectCoinEvent -= CollectCoin;
        ResourceEarnPopup.collectCoinEvent -= CollectCoin;

        CommonUtil.StopAllTweens(_tweens);
    }

    // private void OnCoinPurchased()
    // {
    //     Vector2 positionRelativeToCanvas = ((Vector2)iconRT.position - 0.5f * canvasSize.Value) / canvasScale.Value.x;

    //     collectCoinEvent?.Invoke(positionRelativeToCanvas);
    // }

    private void OnLevelStarted()
    {
        coinText.text = $"{userResourcesObserver.UserResources.CoinQuantity}";
    }

    private void UpdateCoinText(float value)
    {
        coinText.text = $"{(int)value}";

        if (!_isInTransition)
        {
            _tweens.Add(Tween.Scale(container, 1.2f, cycles: 2, cycleMode: CycleMode.Yoyo, duration: 0.1f)
            .OnComplete(() => _isInTransition = false));

            _isInTransition = true;
        }
    }

    private void ShowIAPShop()
    {
        goToIAPShopEvent?.Invoke();
    }

    private void CollectCoin(int value)
    {
        if (isPreventCollectCoin)
        {
            return;
        }

        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        Vector2 positionRelativeToCanvas = ((Vector2)iconRT.position - 0.5f * GamePersistentVariable.canvasSize) / GamePersistentVariable.canvasScale;

        collectCoinEvent?.Invoke(positionRelativeToCanvas, value);
    }
}
