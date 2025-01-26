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

    [SerializeField] private Button addMoreScrewPortButton;
    [SerializeField] private Button breakObjectButton;
    [SerializeField] private Button clearAllScrewPortsButton;
    [SerializeField] private Button disableBreakObjectModeEarlyButton;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private Vector2Variable canvasSize;
    [SerializeField] private Vector2Variable canvasSizeOfReferenceDevice;
    [SerializeField] private UserResourcesObserver userResourcesObserver;
    [SerializeField] private ScrewBoxesObserver screwBoxesObserver;
    [SerializeField] private LevelBoosterObserver levelBoosterObserver;

    [Header("CUSTOMIZE")]
    [SerializeField] private float transitionTime;
    [SerializeField] private float waitTimeBetweenClicks;

    public static event Action addMoreScrewPortEvent;
    public static event Action enableBreakObjectModeEvent;
    public static event Action disableBreakObjectModeEvent;
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

    private Vector2 _initialBoosterContainerPosition;
    private Vector2 _initialAddMoreScrewPortButtonPosition;
    private Vector2 _initialbreakObjectButtonPosition;
    private Vector2 _initialClearAllScrewPortsButtonPosition;

    private void Awake()
    {
        ScrewSelectionInput.breakObjectEvent += DisableBreakObjectMode;
        LevelLoader.startLevelEvent += Reset;
        BuyBoosterPopup.useBoosterEvent += UseBooster;
        ScrewSelectionInput.breakObjectEvent += UseBreakObjectBooster;
        ScrewBoxManager.enableBoosterEvent += EnableBooster;
        ScrewManager.enableBoosterEvent += EnableBoosterWithoutWarningPopup;
        ScrewBoxManager.screwPortsClearedEvent += OnAllScrewPortsCleared;
        RevivePopup.reviveEvent += OnRevived;

        addMoreScrewPortButton.onClick.AddListener(() => UseBooster(BoosterType.AddScrewPort));
        breakObjectButton.onClick.AddListener(() => UseBooster(BoosterType.BreakObject));
        clearAllScrewPortsButton.onClick.AddListener(() => UseBooster(BoosterType.ClearScrewPorts));
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
        ScrewSelectionInput.breakObjectEvent -= DisableBreakObjectMode;
        LevelLoader.startLevelEvent -= Reset;
        BuyBoosterPopup.useBoosterEvent -= UseBooster;
        ScrewSelectionInput.breakObjectEvent -= UseBreakObjectBooster;
        ScrewBoxManager.enableBoosterEvent -= EnableBooster;
        ScrewManager.enableBoosterEvent -= EnableBoosterWithoutWarningPopup;
        ScrewBoxManager.screwPortsClearedEvent -= OnAllScrewPortsCleared;
        RevivePopup.reviveEvent -= OnRevived;

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

        if (boosterType == BoosterType.AddScrewPort)
        {
            AddMoreScrewPort();
        }
        else if (boosterType == BoosterType.BreakObject)
        {
            EnableBreakObjectMode();
        }
        else if (boosterType == BoosterType.ClearScrewPorts)
        {
            ClearAllScrewPorts();
        }
    }

    private void AddMoreScrewPort()
    {
        if (!IsBoosterAvailable(boosterIndex: 0))
        {
            showBuyBoosterPopupEvent?.Invoke(BoosterType.AddScrewPort);

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

        ConsumeBooster(boosterIndex: 0);

        addMoreScrewPortEvent?.Invoke();

        _numScrewPortsAdded++;

        if (_numScrewPortsAdded == 2)
        {
            addMoreScrewPortButton.interactable = false;

            Tween.LocalPositionY(addMoreScrewPortButtonRT, addMoreScrewPortButtonRT.localPosition.y - 600, duration: 0.3f);

            Tween.LocalPositionX(breakModeButtonRT, -0.7f * breakModeButtonRT.sizeDelta.x, duration: 0.3f);
            Tween.LocalPositionX(clearAllScrewPortsButtonRT, 0.7f * breakModeButtonRT.sizeDelta.x, duration: 0.3f);
        }

        Tween.Delay(waitTimeBetweenClicks).OnComplete(() =>
        {
            _isInTransition = false;
        });
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

    private void DisableBreakObjectMode()
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

    private void ClearAllScrewPorts()
    {
        if (screwBoxesObserver.NumScrewInScrewPorts == 0)
        {
            // shakeScrewPortsEvent?.Invoke();
            showNotificationEvent?.Invoke(GameConstants.TIPS, GameConstants.NO_SCREW_TO_BE_CLEARED);

            return;
        }

        if (!IsBoosterAvailable(boosterIndex: 2))
        {
            showBuyBoosterPopupEvent?.Invoke(BoosterType.ClearScrewPorts);

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

        clearAllScrewPortsEvent?.Invoke();

        SoundManager.Instance.PlaySoundClearScrewPorts();

        Tween.Delay(waitTimeBetweenClicks).OnComplete(() =>
        {
            _isInTransition = false;
        });
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

    private void UseBreakObjectBooster()
    {
        ConsumeBooster(boosterIndex: 1);
    }

    private void OnAllScrewPortsCleared(int numScrewCleared)
    {
        if (numScrewCleared > 0)
        {
            ConsumeBooster(boosterIndex: 2);
        }
        else
        {
            showNotificationEvent?.Invoke(GameConstants.TIPS, GameConstants.NO_SCREW_TO_BE_CLEARED);
        }
    }
}
