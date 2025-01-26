using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Direction
{
    Up,
    Down,
    Right,
    Left
}

public static class UIUtil
{
    #region POSITION
    /// <summary>
    /// Set local position of target Rect Transform according to reference one
    /// </summary>
    /// <param name="target">target Rect Transform</param>
    /// <param name="reference">reference Rect Transform</param>
    /// <param name="targetOffset">offset from the center of reference Rect Transform plus this value multiplies target Rect Transform size delta</param>
    /// <param name="referenceOffset">offset from the center of reference Rect Transform plus this value multiplies reference Rect Transform size delta</param>
    public static void SetLocalPositionOfRectToAnotherRect(RectTransform target, RectTransform reference, Vector2 targetOffset, Vector2 referenceOffset)
    {
        if (target.IsChildOf(reference))
        {
            target.localPosition = (Vector3)(targetOffset * target.sizeDelta) + (Vector3)(referenceOffset * reference.sizeDelta);
        }
        else
        {
            target.localPosition = reference.localPosition + (Vector3)(targetOffset * target.sizeDelta) + (Vector3)(referenceOffset * reference.sizeDelta);
        }
    }

    /// <summary>
    /// Set local position X of target Rect Transform according to reference one
    /// </summary>
    public static void SetLocalPositionOfRectToAnotherRectHorizontally(RectTransform target, RectTransform reference, float targetOffset, float referenceOffset)
    {
        if (target.IsChildOf(reference))
        {
            target.localPosition = new Vector3(targetOffset * target.sizeDelta.x + referenceOffset * reference.sizeDelta.x, 0);
        }
        else
        {
            target.localPosition = reference.localPosition + new Vector3(targetOffset * target.sizeDelta.x + referenceOffset * reference.sizeDelta.x, 0);
        }
    }

    /// <summary>
    /// Set local position Y of target Rect Transform according to reference one
    /// </summary>
    public static void SetLocalPositionOfRectToAnotherRectVertically(RectTransform target, RectTransform reference, float targetOffset, float referenceOffset)
    {
        if (target.IsChildOf(reference))
        {
            target.localPosition = new Vector3(0, targetOffset * target.sizeDelta.y + referenceOffset * reference.sizeDelta.y, 0);
        }
        else
        {
            target.localPosition = reference.localPosition + new Vector3(0, targetOffset * target.sizeDelta.y + referenceOffset * reference.sizeDelta.y);
        }
    }

    public static void SetLocalPositionOfRectToAnotherRect(Button target, RectTransform reference, Vector2 targetOffset, Vector2 referenceOffset)
    {
        RectTransform targetRectTransform = target.GetComponent<RectTransform>();

        SetLocalPositionOfRectToAnotherRect(targetRectTransform, reference, targetOffset, referenceOffset);
    }

    public static void SetLocalPositionOfRectToAnotherRect(Image target, Image reference, Vector2 targetOffset, Vector2 referenceOffset)
    {
        RectTransform targetRectTransform = target.GetComponent<RectTransform>();
        RectTransform referenceRectTransform = reference.GetComponent<RectTransform>();

        SetLocalPositionOfRectToAnotherRect(targetRectTransform, referenceRectTransform, targetOffset, referenceOffset);
    }

    public static void SetLocalPositionOfRectToAnotherRectVertically(Button target, RectTransform reference, float targetOffset, float referenceOffset)
    {
        RectTransform targetRectTransform = target.GetComponent<RectTransform>();

        SetLocalPositionOfRectToAnotherRectVertically(targetRectTransform, reference, targetOffset, referenceOffset);
    }

    public static void SetLocalPositionOfRectToAnotherRectVertically(Image target, RectTransform reference, float targetOffset, float referenceOffset)
    {
        RectTransform targetRectTransform = target.GetComponent<RectTransform>();

        SetLocalPositionOfRectToAnotherRectVertically(targetRectTransform, reference, targetOffset, referenceOffset);
    }

