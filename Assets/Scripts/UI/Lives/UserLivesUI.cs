using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class UserLivesUI : MonoBehaviour
{
    [SerializeField] private TMP_Text numLivesText;
    [SerializeField] private TMP_Text replenishLifeTimeText;
    [SerializeField] private Button openLivesShopPopupButton;

    private LivesData _livesData;

    public static Action<ScreenRoute> switchRouteEvent;

    void Awake()
    {
        ReplenishLifeManager.updateLivesReplenishTimeEvent += UpdateLivesReplenishTime;
        ReplenishLifeManager.updateLivesNumberEvent += UpdateLivesNumber;

        openLivesShopPopupButton.onClick.AddListener(OpenLivesShopPopup);
    }

    void OnDestroy()
    {
        ReplenishLifeManager.updateLivesReplenishTimeEvent -= UpdateLivesReplenishTime;
        ReplenishLifeManager.updateLivesNumberEvent -= UpdateLivesNumber;
    }

    private void UpdateLivesReplenishTime(string time)
    {
        replenishLifeTimeText.text = time;
    }

    private void UpdateLivesNumber(int lives)
    {
        numLivesText.text = $"{lives}";

        if (lives == GameConstants.DEFAULT_LIVES)
        {
            replenishLifeTimeText.text = $"Full";
        }
    }

    private void OpenLivesShopPopup()
    {
        switchRouteEvent?.Invoke(ScreenRoute.LivesShop);
    }
}
