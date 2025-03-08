using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static Saferio.Util.SaferioTween.SaferioCustomDelegate;

namespace Saferio.Util.SaferioTween
{
    public static class SaferioTween
    {
        private const float ALLOWED_ERROR = 0.01f;

        #region POSITION
        public static void Position
        (
            Transform target,
            Vector3 end,
            float duration,
            Action onCompletedAction = null
        )
        {
            SaferioTweenManager.RunCoroutine(PositionCoroutine(target, end, duration, onCompletedAction), target);
        }

        public static void PositionX(Transform target, float end, float duration, Action onCompletedAction = null)
        {
            Position(target, target.position.ChangeX(end), duration, onCompletedAction);
        }

        public static void PositionY(Transform target, float end, float duration, Action onCompletedAction = null)
        {
            Position(target, target.position.ChangeY(end), duration, onCompletedAction);
        }

        public static void PositionZ(Transform target, float end, float duration, Action onCompletedAction = null)
        {
            Position(target, target.position.ChangeZ(end), duration, onCompletedAction);
        }

        public static void Position(RectTransform target, Vector3 end, float duration, Action onCompletedAction = null)
        {
            SaferioTweenManager.RunCoroutine(
                LocalPositionCoroutineNoIterative(getValue: () => target.localPosition, setValue: value => target.localPosition = value, end, duration, onCompletedAction), target
            );

            // SaferioTweenManager.RunCoroutine(PositionCoroutine(target, end, duration, onCompletedAction), target);
        }

        // public static void PositionX(RectTransform target, Vector3 end, float duration, Action onCompletedAction = null)
        // {
        //     Position(target, target.position.ChangeX(end), duration, onCompletedAction);
        // }

        public static void PositionY(RectTransform target, float end, float duration, Action onCompletedAction = null)
        {
            Position(target, target.localPosition.ChangeY(end), duration, onCompletedAction);
        }

        public static void LocalPosition
        (
            Transform target,
            Vector3 end,
            float duration,
            Action onCompletedAction = null
        )
        {
            SaferioTweenManager.RunCoroutine(LocalPositionCoroutine(target, end, duration, onCompletedAction), target);
        }

        public static IEnumerator PositionCoroutine
        (
            Transform target,
            Vector3 end,
            float duration,
            Action onCompletedAction
        )
        {
            float updateDuration = Time.deltaTime;

            WaitForSeconds waitForSeconds = new WaitForSeconds(updateDuration);

            Vector3 deltaPosition = (end - target.position) / (duration / updateDuration);

            int totalStep = (int)((end - target.position).magnitude / deltaPosition.magnitude);
            int stepPassed = 0;

            while (stepPassed < totalStep)
            {
                target.position += deltaPosition;

                stepPassed++;

                yield return waitForSeconds;
            }

            target.position = end;

            onCompletedAction?.Invoke();
        }

        public static IEnumerator LocalPositionCoroutine
        (
            Transform target,
            Vector3 end,
            float duration,
            Action onCompletedAction
        )
        {
            float updateDuration = Time.deltaTime;

            WaitForSeconds waitForSeconds = new WaitForSeconds(updateDuration);

            Vector3 deltaPosition = (end - target.localPosition) / (duration / updateDuration);

            int totalStep = (int)((end - target.localPosition).magnitude / deltaPosition.magnitude);
            int stepPassed = 0;

            while (stepPassed < totalStep)
            {
                target.localPosition += deltaPosition;

                stepPassed++;

                yield return waitForSeconds;
            }

            target.localPosition = end;

            onCompletedAction?.Invoke();
        }

        public static IEnumerator LocalPositionCoroutineNoIterative
        (
            GetVector3Action getValue,
            Action<Vector3> setValue,
            Vector3 end,
            float duration,
            Action onCompletedAction
        )
        {
            float updateDuration = Time.deltaTime;

            WaitForSeconds waitForSeconds = new WaitForSeconds(updateDuration);

            Vector3 deltaPosition = (end - getValue.Invoke()) / (duration / updateDuration);

            int numStep = (int)((getValue.Invoke() - end).magnitude / deltaPosition.magnitude);
            int currentStep = 0;

            while (currentStep < numStep)
            {
                setValue.Invoke(getValue.Invoke() + deltaPosition);

                currentStep++;

                yield return waitForSeconds;
            }

            setValue.Invoke(end);

            onCompletedAction?.Invoke();
        }
        #region POSITION - ASYNC
        public static async void PositionAsync
        (
            Transform target,
            Vector3 end,
            float duration,
            Action onCompletedAction = null
        )
        {
            await PositionAsync(SaferioTweenManager.CancellationTokenSourceOnDestroyed.Token, target, end, duration, onCompletedAction);
        }

