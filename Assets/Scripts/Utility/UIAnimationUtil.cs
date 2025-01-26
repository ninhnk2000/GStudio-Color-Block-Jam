using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

// public interface IUITransition {

// }

public interface IUISlide
{
    public void SlideIn(RectTransform target, float visiblePosition, float invisiblePosition);
    public void SlideOut(RectTransform target, float visiblePosition, float invisiblePosition);
}

public struct UISlideVertical : IUISlide
{
    public void SlideIn(RectTransform target, float visiblePosition, float invisiblePosition)
    {
        Vector3 localPosition = target.localPosition;

        localPosition.y = invisiblePosition;

        target.localPosition = localPosition;

        Tween.LocalPositionY(target, visiblePosition, duration: 0.5f);
    }

    public void SlideOut(RectTransform target, float visiblePosition, float invisiblePosition)
    {
        Tween.LocalPositionY(target, invisiblePosition, duration: 0.5f);
    }
}

public struct UISlideHorizontal : IUISlide
{
    public void SlideIn(RectTransform target, float visiblePosition, float invisiblePosition)
    {
        target.gameObject.SetActive(true);

        target.localPosition = UIUtil.WithLocalPositionX(target, invisiblePosition);

        Tween.LocalPositionX(target, visiblePosition, duration: 0.3f);
    }

    public void SlideOut(RectTransform target, float visiblePosition, float invisiblePosition)
    {
        Tween.LocalPositionX(target, invisiblePosition, duration: 0.3f)
        .OnComplete(() =>
        {
            target.localPosition = UIUtil.WithLocalPositionX(target, visiblePosition);

            target.gameObject.SetActive(false);
        });
    }
}

public static class UIAnimationUtil
{

}
