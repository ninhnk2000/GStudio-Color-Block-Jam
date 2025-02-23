using TMPro;
using UnityEngine;

public class WoodenBlock : BaseBlock
{
    [Header("FREEZE BLOCK")]
    private bool _isFreeze;
    [SerializeField] private int remainingBlockToMelt;

    [SerializeField] private GameObject ice;
    [SerializeField] private TMP_Text remainingBlockToMeltText;
    [SerializeField] private ParticleSystem breakIceFx;

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
