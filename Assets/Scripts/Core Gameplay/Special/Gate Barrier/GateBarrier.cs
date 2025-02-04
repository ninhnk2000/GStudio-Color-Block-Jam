using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class GateBarrier : MonoBehaviour
{
    [SerializeField] private bool _isOpen;

    private List<Tween> _tweens;
    private Collider _barrierCollider;
    private float _initialPositionY;

    private void Awake()
    {
        BaseBlock.blockCompletedEvent += ChangeState;

        _tweens = new List<Tween>();

        _initialPositionY = transform.position.y;

        _barrierCollider = GetComponent<Collider>();
    }

    private void OnDestroy()
    {
        BaseBlock.blockCompletedEvent -= ChangeState;

        CommonUtil.StopAllTweens(_tweens);
    }

    private void ChangeState()
    {
        CommonUtil.StopAllTweens(_tweens);

        if (_isOpen)
        {
            _tweens.Add(Tween.PositionY(transform, _initialPositionY, duration: 0.3f));
        }
        else
        {
            _tweens.Add(Tween.PositionY(transform, _initialPositionY + 4, duration: 0.3f));
        }

        _isOpen = !_isOpen;

        _barrierCollider.enabled = !_isOpen;
    }
}
