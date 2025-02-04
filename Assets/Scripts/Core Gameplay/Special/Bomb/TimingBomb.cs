using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimingBomb : MonoBehaviour
{
    [SerializeField] private TMP_Text secondToExplodeText;
    [SerializeField] private int secondToExplode;

    public static event Action loseLevelEvent;

    private void Awake()
    {
        StartCoroutine(Ticking());
    }

    private IEnumerator Ticking()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);

        int remainingSecond = secondToExplode;

        while (remainingSecond >= 0)
        {
            secondToExplodeText.text = $"{remainingSecond}";

            remainingSecond--;

            yield return waitForSeconds;
        }

        loseLevelEvent?.Invoke();
    }
}
