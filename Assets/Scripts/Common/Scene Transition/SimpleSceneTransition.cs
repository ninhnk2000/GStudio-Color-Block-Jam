using System;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class SimpleSceneTransition : MonoBehaviour
{
    [SerializeField] private Image blackBackground;

    [Header("CUSTOMIZE")]
    [SerializeField] private float startDelay;
    [SerializeField] private float fadeOutDuration;

    private List<Tween> _tweens;

    private void Awake()
    {
        MenuScreen.showMenuBlackScreenEvent += Show;

        _tweens = new List<Tween>();

        // if (!DataUtility.Load(GameConstants.IS_FIRST_TIME_OPEN_APP, true))
        // {
        //     blackBackground.gameObject.SetActive(false);

        //     DataUtility.Save(GameConstants.IS_FIRST_TIME_OPEN_APP, false);
        // }
    }

    private void OnDestroy()
    {
        MenuScreen.showMenuBlackScreenEvent -= Show;

        CommonUtil.StopAllTweens(_tweens);
    }

    private void Show(bool isShow)
    {
        blackBackground.gameObject.SetActive(isShow);
    }

    private void BlackScreenTransition(float delay, Action onCompletedAction)
    {
        blackBackground.color = ColorUtil.WithAlpha(blackBackground.color, 1);

        blackBackground.gameObject.SetActive(true);

        _tweens.Add(Tween.Alpha(blackBackground, 1, 0, startDelay: delay, duration: fadeOutDuration).OnComplete(() =>
        {
            blackBackground.gameObject.SetActive(false);

            onCompletedAction?.Invoke();
        }));
    }
}
