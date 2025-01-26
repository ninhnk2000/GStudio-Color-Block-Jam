using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class LuckyWheelScreen : UIScreen
{
    [SerializeField] private Button rollButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private RectTransform rewardContainer;

    [SerializeField] private float maxRollSpeed;

    private float _rollSpeed;
    private float _deltaRollSpeed;
    float _expectedTotalAngleRotated;
    private float _angleRotated;
    private bool _isRotatingLastFrame;

    protected override void RegisterMoreEvent()
    {
        rollButton.onClick.AddListener(Roll);
        closeButton.onClick.AddListener(Hide);
    }

    private void Update()
    {
        if (_angleRotated < _expectedTotalAngleRotated)
        {
            rewardContainer.Rotate(_rollSpeed * new Vector3(0, 0, 1));

            _angleRotated += _rollSpeed;
            _rollSpeed -= _deltaRollSpeed;
        }
        else
        {
            if (_isRotatingLastFrame)
            {
                float min = float.MaxValue;
                float targetAngle = 0;

                for (int i = 0; i < 8; i++)
                {
                    float angleZ = 45f * i;

                    if (Mathf.Abs(angleZ - rewardContainer.rotation.eulerAngles.z) < min)
                    {
                        targetAngle = angleZ;

                        min = Mathf.Abs(angleZ - rewardContainer.rotation.eulerAngles.z);
                    }
                }

                Tween.Rotation(rewardContainer, new Vector3(0, 0, targetAngle), duration: 0.2f);

                _isRotatingLastFrame = false;
            }
        }
    }

    protected override void Show()
    {
        _transitionAnimation.Show();
    }

    protected override void Hide()
    {
        _transitionAnimation.Hide();
    }

    private void Roll()
    {
        _rollSpeed = maxRollSpeed;

        int randomFactor = UnityEngine.Random.Range(1, 8);

        _expectedTotalAngleRotated = 180f * 6 + 45f * randomFactor;

        // Ensure velocity is less than zero
        float expectedRollTime = 1.1f * _expectedTotalAngleRotated / maxRollSpeed;

        _angleRotated = 0;

        // s = v0 * t + 0.5f * a * t * t;
        // a = (s - v0 * t) / (t * t);
        _deltaRollSpeed = (_expectedTotalAngleRotated - maxRollSpeed * expectedRollTime) / (Mathf.Pow(expectedRollTime, 2));

        _isRotatingLastFrame = true;
    }
}
