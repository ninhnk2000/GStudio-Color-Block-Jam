using TMPro;
using UnityEngine;

public class FreezeBlock : BaseBlock
{
    [Header("FREEZE BLOCK")]
    [SerializeField] private int remainingBlockToMelt;
    [SerializeField] private GameObject ice;
    [SerializeField] private TMP_Text remainingBlockToMeltText;
    [SerializeField] private ParticleSystem breakIceFx;

    private bool _isFreeze;

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

    public override void Move(Vector3 targetPosition)
    {
        if (_isFreeze)
        {
            return;
        }

        base.Move(targetPosition);
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