    public static void SetLocalPositionOfRectToAnotherRectVertically(Image target, Image reference, float targetOffset, float referenceOffset)
    {
        RectTransform targetRectTransform = target.GetComponent<RectTransform>();
        RectTransform referenceRectTransform = reference.GetComponent<RectTransform>();

        SetLocalPositionOfRectToAnotherRectVertically(targetRectTransform, referenceRectTransform, targetOffset, referenceOffset);
    }

    public static void HideUIElement(RectTransform target, Vector2 canvasSize)
    {
        target.localPosition = target.localPosition - new Vector3(canvasSize.x, 0, 0);
    }

    public static Vector3 WithLocalPositionX(RectTransform target, float localPositionX)
    {
        return new Vector3(localPositionX, target.localPosition.y, target.localPosition.z);
    }
    #endregion

    #region TEXT
    public static void SetTextSize(TMP_Text target, float size)
    {
        target.fontSize = size;
        target.rectTransform.sizeDelta = new Vector2(target.preferredWidth, target.preferredHeight);
    }

    public static void SetFontSize(TMP_Text target, float size)
    {
        target.fontSize = size;
        target.rectTransform.sizeDelta = new Vector2(target.preferredWidth, target.preferredHeight);
    }

    public static void SetFontSizeOnly(TMP_Text target, float size)
    {
        target.fontSize = size;
    }

    public static void EnableTextAutoSize(TextMeshProUGUI text)
    {
        text.autoSizeTextContainer = true;
    }
    #endregion

    #region ANIMATION
    public static void AnimateSizeAndPosition(RectTransform target, Vector2 prevSize, Vector2 prevPos, Vector2 newSize, Vector2 newPos,
        List<Tween> tweens,
        float duration = 0.5f,
        Action onAnimationCompleted = null
    )
    {
        target.sizeDelta = prevSize;
        target.localPosition = prevPos;

        tweens.Add(
            Tween.Custom(prevSize, newSize, duration: duration, onValueChange: newVal => target.sizeDelta = newVal)
            .OnComplete(() => onAnimationCompleted?.Invoke())
        );
        tweens.Add(Tween.Custom(prevPos, newPos, duration: duration, onValueChange: newVal => target.localPosition = newVal));
    }

    public static void AnimateSizeAndPositionRandomly(RectTransform target, List<Tween> tweens, float duration = 0.5f, Action onAnimationCompleted = null)
    {
        Vector2 endSize = target.sizeDelta;
        Vector2 endPos = (Vector2)target.localPosition;

        target.sizeDelta = GetRandomAbsoluteVec2Int(max: 500);
        target.localPosition = GetRandomVec2Int();

        Vector2 startSize = target.sizeDelta;
        Vector2 startPos = (Vector2)target.localPosition;

        target.gameObject.SetActive(true);

        tweens.Add(
            Tween.Custom(startSize, endSize, duration: duration, onValueChange: newVal => target.sizeDelta = newVal)
            .OnComplete(() => onAnimationCompleted?.Invoke())
        );
        tweens.Add(Tween.Custom(startPos, endPos, duration: duration, onValueChange: newVal => target.localPosition = newVal));
    }

    public static void AnimateSizeAndPositionRandomly(Button target, List<Tween> tweens)
    {
        AnimateSizeAndPositionRandomly(target.GetComponent<RectTransform>(), tweens);
    }

    public static void SlideHorizontal(
        RectTransform target, List<Tween> tweens, Vector2 canvasSize,
        float duration = 0.5f, Direction slideFrom = Direction.Left,
        Action onAnimationCompleted = null
    )
    {
        float currentPos = target.localPosition.x;

        Vector2 localPosition = target.localPosition;

        if (slideFrom == Direction.Left)
        {
            localPosition.x -= canvasSize.x;
        }
        else
        {
            localPosition.x += canvasSize.x;
        }

        target.localPosition = localPosition;
        target.gameObject.SetActive(true);

        tweens.Add(
            Tween.LocalPositionX(target, currentPos, duration: duration).OnComplete(() =>
            {
                onAnimationCompleted?.Invoke();
            })
        );
    }

