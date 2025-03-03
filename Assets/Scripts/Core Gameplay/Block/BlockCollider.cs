using System.Collections.Generic;
using PrimeTween;
using Saferio.Util.SaferioTween;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    [SerializeField] private BlockServiceLocator blockServiceLocator;

    private List<Tween> _tweens;
    private Rigidbody _blockRigidbody;

    private void Awake()
    {
        _tweens = new List<Tween>();

        _blockRigidbody = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        CommonUtil.StopAllTweens(_tweens);
    }

    public Vector3 boxCastDirection = -Vector3.forward;

    private void OnTriggerEnter(Collider other)
    {
        return;

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

                if (barricade.Direction == Direction.Up)
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

    public bool CheckDisintegration(Vector3 direction, float maxDistance)
    {
        maxDistance = 0.5f * GamePersistentVariable.tileSize;

        Direction sparsedDirection = Direction.Right;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            if (direction.x > 0)
            {
                sparsedDirection = Direction.Right;
            }
            else
            {
                sparsedDirection = Direction.Left;
            }
        }
        else
        {
            if (direction.z > 0)
            {
                sparsedDirection = Direction.Up;
            }
            else
            {
                sparsedDirection = Direction.Down;
            }
        }


        Vector3 halfExtent = 0.4f * blockServiceLocator.Size;
        float extrude;

        // if (sparsedDirection == Direction.Right || sparsedDirection == Direction.Left)
        // {
        //     halfExtent.x = 0.1f;
        //     extrude = blockServiceLocator.Size.x;
        // }
        // else
        // {
        //     halfExtent.z = 0.1f;
        //     extrude = blockServiceLocator.Size.z;
        // }

        RaycastHit[] hits = Physics.BoxCastAll(transform.position,
            halfExtent, direction, Quaternion.identity, maxDistance);

        BaseBarricade matchBarricade = null;

        // int random = Random.Range(111, 9999);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider != null)
            {
                if (hits[i].collider.transform == transform)
                {
                    continue;
                }

                GateBarrier gateBarrier = hits[i].collider.GetComponent<GateBarrier>();

                if (gateBarrier != null)
                {
                    return false;
                }

                BaseBlock block = hits[i].collider.GetComponent<BaseBlock>();

                if (block != null)
                {
                    if (sparsedDirection == Direction.Up && block.transform.position.z > transform.position.z)
                    {
                        return false;
                    }
                    if (sparsedDirection == Direction.Down && block.transform.position.z < transform.position.z)
                    {
                        return false;
                    }
                    if (sparsedDirection == Direction.Right && block.transform.position.x > transform.position.x)
                    {
                        return false;
                    }
                    if (sparsedDirection == Direction.Left && block.transform.position.x < transform.position.x)
                    {
                        return false;
                    }
                }

                JumpingBarricade jumpingBarricade = hits[i].collider.GetComponent<JumpingBarricade>();

                if (jumpingBarricade != null)
                {
                    return false;
                }

                BaseBarricade gate = null;

                if (hits[i].collider.transform.parent != null)
                {
                    gate = hits[i].collider.transform.parent.GetComponent<BaseBarricade>();
                }

                if (gate != null)
                {
                    if (gate.Faction == blockServiceLocator.block.BlockProperty.Faction)
                    {
                        if (sparsedDirection == Direction.Up || sparsedDirection == Direction.Down)
                        {
                            if (transform.position.x - 0.5f * blockServiceLocator.Size.x > gate.transform.position.x - 0.63f * gate.Size.x
                            && transform.position.x + 0.5f * blockServiceLocator.Size.x < gate.transform.position.x + 0.63f * gate.Size.x
                            )
                            {
                                matchBarricade = gate;
                            }
                        }

                        if (sparsedDirection == Direction.Right || sparsedDirection == Direction.Left)
                        {
                            if (transform.position.z - 0.5f * blockServiceLocator.Size.z > gate.transform.position.z - 0.63f * gate.Size.z
                            && transform.position.z + 0.5f * blockServiceLocator.Size.z < gate.transform.position.z + 0.63f * gate.Size.z
                            )
                            {
                                matchBarricade = gate;
                            }
                        }
                    }
                }

                BarricadeTile barricadeTile = hits[i].collider.GetComponent<BarricadeTile>();

                if (barricadeTile != null)
                {
                    if (barricadeTile.Faction != blockServiceLocator.block.BlockProperty.Faction)
                    {
                        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                        {
                            if (direction.x > 0)
                            {
                                // Debug.Log("SAFERIO " + barricadeTile.Faction + "/" + direction + "/" + random);
                                // Debug.Log("SAFERIO " + barricadeTile.transform.position + "/" + transform.position + "/" + random);

                                if (barricadeTile.transform.position.x > transform.position.x)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (barricadeTile.transform.position.x < transform.position.x)
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if (direction.z > 0)
                            {
                                if (barricadeTile.transform.position.z > transform.position.z)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (barricadeTile.transform.position.z < transform.position.z)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        // matchBarricade = barricadeTile.BarricadeServiceLocator.barricade;

                        // // invalid
                        // if (barricadeTile.Direction == Direction.Right || barricadeTile.Direction == Direction.Left)
                        // {
                        //     if (Mathf.Abs(direction.z) > Mathf.Abs(direction.x))
                        //     {
                        //         matchBarricade = null;
                        //     }
                        // }
                        // if (barricadeTile.Direction == Direction.Up || barricadeTile.Direction == Direction.Down)
                        // {
                        //     if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                        //     {
                        //         matchBarricade = null;
                        //     }
                        // }
                    }
                }
            }
        }

        if (matchBarricade != null)
        {
            blockServiceLocator.block.Disintegrate(matchBarricade.Direction);

            return true;
        }
        else
        {
            return false;
        }
    }
}
