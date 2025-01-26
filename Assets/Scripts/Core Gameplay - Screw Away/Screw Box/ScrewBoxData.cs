using System;
using UnityEngine;
using static GameEnum;

[Serializable]
public class ScrewBoxData
{
    private GameFaction _faction;
    private bool _isLocked;

    public GameFaction Faction
    {
        get => _faction;
        set => _faction = value;
    }

    public bool IsLocked
    {
        get => _isLocked;
        set => _isLocked = value;
    }
}
