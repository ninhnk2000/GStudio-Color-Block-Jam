using PrimeTween;
using TMPro;
using UnityEngine;

public class BlockLock : MonoBehaviour
{
    [SerializeField] private TMP_Text remainingKeyToUnlockText;
    [SerializeField] private int remainingKeyToUnlock;

    private void Awake()
    {
        Key.unlockWithKeyEvent += UnlockWithAKey;

        remainingKeyToUnlockText.text = $"{remainingKeyToUnlock}";
    }

    private void OnDestroy()
    {
        Key.unlockWithKeyEvent -= UnlockWithAKey;
    }

    private void UnlockWithAKey(Key key)
    {
        Tween.Rotation(key.transform, new Vector3(90, 0, -90), duration: 0.3f);

        Tween.Position(key.transform, transform.position + new Vector3(0, 2, 0), duration: 1f)
            .Chain(Tween.Position(key.transform, transform.position, duration: 1f))
            .Chain(Tween.Rotation(key.transform, new Vector3(90, 0, 0), duration: 0.5f))
            .OnComplete(() =>
            {
                remainingKeyToUnlock--;

                remainingKeyToUnlockText.text = $"{remainingKeyToUnlock}";

                key.gameObject.SetActive(false);
            });
    }
}
