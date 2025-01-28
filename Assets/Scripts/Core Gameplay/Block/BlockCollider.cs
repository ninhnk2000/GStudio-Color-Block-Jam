using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    [SerializeField] private BlockServiceLocator blockServiceLocator;

    private List<Tween> _tweens;

    private void Awake()
    {
        _tweens = new List<Tween>();
    }

    private void OnDestroy()
    {
        CommonUtil.StopAllTweens(_tweens);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (blockServiceLocator.block.BlockProperty.IsDisintegrating)
        {
            return;
        }

        BarricadeTile barricadeTile = other.gameObject.GetComponent<BarricadeTile>();

        if (barricadeTile != null)
        {
            if (barricadeTile.Faction == blockServiceLocator.block.BlockProperty.Faction)
            {
                blockServiceLocator.block.Disintegrate(barricadeTile.Direction);
            }
            else
            {
                blockServiceLocator.block.BlockProperty.IsPreventDisintegrating = true;

                _tweens.Add(Tween.Delay(0.5f).OnComplete(() =>
                {
                    blockServiceLocator.block.BlockProperty.IsPreventDisintegrating = false;
                }));
            }
        }
    }
}
