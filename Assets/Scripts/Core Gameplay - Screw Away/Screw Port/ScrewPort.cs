using UnityEngine;
using static GameEnum;

public class ScrewPort : MonoBehaviour
{
    [SerializeField] private BaseScrew screw;
    [SerializeField] private HingeJoint joint;

    // void Awake()
    // {
    //     ScrewBoxManager.looseScrewEvent += OnScrewLoosed;
    // }

    // void OnDestroy()
    // {
    //     ScrewBoxManager.looseScrewEvent -= OnScrewLoosed;
    // }

    // private void OnScrewLoosed(string screwId, GameFaction faction, Vector3 screwBoxPosition)
    // {
    //     if (screwId == screw.ScrewId)
    //     {
    //         joint.breakForce = 0;
    //     }
    // }
}
