using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static GameEnum;

public class IAPShopPopup : BasePopup
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private CanvasGroup popupCanvasGroup;
    [SerializeField] private RectTransform scrollContent;
    [SerializeField] private RectTransform firstPackRT;
    [SerializeField] private RectTransform firstCoinPackRT;

    [Header("CUSTOMIZE")]
    [SerializeField] private float scrollToCoinPackDuration;

    #region PRIVATE FIELD
    private ScreenRoute _prevRoute;
    #endregion

    public static event Action<ScreenRoute> switchRouteEvent;
    public static event Action fetchLocalizedPriceIAPEvent;

    protected override void RegisterMoreEvent()
    {
        BuyBoosterPopup.openIAPShopPopupEvent += ShowAndScrollToCoinPack;
        RevivePopup.openIAPShopPopupEvent += ShowAndScrollToCoinPack;
        LivesShopPopup.openIAPShopPopupEvent += ShowAndScrollToCoinPack;
        ResourceEarnPopup.showResourcesEarnPopupEvent += OnResourcesEarnPopupShown;
        CoinContainerUI.goToIAPShopEvent += Show;
    }

    protected override void UnregisterMoreEvent()
    {
        BuyBoosterPopup.openIAPShopPopupEvent -= ShowAndScrollToCoinPack;
        RevivePopup.openIAPShopPopupEvent -= ShowAndScrollToCoinPack;
        LivesShopPopup.openIAPShopPopupEvent -= ShowAndScrollToCoinPack;
        ResourceEarnPopup.showResourcesEarnPopupEvent -= OnResourcesEarnPopupShown;
        CoinContainerUI.goToIAPShopEvent -= Show;
    }

    protected override void Show(Action onCompletedAction = null)
    {
        base.Show(onCompletedAction);

        fetchLocalizedPriceIAPEvent?.Invoke();
    }

    private void ShowAndScrollToCoinPack(ScreenRoute prevRoute)
    {
        scrollRect.vertical = false;

        Show(onCompletedAction: () =>
        {
            float distance = 0.9f * (firstPackRT.localPosition.y - firstCoinPackRT.localPosition.y);

            _tweens.Add(Tween.LocalPositionY(scrollContent, distance, duration: scrollToCoinPackDuration)
            .OnComplete(() =>
            {
                scrollRect.vertical = true;
            }));
        });

        _prevRoute = prevRoute;
    }

    protected override void Hide()
    {
        base.Hide(onCompletedAction: () =>
        {
            switchRouteEvent?.Invoke(_prevRoute);
        });
    }

    private void OnResourcesEarnPopupShown(bool isShow)
    {
        float start = 0;
        float end = 1;

        if (isShow)
        {
            start = 1;
            end = 0;
        }

        _tweens.Add(Tween.Custom(start, end, duration: 0.3f, onValueChange: newVal =>
        {
            popupCanvasGroup.alpha = newVal;
        }));
    }
}
