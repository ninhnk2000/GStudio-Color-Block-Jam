using UnityEngine;
using static GameEnum;

public interface IScrew
{
    public GameFaction Faction
    {
        get;
    }

    public void Select();

    public int CountBlockingObjects();
}
