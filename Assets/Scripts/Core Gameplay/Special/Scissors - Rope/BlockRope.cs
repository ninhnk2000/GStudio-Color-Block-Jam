using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BlockRope : MonoBehaviour
{
    [SerializeField] private GameFaction faction;

    [SerializeField] private RopeMaterialPropertyBlock ropeMaterialPropertyBlock;

    void Awake()
    {
        Scissors.cutBlockRopeEvent += Cut;

        ropeMaterialPropertyBlock.SetFaction(faction);
    }

    void OnDestroy()
    {
        Scissors.cutBlockRopeEvent -= Cut;
    }

    void OnValidate()
    {
        ropeMaterialPropertyBlock.SetFaction(faction);
    }

    private void Cut(Scissors scissors)
    {
        if (scissors.Faction == faction)
        {
            Tween.Rotation(scissors.transform, new Vector3(0, 0, 0), duration: 0.3f);

            Tween.Position(scissors.transform, transform.position + new Vector3(0, 6, 0), duration: 1f, ease: Ease.Default)
                .Chain(Tween.Position(scissors.transform, transform.position + new Vector3(0, 2.5f, 0), duration: 1f, ease: Ease.Default))
                .OnComplete(() =>
                {
                    scissors.CutAnimation();

                    TiedBlock tiedBlock = GetComponentInParent<TiedBlock>();

                    if (tiedBlock != null)
                    {
                        tiedBlock.CutRope();
                    }

                    Tween.Delay(0.5f).OnComplete(() =>
                    {
                        ropeMaterialPropertyBlock.Cut();

                        scissors.Hide();
                    });
                });
        }
    }
}
