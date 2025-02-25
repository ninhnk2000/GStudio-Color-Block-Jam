using System;
using System.Linq;
using System.Threading.Tasks;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class BoosterItemUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform quantityTextContainer;
    [SerializeField] private RectTransform quantityTextRT;
    [SerializeField] private RectTransform addButtonRT;
    [SerializeField] private Image boosterBackground;
    [SerializeField] private Image icon;

    #region LOCK
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject levelToUnlockContainer;
    [SerializeField] private LeanLocalizedTextMeshProUGUI levelToUnlockText;
    #endregion

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Button useBoosterButton;
    [SerializeField] private Button addButton;

    [SerializeField] private Sprite[] boosterBackgroundSprites;
    [SerializeField] private Sprite disabledBoosterBackgroundSprites;
    [SerializeField] private Sprite disabledIconSprites;
    [SerializeField] private Sprite activeIconSprites;

    [Header("CUSTOMIZE")]
    [SerializeField] private int boosterIndex;
    [SerializeField] private int levelToUnlock;
    [SerializeField] private bool isTutorial;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private UserResourcesObserver userResourcesObserver;
    [SerializeField] private IntVariable currentLevel;

    public static event Action<BoosterType> showBuyBoosterPopupEvent;
    public static event Action<BoosterType> useBoosterEvent;

    void Awake()
    {
        BuyBoosterPopup.updateBoosterQuantityEvent += UpdateQuantityText;
        BoosterUI.updateBoosterQuantityEvent += UpdateQuantityText;
        ResourceEarnPopup.showResourcesEarnPopupEvent += OnResourceEarnPopupShow;
        LevelLoader.startLevelEvent += OnLevelStarted;

        useBoosterButton.onClick.AddListener(UseBooster);
        addButton.onClick.AddListener(ShowBuyBoosterPopup);
    }

    void OnDestroy()
    {
        BuyBoosterPopup.updateBoosterQuantityEvent -= UpdateQuantityText;
        BoosterUI.updateBoosterQuantityEvent -= UpdateQuantityText;
        ResourceEarnPopup.showResourcesEarnPopupEvent -= OnResourceEarnPopupShow;
        LevelLoader.startLevelEvent -= OnLevelStarted;
    }

    private void OnLevelStarted()
    {
        Setup();
    }

    private async void Setup()
    {
        userResourcesObserver.Load();
        currentLevel.Load();

        int quantity = userResourcesObserver.UserResources.BoosterQuantities[boosterIndex];

        bool isLock = currentLevel.Value < levelToUnlock;

        Lock(isLocked: isLock);

        if (quantity > 0)
        {
            if (!isLock)
            {
                quantityTextContainer.gameObject.SetActive(true);
                addButtonRT.gameObject.SetActive(false);

                quantityText.text = $"{quantity}";
            }
        }
        else
        {
            quantityTextContainer.gameObject.SetActive(false);

            if (!isLock)
            {
                addButtonRT.gameObject.SetActive(true);
            }
        }
    }

    private void Lock(bool isLocked)
    {
        if (isTutorial)
        {
            return;
        }

        lockIcon.SetActive(isLocked);
        levelToUnlockContainer.SetActive(isLocked);

        quantityTextContainer.gameObject.SetActive(!isLocked);
        addButton.gameObject.SetActive(!isLocked);

        canvasGroup.interactable = !isLocked;

        if (isLocked)
        {
            levelToUnlockText.UpdateTranslationWithParameter(GameConstants.LEVEL_PARAMETER, $"{levelToUnlock}");

            boosterBackground.sprite = disabledBoosterBackgroundSprites;
            icon.sprite = disabledIconSprites;
        }
        else
        {
            LevelDifficulty levelDifficulty = CommonUtil.GetLevelDifficulty(currentLevel.Value);

            boosterBackground.sprite = boosterBackgroundSprites[(int)levelDifficulty];
            icon.sprite = activeIconSprites;
        }
    }

    private void UseBooster()
    {
        useBoosterEvent?.Invoke((BoosterType)boosterIndex);
    }

    private void ShowBuyBoosterPopup()
    {
        showBuyBoosterPopupEvent?.Invoke((BoosterType)boosterIndex);
    }

    private void UpdateQuantityText(int boosterIndex)
    {
        if (boosterIndex == this.boosterIndex)
        {
            userResourcesObserver.Load();

            int quantity = userResourcesObserver.UserResources.BoosterQuantities[boosterIndex];

            bool isLock = currentLevel.Value < levelToUnlock;

            if (quantity > 0)
            {
                quantityTextContainer.gameObject.SetActive(true);
                addButtonRT.gameObject.SetActive(false);

                quantityText.text = $"{quantity}";
            }
            else
            {
                quantityTextContainer.gameObject.SetActive(false);

                if (!isLock)
                {
                    addButtonRT.gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnResourceEarnPopupShow(bool isShow)
    {
        if (!isShow)
        {
            userResourcesObserver.Load();

            int quantity = userResourcesObserver.UserResources.BoosterQuantities[boosterIndex];

            bool isLock = currentLevel.Value < levelToUnlock;

            if (quantity > 0)
            {
                quantityTextContainer.gameObject.SetActive(true);
                addButtonRT.gameObject.SetActive(false);

                quantityText.text = $"{quantity}";
            }
            else
            {
                quantityTextContainer.gameObject.SetActive(false);

                if (!isLock)
                {
                    addButtonRT.gameObject.SetActive(true);
                }
            }
        }
    }
}
