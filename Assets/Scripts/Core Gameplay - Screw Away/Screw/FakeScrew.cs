using UnityEngine;

public class FakeScrew : BasicScrew
{
    public void CloneFromScrew(BaseScrew screw)
    {
        transform.position = screw.transform.position;
        transform.localScale = new Vector3(
            screw.transform.lossyScale.x / transform.parent.lossyScale.x,
            screw.transform.lossyScale.y / transform.parent.lossyScale.y,
            screw.transform.lossyScale.z / transform.parent.lossyScale.z
        );
        transform.rotation = screw.transform.rotation;

        // screwServiceLocator.screwMaterialPropertyBlock.SetColor(FactionUtility.GetColorForFaction(screw.Faction));
        screwServiceLocator.screwMaterialPropertyBlock.SetColor(screw.Faction);
    }
}
