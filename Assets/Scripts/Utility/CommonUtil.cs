using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public static class CommonUtil
{
    public static void StopTween(Tween tween)
    {
        if (tween.isAlive)
        {
            tween.Stop();
        }
    }

    public static void StopAllTweens(List<Tween> tweens)
    {
        for (int i = 0; i < tweens.Count; i++)
        {
            if (tweens[i].isAlive)
            {
                tweens[i].Stop();
            }
        }
    }

    public static void OnHitColorEffect(
        Renderer meshRenderer,
        MaterialPropertyBlock materialPropertyBlock,
        Color startColor,
        Color endColor,
        float duration,
        List<Tween> tweens
    )
    {
        Tween tween = Tween.Custom(
            startColor, endColor,
            duration: duration,
            onValueChange: newVal =>
            {
                materialPropertyBlock.SetColor("_Albedo", newVal);
                meshRenderer.SetPropertyBlock(materialPropertyBlock);
            },
            cycles: 2,
            cycleMode: CycleMode.Yoyo
            );

        tweens.Add(tween);
    }

    public static bool IsNull(object testObject)
    {
        return testObject == null;
    }

    public static bool IsNotNull(object testObject)
    {
        return testObject != null;
    }

    public static GameObject GetParentGameObject(GameObject child)
    {
        return child.transform.parent.gameObject;
    }

    #region CONSOLE
    public static void ClearLog()
    {
#if UNITY_EDITOR
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
#endif
    }
    #endregion

    #region COLOR
    public static Color ChangeAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
    #endregion

    private static IEnumerator WaitFor(float amount, Action onCompletedAction)
    {
        float deltaTime = Time.deltaTime;

        WaitForSeconds waitForSeconds = new WaitForSeconds(deltaTime);

        float time = 0;

        while (time < amount)
        {
            time += deltaTime;

            yield return waitForSeconds;
        }

        onCompletedAction?.Invoke();
    }

    public static Vector2 GetScreenSizeWorld(Camera camera)
    {
        float orthoSize = camera.orthographicSize;
        float screenHeight = orthoSize * 2;
        float screenWidth = screenHeight * camera.aspect;

        return new Vector2(screenWidth, screenHeight);
    }

    #region LEVEL
    public static LevelDifficulty GetLevelDifficulty(int level)
    {
        int modulusLevel = level % 5;

        if (modulusLevel >= 1 && modulusLevel <= 3)
        {
            return LevelDifficulty.Normal;
        }
        else if (modulusLevel == 4)
        {
            return LevelDifficulty.VeryHard;
        }
        else
        {
            return LevelDifficulty.Hard;
        }
    }
    #endregion
}
