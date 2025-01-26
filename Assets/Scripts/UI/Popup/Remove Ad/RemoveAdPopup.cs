using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class RemoveAdPopup : BasePopup
{
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text priceText;

    [SerializeField] private IAPPackageData iapData;

    #region EVENT
    public static event Action<string> buyIAPEvent;
    public static event Action<string, string> showNotificationEvent;
    #endregion

    protected override void MoreActionInAwake()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            priceText.text = $"6.99$";
        }
    }

    protected override void RegisterMoreEvent()
    {
        SaferioIAPManager.updatePriceEvent += UpdatePrice;
        ResourceEarnPopup.showResourcesEarnPopupEvent += OnResourceEarnPopupShow;

        buyButton.onClick.AddListener(BuyRemoveAd);
    }

    protected override void UnregisterMoreEvent()
    {
        SaferioIAPManager.updatePriceEvent -= UpdatePrice;
        ResourceEarnPopup.showResourcesEarnPopupEvent -= OnResourceEarnPopupShow;
    }

    private void BuyRemoveAd()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Hide();

            showNotificationEvent?.Invoke(GameConstants.NO_INTERNET, GameConstants.PLEASE_CHECK_YOUR_INTERNET_CONNECTION);
        }
        else
        {
            buyIAPEvent?.Invoke(iapData.ProductId);
        }
    }

    private void OnResourceEarnPopupShow(bool isShow)
    {
        if (isShow)
        {
            Hide();
        }
    }

    private void UpdatePrice(string productId, string price)
    {
        if (productId == iapData.ProductId)
        {
            priceText.text = price;
        }
    }
}
