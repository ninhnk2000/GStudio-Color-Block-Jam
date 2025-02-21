using System;
using System.Collections;
using UnityEngine;
using static GameEnum;

public class ReplenishLifeManager : MonoBehaviour
{
    private const int MaxLives = 5;
    private const int ReplenishTime = 1800;

    private LivesData _livesData;

    public static event Action<string> updateLivesReplenishTimeEvent;
    public static event Action<int> updateLivesNumberEvent;
    public static event Action<ScreenRoute> switchRouteEvent;

    void Awake()
    {
        MenuScreen.changeLivesNumberEvent += ChangeLivesNumber;
        LivesShopPopup.changeLivesNumberEvent += ChangeLivesNumber;
        ReplayPopup.changeLivesNumberEvent += ChangeLivesNumber;
    }

    void Start()
    {
        LivesData defaultLivesData = new LivesData();

        defaultLivesData.LastReplenishTime = DateTime.Now;

        _livesData = DataUtility.Load(GameConstants.USER_LIVES_DATA, new LivesData());

        GamePersistentVariable.livesData = _livesData;

        StartCoroutine(Counting());

        updateLivesNumberEvent?.Invoke(_livesData.CurrentLives);
    }

    void OnDestroy()
    {
        MenuScreen.changeLivesNumberEvent -= ChangeLivesNumber;
        LivesShopPopup.changeLivesNumberEvent -= ChangeLivesNumber;
        ReplayPopup.changeLivesNumberEvent -= ChangeLivesNumber;
    }

    private IEnumerator Counting()
    {
        WaitForSeconds waitOneSecond = new WaitForSeconds(1);

        while (true)
        {
            if (_livesData.CurrentLives < MaxLives)
            {
                TimeSpan timeSinceLastReplenish = DateTime.Now - _livesData.LastReplenishTime;

                double remainingTime = ReplenishTime - timeSinceLastReplenish.TotalSeconds;

                updateLivesReplenishTimeEvent?.Invoke(ConvertTimeFormat(remainingTime));

                if (remainingTime <= 0)
                {
                    _livesData.CurrentLives++;
                    _livesData.LastReplenishTime = DateTime.Now;

                    updateLivesNumberEvent?.Invoke(_livesData.CurrentLives);
                }

                DataUtility.Save(GameConstants.USER_LIVES_DATA, _livesData);

                GamePersistentVariable.livesData = _livesData;
            }
            else
            {

            }

            yield return waitOneSecond;
        }
    }

    private void Replenish(int numLives)
    {

    }

    private void ChangeLivesNumber(int value)
    {
        _livesData.CurrentLives += value;

        DataUtility.Save(GameConstants.USER_LIVES_DATA, _livesData);

        GamePersistentVariable.livesData = _livesData;

        updateLivesNumberEvent?.Invoke(_livesData.CurrentLives);
    }

    #region UTIL
    private string ConvertTimeFormat(double time)
    {
        return $"{TimeSpan.FromSeconds(time).ToString(@"hh\:mm\:ss")}";
    }
    #endregion
}

[System.Serializable]
public class LivesData
{
    private int currentLives;
    private DateTime lastReplenishTime;

    public int CurrentLives
    {
        get => currentLives;
        set => currentLives = value;
    }

    public DateTime LastReplenishTime
    {
        get => lastReplenishTime;
        set => lastReplenishTime = value;
    }
}
