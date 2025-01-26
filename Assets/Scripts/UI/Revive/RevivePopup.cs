using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class RevivePopup : BasePopup
{
    [SerializeField] private Button reviveByDiamondButton;
    [SerializeField] private Image reviveByDiamondButtonForeground;
    [SerializeField] private Image boosterImage;
    [SerializeField] private Button reviveByAdsButton;
    [SerializeField] private TMP_Text costText;

    [Header("SPRITE")]
    [SerializeField] private Sprite unlockBoxSprite;
    [SerializeField] private Sprite clearHolesSprite;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private UserResourcesObserver userResourcesObserver;
    [SerializeField] private BoosterDataObserver boosterDataObserver;

    #region PRIVATE FIELD
    private BoosterType _currentBoosterOnRevive;
    #endregion

    public static event Action<BoosterType> reviveEvent;
    public static event Action reviveRefusedEvent;
    public static event Action<ScreenRoute> openIAPShopPopupEvent;
    public static event Action<ScreenRoute> switchRouteEvent;

    protected override void MoreActionInAwake()
    {
        costText.text = $"{boosterDataObserver.BoosterCosts.Last()}";
    }

    protected override void RegisterMoreEvent()
    {
        GameStateLose.showRevivePopupEvent += ShowRevivePopup;

        reviveByDiamondButton.onClick.AddListener(ReviveByDiamond);
        reviveByAdsButton.onClick.AddListener(ReviveByAds);
    }

    protected override void MoreActionOnDestroy()
    {
        GameStateLose.showRevivePopupEvent -= ShowRevivePopup;
    }

    protected override void Hide()
    {
        base.Hide(onCompletedAction: () =>
        {
            reviveRefusedEvent?.Invoke();
        });
    }

    private void ShowRevivePopup(BoosterType boosterType)
    {
        Show();

        SoundManager.Instance.PlaySoundLose();

        if (boosterType == BoosterType.UnlockScrewBox)
        {
            boosterImage.sprite = unlockBoxSprite;
        }
        else if (boosterType == BoosterType.ClearScrewPorts)
        {
            boosterImage.sprite = clearHolesSprite;
        }

        UIUtil.SetSizeKeepRatioY(boosterImage, 0.2f * container.sizeDelta.x);

        reviveByDiamondButton.interactable = true;

        _currentBoosterOnRevive = boosterType;
    }

    private void ReviveByDiamond()
    {
        // if (_isInTransition)
        // {
        //     return;
        // }

        userResourcesObserver.Load();

        reviveByDiamondButton.interactable = false;

        if (userResourcesObserver.UserResources.CoinQuantity >= boosterDataObserver.BoosterCosts.Last())
        {
            base.Hide(onCompletedAction: () =>
            {
                reviveEvent?.Invoke(_currentBoosterOnRevive);

                reviveByDiamondButton.interactable = true;
            });

            userResourcesObserver.UserResources.CoinQuantity -= boosterDataObserver.BoosterCosts.Last();

            userResourcesObserver.Save();
        }
        else
        {
            Hide(onCompletedAction: () =>
            {
                openIAPShopPopupEvent?.Invoke(ScreenRoute.Revive);

                reviveByDiamondButton.interactable = true;
            });
        }
    }

    private void ReviveByAds()
    {
        if (_isInTransition)
        {
            return;
        }

        ActionWatchVideo actionWatchVideo;

        switch (_currentBoosterOnRevive)
        {
            case BoosterType.AddScrewPort: actionWatchVideo = ActionWatchVideo.AddHole; break;
            case BoosterType.BreakObject: actionWatchVideo = ActionWatchVideo.BreakObject; break;
            case BoosterType.ClearScrewPorts: actionWatchVideo = ActionWatchVideo.ClearHoles; break;
            case BoosterType.UnlockScrewBox: actionWatchVideo = ActionWatchVideo.UnlockScrewBox; break;
            default: actionWatchVideo = ActionWatchVideo.UnlockScrewBox; break;
        }

        AdmobAdsMax.Instance.ShowVideoReward(OnRewaredAdCompleted, actionNotLoadedVideo: ShowAdsNotLoadedPopup, actionClose: null, actionType: actionWatchVideo);
    }

    private void OnRewaredAdCompleted()
    {
        base.Hide(onCompletedAction: () =>
        {
            reviveEvent?.Invoke(_currentBoosterOnRevive);
        });
    }

    private void ShowAdsNotLoadedPopup()
    {
        switchRouteEvent?.Invoke(ScreenRoute.NoInternet);
    }
}
