using System;
using PrimeTween;
using Saferio.Util.SaferioTween;
using UnityEngine;
using UnityEngine.UI;

public class SaferioToggle : MonoBehaviour
{
    [SerializeField] private Image activeBackground;
    [SerializeField] private Image inactiveBackground;
    [SerializeField] private RectTransform icon;
    [SerializeField] private Button switchButton;

    [SerializeField] private float transitionDuration;

    private bool _isOn;

    private Action<bool> onSwitched;

    private void Awake()
    {
        switchButton.onClick.AddListener(Switch);
    }

    public void RegisterOnSwitchedEvent(Action<bool> action)
    {
        onSwitched = action;
    }

    public void SetState(bool isOn)
    {
        if (isOn)
        {
            activeBackground.color = ColorUtil.WithAlpha(activeBackground.color, 1);
            // inactiveBackground.color = ColorUtil.WithAlpha(activeBackground.color, 0);
            icon.localPosition = icon.localPosition.ChangeX(52);
        }
        else
        {
            activeBackground.color = ColorUtil.WithAlpha(activeBackground.color, 0);
            // inactiveBackground.color = ColorUtil.WithAlpha(activeBackground.color, 1);
            icon.localPosition = icon.localPosition.ChangeX(-52);
        }

        _isOn = isOn;
    }

    public void Switch()
    {
        _isOn = !_isOn;

        float inactiveBackgroundAlpha = 0;
        float activeBackgroundAlpha = 1;
        float iconLocalPositionX = 52;

        if (!_isOn)
        {
            inactiveBackgroundAlpha = 1;
            activeBackgroundAlpha = 0;
            iconLocalPositionX = -52;
        }

        Tween.Alpha(activeBackground, activeBackground.color.a, activeBackgroundAlpha, duration: transitionDuration);
        // Tween.Alpha(inactiveBackground, inactiveBackground.color.a, inactiveBackgroundAlpha, duration: transitionDuration);

        Tween.LocalPositionX(icon, iconLocalPositionX, duration: transitionDuration)
        .OnComplete(() =>
        {
            onSwitched?.Invoke(_isOn);
        });
    }
}
