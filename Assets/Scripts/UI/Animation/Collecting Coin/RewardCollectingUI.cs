using System;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class RewardCollectingUI : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject container;

    [Header("CUSTOMIZE")]
    [SerializeField] private int poolSize;

    private RectTransform[] _rewards;

    #region EVENT
    public static event Action<float> addCoinEvent;
    #endregion


    private void Awake()
    {
        CoinContainerUI.collectCoinEvent += Collect;
        WinPopup.collectCoinEvent += Collect;

        CreatePool();
    }

    private void OnDestroy()
    {
        CoinContainerUI.collectCoinEvent -= Collect;
        WinPopup.collectCoinEvent -= Collect;
    }

    private void CreatePool()
    {
        _rewards = new RectTransform[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            _rewards[i] = Instantiate(prefab, container.transform).GetComponent<RectTransform>();

            _rewards[i].gameObject.SetActive(false);
        }
    }

    private async void Collect(Vector3 position)
    {
        Collect(position);
    }

    private async void Collect(Vector3 position, int totalValue = 0)
    {
        for (int i = 0; i < poolSize; i++)
        {
            _rewards[i].gameObject.SetActive(true);

            _rewards[i].localPosition = new Vector3(UnityEngine.Random.Range(-150, 150), UnityEngine.Random.Range(-150, 150));
            _rewards[i].localScale = Vector3.zero;

            Tween.Scale(_rewards[i], 1, duration: UnityEngine.Random.Range(0.2f, 0.6f));
        }

        await Task.Delay(620);

        for (int i = 0; i < poolSize; i++)
        {
            int index = i;

            Tween.LocalPosition(_rewards[index], position, duration: UnityEngine.Random.Range(0.2f, 0.6f)).OnComplete(() =>
            {
                addCoinEvent?.Invoke((float)totalValue / poolSize);

                _rewards[index].gameObject.SetActive(false);
            });
        }
    }
}
