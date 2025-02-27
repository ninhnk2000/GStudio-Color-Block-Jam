using System;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static GameEnum;

public class BasePopup : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] protected RectTransform container;
    [SerializeField] private Button closeButton;

    [Header("CUSTOMIZE")]
    [SerializeField] private ScreenRoute route;
    [SerializeField] private bool isRouteActiveFromStart;
    [SerializeField] protected bool isAbleToHideOnClickOutside;

    #region PRIVATE FIELD
    private ISaferioUIAnimation _transitionAnimation;
    protected bool _isShown;
    protected bool _isInTransition;
    protected List<Tween> _tweens;
    #endregion

    public static event Action popupShowEvent;
    public static event Action popupHideEvent;
    public static event Action<bool> enableSwipingScreenEvent;

    private void Awake()
    {
        SwitchRouteButton.switchRouteEvent += OnRouteSwitched;
        TopBar.showPopupEvent += OnRouteSwitched;
        MenuScreen.switchRouteEvent += OnRouteSwitched;
        GameplayScreen.switchRouteEvent += OnRouteSwitched;
        SettingPopup.switchRouteEvent += OnRouteSwitched;
        IAPShopPopup.switchRouteEvent += OnRouteSwitched;
        BuyBoosterPopup.switchRouteEvent += OnRouteSwitched;
        RevivePopup.switchRouteEvent += OnRouteSwitched;
        UserLivesUI.switchRouteEvent += OnRouteSwitched;
        LivesShopPopup.switchRouteEvent += OnRouteSwitched;
        ReplenishLifeManager.switchRouteEvent += OnRouteSwitched;
        ReplayPopup.switchRouteEvent += OnRouteSwitched;
        PausePopup.switchRouteEvent += OnRouteSwitched;
        ReturnHomePopup.switchRouteEvent += OnRouteSwitched;

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Hide);
        }

        RegisterMoreEvent();

        _tweens = new List<Tween>();
        _transitionAnimation = GetComponent<ISaferioUIAnimation>();

        if (isRouteActiveFromStart)
        {
            gameObject.SetActive(true);

            _isShown = true;
        }
        else
        {
            gameObject.SetActive(false);
        }

        MoreActionInAwake();
    }

    void Update()
    {
        HideOnClickOutside();
    }

    private void OnDestroy()
    {
        SwitchRouteButton.switchRouteEvent -= OnRouteSwitched;
        TopBar.showPopupEvent -= OnRouteSwitched;
        MenuScreen.switchRouteEvent -= OnRouteSwitched;
        GameplayScreen.switchRouteEvent -= OnRouteSwitched;
        SettingPopup.switchRouteEvent -= OnRouteSwitched;
        IAPShopPopup.switchRouteEvent -= OnRouteSwitched;
        BuyBoosterPopup.switchRouteEvent -= OnRouteSwitched;
        RevivePopup.switchRouteEvent -= OnRouteSwitched;
        UserLivesUI.switchRouteEvent -= OnRouteSwitched;
        LivesShopPopup.switchRouteEvent -= OnRouteSwitched;
        ReplenishLifeManager.switchRouteEvent -= OnRouteSwitched;
        ReplayPopup.switchRouteEvent -= OnRouteSwitched;
        PausePopup.switchRouteEvent -= OnRouteSwitched;
        ReturnHomePopup.switchRouteEvent -= OnRouteSwitched;

        UnregisterMoreEvent();

        CommonUtil.StopAllTweens(_tweens);

        MoreActionOnDestroy();
    }

    protected virtual void RegisterMoreEvent()
    {

    }

    protected virtual void UnregisterMoreEvent()
    {

    }

    protected virtual void MoreActionInAwake()
    {

    }

    protected virtual void MoreActionOnDestroy()
    {

    }

    private void OnRouteSwitched(ScreenRoute route)
    {
        if (route == this.route)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    protected virtual void Show(Action onCompletedAction = null)
    {
        if (_isInTransition)
        {
            return;
        }

        gameObject.SetActive(true);

        _transitionAnimation.Show(OnTransitionCompleted);

        popupShowEvent?.Invoke();

        _isShown = true;
        _isInTransition = true;

        enableSwipingScreenEvent?.Invoke(false);

        void OnTransitionCompleted()
        {
            _isInTransition = false;

            onCompletedAction?.Invoke();
        }
    }

    protected void Show()
    {
        Show(onCompletedAction: null);
    }

    protected virtual void Hide(Action onCompletedAction = null)
    {
        if (_isInTransition)
        {
            return;
        }

        if (_isShown)
        {
            _transitionAnimation.Hide(OnTransitionCompleted);

            popupHideEvent?.Invoke();

            _isShown = false;
            _isInTransition = true;

            AfterHide();

            SoundManager.Instance.PlaySoundClose();

            enableSwipingScreenEvent?.Invoke(true);
        }

        void OnTransitionCompleted()
        {
            _isInTransition = false;

            onCompletedAction?.Invoke();

            gameObject.SetActive(false);
        }
    }

    protected virtual void Hide()
    {
        Hide(onCompletedAction: null);
    }

    protected virtual void HideImmediately()
    {
        if (_isInTransition)
        {
            return;
        }

        if (_isShown)
        {
            gameObject.SetActive(false);

            popupHideEvent?.Invoke();

            _isShown = false;

            AfterHide();

            enableSwipingScreenEvent?.Invoke(true);
        }
    }

    protected virtual void AfterHide()
    {

    }

    private void HideOnClickOutside()
    {
        if (!isAbleToHideOnClickOutside)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var raycastResults = new System.Collections.Generic.List<RaycastResult>();

            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            bool clickedOutside = true;

            foreach (var result in raycastResults)
            {
                if (result.gameObject == container.gameObject)
                {
                    clickedOutside = false;

                    break;
                }
            }

            if (clickedOutside)
            {
                Hide();
            }
        }
    }
}
