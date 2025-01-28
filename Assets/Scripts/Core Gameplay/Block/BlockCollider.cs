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

    public Vector3 boxCastDirection = -Vector3.forward;
    public Color gizmoColor = Color.green;

    private void OnTriggerEnter(Collider other)
    {
        // BarricadeTile barricadeTile = other.gameObject.GetComponent<BarricadeTile>();

        // if (barricadeTile != null)
        // {
        //     if (barricadeTile.Faction != blockServiceLocator.block.BlockProperty.Faction)
        //     {
        //         blockServiceLocator.block.StopDisintegrating();
        //     }
        // }

        if (blockServiceLocator.block.BlockProperty.IsDisintegrating)
        {
            return;
        }

        BaseBarricade barricade = other.gameObject.GetComponent<BaseBarricade>();

        if (barricade != null)
        {
            if (barricade.Faction == blockServiceLocator.block.BlockProperty.Faction)
            {
                Vector3 direction;

                if (barricade.Direction == Direction.Right)
                {
                    direction = Vector3.right;
                }
                else if (barricade.Direction == Direction.Left)
                {
                    direction = -Vector3.right;
                }
                else if (barricade.Direction == Direction.Up)
                {
                    direction = Vector3.forward;
                }
                else
                {
                    direction = -Vector3.forward;
                }

                RaycastHit[] hits = Physics.BoxCastAll(transform.position, 0.5f * blockServiceLocator.Size, direction, Quaternion.identity, 10);

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider != null)
                    {
                        BarricadeTile barricadeTile = hits[i].collider.GetComponent<BarricadeTile>();
                        
                        if (barricadeTile != null)
                        {
                            if (barricadeTile.Faction != blockServiceLocator.block.BlockProperty.Faction)
                            {
                                return;
                            }
                        }
                    }
                }

                blockServiceLocator.block.Disintegrate(barricade.Direction);
            }
            // else
            // {
            //     CommonUtil.StopAllTweens(_tweens);

            //     blockServiceLocator.block.BlockProperty.IsPreventDisintegrating = true;

            //     // _tweens.Add(Tween.Delay(2f).OnComplete(() =>
            //     // {
            //     //     blockServiceLocator.block.BlockProperty.IsPreventDisintegrating = false;
            //     // }));
            // }
        }
    }
}
