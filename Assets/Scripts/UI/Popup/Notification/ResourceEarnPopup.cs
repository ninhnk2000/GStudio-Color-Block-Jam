using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceEarnPopup : BasePopup
{
    [SerializeField] private RectTransform[] resourceGroups;

    [SerializeField] private Button okButton;

    private ResourceGroupUI[] _resourceGroupsUI;

    private int _coinQuantity;
    private bool _isRemoveAds;

    public static event Action<int> collectCoinEvent;
    public static event Action removeAdsEvent;
    public static event Action<bool> showResourcesEarnPopupEvent;

    protected override void MoreActionInAwake()
    {
        _resourceGroupsUI = new ResourceGroupUI[resourceGroups.Length];

        for (int i = 0; i < resourceGroups.Length; i++)
        {
            _resourceGroupsUI[i] = resourceGroups[i].GetComponent<ResourceGroupUI>();
        }
    }

    protected override void RegisterMoreEvent()
    {
        SaferioIAPManager.showResourcesEarnPopupEvent += ShowResourcesEarned;
        
        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(Claim);
    }

    protected override void UnregisterMoreEvent()
    {
        SaferioIAPManager.showResourcesEarnPopupEvent -= ShowResourcesEarned;
    }

    private void Claim()
    {
        base.Hide(onCompletedAction: () =>
        {
            showResourcesEarnPopupEvent?.Invoke(false);
        });

        if (_coinQuantity > 0)
        {
            collectCoinEvent?.Invoke(_coinQuantity);

            _coinQuantity = 0;
        }

        if (_isRemoveAds)
        {
            removeAdsEvent?.Invoke();

            _isRemoveAds = false;
        }
    }

    private void ShowResourcesEarned(int coinQuantity, int[] boosterQuantities, bool isRemoveAd)
    {
        showResourcesEarnPopupEvent?.Invoke(true);
        
        Show();

        List<RectTransform> validResourceGroups = new List<RectTransform>();

        if (isRemoveAd)
        {
            validResourceGroups.Add(resourceGroups[0]);

            _isRemoveAds = true;
        }

        if (coinQuantity > 0)
        {
            _resourceGroupsUI[1].SetQuantity(coinQuantity);

            validResourceGroups.Add(resourceGroups[1]);
        }

        for (int i = 0; i < boosterQuantities.Length; i++)
        {
            if (boosterQuantities[i] > 0)
            {
                _resourceGroupsUI[2 + i].SetQuantity(boosterQuantities[i]);

                validResourceGroups.Add(resourceGroups[2 + i]);
            }
        }

        Vector3 position = Vector3.zero;

        for (int i = 0; i < resourceGroups.Length; i++)
        {
            resourceGroups[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < validResourceGroups.Count; i++)
        {
            position.x = (-(validResourceGroups.Count - 1) / 2f + i) * 0.17f * container.sizeDelta.x;

            validResourceGroups[i].gameObject.SetActive(true);
            validResourceGroups[i].localPosition = position;
        }

        _coinQuantity = coinQuantity;
    }

    // private void Claim()
    // {
    //     Hide();

    //     if (_coinQuantity > 0)
    //     {
    //         collectCoinEvent?.Invoke(_coinQuantity);

    //         _coinQuantity = 0;
    //     }
    // }
}
