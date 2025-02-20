using System;
using System.Collections;
using Lean.Localization;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class BoosterTutorial : MonoBehaviour
{
    [SerializeField] private Button claimButton;
    [SerializeField] private Button boosterButton;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup canvasGroupBooster;

    [SerializeField] private RectTransform tutorialHand;
    [SerializeField] private Image tutorialHandImage;
    [SerializeField] private Image blackBackground;

    [Header("CUSTOMIZE")]
    [SerializeField] private BoosterType boosterType;

    private bool _isUsedBooster;

    public static Action<BoosterType> useBoosterType;

    void Awake()
    {
        claimButton.onClick.AddListener(Claim);
        boosterButton.onClick.AddListener(UseBooster);

        canvasGroupBooster.alpha = 0;
        canvasGroupBooster.interactable = false;
    }

    private void Claim()
    {
        Tween.Custom(1, 0, duration: 0.3f, onValueChange: newVal =>
        {
            canvasGroup.alpha = newVal;
        });

        Tween.Custom(0, 1, duration: 0.3f, onValueChange: newVal =>
        {
            canvasGroupBooster.alpha = newVal;
        }).OnComplete(() =>
        {
            canvasGroupBooster.interactable = true;

            StartCoroutine(PlayTutorialHandAnimation());
        });
    }

    private IEnumerator PlayTutorialHandAnimation()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        int phase = 0;

        while (!_isUsedBooster)
        {
            if (phase == 0)
            {
                Tween.Scale(tutorialHand, 1.3f, duration: 0.3f)
                .OnComplete(() =>
                {
                    phase = 1;
                });
            }
            else if (phase == 1)
            {
                Tween.Scale(tutorialHand, 1f, duration: 0.3f)
                .OnComplete(() =>
                {
                    phase = 0;
                });
            }

            yield return waitForSeconds;
        }
    }

    private void UseBooster()
    {
        _isUsedBooster = true;

        Tween.Alpha(blackBackground, 0, duration: 0.3f);
        Tween.Alpha(tutorialHandImage, 0, duration: 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });

        useBoosterType?.Invoke(boosterType);
    }
}
