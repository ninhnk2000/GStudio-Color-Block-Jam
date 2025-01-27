using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BarricadeTile : MonoBehaviour
{
    [SerializeField] private BarricadeServiceLocator barricadeServiceLocator;

    [SerializeField] private ParticleSystem disintegrationFx;

    private bool _isReadyToPlayFx;

    public Direction Direction
    {
        get => barricadeServiceLocator.barricade.Direction;
    }

    public GameFaction Faction
    {
        get => barricadeServiceLocator.barricadeFaction.Faction;
    }

    private void Awake()
    {
        BaseBlock.disintegrateBlockEvent += PlayDisintegrationFx;
    }

    private void OnDestroy()
    {
        BaseBlock.disintegrateBlockEvent -= PlayDisintegrationFx;
    }

    private void PlayDisintegrationFx()
    {
        if (_isReadyToPlayFx)
        {
            disintegrationFx.Play();

            Tween.Delay(barricadeServiceLocator.barricade.DisintegrationDuration).OnComplete(() =>
            {
                disintegrationFx.Stop();

                _isReadyToPlayFx = false;
            });
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        BaseBlock block = other.gameObject.GetComponent<BaseBlock>();

        if (block != null)
        {
            if (!_isReadyToPlayFx)
            {
                _isReadyToPlayFx = true;
            }
            else
            {
                if (block.Faction == barricadeServiceLocator.barricadeFaction.Faction)
                {
                    disintegrationFx.Play();

                    Tween.Delay(barricadeServiceLocator.barricade.DisintegrationDuration).OnComplete(() =>
                    {
                        disintegrationFx.Stop();

                        _isReadyToPlayFx = false;
                    });
                }
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
