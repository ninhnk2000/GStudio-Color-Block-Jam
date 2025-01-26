using System;
using UnityEngine;

public class UnscrewTask : BaseWeeklyTask
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
        BaseScrew.screwLoosenedEvent += OnScrewLoosened;
    }

    public override void GetDesription(out string translationName, out string parameter)
    {
        translationName = GameConstants.UNSCREW_TASK_TRANSLATION_NAME;

        parameter = GameConstants.TASK_DESCRIPTION_PARAMETER;
    }

    private void OnScrewLoosened()
    {
        MakeProgress(1);
    }
}
