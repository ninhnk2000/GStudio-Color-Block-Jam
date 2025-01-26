using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Saferio/Screw Away/LevelObserver")]
public class LevelObserver : ScriptableObject
{
    private float _progress;

    public float Progress
    {
        get => _progress;
        set => _progress = value;
    }
}
