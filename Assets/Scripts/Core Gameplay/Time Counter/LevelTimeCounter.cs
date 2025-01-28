using System.Collections;
using PrimeTween;
using TMPro;
using UnityEngine;
using static GameEnum;

public class LevelTimeCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;

    [SerializeField] private int totalSecond;

    private Coroutine _countingCoroutine;
    private bool _isFreeze;

    private void Awake()
    {
        LevelLoader.startLevelEvent += OnLevelStarted;
        BoosterItemUI.useBoosterEvent += FreezeTime;
    }

    private void OnDestroy()
    {
        LevelLoader.startLevelEvent -= OnLevelStarted;
        BoosterItemUI.useBoosterEvent -= FreezeTime;
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
    }

    private void FreezeTime(BoosterType boosterType)
    {
        if (boosterType == BoosterType.FreezeTime)
        {
            _isFreeze = true;

            Tween.Delay(10).OnComplete(() =>
            {
                _isFreeze = false;
            });
        }
    }

    string ConvertSecondsToMinutesSeconds(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int remainingSeconds = Mathf.FloorToInt(seconds % 60);

        return string.Format("{0:D2}:{1:D2}", minutes, remainingSeconds);
    }
}
