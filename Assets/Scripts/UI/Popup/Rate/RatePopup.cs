using System;
using UnityEngine;
using UnityEngine.UI;

public class RatePopup : BasePopup
{
    [SerializeField] private Button rateButton;

    private int _numStar;

    public static event Action launchInAppReviewEvent;

    protected override void RegisterMoreEvent()
    {
        RateStarItemUI.selectRateStarEvent += SelectRateStar;

        rateButton.onClick.AddListener(Rate);
    }

    protected override void UnregisterMoreEvent()
    {
        RateStarItemUI.selectRateStarEvent -= SelectRateStar;
    }

    private void Rate()
    {
        if (_numStar == 5)
        {
            OpenReviewPage();
        }
        else
        {
            Hide();
        }

        // launchInAppReviewEvent?.Invoke();
    }

    private void SelectRateStar(int index)
    {
        _numStar = index + 1;
    }

    public void OpenReviewPage()
    {
        string url = "https://play.google.com/store/apps/details?id=com.gplay.wood.cube.out";

        Application.OpenURL(url);
    }
}
