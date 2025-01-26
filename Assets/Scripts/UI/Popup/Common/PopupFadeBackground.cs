using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class PopupFadeBackground : MonoBehaviour
{
    [SerializeField] private Image fadeBackground;

    [Header("CUSTOMIZE")]
    [SerializeField] private float transitionDuration;

    private Tween _transitionTween;

    void Awake()
    {
        BasePopup.popupShowEvent += OnPopupShow;
        BasePopup.popupHideEvent += OnPopupClose;

        fadeBackground.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        BasePopup.popupShowEvent -= OnPopupShow;
        BasePopup.popupHideEvent -= OnPopupClose;

        CommonUtil.StopTween(_transitionTween);
    }

    private void OnPopupShow()
    {
        fadeBackground.gameObject.SetActive(true);

        CommonUtil.StopTween(_transitionTween);

        _transitionTween = Tween.Alpha(fadeBackground, 0.95f, duration: transitionDuration);
    }

    private void OnPopupClose()
    {
        Transform popupContainer = transform.parent;

        int numActivePopup = 0;

        for (int i = 1; i < popupContainer.childCount; i++)
        {
            if (popupContainer.GetChild(i).gameObject.activeSelf)
            {
                numActivePopup++;
            }
        }

        if (numActivePopup > 1)
        {
            return;
        }

        CommonUtil.StopTween(_transitionTween);

        _transitionTween = Tween.Alpha(fadeBackground, 0, duration: transitionDuration).OnComplete(() =>
        {
            fadeBackground.gameObject.SetActive(false);
        });
    }
}
