using UnityEngine;
using static GameEnum;

public class ScrewFaction : MonoBehaviour
{
    [SerializeField] private ScrewServiceLocator screwServiceLocator;

    [SerializeField] private GameFaction faction;

    public GameFaction Faction
    {
        get => faction;
        set => faction = value;
    }

    void Awake()
    {
        // screwServiceLocator.screwMaterialPropertyBlock.SetColor(FactionUtility.GetColorForFaction(faction));
        screwServiceLocator.screwMaterialPropertyBlock.SetColor(faction);
    }

    public void SetColorByFaction()
    {
        // screwServiceLocator.screwMaterialPropertyBlock.SetColor(FactionUtility.GetColorForFaction(faction));
        screwServiceLocator.screwMaterialPropertyBlock.SetColor(faction);
    }
}
