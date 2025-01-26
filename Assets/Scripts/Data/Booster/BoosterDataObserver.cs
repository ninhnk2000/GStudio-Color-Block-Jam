using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/Screw Away/BoosterDataObserver")]
public class BoosterDataObserver : ScriptableObject
{
    [SerializeField] private int[] boosterCosts;

    public int[] BoosterCosts
    {
        get => boosterCosts;
    }
}
