using TMPro;
using UnityEngine;

public class UserLivesUI : MonoBehaviour
{
    [SerializeField] private TMP_Text numLivesText;
    [SerializeField] private TMP_Text replenishLifeTimeText;

    void Awake()
    {
        ReplenishLifeManager.updateLivesReplenishTimeEvent += UpdateLivesReplenishTime;
    }

    void OnDestroy()
    {
        ReplenishLifeManager.updateLivesReplenishTimeEvent -= UpdateLivesReplenishTime;
    }

    private void UpdateLivesReplenishTime(string time)
    {
        replenishLifeTimeText.text = time;
    }
}
