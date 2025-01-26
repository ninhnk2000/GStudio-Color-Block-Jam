
using System;
using PrimeTween;
using UnityEngine;

public class SlideTransition : MonoBehaviour, ISaferioUIAnimation
{
    [SerializeField] private RectTransform target;

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

        UIUtil.SetLocalPositionX(target, canvasSize.Value.x);

        Tween.LocalPositionX(target, 0, duration: duration).OnComplete(() =>
        {
            _isInTransition = false;
        });

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

        UIUtil.SetLocalPositionX(target, canvasSize.Value.x);

        Tween.LocalPositionX(target, 0, duration: duration).OnComplete(() =>
        {
            _isInTransition = false;

            onCompletedAction?.Invoke();
        });
    }

    public void Hide()
    {
        if (_isInTransition)
        {
            return;
        }
        else
        {
            _isInTransition = true;
        }

        Tween.LocalPositionX(target, -canvasSize.Value.x, duration: duration).OnComplete(() =>
        {
            target.gameObject.SetActive(false);

            _isInTransition = false;
        });

        // SaferioTween.LocalPositionAsync(target, new Vector2(-canvasSize.Value.x, 0), duration: duration, onCompletedAction: () =>
        // {
        //     target.gameObject.SetActive(false);
        // });
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

        Tween.LocalPositionX(target, -canvasSize.Value.x, duration: duration).OnComplete(() =>
        {
            target.gameObject.SetActive(false);

            _isInTransition = false;

            onCompletedAction?.Invoke();
        });
    }
}
