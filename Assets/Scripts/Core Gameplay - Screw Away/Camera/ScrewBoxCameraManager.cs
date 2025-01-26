using System;
using System.Threading.Tasks;
using UnityEngine;

public class ScrewBoxCameraManager : MonoBehaviour
{
    [SerializeField] private Camera screwBoxCamera;

    [SerializeField] private ScrewBoxCameraObserver screwBoxCameraObserver;

    public static event Action<Camera> setCameraEvent;

    private async void Awake()
    {
        screwBoxCameraObserver.ScrewBoxCamera = screwBoxCamera;

        // delay for others to register event
        await Task.Delay(20);

        setCameraEvent?.Invoke(screwBoxCamera);
    }
}