        public static async Task PositionAsync
        (
            CancellationToken cancellationToken,
            Transform target,
            Vector3 end,
            float duration,
            Action onCompletedAction = null
        )
        {
            try
            {
                float updateDuration = Time.deltaTime;

                int delayTimeEachStepMilliSecond = (int)(updateDuration * 1000);

                Vector3 deltaPosition = (end - target.position) / (duration / updateDuration);

                int totalStep = (int)((end - target.position).magnitude / deltaPosition.magnitude);
                int stepPassed = 0;

                while (stepPassed < totalStep)
                {
                    target.position += deltaPosition;

                    stepPassed++;

                    await Task.Delay(delayTimeEachStepMilliSecond, cancellationToken);
                }

                target.position = end;

                onCompletedAction?.Invoke();
            }
            catch (TaskCanceledException)
            {

            }
        }

        public static async void LocalPositionAsync
        (
            RectTransform target,
            Vector3 end,
            float duration,
            Action onCompletedAction = null
        )
        {
            await LocalPositionAsync(SaferioTweenManager.CancellationTokenSourceOnDestroyed.Token, target, end, duration, onCompletedAction);
        }

        public static async void LocalPositionAsync
        (
            Transform target,
            Vector3 end,
            float duration,
            Action onCompletedAction = null
        )
        {
            await LocalPositionAsync(SaferioTweenManager.CancellationTokenSourceOnDestroyed.Token, target, end, duration, onCompletedAction);
        }

        public static async Task LocalPositionAsync
        (
            CancellationToken cancellationToken,
            RectTransform target,
            Vector3 end,
            float duration,
            Action onCompletedAction = null
        )
        {
            try
            {
                float updateDuration = Time.deltaTime;

                int delayTimeEachStepMilliSecond = (int)(updateDuration * 1000);

                Vector3 deltaPosition = (end - target.localPosition) / (duration / updateDuration);

                int totalStep = (int)((end - target.localPosition).magnitude / deltaPosition.magnitude);
                int stepPassed = 0;

                while (stepPassed < totalStep)
                {
                    target.localPosition += deltaPosition;

                    stepPassed++;

                    await Task.Delay(delayTimeEachStepMilliSecond, cancellationToken);
                }

                target.localPosition = end;

                onCompletedAction?.Invoke();
            }
            catch (TaskCanceledException)
            {

            }
        }

        public static async Task LocalPositionAsync
        (
            CancellationToken cancellationToken,
            Transform target,
            Vector3 end,
            float duration,
            Action onCompletedAction = null
        )
        {
            try
            {
                float updateDuration = Time.deltaTime;

                int delayTimeEachStepMilliSecond = (int)(updateDuration * 1000);

                Vector3 deltaPosition = (end - target.localPosition) / (duration / updateDuration);

                int totalStep = (int)((end - target.localPosition).magnitude / deltaPosition.magnitude);
                int stepPassed = 0;

                while (stepPassed < totalStep)
                {
                    target.localPosition += deltaPosition;

                    stepPassed++;

                    await Task.Delay(delayTimeEachStepMilliSecond, cancellationToken);
                }

                target.localPosition = end;

                onCompletedAction?.Invoke();
            }
            catch (TaskCanceledException)
            {

            }
        }
        #endregion
        #endregion

        #region SCALE
        public static void Scale(Transform target, Vector3 end, float duration, Action onCompletedAction = null)
        {
            SaferioTweenManager.RunCoroutine(ScaleCoroutine(target, end, duration, onCompletedAction), target);
        }

        public static void ScaleX(Transform target, float end, float duration, Action onCompletedAction = null)
        {
            Scale(target, target.localScale.ChangeX(end), duration, onCompletedAction);
        }

        public static void ScaleY(Transform target, float end, float duration, Action onCompletedAction = null)
        {
            Scale(target, target.localScale.ChangeY(end), duration, onCompletedAction);
        }

        public static void ScaleZ(Transform target, float end, float duration, Action onCompletedAction = null)
        {
            Scale(target, target.localScale.ChangeZ(end), duration, onCompletedAction);
        }

        public static IEnumerator ScaleCoroutine
        (
            Transform target,
            Vector3 end,
            float duration,
            Action onCompletedAction
        )
        {
            float updateDuration = Time.deltaTime;

            WaitForSeconds waitForSeconds = new WaitForSeconds(updateDuration);

            Vector3 deltaValue = (end - target.localScale) / (duration / updateDuration);

            int totalStep = (int)((end - target.localScale).magnitude / deltaValue.magnitude);
            int stepPassed = 0;

            while (stepPassed < totalStep)
            {
                target.localScale += deltaValue;

                stepPassed++;

                yield return waitForSeconds;
            }

            target.localScale = end;

            onCompletedAction?.Invoke();
        }

