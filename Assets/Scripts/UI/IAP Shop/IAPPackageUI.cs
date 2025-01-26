using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class IAPPackageUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinQuantityText;
    [SerializeField] private TMP_Text addHoleQuantityText;
    [SerializeField] private TMP_Text breakObjectQuantityText;
    [SerializeField] private TMP_Text clearHolesQuantityText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;

    [Header("CUSTOMIZE")]
    [SerializeField] private bool isContainRemoveAd;

    [SerializeField] private IAPPackageData _packageData;

    #region EVENT
    public static event Action<string> buyIAPEvent;
    public static event Action<string, string> showNotificationEvent;
    #endregion

    private void Awake()
    {
        SaferioIAPManager.updatePriceEvent += UpdatePrice;
        ResourceEarnPopup.removeAdsEvent += OnAdsRemoved;

        buyButton.onClick.AddListener(Buy);

        Setup(_packageData);
    }

    void OnDestroy()
    {
        SaferioIAPManager.updatePriceEvent -= UpdatePrice;
        ResourceEarnPopup.removeAdsEvent -= OnAdsRemoved;
    }

    public void Setup(IAPPackageData data)
    {
        if (isContainRemoveAd && UserData.IsRemoveAds)
        {
            gameObject.SetActive(false);

            return;
        }

        if (coinQuantityText != null)
        {
            coinQuantityText.text = $"{data.CoinQuantity}";
        }

        if (addHoleQuantityText != null)
        {
            addHoleQuantityText.text = $"{data.AddHoleBoosterQuantity}";
        }

        if (breakObjectQuantityText != null)
        {
            breakObjectQuantityText.text = $"{data.BreakObjectBoosterQuantity}";
        }

        if (clearHolesQuantityText != null)
        {
            clearHolesQuantityText.text = $"{data.ClearHolesBoosterQuantity}";
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            priceText.text = $"{data.Price}$";
        }
    }

    private void Buy()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            showNotificationEvent?.Invoke(GameConstants.NO_INTERNET, GameConstants.PLEASE_CHECK_YOUR_INTERNET_CONNECTION);
        }
        else
        {
            buyIAPEvent?.Invoke(_packageData.ProductId);
        }
    }

    private void UpdatePrice(string productId, string price)
    {
        if (productId == _packageData.ProductId)
        {
            priceText.text = price;
        }
    }

    private void OnAdsRemoved()
    {
        // if (isContainRemoveAd)
        // {
        //     gameObject.SetActive(false);
        // }
    }
}
