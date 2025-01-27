using PrimeTween;
using UnityEngine;

public class BarricadeTile : MonoBehaviour
{
    [SerializeField] private BaseBarricade barricade;

    [SerializeField] private ParticleSystem disintegrationFx;

    public Direction Direction
    {
        get => barricade.Direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        disintegrationFx.Play();

        Tween.Delay(barricade.DisintegrationDuration).OnComplete(() =>
        {
            disintegrationFx.Stop();
        });
    }
}
