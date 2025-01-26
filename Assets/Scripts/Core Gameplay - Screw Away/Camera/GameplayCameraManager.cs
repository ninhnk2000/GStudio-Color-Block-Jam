using PrimeTween;
using UnityEngine;

public class GameplayCameraManager : MonoBehaviour
{
    public static GameplayCameraManager Instance;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera screwBoxCamera;

    private void Awake()
    {
        ScrewBox.shakeCameraEvent += ShakeAllCameras;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }

    void OnDestroy()
    {
        ScrewBox.shakeCameraEvent -= ShakeAllCameras;
    }

    public static void ConvertCameraSpaceSmoothly(Transform target)
    {
        float ratio = Instance.screwBoxCamera.orthographicSize / Instance.mainCamera.orthographicSize;

        target.position *= ratio;
        target.localScale *= ratio;
    }

    private void ShakeAllCameras()
    {
        Tween.ShakeCamera(mainCamera, strengthFactor: 0.3f, duration: 0.3f);
        Tween.ShakeCamera(screwBoxCamera, strengthFactor: 0.3f, duration: 0.3f);
    }
}
