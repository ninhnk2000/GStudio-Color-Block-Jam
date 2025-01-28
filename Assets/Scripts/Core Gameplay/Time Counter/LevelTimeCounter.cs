using System.Collections;
using TMPro;
using UnityEngine;

public class LevelTimeCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;

    [SerializeField] private int totalSecond;

    private Coroutine _countingCoroutine;

    private void Awake()
    {
        LevelLoader.startLevelEvent += OnLevelStarted;
    }

    private void OnDestroy()
    {
        LevelLoader.startLevelEvent -= OnLevelStarted;
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
            timeText.text = ConvertSecondsToMinutesSeconds(remainingSecond);

            remainingSecond--;

            yield return waitForSeconds;
        }
    }

    string ConvertSecondsToMinutesSeconds(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int remainingSeconds = Mathf.FloorToInt(seconds % 60);

        return string.Format("{0:D2}:{1:D2}", minutes, remainingSeconds);
    }
}
