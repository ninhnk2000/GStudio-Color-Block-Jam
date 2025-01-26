using System;
using UnityEngine;

public class CompleteLevelTask : BaseWeeklyTask
{
    public override float RequirementValue
    {
        get; set;
    }

    public override float CurrentValue
    {
        get; set;
    }

    public override int Reward
    {
        get; set;
    }

    public override bool IsDone
    {
        get; set;
    }

    public override void Init()
    {

    }

    public override void GetDesription(out string translationName, out string parameter)
    {
        translationName = GameConstants.COMPLETE_LEVEL_TRANSLATION_NAME;

        parameter = GameConstants.TASK_DESCRIPTION_PARAMETER;
    }
}
