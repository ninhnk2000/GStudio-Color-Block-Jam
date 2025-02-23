using PrimeTween;
using TMPro;
using UnityEngine;

public class LockedBlock : BaseBlock
{
    [Header("LOCKED BLOCK")]
    [SerializeField] private Transform blockLock;
    [SerializeField] private int remainingKeyToUnlock;
    private bool _isLocked;

    [SerializeField] private TMP_Text remainingKeyToUnlockText;
    [SerializeField] private ParticleSystem breakIceFx;

    protected override void MoreLogicInAwake()
    {
        Key.unlockWithKeyEvent += UnlockWithAKey;

        _isLocked = true;

        remainingKeyToUnlockText.text = $"{remainingKeyToUnlock}";
    }

    protected override void MoreLogicOnDestroy()
    {
        Key.unlockWithKeyEvent -= UnlockWithAKey;
    }

    public override void Move(Vector2 direction)
    {
        if (_isLocked)
        {
            return;
        }

        base.Move(direction);
    }

    public override void Stop()
    {
        if (_isLocked)
        {
            return;
        }

        base.Stop();
    }

    private void UnlockWithAKey(Key key)
    {
        Tween.Rotation(key.transform, new Vector3(90, 0, -90), duration: 0.3f);

        Tween.Position(key.transform, blockLock.position + new Vector3(0, 2, 0), duration: 0.5f)
            .Chain(Tween.Position(key.transform, blockLock.position, duration: 0.5f))
            .Chain(Tween.Rotation(key.transform, new Vector3(90, 0, 0), duration: 0.25f))
            .OnComplete(() =>
            {
                remainingKeyToUnlock--;

                remainingKeyToUnlockText.text = $"{remainingKeyToUnlock}";

                key.gameObject.SetActive(false);

                if (remainingKeyToUnlock == 0)
                {
                    _isLocked = false;

                    blockLock.gameObject.SetActive(false);

                    breakIceFx.Play();
                }
            });
    }
}
