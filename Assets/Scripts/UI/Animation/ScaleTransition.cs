using System;
using PrimeTween;
using Saferio.Util.SaferioTween;
using UnityEngine;

public class ScaleTransition : MonoBehaviour, ISaferioUIAnimation
{
    [SerializeField] private RectTransform target;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private Vector2Variable canvasSize;

    [Header("CUSTOMIZE")]
    [SerializeField] private float duration;

    #region PRIVATE FIELD
    private bool _isInTransition;
    #endregion

    public void Show()
    {
        if (_isInTransition)
        {
            return;
        }
        else
        {
            _isInTransition = true;
        }

        target.gameObject.SetActive(true);

        target.localScale = Vector3.zero;

        // SaferioTween.ScaleAsync(target, 1.1f * Vector3.one, duration: 0.5f * duration, onCompletedAction: () =>
        // {
        //     SaferioTween.ScaleAsync(target, Vector3.one, duration: 0.5f * duration);
        // });

        // canvasGroup.alpha = 0;

        // Tween.Custom(0, 1, duration: 0.5f * duration, onValueChange: newVal =>
        // {
        //     canvasGroup.alpha = newVal;
        // });

        canvasGroup.interactable = false;

        Tween.Scale(target, 1.1f, duration: 0.5f * duration)
            .Chain(Tween.Scale(target, 1f, duration: 0.5f * duration).OnComplete(() =>
            {
                _isInTransition = false;

                canvasGroup.interactable = true;
            }));

        // SaferioTween.LocalPositionAsync(target, Vector2.zero, duration: duration);
    }

    public void Show(Action onCompletedAction)
    {
        if (_isInTransition)
        {
            return;
        }
        else
        {
            _isInTransition = true;
        }

        target.gameObject.SetActive(true);

        target.localScale = Vector3.zero;

        canvasGroup.interactable = false;

        Tween.Scale(target, 1.1f, duration: 0.5f * duration)
            .Chain(Tween.Scale(target, 1f, duration: 0.5f * duration).OnComplete(() =>
            {
                _isInTransition = false;

                canvasGroup.interactable = true;

                onCompletedAction?.Invoke();
            }));

        canvasGroup.alpha = 0;

        Tween.Custom(0, 1, duration: 0.5f * duration, onValueChange: newVal =>
        {
            canvasGroup.alpha = newVal;
        });
    }

    public void Hide()
    {
        Hide(onCompletedAction: null);
    }

    public void Hide(Action onCompletedAction)
    {
        if (_isInTransition)
        {
            return;
        }
        else
        {
            _isInTransition = true;
        }

        canvasGroup.interactable = false;

        Tween.Scale(target, 1.1f, duration: 0.5f * duration).OnComplete(() =>
        {
            Tween.Scale(target, 0.5f, duration: 0.5f * duration).OnComplete(() =>
            {
                target.gameObject.SetActive(false);

                _isInTransition = false;

                onCompletedAction?.Invoke();
            });

            Tween.Custom(1, 0, duration: 0.5f * duration, onValueChange: newVal =>
            {
                canvasGroup.alpha = newVal;
            });
        });
    }
}
