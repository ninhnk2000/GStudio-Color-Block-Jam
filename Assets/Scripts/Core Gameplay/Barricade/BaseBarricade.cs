using UnityEngine;
using static GameEnum;

public class BaseBarricade : MonoBehaviour
{
    [SerializeField] private BarricadeServiceLocator barricadeServiceLocator;

    [Header("CUSTOMIZE")]
    [SerializeField] private BarricadeProperty barricadeProperty;
    [SerializeField] private float disintegrationDuration;

    public BarricadeServiceLocator BarricadeServiceLocator
    {
        get => barricadeServiceLocator;
    }

    public BarricadeProperty BarricadeProperty
    {
        get => barricadeProperty;
    }

    public Direction Direction
    {
        get => barricadeProperty.Direction;
        set => barricadeProperty.Direction = value;
    }

    public GameFaction Faction
    {
        get => barricadeProperty.Faction;
    }

    public float DisintegrationDuration
    {
        get => disintegrationDuration;
    }

    private void Awake()
    {
        barricadeServiceLocator.barricadeFaction.SetFaction(barricadeProperty.Faction);
    }

    private void OnValidate()
    {
        barricadeServiceLocator.barricadeFaction.SetFaction(barricadeProperty.Faction);
    }
}
