using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;

public static class AnimationUtil
{
    public static void TweenScale(Transform target, Vector3 endScale, float duration, List<Tween> tweens, Action onComplete)
    {
        Tween tween = Tween.Scale(target, endScale, duration).OnComplete(() => onComplete?.Invoke());

        tweens.Add(tween);
    }

    public static void ScaleUpDown(Transform target, float scaleMultiplier, float duration, Action onComplete = null)
    {
        Vector3 endScale = scaleMultiplier * target.localScale;

        Tween tween = Tween.Scale(
            target, endScale, duration
            , cycles: 4, cycleMode: CycleMode.Yoyo
            )
            .OnComplete(() => onComplete?.Invoke());
    }

    public static void ScaleUpDownY
    (
        Transform target, float scaleMultiplier, float duration, Action onComplete = null
    )
    {
        Vector3 endScale = scaleMultiplier * target.localScale;

        Tween tween = Tween.Scale(
            target, endScale, duration
            , cycles: 4, cycleMode: CycleMode.Yoyo
            )
            .OnComplete(() => onComplete?.Invoke());
    }

    public static IEnumerator Vibrate
    (
        Transform target,
        int numStep,
        float vibrateDistance,
        float duration,
        List<Tween> tweens,
        Action onComplete = null
    )
    {
        int maxStep = numStep;
        int step = 0;

        bool isWait = false;

        Vector3 startPosition = target.position;

        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

        Tween tween;

        while (step <= maxStep)
        {
            if (isWait)
            {
                yield return new WaitUntil(() => isWait == false);

                // TODO: check what happen if don't use "continue" here
                continue;
            }
            else
            {
                isWait = true;
            }

            int remainingStep = maxStep - step;
            int multiplier = remainingStep % 2 == 0 ? 1 : -1;

            if (step == 0)
            {
                tween = Tween.PositionX(target, startPosition.x + multiplier * remainingStep * vibrateDistance, duration: duration)
                .OnComplete(() =>
                {
                    step++;

                    isWait = false;
                });
            }
            else if (step < maxStep)
            {
                tween = Tween.PositionX
                (
                    target, startPosition.x + multiplier * remainingStep * vibrateDistance, duration: duration
                )
                .OnComplete(() =>
                {
                    step++;

                    isWait = false;
                });
            }
            else
            {
                tween = Tween.PositionX(target, startPosition.x, duration: duration)
                .OnComplete(() =>
                {
                    step++;

                    isWait = false;
                });
            }

            tweens.Add(tween);

            yield return waitForSeconds;
        }
    }

    public static IEnumerator UIVibrate
    (
        RectTransform target,
        int numStep,
        float vibrateAltitudeMultiplier,
        float duration,
        List<Tween> tweens,
        Action onComplete = null
    )
    {
        int maxStep = numStep;
        int step = 0;

        bool isWait = false;

        Vector3 startPosition = target.localPosition;

        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

        Tween tween;

        while (step <= maxStep)
        {
            if (isWait)
            {
                yield return new WaitUntil(() => isWait == false);
            }
            else
            {
                isWait = true;
            }

            int remainingStep = maxStep - step;
            int multiplier = remainingStep % 2 == 0 ? 1 : -1;

            if (step == 0)
            {
                tween = Tween.LocalPositionX(target, startPosition.x + remainingStep * vibrateAltitudeMultiplier * target.sizeDelta.x, duration: duration)
                .OnComplete(() =>
                {
                    step++;

                    isWait = false;
                });
            }
            else if (step < maxStep)
            {
                tween = Tween.LocalPositionX
                (
                    target, startPosition.x + multiplier * remainingStep * vibrateAltitudeMultiplier * target.sizeDelta.x, duration: duration
                )
                .OnComplete(() =>
                {
                    step++;

                    isWait = false;
                });
            }
            else
            {
                tween = Tween.LocalPositionX(target, startPosition.x, duration: duration)
                .OnComplete(() =>
                {
                    step++;

                    isWait = false;
                });
            }

            tweens.Add(tween);

            yield return waitForSeconds;
        }
    }

    public static void ShowDamage(TMP_Text text, List<Tween> tweens)
    {
        Tween tween;

        text.color = ChangeAlpha(text.color, 0);

        text.gameObject.SetActive(true);

        tween = Tween.Alpha(text, 1, duration: 0.3f)
        .OnComplete(() =>
        {
            Vector3 startPosition = text.rectTransform.localPosition;
            float endPositionY = text.rectTransform.localPosition.y + 1;

            tween = Tween.LocalPositionY(text.rectTransform, endPositionY, duration: 0.5f);

            tweens.Add(tween);

            tween = Tween.Delay(0.3f)
            .OnComplete(() =>
            {
                tween = Tween.Alpha(text, 0, duration: 0.3f).OnComplete(() =>
                {
                    text.color = ChangeAlpha(text.color, 1);

                    text.gameObject.SetActive(false);

                    text.rectTransform.localPosition = startPosition;
                });

                tweens.Add(tween);
            });

            tweens.Add(tween);
        });

        tweens.Add(tween);
    }

    private static Color ChangeAlpha(Color baseColor, float alpha)
    {
        return new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
    }







    public static void SetSize(RectTransform target, Vector2 size)
    {
        target.sizeDelta = size;
    }
}
