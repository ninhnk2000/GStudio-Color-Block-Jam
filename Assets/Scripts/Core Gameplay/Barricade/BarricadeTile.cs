using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BarricadeTile : MonoBehaviour
{
    [SerializeField] private BarricadeServiceLocator barricadeServiceLocator;

    [SerializeField] private ParticleSystem disintegrationFx;

    public ParticleSystem DisintegrationFx
    {
        get => disintegrationFx;
    }

    private bool _isReadyToPlayFx;

    public Direction Direction
    {
        get => barricadeServiceLocator.barricade.Direction;
    }

    public GameFaction Faction
    {
        get => barricadeServiceLocator.barricadeFaction.Faction;
    }

    private void PlayDisintegrationFx()
    {
        disintegrationFx.Play();

        Tween.Delay(barricadeServiceLocator.barricade.DisintegrationDuration).OnComplete(() =>
        {
            disintegrationFx.Stop();

            _isReadyToPlayFx = false;
        });
    }

    private void OnCollisionEnter(Collision other)
    {
        BaseBlock block = other.gameObject.GetComponent<BaseBlock>();

        if (block != null)
        {
            if (block.Faction == barricadeServiceLocator.barricadeFaction.Faction &&
                block.BlockProperty.IsReadyTriggerDisintegrateFx
            )
            {
                PlayDisintegrationFx();

                // if (!_isReadyToPlayFx)
                // {
                //     _isReadyToPlayFx = true;
                // }
            }
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     disintegrationFx.Play();

    //     Tween.Delay(barricade.DisintegrationDuration).OnComplete(() =>
    //     {
    //         disintegrationFx.Stop();
    //     });
    // }
}
