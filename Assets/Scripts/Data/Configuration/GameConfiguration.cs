using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/Tag Area/GameConfiguration")]
public class GameConfiguration : ScriptableObject
{
    [SerializeField] private int maxGameTime;

    public int MaxGameTime
    {
        get => maxGameTime;
        set => maxGameTime = value;
    }
}
