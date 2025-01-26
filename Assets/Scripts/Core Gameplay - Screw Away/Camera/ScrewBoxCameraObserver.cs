using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Saferio/ScrewBoxCameraObserver")]
public class ScrewBoxCameraObserver : ScriptableObject
{
    [SerializeField] private Camera screwBoxCamera;

    public Camera ScrewBoxCamera
    {
        get => screwBoxCamera;
        set => screwBoxCamera = value;
    }
}
