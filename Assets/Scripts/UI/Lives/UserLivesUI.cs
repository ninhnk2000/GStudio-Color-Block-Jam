using TMPro;
using UnityEngine;

public class UserLivesUI : MonoBehaviour
{
    [SerializeField] private TMP_Text numLivesText;
    [SerializeField] private TMP_Text replenishLifeTimeText;

    private LivesData _livesData;

    void Awake()
    {
        ReplenishLifeManager.updateLivesReplenishTimeEvent += UpdateLivesReplenishTime;
        ReplenishLifeManager.updateLivesNumberEvent += UpdateLivesNumber;
    }

    void OnDestroy()
    {
        ReplenishLifeManager.updateLivesReplenishTimeEvent -= UpdateLivesReplenishTime;
        ReplenishLifeManager.updateLivesNumberEvent -= UpdateLivesNumber;
    }

    private void UpdateLivesReplenishTime(string time)
    {
        replenishLifeTimeText.text = time;
    }

    private void UpdateLivesNumber(int lives)
    {
        numLivesText.text = $"{lives}";

        if (lives == GameConstants.DEFAULT_LIVES)
        {
            replenishLifeTimeText.text = $"Full";
        }
    }
}
