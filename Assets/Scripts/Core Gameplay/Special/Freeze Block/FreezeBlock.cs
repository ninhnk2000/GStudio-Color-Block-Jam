using TMPro;
using UnityEngine;

public class FreezeBlockProperty : BaseBlock
{
    [Header("FREEZE BLOCK")]
    private bool _isFreeze;
    [SerializeField] private int remainingBlockToMelt;

    [SerializeField] private TMP_Text remainingBlockToMeltText;

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
        }

        remainingBlockToMeltText.text = $"{remainingBlockToMelt}";
    }
}
