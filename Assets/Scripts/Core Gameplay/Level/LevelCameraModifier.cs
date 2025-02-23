using UnityEngine;

public class LevelCameraModifier : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private float fieldOfView;

    public float FieldOfView
    {
        get => fieldOfView;
    }
}
