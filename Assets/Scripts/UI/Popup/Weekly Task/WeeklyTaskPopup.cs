using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeeklyTaskPopup : BasePopup
{
    [SerializeField] private RectTransform taskContainer;
    [SerializeField] private Image progressBarFill;
    [SerializeField] private GameTaskManager gameTaskManager;
    [SerializeField] private TMP_Text remainingTime;

    protected override void MoreActionInAwake()
    {
        DelaySpawnTasks();

        CountingTime();
    }

    private async void DelaySpawnTasks()
    {
        await Task.Delay(2000);

        int currentTotalScore = 0;
        int totalScore = 0;

        for (int i = 0; i < gameTaskManager.Tasks.Length; i++)
        {
            BaseWeeklyTask taskData = gameTaskManager.Tasks[i];

            TaskItemUI taskItemUI = ObjectPoolingEverything.GetFromPool<TaskItemUI>(GameConstants.TASK_ITEM_UI);

            taskItemUI.Container.SetParent(taskContainer);
            taskItemUI.Container.localScale = Vector3.one;

            UIUtil.SetLocalPositionX(taskItemUI.Container, 0);
            UIUtil.SetLocalPositionY(taskItemUI.Container, 240 - i * 1.1f * taskItemUI.Container.sizeDelta.y);

            taskItemUI.Setup(taskData);

            if (taskData.IsDone)
            {
                currentTotalScore += taskData.Reward;
            }

            totalScore += taskData.Reward;
        }

        progressBarFill.fillAmount = (float)currentTotalScore / totalScore;
    }

    private async void CountingTime()
    {
        while (true)
        {
            remainingTime.text = GetRemainingTime();

            await Task.Delay(60000);
        }
    }

    private string GetRemainingTime()
    {
        DateTime currentTime = DateTime.Now;
        DateTime endOfWeek = currentTime.AddDays(DayOfWeek.Saturday - currentTime.DayOfWeek).Date.AddDays(1).AddTicks(-1);
        TimeSpan remainingTime = endOfWeek - currentTime;

        int days = remainingTime.Days;
        int hours = remainingTime.Hours;
        int minutes = remainingTime.Minutes;

        string formattedTime = $"{days:D2}:{hours:D2}:{minutes:D2}";

        return formattedTime;
    }
}
