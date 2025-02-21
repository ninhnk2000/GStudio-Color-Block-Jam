using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class LevelTimeCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Image freezedBackground;

    [SerializeField] private int totalSecond;

    private List<Tween> _tweens;
    private Coroutine _countingCoroutine;
    private bool _isFreeze;

    public static event Action loseLevelEvent;

    private void Awake()
    {
        LevelLoader.startLevelEvent += OnLevelStarted;
        BoosterUI.freezeTimeEvent += FreezeTime;
        RevivePopup.reviveEvent += Revive;

        _tweens = new List<Tween>();
    }

    private void OnDestroy()
    {
        LevelLoader.startLevelEvent -= OnLevelStarted;
        BoosterUI.freezeTimeEvent -= FreezeTime;
        RevivePopup.reviveEvent -= Revive;

        CommonUtil.StopAllTweens(_tweens);
    }

    private void OnLevelStarted()
    {
        if (_countingCoroutine != null)
        {
            StopCoroutine(_countingCoroutine);
        }

        _countingCoroutine = StartCoroutine(Counting());
    }

    private IEnumerator Counting()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);

        int remainingSecond = totalSecond;

        while (remainingSecond > 0)
        {
            if (!_isFreeze)
            {
                timeText.text = ConvertSecondsToMinutesSeconds(remainingSecond);

                remainingSecond--;
            }

            yield return waitForSeconds;
        }

        loseLevelEvent?.Invoke();
    }

    private void FreezeTime()
    {
        _isFreeze = true;

        _tweens.Add(Tween.Delay(10).OnComplete(() =>
        {
            FreezedEffect(isFreeze: false);

            _isFreeze = false;
        }));

        FreezedEffect(isFreeze: true);
    }

    private void FreezedEffect(bool isFreeze)
    {
        if (isFreeze)
        {
            freezedBackground.gameObject.SetActive(true);

            freezedBackground.color = ColorUtil.WithAlpha(freezedBackground.color, 0);

            _tweens.Add(Tween.Alpha(freezedBackground, 1, duration: 0.3f));
        }
        else
        {
            _tweens.Add(Tween.Alpha(freezedBackground, 0, duration: 0.3f)
                .OnComplete(() =>
                {
                    freezedBackground.gameObject.SetActive(false);
                })
            );
        }
    }

    private void Revive(BoosterType boosterType)
    {
        totalSecond = 20;

        _countingCoroutine = StartCoroutine(Counting());
    }

    string ConvertSecondsToMinutesSeconds(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int remainingSeconds = Mathf.FloorToInt(seconds % 60);

        return string.Format("{0:D2}:{1:D2}", minutes, remainingSeconds);
    }
}
