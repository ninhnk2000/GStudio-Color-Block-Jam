using System;
using UnityEngine;
using UnityEngine.UI;

public class IAPShopItem : MonoBehaviour
{
    [SerializeField] private string productId;

    [SerializeField] private Button buyButton;

    public static event Action<string> buyIAPEvent;

    private void Awake()
    {
        buyButton.onClick.AddListener(BuyIAP);
    }

    private void BuyIAP()
    {
        buyIAPEvent?.Invoke(productId);
    }
}
