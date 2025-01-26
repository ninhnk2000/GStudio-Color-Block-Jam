using UnityEngine;
using UnityEngine.UI;

public class NoAdsLoadedPopup : BasePopup
{
    [SerializeField] private Button okButton;

    protected override void RegisterMoreEvent()
    {
        okButton.onClick.AddListener(Hide);
    }
}
