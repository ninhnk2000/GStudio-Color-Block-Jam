using System.Threading.Tasks;
using Lean.Localization;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskItemUI : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Image progressBarFill;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private RectTransform rewardContainer;
    [SerializeField] private RectTransform doneTick;

    [SerializeField] private LeanLocalizedTextMeshProUGUI localizedDescriptionText;

    #region CACHE
    private BaseWeeklyTask _cachedTaskData;
    #endregion

    public RectTransform Container
    {
        get => container;
    }

    private void Awake()
    {
        BaseWeeklyTask.taskCompletedEvent += OnTaskCompleted;
    }

    private void OnDestroy()
    {
        BaseWeeklyTask.taskCompletedEvent -= OnTaskCompleted;
    }

    private async void OnEnable()
    {
        await Task.Delay(500);

        SetDescription();
    }

    public void SetDescription()
    {
        if (_cachedTaskData == null)
        {
            return;
        }

        string translationName;
        string parameter;

        _cachedTaskData.GetDesription(out translationName, out parameter);

        localizedDescriptionText.TranslationName = translationName;

        localizedDescriptionText.UpdateTranslationWithParameter(parameter, _cachedTaskData.RequirementValue.ToString());
    }

    public void Setup(BaseWeeklyTask taskData)
    {
        float progress = taskData.CurrentValue / taskData.RequirementValue;

        progressText.text = $"{taskData.CurrentValue}/{taskData.RequirementValue}";

        Tween.Custom(progressBarFill.fillAmount, progress, duration: 0.3f, onValueChange: newVal =>
        {
            progressBarFill.fillAmount = newVal;
        });

        rewardText.text = $"{taskData.Reward}";

        _cachedTaskData = taskData;

        rewardContainer.gameObject.SetActive(!taskData.IsDone);
        doneTick.gameObject.SetActive(taskData.IsDone);
    }

    private void OnTaskCompleted(BaseWeeklyTask taskData)
    {
        if (taskData == _cachedTaskData)
        {
            rewardContainer.gameObject.SetActive(false);
            doneTick.gameObject.SetActive(true);
        }
    }
}
