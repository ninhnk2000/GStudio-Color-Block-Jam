using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class MultiPhaseMilestoneUI : MonoBehaviour
{
    [SerializeField] private Image inProgressImage;
    [SerializeField] private Image doneImage;

    [SerializeField] private float transitionDuration;

    private int _phaseIndex;
    private List<Tween> _tweens;

    public int PhaseIndex
    {
        get => _phaseIndex;
        set => _phaseIndex = value;
    }

    private void Awake()
    {
        MultiPhaseLevelManager.phaseCompletedEvent += OnPhaseCompleted;

        _tweens = new List<Tween>();

        doneImage.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        MultiPhaseLevelManager.phaseCompletedEvent -= OnPhaseCompleted;

        CommonUtil.StopAllTweens(_tweens);
    }

    private void OnPhaseCompleted(int nextPhase)
    {
        if (nextPhase == _phaseIndex)
        {
            doneImage.gameObject.SetActive(true);

            doneImage.color = ColorUtil.WithAlpha(doneImage.color, 0);

            _tweens.Add(Tween.Alpha(inProgressImage, 0, duration: transitionDuration));
            _tweens.Add(Tween.Alpha(doneImage, 1, duration: transitionDuration));
        }
    }
}
