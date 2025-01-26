using System;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class TopBar : MonoBehaviour
{
    [SerializeField] private Button openSettingPopupButton;

    public static event Action<ScreenRoute> showPopupEvent;

    void Awake()
    {
        openSettingPopupButton.onClick.AddListener(OpenSettingPopup);
    }

    private void OpenSettingPopup()
    {
        showPopupEvent?.Invoke(ScreenRoute.Setting);
    }
}
