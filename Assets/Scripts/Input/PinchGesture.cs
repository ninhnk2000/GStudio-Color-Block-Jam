using System;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class PinchGesture : MonoBehaviour
{
    public Camera cameraToZoom;

    [SerializeField] private Slider zoomSlider;

    public float zoomSpeed = 0.1f;
    public float minZoom = 10f;
    public float maxZoom = 100f;
    public float changeThreshold;
    public float delayToEnableSwipeGesture;

    private List<Tween> _tweens;
    private bool _isDisabled;
    private float _zoomPercent;
    private float _lastZoomPercent;
    private bool _isZoomByPinching;

    #region ACTION
    public static event Action<float> pinchGestureEvent;
    public static event Action<bool> enableSwipingEvent;
    #endregion

    private void Awake()
    {
        LevelLoader.startLevelEvent += OnLevelStarted;
        BasePopup.enableSwipingScreenEvent += EnablePinching;
        LevelTutorial.enablePinchingEvent += EnablePinching;

        zoomSlider.onValueChanged.AddListener(OnZoomSliderValueChanged);

        _tweens = new List<Tween>();

        minZoom = 0.85f * cameraToZoom.orthographicSize;
        maxZoom = 1.15f * cameraToZoom.orthographicSize;
    }

    void OnDestroy()
    {
        LevelLoader.startLevelEvent -= OnLevelStarted;
        BasePopup.enableSwipingScreenEvent -= EnablePinching;
        LevelTutorial.enablePinchingEvent -= EnablePinching;

        CommonUtil.StopAllTweens(_tweens);
    }

    private void OnZoomSliderValueChanged(float value)
    {
        if (_isDisabled)
        {
            return;
        }

        pinchGestureEvent?.Invoke(maxZoom - value * (maxZoom - minZoom));

        if (!_isZoomByPinching)
        {
            enableSwipingEvent?.Invoke(false);
        }

        CommonUtil.StopAllTweens(_tweens);

        _tweens.Add(Tween.Delay(delayToEnableSwipeGesture).OnComplete(() =>
        {
            enableSwipingEvent?.Invoke(true);
        }));

        _lastZoomPercent = value;
    }

    private void Update()
    {
        if (_isDisabled)
        {
            return;
        }

        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            float targetOrthographicSize = Mathf.Clamp(cameraToZoom.orthographicSize + deltaMagnitudeDiff * zoomSpeed, minZoom, maxZoom);

            // pinchGestureEvent?.Invoke(targetOrthographicSize);

            _zoomPercent = (maxZoom - targetOrthographicSize) / (maxZoom - minZoom);

            zoomSlider.interactable = false;

            _isZoomByPinching = true;

            // if (Mathf.Abs(_zoomPercent - _lastZoomPercent) < changeThreshold)
            // {
            //     return;
            // }

            // zoomSlider.value = _zoomPercent;
            // updateZoomSliderEvent?.Invoke(targetOrthographicSize / maxZoom);
        }

        if (_isZoomByPinching)
        {
            if (!Mathf.Approximately(zoomSlider.value, _zoomPercent))
            {
                zoomSlider.value = Mathf.Lerp(zoomSlider.value, _zoomPercent, 1f / 4);
            }
            else
            {
                zoomSlider.interactable = true;

                _isZoomByPinching = false;
            }
        }
    }

    private void OnLevelStarted()
    {
        _isDisabled = false;
        
        zoomSlider.value = 0;

        OnZoomSliderValueChanged(0);

        // pinchGestureEvent?.Invoke(maxZoom);
    }

    private void EnablePinching(bool isEnable)
    {
        _isDisabled = !isEnable;
    }
}
