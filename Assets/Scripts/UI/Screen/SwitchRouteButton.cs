using System;
using PrimeTween;
using Saferio.Util.SaferioTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class SwitchRouteButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private RectTransform iconRT;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text text;

    [Header("CUSTOMIZE")]
    [SerializeField] private ScreenRoute route;

    #region PRIVATE FIELD
    private Vector3 _iconInitialPosition;
    #endregion

    #region ACTION
    public static event Action<ScreenRoute> switchRouteEvent;
    #endregion

    private void Awake()
    {
        switchRouteEvent += OnRouteSwitched;

        button.onClick.AddListener(SwitchRoute);

        _iconInitialPosition = iconRT.localPosition;
    }

    private void OnDestroy()
    {
        switchRouteEvent -= OnRouteSwitched;
    }

    private void SwitchRoute()
    {
        switchRouteEvent?.Invoke(route);
    }

    private void OnRouteSwitched(ScreenRoute route)
    {
        if (route == this.route)
        {
            HandleEnterRoute();
        }
        else
        {
            HandleExitRoute();
        }
    }

    private void HandleEnterRoute()
    {
        SaferioTween.LocalPositionAsync(iconRT, _iconInitialPosition + new Vector3(0, 0.2f * iconRT.sizeDelta.y), duration: 0.2f);

        Tween.Color(icon, Color.red, duration: 0.2f);
        Tween.Color(text, Color.red, duration: 0.2f);
    }

    private void HandleExitRoute()
    {
        SaferioTween.LocalPositionAsync(iconRT, _iconInitialPosition, duration: 0.2f);

        Tween.Color(icon, Color.white, duration: 0.2f);
        Tween.Color(text, Color.white, duration: 0.2f);
    }
}
