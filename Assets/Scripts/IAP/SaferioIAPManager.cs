using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class SaferioIAPManager : MonoBehaviour, IDetailedStoreListener
{
    [SerializeField] private GooglePlayStoreProduct[] googlePlayStoreProducts;
    [SerializeField] private IAPDataContainer IAPDataContainer;
    [SerializeField] private UserResourcesObserver userResourcesObserver;

    [SerializeField] private string removeAdsProductId;

    private IStoreController controller;
    private IExtensionProvider extensions;

    #region EVENT
    public static event Action iapProductPurchasedCompletedEvent;
    public static event Action removeAdPurchasedCompletedEvent;
    public static event Action<int> collectCoinEvent;
    public static event Action<string, string> updatePriceEvent;
    public static event Action<int, int[], bool> showResourcesEarnPopupEvent;
    public static event Action removeAdsEvent;
    #endregion

    private void Awake()
    {
        IAPPackageUI.buyIAPEvent += BuyProduct;
        IAPCoinItemUI.buyIAPEvent += BuyProduct;
        IAPShopItem.buyIAPEvent += BuyProduct;
        RemoveAdPopup.buyIAPEvent += BuyProduct;
        IAPShopPopup.fetchLocalizedPriceIAPEvent += FetchLocalizedPrice;

        for (int i = 0; i < IAPDataContainer.ProductsData.Length; i++)
        {
            googlePlayStoreProducts[i].Id = IAPDataContainer.ProductsData[i].ProductId;

            // if (IAPDataContainer.ProductsData[i].IsRemoveAd)
            // {
            //     googlePlayStoreProducts[i].ProductType = ProductType.NonConsumable;
            // }
            // else
            // {
            //     googlePlayStoreProducts[i].ProductType = ProductType.Consumable;
            // }

            googlePlayStoreProducts[i].ProductType = ProductType.Consumable;
        }

        Init();

        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        IAPPackageUI.buyIAPEvent -= BuyProduct;
        IAPCoinItemUI.buyIAPEvent -= BuyProduct;
        IAPShopItem.buyIAPEvent -= BuyProduct;
        RemoveAdPopup.buyIAPEvent -= BuyProduct;
        IAPShopPopup.fetchLocalizedPriceIAPEvent -= FetchLocalizedPrice;
    }

    private void Init()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

#if UNITY_ANDROID
        foreach (var product in googlePlayStoreProducts)
        {
            builder.AddProduct(product.Id, product.ProductType);
        }
#endif

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyProduct(string productId)
    {
        controller.InitiatePurchase(productId);

        // foreach (var productData in IAPDataContainer.ProductsData)
        // {
        //     if (productData.ProductId == productId)
        //     {
        //         // collectCoinEvent?.Invoke(productData.CoinQuantity);

        //         userResourcesObserver.ChangeBoosterQuantity(0, productData.AddHoleBoosterQuantity);
        //         userResourcesObserver.ChangeBoosterQuantity(1, productData.BreakObjectBoosterQuantity);
        //         userResourcesObserver.ChangeBoosterQuantity(2, productData.ClearHolesBoosterQuantity);

        //         if (productData.IsRemoveAd)
        //         {
        //             UserData.IsRemoveAds = true;
        //         }

        //         int[] boosterQuantites = new int[3] {
        //                 productData.AddHoleBoosterQuantity,
        //                 productData.BreakObjectBoosterQuantity,
        //                 productData.ClearHolesBoosterQuantity
        //             };

        //         showResourcesEarnPopupEvent?.Invoke(productData.CoinQuantity, boosterQuantites, productData.IsRemoveAd);

        //         break;
        //     }
        // }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;

        FetchLocalizedPrice();

        CheckRestoredRemoveAds();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log(error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        bool validPurchase = true;

        if (validPurchase)
        {
            if (e.purchasedProduct.definition.id == GameConstants.REMOVE_AD_ID)
            {
                removeAdPurchasedCompletedEvent?.Invoke();

                UserData.IsRemoveAds = true;
            }
            else
            {
                foreach (var productData in IAPDataContainer.ProductsData)
                {
                    if (productData.ProductId == e.purchasedProduct.definition.id)
                    {
                        // collectCoinEvent?.Invoke(productData.CoinQuantity);

                        userResourcesObserver.ChangeBoosterQuantity(0, productData.AddHoleBoosterQuantity);
                        userResourcesObserver.ChangeBoosterQuantity(1, productData.BreakObjectBoosterQuantity);
                        userResourcesObserver.ChangeBoosterQuantity(2, productData.ClearHolesBoosterQuantity);

                        if (productData.IsRemoveAd)
                        {
                            UserData.IsRemoveAds = true;
                        }

                        int[] boosterQuantites = new int[3] {
                        productData.AddHoleBoosterQuantity,
                        productData.BreakObjectBoosterQuantity,
                        productData.ClearHolesBoosterQuantity
                    };

                        showResourcesEarnPopupEvent?.Invoke(productData.CoinQuantity, boosterQuantites, productData.IsRemoveAd);

                        break;
                    }
                }
            }
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureDescription p)
    {
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {

    }

    private void FetchLocalizedPrice()
    {
        List<Product> products = GetAvailableProducts();

        // string productId;
        // string localizedPrice;

        for (int i = 0; i < products.Count; i++)
        {
            string productId = products[i].definition.id;

            string localizedPrice = GetLocalizedPrice(products[i]);

            updatePriceEvent?.Invoke(productId, localizedPrice);
        }
    }

    public List<Product> GetAvailableProducts()
    {
        if (controller != null)
        {
            return new List<Product>(controller.products.all);
        }
        else
        {
            return new List<Product>();
        }
    }

    public string GetLocalizedPrice(Product product)
    {
        var price = product.metadata.localizedPriceString;
        var cultureInfo = new CultureInfo("en-US");

        return price;
    }

    private bool IsProductPurchased(string productId)
    {
        if (controller != null)
        {
            Product product = controller.products.WithID(productId);

            if (product != null && product.hasReceipt)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private void CheckRestoredRemoveAds()
    {
        UserData.IsRemoveAds = IsProductPurchased(removeAdsProductId);

        if (UserData.IsRemoveAds)
        {
            removeAdsEvent?.Invoke();
        }
    }
}

[Serializable]
public class GooglePlayStoreProduct
{
    [SerializeField] private string id;
    [SerializeField] private ProductType productType;

    public string Id
    {
        get => id;
        set => id = value;
    }

    public ProductType ProductType
    {
        get => productType;
        set => productType = value;
    }
}
