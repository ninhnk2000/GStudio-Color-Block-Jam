using UnityEngine;
using static GameEnum;

public class BaseBarricade : MonoBehaviour
{
    [SerializeField] private BarricadeServiceLocator barricadeServiceLocator;

    [Header("CUSTOMIZE")]
    [SerializeField] private Direction direction;
    [SerializeField] private float disintegrationDuration;

    public Direction Direction
    {
        get => direction;
    }

    public GameFaction Faction
    {
        get => barricadeServiceLocator.barricadeFaction.Faction;
    }

    public float DisintegrationDuration
    {
        get => disintegrationDuration;
    }
}
