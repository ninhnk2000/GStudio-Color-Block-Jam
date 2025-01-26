using System;
using UnityEngine;

[Serializable]
public abstract class BaseWeeklyTask
{
    public abstract float RequirementValue
    {
        get; set;
    }

    public abstract float CurrentValue
    {
        get; set;
    }

    public float Progress
    {
        get => CurrentValue / RequirementValue;
    }

    public abstract int Reward
    {
        get; set;
    }

    public abstract bool IsDone
    {
        get; set;
    }

    public static event Action<BaseWeeklyTask> taskCompletedEvent;

    public abstract void Init();

    public abstract void GetDesription(out string translationName, out string parameter);

    public void MakeProgress(float addedValue)
    {
        if (!IsDone)
        {
            CurrentValue += addedValue;

            if (CurrentValue >= RequirementValue)
            {
                taskCompletedEvent?.Invoke(this);

                CurrentValue = RequirementValue;

                IsDone = true;
            }
        }
    }
}