        #region SCALE - ASYNC
        public static async void ScaleAsync
        (
            Transform target,
            Vector3 end,
            float duration,
            Action onCompletedAction = null
        )
        {
            await ScaleAsync(SaferioTweenManager.CancellationTokenSourceOnDestroyed.Token, target, end, duration, onCompletedAction);
        }

        public static async Task ScaleAsync
        (
            CancellationToken cancellationToken,
            Transform target,
            Vector3 end,
            float duration,
            Action onCompletedAction = null
        )
        {
            try
            {
                float updateDuration = Time.deltaTime;

                int delayTimeEachStepMilliSecond = (int)(updateDuration * 1000);

                Vector3 deltaValue = (end - target.localScale) / (duration / updateDuration);

                int totalStep = (int)((end - target.localScale).magnitude / deltaValue.magnitude);
                int stepPassed = 0;

                while (stepPassed < totalStep)
                {
                    target.localScale += deltaValue;

                    stepPassed++;

                    await Task.Delay(delayTimeEachStepMilliSecond, cancellationToken);
                }

                target.localScale = end;

                onCompletedAction?.Invoke();
            }
            catch (TaskCanceledException)
            {

            }
        }
        #endregion
        #endregion

        #region TIME
        public static int Delay(float time, Action onCompletedAction)
        {
            return SaferioTweenManager.RunCoroutine(DelayCoroutine(time, onCompletedAction));
        }

        public static IEnumerator DelayCoroutine(float time, Action onCompletedAction)
        {
            yield return new WaitForSeconds(time);

            onCompletedAction?.Invoke();
        }

        public async static void DelayAsync(float second, Action onCompletedAction)
        {
            await DelayAsync(SaferioTweenManager.CancellationTokenSourceOnDestroyed.Token, second, onCompletedAction);
        }

        private async static Task DelayAsync
        (
            CancellationToken cancellationToken,
            float second,
            Action onCompletedAction
        )
        {
            try
            {
                await Task.Delay(ToMillisecond(second), cancellationToken);

                onCompletedAction?.Invoke();
            }
            catch (TaskCanceledException e)
            {
                // DebugUtil.DistinctLog(e);
            }
        }
        #endregion

        #region COLOR
        public async static void AlphaAsync(Image image, float endAlpha, float duration, Action onCompletedAction = null)
        {
            await AlphaAsync(SaferioTweenManager.CancellationTokenSourceOnDestroyed.Token, image, endAlpha, duration, onCompletedAction);
        }

        private async static Task AlphaAsync
        (
            CancellationToken cancellationToken,
            Image image,
            float endAlpha,
            float duration,
            Action onCompletedAction = null
        )
        {
            try
            {
                float updateDuration = Time.deltaTime;

                int delayTimeEachStepMilliSecond = (int)(updateDuration * 1000);

                float deltaAlpha = (endAlpha - image.color.a) / (duration / updateDuration);

                int totalStep = (int)((endAlpha - image.color.a) / deltaAlpha);
                int stepPassed = 0;

                while (stepPassed < totalStep)
                {
                    image.color += new Color(0, 0, 0, deltaAlpha);

                    stepPassed++;

                    await Task.Delay(delayTimeEachStepMilliSecond, cancellationToken);
                }

                image.color = new Color(image.color.r, image.color.g, image.color.b, endAlpha);

                onCompletedAction?.Invoke();
            }
            catch (TaskCanceledException e)
            {
                // DebugUtil.DistinctLog(e);
            }
        }
        #endregion

        #region SEQUENCE
        // public static IEnumerator Sequence(Action[] list)
        // {
        //     int currentIndex = 0;

        //     while (currentIndex < list.Length)
        //     {
        //         list[currentIndex].Invoke();
        //     }
        // }
        #endregion

        #region STOP
        public static void Stop(int id)
        {
            SaferioTweenManager.StopCoroutine(id);
        }
        #endregion

        #region UTIL
        private static int ToMillisecond(float second)
        {
            return (int)(second * 1000);
        }
        #endregion
    }

    public static class SaferioVector3Extension
    {
        public static Vector3 ChangeX(this Vector3 currentValue, float value)
        {
            return new Vector3(value, currentValue.y, currentValue.z);
        }

        public static Vector3 ChangeY(this Vector3 currentValue, float value)
        {
            return new Vector3(currentValue.x, value, currentValue.z);
        }

        public static Vector3 ChangeZ(this Vector3 currentValue, float value)
        {
            return new Vector3(currentValue.x, currentValue.y, value);
        }

        public static Vector3 AddY(this Vector3 currentValue, float value)
        {
            return new Vector3(currentValue.x, currentValue.y + value, currentValue.z);
        }
    }

    public static class SaferioCustomDelegate
    {
        public delegate Vector3 GetVector3Action();
    }
}