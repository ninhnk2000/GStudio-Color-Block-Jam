using UnityEngine;

public class BaseBarricade : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private Direction direction;
    [SerializeField] private float disintegrationDuration;

    public Direction Direction
    {
        get => direction;
    }

    public float DisintegrationDuration
    {
        get => disintegrationDuration;
    }

    public void Disintegrate()
    {

    }
}
