using System;
using PrimeTween;
using UnityEngine;

public class BoosterHammer : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitFx;

    [SerializeField] private BoosterHammerMaterialPropertyBlock boosterHammerMaterialPropertyBlock;

    private Transform _initialParent;

    void Awake()
    {
        _initialParent = transform.parent;
    }

    public void HitTarget(Transform target, Action onCompletedAction)
    {
        transform.SetParent(target);

        gameObject.SetActive(true);

        transform.localPosition = new Vector3(20, 0, -2);
        transform.localScale *= 6 / transform.lossyScale.x;
        transform.localRotation = Quaternion.Euler(new Vector3(-90, 90, 0));

        Tween.LocalPositionX(transform, 1.5f, duration: 0.5f)
        .Chain(

            Tween.LocalRotation(transform, new Vector3(-90, 30, 0), duration: 0.3f)
            .OnComplete(() =>
            {
                Tween.LocalRotation(transform, new Vector3(-90, 120, 0), duration: 0.1f)
                .OnComplete(() =>
                {
                    transform.SetParent(_initialParent);

                    PlayHitFx();

                    Tween.ShakeCamera(Camera.main, 1, duration: 0.1f).OnComplete(() =>
                    {
                        Dissolve();
                    });

                    onCompletedAction?.Invoke();
                });
            })
        );
    }

    public void PlayHitFx()
    {
        hitFx.gameObject.SetActive(true);

        hitFx.Play();
    }

    public void Dissolve()
    {
        boosterHammerMaterialPropertyBlock.Dissolve();
    }
}
