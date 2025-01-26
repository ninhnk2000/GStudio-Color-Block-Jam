using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class IAPCoinItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinQuantityText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;

    [SerializeField] private IAPPackageData _packageData;

    #region EVENT
    public static event Action<string> buyIAPEvent;
    public static event Action<string, string> showNotificationEvent;
    #endregion

    private void Awake()
    {
        SaferioIAPManager.updatePriceEvent += UpdatePrice;

        buyButton.onClick.AddListener(Buy);

        Setup(_packageData);
    }

    void OnDestroy()
    {
        SaferioIAPManager.updatePriceEvent -= UpdatePrice;
    }

    public void Setup(IAPPackageData data)
    {
        coinQuantityText.text = $"{data.CoinQuantity}";

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
}
