using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/ScrewBoxesObserver")]
public class ScrewBoxesObserver : ScriptableObject
{
    private int _numLockedScrewBoxes;
    private int _numScrewInScrewPorts;

    public int NumLockedScrewBoxes
    {
        get => _numLockedScrewBoxes;
        set => _numLockedScrewBoxes = value;
    }

    public int NumScrewInScrewPorts
    {
        get => _numScrewInScrewPorts;
        set => _numScrewInScrewPorts = value;
    }
}