    public static async Task SlideOutHorizontal(
        RectTransform target, List<Tween> tweens, Vector2 canvasSize,
        float duration = 0.5f, Direction slideTo = Direction.Left,
        Action onAnimationCompleted = null
    )
    {
        Vector2 initialPos = target.localPosition;
        float hidePos;

        Vector2 localPosition = target.localPosition;

        if (slideTo == Direction.Left)
        {
            hidePos = localPosition.x - canvasSize.x;
        }
        else
        {
            hidePos = localPosition.x + canvasSize.x;
        }

        target.localPosition = localPosition;

        Tween tween = Tween.LocalPositionX(target, hidePos, duration: duration).OnComplete(() =>
            {
                target.localPosition = initialPos;
                target.gameObject.SetActive(false);

                onAnimationCompleted?.Invoke();
            });

        tweens.Add(tween);

        await tween;
    }

    public static void SlideVertical(
        RectTransform target, List<Tween> tweens,
        float endPositionY, float duration = 0.5f,
        Action onAnimationCompleted = null
    )
    {
        target.gameObject.SetActive(true);

        tweens.Add(
            Tween.LocalPositionY(target, endPositionY, duration: duration).OnComplete(() =>
            {
                onAnimationCompleted?.Invoke();
            })
        );
    }

    public static async Task SlideOutVertical(
        RectTransform target, List<Tween> tweens, Vector2 canvasSize,
        float duration = 0.5f, Direction slideTo = Direction.Down, bool isDisableTargetOnCompleted = true,
        Action onAnimationCompleted = null
    )
    {
        Vector2 initialPos = target.localPosition;
        float hidePos;

        Vector2 localPosition = target.localPosition;

        if (slideTo == Direction.Down)
        {
            hidePos = localPosition.y - canvasSize.y;
        }
        else
        {
            hidePos = localPosition.y + canvasSize.y;
        }

        target.localPosition = localPosition;

        Tween tween = Tween.LocalPositionY(target, hidePos, duration: duration).OnComplete(() =>
            {
                target.localPosition = initialPos;

                if (isDisableTargetOnCompleted)
                {
                    target.gameObject.SetActive(false);
                }

                onAnimationCompleted?.Invoke();
            });

        tweens.Add(tween);

        await tween;
    }

    public static void ScaleUp(
        RectTransform target, List<Tween> tweens,
        float duration = 0.5f,
        Action onAnimationCompleted = null
    )
    {
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        target.localScale = startScale;

        tweens.Add(
            Tween.Scale(target, endScale, duration: duration).OnComplete(() =>
            {
                onAnimationCompleted?.Invoke();
            })
        );
    }

    public static async Task ScaleDown(
        RectTransform target, List<Tween> tweens,
        float duration = 0.5f,
        Action onAnimationCompleted = null
    )
    {
        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.zero;

        target.localScale = startScale;

        Tween tween;

        tween = Tween.Scale(target, endScale, duration: duration).OnComplete(() =>
            {
                target.localScale = startScale;
                target.gameObject.SetActive(false);

                onAnimationCompleted?.Invoke();
            });

        tweens.Add(tween);

        await tween;
    }

    public static async Task ScaleToAsync(
        RectTransform target, List<Tween> tweens,
        Vector3 startScale, Vector3 endScale,
        bool isKeepScaleAfterFinishing = false,
        float duration = 0.5f,
        Action onAnimationCompleted = null
    )
    {
        Vector3 initialScale = target.localScale;

        target.localScale = startScale;

        Tween tween;

        tween = Tween.Scale(target, endScale, duration: duration).OnComplete(() =>
            {
                if (isKeepScaleAfterFinishing)
                {
                    target.localScale = initialScale;
                }

                onAnimationCompleted?.Invoke();
            });

        tweens.Add(tween);

        await tween;
    }

    public static void ScaleTo(
        RectTransform target, List<Tween> tweens,
        Vector3 startScale, Vector3 endScale,
        bool isKeepScaleAfterFinishing = false,
        float duration = 0.5f,
        Ease easing = Ease.Default,
        Action onAnimationCompleted = null
    )
    {
        Vector3 initialScale = target.localScale;

        target.localScale = startScale;

        tweens.Add(Tween.Scale(target, endScale, duration: duration, ease: easing).OnComplete(() =>
            {
                if (isKeepScaleAfterFinishing)
                {
                    target.localScale = initialScale;
                }

                onAnimationCompleted?.Invoke();
            }));
    }
    #endregion 

