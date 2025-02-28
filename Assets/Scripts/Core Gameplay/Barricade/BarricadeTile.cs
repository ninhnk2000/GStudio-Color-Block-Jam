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

    public BarricadeServiceLocator BarricadeServiceLocator
    {
        get => barricadeServiceLocator;
    }

    public Direction Direction
    {
        get => barricadeServiceLocator.barricade.Direction;
    }

    public GameFaction Faction
    {
        get => barricadeServiceLocator.BarricadeProperty.Faction;
    }

    void Awake()
    {
        GetComponent<BoxCollider>().size *= 0.9f;
    }

    private void PlayDisintegrationFx()
    {
        disintegrationFx.Play();

        Tween.Delay(0.5f * GameGeneralConfiguration.DISINTEGRATION_TIME).OnComplete(() =>
        {
            disintegrationFx.Stop();
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        BaseBlock block = other.GetComponent<BaseBlock>();

        if (block != null)
        {
            if (block.Faction == barricadeServiceLocator.BarricadeProperty.Faction &&
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

    // private void OnCollisionEnter(Collision other)
    // {
    //     BaseBlock block = other.gameObject.GetComponent<BaseBlock>();

    //     if (block != null) 
    //     {
    //         if (block.Faction == barricadeServiceLocator.barricadeFaction.Faction &&
    //             block.BlockProperty.IsReadyTriggerDisintegrateFx
    //         )
    //         {
    //             PlayDisintegrationFx();

    //             // if (!_isReadyToPlayFx)
    //             // {
    //             //     _isReadyToPlayFx = true;
    //             // }
    //         }
    //     }
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     disintegrationFx.Play();

    //     Tween.Delay(barricade.DisintegrationDuration).OnComplete(() =>
    //     {
    //         disintegrationFx.Stop();
    //     });
    // }
}
