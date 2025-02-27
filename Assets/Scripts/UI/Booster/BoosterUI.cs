using System;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class BoosterUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform boosterContainer;
    [SerializeField] private RectTransform addMoreScrewPortButtonRT;
    [SerializeField] private RectTransform breakModeButtonRT;
    [SerializeField] private RectTransform clearAllScrewPortsButtonRT;
    [SerializeField] private RectTransform breakModeContainer;
    [SerializeField] private Image freezedBackground;

    [SerializeField] private Button addMoreScrewPortButton;
    [SerializeField] private Button breakObjectButton;
    [SerializeField] private Button clearAllScrewPortsButton;
    [SerializeField] private Button disableBreakObjectModeEarlyButton;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private Vector2Variable canvasSize;
    [SerializeField] private Vector2Variable canvasSizeOfReferenceDevice;
    [SerializeField] private UserResourcesObserver userResourcesObserver;
    [SerializeField] private LevelBoosterObserver levelBoosterObserver;

    [Header("CUSTOMIZE")]
    [SerializeField] private float transitionTime;

    public static event Action addMoreScrewPortEvent;
    public static event Action enableBreakObjectModeEvent;
    public static event Action disableBreakObjectModeEvent;
    public static event Action<bool> enableVacumnModeEvent;
    public static event Action clearAllScrewPortsEvent;
    public static event Action<BoosterType> showBuyBoosterPopupEvent;
    public static event Action<int> updateBoosterQuantityEvent;
    public static event Action shakeScrewPortsEvent;
    public static event Action<string, string> showNotificationEvent;

    private List<Tween> _tweens;
    private int _numScrewPortsAdded;
    private bool _isInTransition;
    private bool _isEnable;
    private bool _isPreventShowWarningPopup;
    private bool _isTimeFreezing;

    private Vector2 _initialBoosterContainerPosition;
    private Vector2 _initialAddMoreScrewPortButtonPosition;
    private Vector2 _initialbreakObjectButtonPosition;
    private Vector2 _initialClearAllScrewPortsButtonPosition;

    public static event Action freezeTimeEvent;

    private void Awake()
    {
        LevelLoader.startLevelEvent += Reset;
        BuyBoosterPopup.useBoosterEvent += UseBooster;
        BoosterTutorial.useBoosterType += UseBooster;
        ScrewBoxManager.enableBoosterEvent += EnableBooster;
        ScrewManager.enableBoosterEvent += EnableBoosterWithoutWarningPopup;
        RevivePopup.reviveEvent += OnRevived;
        BlockSelectionInput.breakObjectEvent += ConfirmBreakObject;
        BlockSelectionInput.vacumnEvent += ConfirmVacumnBooster;
        LevelTimeCounter.unfreezeTimeEvent += UnfreezeTime;

        addMoreScrewPortButton.onClick.AddListener(() => UseBooster(BoosterType.FreezeTime));
        breakObjectButton.onClick.AddListener(() => UseBooster(BoosterType.BreakObject));
        clearAllScrewPortsButton.onClick.AddListener(() => UseBooster(BoosterType.Vacumn));
        disableBreakObjectModeEarlyButton.onClick.AddListener(DisableBreakObjectModeEarly);

        _tweens = new List<Tween>();

        _isEnable = true;
    }

    private void Start()
    {
        _initialBoosterContainerPosition = boosterContainer.localPosition;
        _initialAddMoreScrewPortButtonPosition = addMoreScrewPortButtonRT.localPosition;
        _initialbreakObjectButtonPosition = breakModeButtonRT.localPosition;
        _initialClearAllScrewPortsButtonPosition = clearAllScrewPortsButtonRT.localPosition;
    }

    void OnDestroy()
    {
        LevelLoader.startLevelEvent -= Reset;
        BuyBoosterPopup.useBoosterEvent -= UseBooster;
        BoosterTutorial.useBoosterType -= UseBooster;
        ScrewBoxManager.enableBoosterEvent -= EnableBooster;
        ScrewManager.enableBoosterEvent -= EnableBoosterWithoutWarningPopup;
        RevivePopup.reviveEvent -= OnRevived;
        BlockSelectionInput.breakObjectEvent -= DisableBreakObjectMode;
        BlockSelectionInput.vacumnEvent -= DisableVacumnMode;
        LevelTimeCounter.unfreezeTimeEvent -= UnfreezeTime;

        CommonUtil.StopAllTweens(_tweens);
    }

    private void Reset()
    {
        Tween.LocalPosition(boosterContainer, _initialBoosterContainerPosition, duration: transitionTime);
        Tween.LocalPosition(addMoreScrewPortButtonRT, _initialAddMoreScrewPortButtonPosition, duration: transitionTime);
        Tween.LocalPosition(breakModeButtonRT, _initialbreakObjectButtonPosition, duration: transitionTime);
        Tween.LocalPosition(clearAllScrewPortsButtonRT, _initialClearAllScrewPortsButtonPosition, duration: transitionTime);

        breakModeContainer.gameObject.SetActive(false);

        addMoreScrewPortButton.interactable = true;

        _numScrewPortsAdded = 0;
        _isInTransition = false;
        _isEnable = true;
        _isPreventShowWarningPopup = false;
        _isTimeFreezing = false;
    }

    private void OnRevived(BoosterType boosterType)
    {
        _isEnable = false;
        _isPreventShowWarningPopup = false;

        _tweens.Add(Tween.Delay(2f).OnComplete(() =>
        {
            _isEnable = true;
            _isPreventShowWarningPopup = true;
        }));
    }

    private void UseBooster(BoosterType boosterType)
    {
        if (!_isEnable)
        {
            if (!_isPreventShowWarningPopup)
            {
                showNotificationEvent?.Invoke(GameConstants.WARNING, GameConstants.TOO_FAST_ACTION);
            }

            return;
        }

        if (boosterType == BoosterType.FreezeTime)
        {
            FreezeTime();
        }
        else if (boosterType == BoosterType.BreakObject)
        {
            EnableBreakObjectMode();
        }
        else if (boosterType == BoosterType.Vacumn)
        {
            EnableVacumnMode();
        }
    }

    private void FreezeTime()
    {
        if (_isTimeFreezing)
        {
            showNotificationEvent?.Invoke(GameConstants.WARNING, GameConstants.BOOSTER_IN_USE);

            return;
        }

        if (!IsBoosterAvailable(boosterIndex: 0))
        {
            showBuyBoosterPopupEvent?.Invoke(BoosterType.FreezeTime);

            return;
        }

        freezeTimeEvent?.Invoke();

        ConsumeBooster(0);

        _isTimeFreezing = true;
    }

    private void UnfreezeTime()
    {
        _isTimeFreezing = false;
    }

    private void EnableBreakObjectMode()
    {
        if (!IsBoosterAvailable(boosterIndex: 1))
        {
            showBuyBoosterPopupEvent?.Invoke(BoosterType.BreakObject);

            return;
        }

        if (_isInTransition)
        {
            return;
        }
        else
        {
            _isInTransition = true;
        }

        breakModeContainer.gameObject.SetActive(true);

        Tween.LocalPositionY(boosterContainer, -canvasSizeOfReferenceDevice.Value.y, duration: transitionTime);
        Tween.LocalPositionY(breakModeContainer, _initialBoosterContainerPosition.y + 0.5f * breakModeContainer.sizeDelta.y, duration: transitionTime)
        .OnComplete(() =>
        {
            enableBreakObjectModeEvent?.Invoke();

            _isInTransition = false;
        });
    }

    private void ConfirmBreakObject()
    {
        ConsumeBooster(boosterIndex: (int)BoosterType.BreakObject);

        DisableBreakObjectMode();
    }

    private void DisableBreakObjectMode()
    {
        Tween.LocalPositionY(boosterContainer, _initialBoosterContainerPosition.y, duration: transitionTime);
        Tween.LocalPositionY(breakModeContainer, -canvasSizeOfReferenceDevice.Value.y, duration: transitionTime)
        .OnComplete(() =>
        {
            breakModeContainer.gameObject.SetActive(false);
        });
    }

    private void EnableVacumnMode()
    {
        if (!IsBoosterAvailable(boosterIndex: 2))
        {
            showBuyBoosterPopupEvent?.Invoke(BoosterType.Vacumn);

            return;
        }

        if (_isInTransition)
        {
            return;
        }
        else
        {
            _isInTransition = true;
        }

        breakModeContainer.gameObject.SetActive(true);

        Tween.LocalPositionY(boosterContainer, -canvasSizeOfReferenceDevice.Value.y, duration: transitionTime);
        Tween.LocalPositionY(breakModeContainer, _initialBoosterContainerPosition.y + 0.5f * breakModeContainer.sizeDelta.y, duration: transitionTime)
        .OnComplete(() =>
        {
            enableVacumnModeEvent?.Invoke(true);

            _isInTransition = false;
        });
    }

    private void ConfirmVacumnBooster(GameFaction faction, Vector3 vacumnPosition)
    {
        ConsumeBooster(boosterIndex: (int)BoosterType.Vacumn);

        DisableVacumnMode(faction, vacumnPosition);
    }

    private void DisableVacumnMode(GameFaction faction, Vector3 vacumnPosition)
    {
        Tween.LocalPositionY(boosterContainer, _initialBoosterContainerPosition.y, duration: transitionTime);
        Tween.LocalPositionY(breakModeContainer, -canvasSizeOfReferenceDevice.Value.y, duration: transitionTime)
        .OnComplete(() =>
        {
            breakModeContainer.gameObject.SetActive(false);
        });
    }

    private void DisableBreakObjectModeEarly()
    {
        DisableBreakObjectMode();

        disableBreakObjectModeEvent?.Invoke();
    }

    private void EnableBooster(bool isEnable)
    {
        _isEnable = isEnable;
    }

    private void EnableBoosterWithoutWarningPopup(bool isEnable)
    {
        _isEnable = isEnable;
        _isPreventShowWarningPopup = true;
    }

    private bool IsBoosterAvailable(int boosterIndex)
    {
        userResourcesObserver.Load();

        if (userResourcesObserver.UserResources.BoosterQuantities[boosterIndex] > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ConsumeBooster(int boosterIndex)
    {
        userResourcesObserver.ConsumeBooster(boosterIndex: boosterIndex);
        levelBoosterObserver.UseBooster(boosterIndex);

        updateBoosterQuantityEvent?.Invoke(boosterIndex);

        SaferioTracking.TrackBoosterUsage((BoosterType)boosterIndex);
    }
}