    #region UTIL OF UTIL
    private static int GetRandomInt(int min = 1000, int max = 2000)
    {
        int randomInt = UnityEngine.Random.Range(-max, max);

        if (randomInt >= 0)
        {
            if (randomInt < min)
            {
                randomInt = min;
            }
        }
        else
        {
            if (randomInt > -min)
            {
                randomInt = -min;
            }
        }

        return randomInt;
    }

    private static int GetRandomAbsoluteInt(int max = 1000)
    {
        return UnityEngine.Random.Range(0, max);
    }

    private static Vector2 GetRandomVec2Int(int max = 1000)
    {
        return Vector2.one * GetRandomInt(max);
    }

    private static Vector2 GetRandomAbsoluteVec2Int(int max = 1000)
    {
        return Vector2.one * GetRandomAbsoluteInt(max);
    }
    #endregion

    #region SCALE
    public static IEnumerator TweenScale(Transform target, Vector3 startScale, Vector3 endScale, float duration, Action onCompletedAction = null)
    {
        WaitForSeconds waitForDeltaTime = new WaitForSeconds(Time.deltaTime);

        int maxStep = (int)(duration / Time.deltaTime);
        int step = 0;

        target.transform.localScale = startScale;

        Vector3 deltaScale = (endScale - startScale) / maxStep;

        while (step < maxStep)
        {
            target.transform.localScale += deltaScale;

            step++;

            yield return waitForDeltaTime;
        }

        onCompletedAction?.Invoke();
    }
    #endregion

    public static bool IsClickOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SetSize(RectTransform target, float width, float height)
    {
        target.sizeDelta = new Vector2(width, height);
    }

    public static void SetSize(Button target, float width, float height)
    {
        RectTransform rectTransform = target.GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(width, height);
    }

    public static void SetSize(RectTransform target, Vector2 size)
    {
        target.sizeDelta = size;
    }

    public static void SetSize(Button target, Vector2 size)
    {
        RectTransform rectTransform = target.GetComponent<RectTransform>();

        rectTransform.sizeDelta = size;
    }

    public static void SetSizeX(RectTransform target, float width)
    {
        target.sizeDelta = new Vector2(width, target.sizeDelta.x);
    }

    public static void SetSizeKeepRatioX(Image target, float height)
    {
        float ratio = target.sprite.rect.size.x / target.sprite.rect.size.y;

        target.rectTransform.sizeDelta = new Vector2(height * ratio, height);
    }

    public static void SetSizeKeepRatioX(RectTransform target, float height)
    {
        Image image = target.GetComponent<Image>();

        float ratio = image.sprite.rect.size.x / image.sprite.rect.size.y;

        image.rectTransform.sizeDelta = new Vector2(height * ratio, height);
    }

    public static void SetSizeKeepRatioY(Image target, float width)
    {
        float ratio = target.sprite.rect.size.y / target.sprite.rect.size.x;

        target.rectTransform.sizeDelta = new Vector2(width, width * ratio);
    }

    public static void SetSizeKeepRatioY(RectTransform target, float width)
    {
        Image image = target.GetComponent<Image>();

        float ratio = image.sprite.rect.size.y / image.sprite.rect.size.x;

        target.sizeDelta = new Vector2(width, width * ratio);
    }

    public static void SetSizeKeepRatioY(Button target, float width)
    {
        Image image = target.GetComponent<Image>();

        float ratio = image.sprite.rect.size.y / image.sprite.rect.size.x;

        image.rectTransform.sizeDelta = new Vector2(width, width * ratio);
    }

    public static void SetLocalPosition(RectTransform target, float positionX, float positionY)
    {
        target.localPosition = new Vector2(positionX, positionY);
    }

    public static void SetLocalPosition(RectTransform target, Vector2 localPosition)
    {
        target.localPosition = localPosition;
    }

