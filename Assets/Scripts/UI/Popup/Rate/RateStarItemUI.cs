using System;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class RateStarItemUI : MonoBehaviour
{
    [SerializeField] private Image activeState;
    [SerializeField] private Button selectButton;

    [SerializeField] private int index;
    [SerializeField] private float transitionDuration;

    private List<Tween> _tweens;

    public static event Action<int> selectRateStarEvent;

    public int Index
    {
        get => index;
    }

    private void Awake()
    {
        selectRateStarEvent += OnRateStarSelected;

        selectButton.onClick.AddListener(Select);

        _tweens = new List<Tween>();
    }

    void OnEnable()
    {
        activeState.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        selectRateStarEvent -= OnRateStarSelected;

        CommonUtil.StopAllTweens(_tweens);
    }

    public void ChangeState(bool _isActive)
    {
        if (_isActive == activeState.gameObject.activeSelf)
        {
            return;
        }

        if (_isActive)
        {
            activeState.gameObject.SetActive(true);

            activeState.color = ColorUtil.WithAlpha(activeState.color, 0);

            _tweens.Add(Tween.Alpha(activeState, activeState.color.a, 1, duration: transitionDuration));
        }
        else
        {
            _tweens.Add(Tween.Alpha(activeState, activeState.color.a, 0, duration: transitionDuration)
            .OnComplete(() =>
            {
                activeState.gameObject.SetActive(false);
            }));
        }
    }

    private void Select()
    {
        ChangeState(true);

        selectRateStarEvent?.Invoke(index);
    }

    private void OnRateStarSelected(int selectedIndex)
    {
        if (index < selectedIndex)
        {
            ChangeState(true);
        }
        else if (index > selectedIndex)
        {
            ChangeState(false);
        }
    }
}
