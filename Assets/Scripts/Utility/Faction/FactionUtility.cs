using UnityEngine;
using static GameEnum;

public static class FactionUtility
{
    public static Color GetColorForFaction(GameFaction faction)
    {
        if (faction == GameFaction.Red)
        {
            return GameConstants.SAFERIO_RED;
        }
        else if (faction == GameFaction.Blue)
        {
            return GameConstants.SAFERIO_BLUE;
        }
        else if (faction == GameFaction.Green)
        {
            return GameConstants.SAFERIO_GREEN;
        }
        else if (faction == GameFaction.Purple)
        {
            return GameConstants.SAFERIO_PURPLE;
        }
        else if (faction == GameFaction.Orange)
        {
            return GameConstants.SAFERIO_ORANGE;
        }
        else if (faction == GameFaction.Yellow)
        {
            return GameConstants.SAFERIO_YELLLOW;
        }
        else if (faction == GameFaction.Cyan)
        {
            return GameConstants.SAFERIO_CYAN;
        }
        else if (faction == GameFaction.White)
        {
            return GameConstants.SAFERIO_WHITE;
        }
        else if (faction == GameFaction.Brown)
        {
            return GameConstants.SAFERIO_BROWN;
        }
        else if (faction == GameFaction.Disabled)
        {
            return GameConstants.SAFERIO_DISABLED;
        }

        return GameConstants.SAFERIO_RED;
    }
}
