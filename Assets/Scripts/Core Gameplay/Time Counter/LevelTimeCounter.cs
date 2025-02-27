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

    [SerializeField] private CanvasGroup freezeProgressGroup;
    [SerializeField] private Slider freezeProgress;
    [SerializeField] private Image freezeProgressFill;

    [SerializeField] private int totalSecond;

    [SerializeField] private IntVariable currentLevel;

    private List<Tween> _tweens;
    private Coroutine _countingCoroutine;
    private bool _isFreeze;

    public static event Action loseLevelEvent;
    public static event Action unfreezeTimeEvent;

    // TEMP
    private int[] levelsTime = new int[50] {
        300, 300, 300, 300, 300, 300, 300, 300, 300, 300,
        100, 75, 150, 120, 100, 80, 120, 120, 100, 140,
        100, 90, 100, 150, 85, 100, 130, 120, 170, 200,
        100, 90, 80, 120, 60, 90, 115, 120, 180, 240,
        130, 150, 120, 160, 195, 120, 120, 200, 200, 200
    };

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

        totalSecond = levelsTime[currentLevel.Value % 50];

        _countingCoroutine = StartCoroutine(Counting());

        if (_isFreeze)
        {
            Unfreeze();
        }

        freezeProgressGroup.gameObject.SetActive(false);
    }

    private IEnumerator Counting()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);

        int remainingSecond = totalSecond;

        timeText.text = ConvertSecondsToMinutesSeconds(remainingSecond);

        while (remainingSecond >= 0)
        {
            if (!_isFreeze && GamePersistentVariable.isLevelDirty)
            {
                timeText.text = ConvertSecondsToMinutesSeconds(remainingSecond);

                remainingSecond--;
            }

            yield return waitForSeconds;
        }

        yield return waitForSeconds;

        loseLevelEvent?.Invoke();
    }

    private void FreezeTime()
    {
        _isFreeze = true;

        freezeProgressGroup.gameObject.SetActive(true);

        freezeProgress.value = 1;

        // progress bar
        _tweens.Add(Tween.Custom(0, 1, duration: 0.3f, onValueChange: newVal =>
        {
            freezeProgressGroup.alpha = newVal;

            _tweens.Add(Tween.Custom(1, 0, duration: 9.7f, onValueChange: newVal =>
            {
                freezeProgress.value = newVal;
                freezeProgressFill.color = ColorUtil.WithAlpha(freezeProgressFill.color, 2 * (newVal - 0.08f));
            }).OnComplete(() =>
            {
                _tweens.Add(Tween.Custom(1, 0, duration: 0.3f, onValueChange: newVal =>
                {
                    freezeProgressGroup.alpha = newVal;
                })
                .OnComplete(() =>
                {
                    freezeProgressGroup.gameObject.SetActive(false);
                })
                );
            }));
        }));

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

    private void Unfreeze()
    {
        CommonUtil.StopAllTweens(_tweens);

        FreezedEffect(isFreeze: false);

        unfreezeTimeEvent?.Invoke();

        _isFreeze = false;
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
