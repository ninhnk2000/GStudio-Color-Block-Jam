using TMPro;
using UnityEngine;

public class TiedBlock : BaseBlock
{
    [Header("TIED BLOCK")]
    [SerializeField] private GameObject remainingRopeTag;
    [SerializeField] private int remainingRope;
    private bool _isTied;

    [SerializeField] private TMP_Text remainingRopeText;
    [SerializeField] private ParticleSystem breakIceFx;

    protected override void MoreLogicInAwake()
    {
        _isTied = true;

        remainingRopeText.text = $"{remainingRope}";
    }

    public override void Move(Vector2 direction)
    {
        if (_isTied)
        {
            return;
        }

        base.Move(direction);
    }

    public override void Stop()
    {
        if (_isTied)
        {
            return;
        }

        base.Stop();
    }

    public void CutRope()
    {
        if (remainingRope <= 0)
        {
            return;
        }

        remainingRope--;

        if (remainingRope == 0)
        {
            _isTied = false;

            remainingRopeText.gameObject.SetActive(false);
            remainingRopeTag.gameObject.SetActive(false);

            breakIceFx.Play();
        }

        remainingRopeText.text = $"{remainingRope}";
    }
}
