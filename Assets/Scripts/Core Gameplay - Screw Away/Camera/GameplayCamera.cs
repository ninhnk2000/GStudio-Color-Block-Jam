using PrimeTween;
using UnityEngine;

public class GameplayCamera : MonoBehaviour
{
    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private Transform background;

    private float _initialOrthographicSize;
    private float _targetOrthographicSize;
    private Vector3 _initialBackgroundScale;
    private Vector3 _targetBackgroundScale;

    private void Awake()
    {
        LevelLoader.startLevelEvent += OnLevelStart;
        PinchGesture.pinchGestureEvent += Zoom;
        BasicObjectPart.shakeCameraEvent += Shake;
        MultiPhaseLevelManager.zoomCameraEvent += Zoom;
        MultiPhaseLevelManager.resetCameraEvent += Reset;
        LevelLoader.setLevelCameraOrthographicSize += SetFieldOfView;

        _initialOrthographicSize = gameplayCamera.orthographicSize;
        _targetOrthographicSize = gameplayCamera.orthographicSize;

        _initialBackgroundScale = background.localScale;
        _targetBackgroundScale = background.localScale;
    }

    private void OnDestroy()
    {
        LevelLoader.startLevelEvent -= OnLevelStart;
        PinchGesture.pinchGestureEvent -= Zoom;
        BasicObjectPart.shakeCameraEvent -= Shake;
        MultiPhaseLevelManager.zoomCameraEvent -= Zoom;
        MultiPhaseLevelManager.resetCameraEvent -= Reset;
        LevelLoader.setLevelCameraOrthographicSize -= SetFieldOfView;
    }

    private void Update()
    {
        gameplayCamera.orthographicSize = Mathf.Lerp(gameplayCamera.orthographicSize, _targetOrthographicSize, 0.333f);

        background.localScale = Vector3.Lerp(background.localScale, _targetBackgroundScale, 0.333f);
    }

    private void OnLevelStart()
    {
        Reset();
    }

    private void Reset()
    {
        // _targetOrthographicSize = _initialOrthographicSize;
        // _targetBackgroundScale = _initialBackgroundScale;
    }

    private void Zoom(float orthographicSize)
    {
        _targetOrthographicSize = orthographicSize;
        _targetBackgroundScale = _initialBackgroundScale * orthographicSize / _initialOrthographicSize;
    }

    private void Shake()
    {
        Tween.ShakeCamera(gameplayCamera, 1, duration: 0.3f);
    }

    private void SetFieldOfView(float fieldOfView)
    {
        gameplayCamera.fieldOfView = fieldOfView;
    }
}