    public static void SetLocalPositionX(RectTransform target, float positionX)
    {
        target.localPosition = new Vector2(positionX, target.localPosition.y);
    }

    public static void SetLocalPositionY(RectTransform target, float positionY)
    {
        target.localPosition = new Vector2(target.localPosition.x, positionY);
    }

    public static void SetLocalPositionY(Image target, float positionY)
    {
        RectTransform rectTransform = target.GetComponent<RectTransform>();

        rectTransform.localPosition = new Vector2(rectTransform.localPosition.x, positionY);
    }

    public static void SetLocalPositionToRightOf(RectTransform target, RectTransform reference, float distanceRatio)
    {
        target.localPosition = new Vector2(
            reference.localPosition.x + distanceRatio * (reference.sizeDelta.x + target.sizeDelta.x),
            reference.localPosition.y
        );
    }

    public static void SetLocalPositionToRightInOf(RectTransform target, RectTransform reference, float distanceRatio)
    {
        target.localPosition = new Vector2(
            reference.localPosition.x + distanceRatio * (reference.sizeDelta.x - target.sizeDelta.x),
            reference.localPosition.y
        );
    }

    public static void SetLocalPositionToLeftOf(RectTransform target, RectTransform reference, float distanceRatio)
    {
        target.localPosition = new Vector2(
            reference.localPosition.x - distanceRatio * (reference.sizeDelta.x + target.sizeDelta.x),
            reference.localPosition.y
        );
    }

    public static void SetLocalPositionToLeftInOf(RectTransform target, RectTransform reference, float distanceRatio, bool isKeepSelfPositionY = false)
    {
        target.localPosition = new Vector2(
            reference.localPosition.x - distanceRatio * (reference.sizeDelta.x - target.sizeDelta.x),
            isKeepSelfPositionY ? target.localPosition.y : reference.localPosition.y
        );
    }

    public static void SetLocalPositionToTopOf(RectTransform target, RectTransform reference, float distanceRatio)
    {
        target.localPosition = new Vector2(
            reference.localPosition.x,
            reference.localPosition.y + distanceRatio * (reference.sizeDelta.y + target.sizeDelta.y)
        );
    }

    public static void SetLocalPositionToBottomOf(RectTransform target, RectTransform reference, float distanceRatio)
    {
        target.localPosition = new Vector2(
            reference.localPosition.x,
            reference.localPosition.y - distanceRatio * (reference.sizeDelta.y + target.sizeDelta.y)
        );
    }

    public static void SetLocalPositionAboveBottomOf(RectTransform target, RectTransform reference, float distanceRatio)
    {
        target.localPosition = new Vector2(
            reference.localPosition.x,
            reference.localPosition.y - distanceRatio * (reference.sizeDelta.y - target.sizeDelta.y)
        );
    }

    public static void SetLocalPositionInsideOf(RectTransform target, RectTransform reference, Direction direction, float distanceRatio)
    {
        Vector2 finalLocalPosition = reference.localPosition;

        if (direction == Direction.Up)
        {
            finalLocalPosition.y += distanceRatio * reference.sizeDelta.y;
        }
        else if (direction == Direction.Down)
        {
            finalLocalPosition.y -= distanceRatio * reference.sizeDelta.y;
        }
        else if (direction == Direction.Right)
        {
            finalLocalPosition.x += distanceRatio * reference.sizeDelta.x;
        }
        else if (direction == Direction.Left)
        {
            finalLocalPosition.x -= distanceRatio * reference.sizeDelta.x;
        }

        target.localPosition = finalLocalPosition;
    }

    public static void ChangeLocalPositionX(RectTransform target, float change)
    {
        target.localPosition += new Vector3(change, 0);
    }

    public static void ChangeLocalPositionY(RectTransform target, float change)
    {
        target.localPosition += new Vector3(0, change);
    }

    public static RectTransform GetRectTransformButton(Button target)
    {
        return target.GetComponent<RectTransform>();
    }

    public static TextMeshProUGUI GetTextMeshProButton(Button target)
    {
        return target.GetComponent<RectTransform>().GetChild(0).GetComponent<TextMeshProUGUI>();
    }
}
