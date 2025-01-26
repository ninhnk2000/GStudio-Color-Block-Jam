using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugPopup : BasePopup
{
    [SerializeField] private TMP_InputField levelInput;

    [SerializeField] private Button debugPlayLevelButton;
    [SerializeField] private Button debugNextLevelButton;
    [SerializeField] private Button debugPrevLevelButton;
    [SerializeField] private Button debugWinLevelButton;
    [SerializeField] private Button debugAdsButton;
    [SerializeField] private Button debugAddBoostersButton;
    [SerializeField] private Button debugAddCoinButton;
    [SerializeField] private TMP_Text debugAdsText;

    [SerializeField] private UserResourcesObserver userResourcesObserver;

    public static event Action<int> toLevelEvent;
    public static event Action nextLevelEvent;
    public static event Action prevLevelEvent;
    public static event Action winLevelEvent;

    protected override void RegisterMoreEvent()
    {
        debugPlayLevelButton.onClick.AddListener(DebugPlayLevel);
        debugNextLevelButton.onClick.AddListener(NextLevel);
        debugPrevLevelButton.onClick.AddListener(PrevLevel);
        debugWinLevelButton.onClick.AddListener(DebugWinLevel);
        debugAdsButton.onClick.AddListener(DebugAdsButton);
        debugAddBoostersButton.onClick.AddListener(AddBooster);
        debugAddCoinButton.onClick.AddListener(AddCoin);
    }

    protected override void MoreActionInAwake()
    {
        if (UserData.IsRemoveAds)
        {
            debugAdsText.text = "Enable Ads";
        }
        else
        {
            debugAdsText.text = "Disable Ads";
        }
    }

    private void DebugPlayLevel()
    {
        int level = int.Parse(levelInput.text);

        toLevelEvent?.Invoke(level);

        Hide();
    }

    private void NextLevel()
    {
        nextLevelEvent?.Invoke();

        Hide();
    }

    private void PrevLevel()
    {
        prevLevelEvent?.Invoke();

        Hide();
    }

    private async void DebugWinLevel()
    {
        Hide();

        await Task.Delay(500);

        winLevelEvent?.Invoke();
    }

    private async void DebugAdsButton()
    {
        if (UserData.IsRemoveAds)
        {
            UserData.IsRemoveAds = false;

            debugAdsText.text = "Disable Ads";
        }
        else
        {
            UserData.IsRemoveAds = true;

            debugAdsText.text = "Enable Ads";
        }
    }

    private void AddBooster()
    {
        for (int i = 0; i < userResourcesObserver.UserResources.BoosterQuantities.Length; i++)
        {
            userResourcesObserver.UserResources.BoosterQuantities[i] += 999;
        }

        userResourcesObserver.Save();
    }

    private void AddCoin()
    {
        userResourcesObserver.UserResources.CoinQuantity += 9999;

        userResourcesObserver.Save();
    }
}
