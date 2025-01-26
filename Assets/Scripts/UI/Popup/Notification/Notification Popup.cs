using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopup : BasePopup
{
    [SerializeField] private LeanLocalizedTextMeshProUGUI localizedTitle;
    [SerializeField] private LeanLocalizedTextMeshProUGUI localizedDescription;
    [SerializeField] private Button okButton;

    protected override void RegisterMoreEvent()
    {
        BoosterUI.showNotificationEvent += ShowNotification;
        IAPPackageUI.showNotificationEvent += ShowNotification;
        IAPCoinItemUI.showNotificationEvent += ShowNotification;
        RemoveAdPopup.showNotificationEvent += ShowNotification;

        okButton.onClick.AddListener(Hide);
    }

    protected override void UnregisterMoreEvent()
    {
        BoosterUI.showNotificationEvent -= ShowNotification;
        IAPPackageUI.showNotificationEvent -= ShowNotification;
        IAPCoinItemUI.showNotificationEvent -= ShowNotification;
        RemoveAdPopup.showNotificationEvent -= ShowNotification;
    }

    private void ShowNotification(string titleTranslation, string descriptionTranslation)
    {
        localizedTitle.TranslationName = titleTranslation;
        localizedDescription.TranslationName = descriptionTranslation;

        Show();
    }
}
