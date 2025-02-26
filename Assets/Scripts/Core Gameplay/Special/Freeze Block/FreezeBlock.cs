using PrimeTween;
using TMPro;
using UnityEngine;

public class FreezeBlock : BaseBlock
{
    [Header("FREEZE BLOCK")]
    [SerializeField] private int remainingBlockToMelt = 3;
    [SerializeField] private GameObject ice;
    [SerializeField] private TMP_Text remainingBlockToMeltText;
    [SerializeField] private ParticleSystem breakIceFx;

    private bool _isFreeze;
    private bool _isInTransition;

    protected override void MoreLogicInAwake()
    {
        blockCompletedEvent += OnBlockDisintegrated;

        _isFreeze = true;

        remainingBlockToMeltText.text = $"{remainingBlockToMelt}";
    }

    protected override void MoreLogicOnDestroy()
    {
        blockCompletedEvent -= OnBlockDisintegrated;
    }

    public override void Move(Vector2 direction)
    {
        if (_isFreeze)
        {
            if (_isInTransition)
            {
                return;
            }
            else
            {
                _isInTransition = true;
            }

            _tweens.Add(Tween.Scale(transform, 1.1f * transform.localScale, cycles: 2, cycleMode: CycleMode.Yoyo, duration: 0.15f)
            .OnComplete(() =>
            {
                _isInTransition = false;
            }));

            return;
        }

        base.Move(direction);
    }

    public override void Stop()
    {
        if (_isFreeze)
        {
            return;
        }

        base.Stop();
    }

    private void OnBlockDisintegrated()
    {
        if (remainingBlockToMelt <= 0)
        {
            return;
        }

        remainingBlockToMelt--;

        if (remainingBlockToMelt == 0)
        {
            _isFreeze = false;

            remainingBlockToMeltText.gameObject.SetActive(false);
            ice.SetActive(false);

            breakIceFx.Play();
        }

        remainingBlockToMeltText.text = $"{remainingBlockToMelt}";
    }
}
