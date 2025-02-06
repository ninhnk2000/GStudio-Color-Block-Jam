using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Saferio.Util.SaferioTween;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform loadingContent;
    [SerializeField] private RectTransform progressFillRT;
    [SerializeField] private RectTransform loadingTextRT;
    [SerializeField] private Image fadeBackground;
    [SerializeField] private Image blackBackground;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Image progressBarFill;
    // [SerializeField] private Image progressFill;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private SkeletonGraphic loadingSpine;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("CUSTOMIZE")]
    [SerializeField] private float startDelay;
    [SerializeField] private float blackScreenStartDelay;
    [SerializeField] private float fadeOutDuration;

    private Vector2 _canvasSize;
    private List<Tween> _tweens;
    private bool _isFirstTimeEnterScene;


    private void Awake()
    {
        LevelLoader.showSceneTransitionEvent += BlackScreenTransition;

        _tweens = new List<Tween>();

        _canvasSize = canvas.sizeDelta;

        progressBar.value = 0;
        progressBarFill.color = ColorUtil.WithAlpha(progressBarFill.color, 0);

        _isFirstTimeEnterScene = true;

        if (!DataUtility.Load(GameConstants.IS_FIRST_TIME_OPEN_APP, true))
        {
            BlackScreenTransition();
        }
        else
        {
            blackBackground.gameObject.SetActive(false);

            DataUtility.Save(GameConstants.IS_FIRST_TIME_OPEN_APP, false);
        }
    }

    private void OnDestroy()
    {
        LevelLoader.showSceneTransitionEvent -= BlackScreenTransition;

        CommonUtil.StopAllTweens(_tweens);
    }

    private void Transition()
    {
        container.gameObject.SetActive(true);

        // _tweens.Add(Tween.Alpha(progressBarFill, 0, 1, duration: 0.2f * startDelay));

        _tweens.Add(Tween.Custom(0, 1, duration: startDelay, ease: Ease.Linear, onValueChange: newVal =>
        {
            progressBar.value = newVal;
            progressBarFill.color = ColorUtil.WithAlpha(progressBarFill.color, newVal * 2);

            loadingText.text = $"{(int)(newVal * 100)}%";
        }));

        _tweens.Add(Tween.Delay(startDelay).OnComplete(
            () =>
            {
                _tweens.Add(
                    Tween.Custom(1, 0, duration: fadeOutDuration, onValueChange: newVal =>
                    {
                        canvasGroup.alpha = newVal;
                        loadingSpine.color = ColorUtil.WithAlpha(loadingSpine.color, newVal);
                    })
                    .OnComplete(() =>
                    {
                        container.gameObject.SetActive(false);
                    })
                );
            }
        ));

        //     SaferioTween.DelayAsync(startDelay, onCompletedAction: (() =>
        //     {
        //         _tweens.Add(
        //             Tween.Custom(1, 0, duration: fadeOutDuration, onValueChange: newVal =>
        //             {
        //                 canvasGroup.alpha = newVal;
        //                 loadingSpine.color = ColorUtil.WithAlpha(loadingSpine.color, newVal);
        //             })
        //             .OnComplete(() =>
        //             {
        //                 container.gameObject.SetActive(false);
        //             })
        //         );

        //         // SaferioTween.LocalPositionAsync(loadingContent, new Vector3(0, -_canvasSize.y), duration: 0.6f * fadeOutDuration);
        //         // SaferioTween.AlphaAsync(fadeBackground, 0, duration: fadeOutDuration, onCompletedAction: () =>
        //         // {
        //         //     container.gameObject.SetActive(false);
        //         // });
        //     }));
    }

    private void BlackScreenTransition()
    {
        BlackScreenTransition(blackScreenStartDelay, onCompletedAction: null);
    }

    private void BlackScreenTransition(float delay, Action onCompletedAction)
    {
        if (_isFirstTimeEnterScene)
        {
            _isFirstTimeEnterScene = false;

            return;
        }

        blackBackground.color = ColorUtil.WithAlpha(blackBackground.color, 1);

        blackBackground.gameObject.SetActive(true);

        Tween.Alpha(blackBackground, 1, 0, startDelay: delay, duration: fadeOutDuration).OnComplete(() =>
        {
            blackBackground.gameObject.SetActive(false);

            onCompletedAction?.Invoke();
        });
    }
}
