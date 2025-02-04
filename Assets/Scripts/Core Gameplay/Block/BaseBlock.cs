using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using PrimeTween;
using Saferio.Util.SaferioTween;
using UnityEngine;
using static GameEnum;

public class BaseBlock : MonoBehaviour
{
    [SerializeField] private BlockServiceLocator blockServiceLocator;
    [SerializeField] private BlockProperty blockProperty;
    [SerializeField] private GameObject tilePrefab;

    private List<Tween> _tweens;
    private Rigidbody _blockRigidBody;
    private MeshCollider _blockCollider;
    private Vector3 _targetPosition;

    [Header("CUSTOMIZE")]
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float snappingLerpRatio;
    [SerializeField] private LayerMask layerMaskCheckTile;

    #region PRIVATE FIELD
    private Vector3 _initialPosition;
    private Vector3 _snapPosition;
    private bool _isSnapping;
    private float _tileSize;
    #endregion

    public BlockServiceLocator BlockServiceLocator
    {
        get => blockServiceLocator;
    }

    public BlockProperty BlockProperty
    {
        get => blockProperty;
    }

    public Rigidbody BlockRigidbody
    {
        get => _blockRigidBody;
    }

    public GameFaction Faction
    {
        get => blockProperty.Faction;
    }

    public static event Action disintegrateBlockEvent;
    public static event Action blockCompletedEvent;
    public static event Action<int, bool> movePairedBlock;

    #region LIFE CYCLE
    private void Awake()
    {
        BlockSelectionInput.vacumnEvent += Vacumn;

        _tweens = new List<Tween>();

        _tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size.x;

        _blockRigidBody = GetComponent<Rigidbody>();
        _blockCollider = GetComponent<MeshCollider>();

        _blockRigidBody.isKinematic = true;

        speedMultiplier = 25;
        _initialPosition = transform.position;

        ScaleOnLevelStarted();

        InitInsideBlock();
    }

    private void OnValidate()
    {
        blockServiceLocator.Init();

        blockServiceLocator.blockMaterialPropertyBlock.SetFaction(blockProperty.Faction);
    }

    private void OnDestroy()
    {
        BlockSelectionInput.vacumnEvent -= Vacumn;

        CommonUtil.StopAllTweens(_tweens);
    }

    private void Update()
    {
        if (_isSnapping)
        {
            transform.position = Vector3.Lerp(transform.position, _snapPosition, snappingLerpRatio);

            if (Vector3.Distance(transform.position, _snapPosition) < GameConstants.TINY_FLOAT_VALUE)
            {
                Vector3 direction;
                float maxDistance;

                if (transform.position.x > 0)
                {
                    direction = Vector3.right;
                }
                else
                {
                    direction = -Vector3.right;
                }

                maxDistance = 0.8f * blockProperty.NumTileX * _tileSize;

                bool IsDisintegrate = blockServiceLocator.blockCollider.CheckDisintegration(direction, maxDistance);

                if (!IsDisintegrate)
                {
                    if (transform.position.z > 0)
                    {
                        direction = Vector3.forward;
                    }
                    else
                    {
                        direction = -Vector3.forward;
                    }

                    maxDistance = 0.8f * blockProperty.NumTileZ * _tileSize;

                    blockServiceLocator.blockCollider.CheckDisintegration(direction, maxDistance);
                }

                _isSnapping = false;
            }
        }

        if (blockProperty.IsRotating)
        {
            transform.RotateAround(Vector3.up, 6);
        }

        if (blockProperty.IsMoving)
        {
            _blockRigidBody.linearVelocity = speedMultiplier * (_targetPosition - transform.position);
        }
    }
    #endregion

    #region FOR BETTER GAME FEEL
    private void ScaleOnLevelStarted()
    {
        Vector3 _prevScale = transform.localScale;

        transform.localScale = Vector3.zero;

        _tweens.Add(Tween.Scale(transform, _prevScale, duration: 0.5f));
    }
    #endregion 

    #region MOVEMENT
    public void Move(Vector3 targetPosition)
    {
        if (blockProperty.IsDisintegrating)
        {
            return;
        }

        if (blockProperty.IsMoving)
        {
            // return;
        }
        else
        {
            blockServiceLocator.blockMaterialPropertyBlock.ShowOutline(true);

            _blockRigidBody.isKinematic = false;

            movePairedBlock?.Invoke(gameObject.GetInstanceID(), true);

            _isSnapping = false;

            blockProperty.IsMoving = true;
        }

        _targetPosition = targetPosition.ChangeY(1.2f * _initialPosition.y);
    }

    public void Stop()
    {
        if (blockProperty.IsDisintegrating)
        {
            return;
        }

        _blockRigidBody.isKinematic = true;

        movePairedBlock?.Invoke(gameObject.GetInstanceID(), false);

        Snap();

        blockServiceLocator.blockMaterialPropertyBlock.ShowOutline(false);

        blockProperty.IsMoving = false;
    }

    public void Snap()
    {
        float tileDistance = GetTileDistance();

        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5, layerMaskCheckTile);

        if (hit.collider != null)
        {
            _snapPosition = hit.collider.transform.position;

            if (_snapPosition.x > transform.position.x)
            {
                _snapPosition.x -= (BlockProperty.NumTileX - 1) / 2f * tileDistance;
            }
            else
            {
                _snapPosition.x += (BlockProperty.NumTileX - 1) / 2f * tileDistance;
            }

            if (_snapPosition.z > transform.position.z)
            {
                _snapPosition.z -= (BlockProperty.NumTileZ - 1) / 2f * tileDistance;
            }
            else
            {
                _snapPosition.z += (BlockProperty.NumTileZ - 1) / 2f * tileDistance;
            }

            if (BlockProperty.NumTileX % 2 == 1)
            {
                _snapPosition.x = hit.collider.transform.position.x;
            }

            if (BlockProperty.NumTileZ % 2 == 1)
            {
                _snapPosition.z = hit.collider.transform.position.z;
            }

            // _snapPosition.x -= (BlockProperty.NumTileX - 1) / 2f * tileDistance;
            // _snapPosition.z += (BlockProperty.NumTileZ - 1) / 2f * tileDistance;

            _snapPosition.y = _initialPosition.y;

            _blockRigidBody.isKinematic = true;

            _isSnapping = true;
        }

        // _snapPosition = finalPosition;

        // _blockRigidBody.isKinematic = true;

        // _isSnapping = true;
    }
    #endregion

    #region DISINTEGRATE
    public async Task Disintegrate(Direction direction)
    {
        if (blockProperty.IsDisintegrating)
        {
            return;
        }

        blockServiceLocator.blockMaterialPropertyBlock.ShowOutline(false);

        Snap();

        blockProperty.IsMoving = false;
        blockProperty.IsDisintegrating = true;

        while (_isSnapping)
        {
            await Task.Delay(20);
        }

        // if (blockProperty.IsPreventDisintegrating)
        // {
        //     blockProperty.IsDisintegrating = false;

        //     return;
        // }

        blockProperty.IsReadyTriggerDisintegrateFx = true;

        disintegrateBlockEvent?.Invoke();

        // _blockCollider.enabled = false;

        if (direction == Direction.Right)
        {
            _tweens.Add(Tween.PositionX(transform, transform.position.x + (blockProperty.NumTileX + 1) * _tileSize, duration: GameGeneralConfiguration.DISINTEGRATION_TIME));
        }
        if (direction == Direction.Left)
        {
            _tweens.Add(Tween.PositionX(transform, transform.position.x - (blockProperty.NumTileX + 1) * _tileSize, duration: GameGeneralConfiguration.DISINTEGRATION_TIME));
        }
        if (direction == Direction.Up)
        {
            _tweens.Add(Tween.PositionZ(transform, transform.position.z + (blockProperty.NumTileZ + 1) * _tileSize, duration: GameGeneralConfiguration.DISINTEGRATION_TIME));
        }
        else if (direction == Direction.Down)
        {
            _tweens.Add(Tween.PositionZ(transform, transform.position.z - (blockProperty.NumTileZ + 1) * _tileSize, duration: GameGeneralConfiguration.DISINTEGRATION_TIME));
        }

        blockServiceLocator.blockMaterialPropertyBlock.Disintegrate(direction);

        // DOUBLE BLOCK
        EnableInsideBlock();
    }

    public void StopDisintegrating()
    {
        CommonUtil.StopAllTweens(_tweens);

        blockServiceLocator.blockMaterialPropertyBlock.StopDisintegrating();

        blockProperty.IsDisintegrating = false;
    }
    #endregion

    #region BOOSTER
    public void Break()
    {
        Tween.ShakeScale(transform, 1.5f * Vector3.one, duration: 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);

            blockProperty.IsDone = true;

            blockCompletedEvent?.Invoke();
        });
    }

    public void Vacumn()
    {
        Tween.Scale(transform, 0, duration: 0.6f);
        Tween.LocalPositionY(transform, transform.position.y + 20, duration: 0.6f)
        .OnComplete(() =>
        {
            gameObject.SetActive(false);

            blockProperty.IsDone = true;

            blockCompletedEvent?.Invoke();
        });

        blockProperty.IsRotating = true;
    }

    public void Vacumn(GameFaction faction)
    {
        if (blockProperty.IsRotating)
        {
            return;
        }

        if (faction == Faction)
        {
            Vacumn();
        }
    }
    #endregion

    #region UTIL
    public void InvokeBlockCompletedEvent()
    {
        blockCompletedEvent?.Invoke();
    }

    private float GetTileDistance()
    {
        return 1.05f * tilePrefab.GetComponent<MeshRenderer>().bounds.size.x;
    }
    #endregion

    #region DOUBLE BLOCK
    private void InitInsideBlock()
    {
        if (transform.childCount > 0)
        {
            Transform insideBlock = transform.GetChild(0);

            Collider[] colliders = insideBlock.GetComponents<Collider>();

            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }
        }
    }

    private void EnableInsideBlock()
    {
        if (transform.childCount > 0)
        {
            Transform insideBlock = transform.GetChild(0);

            insideBlock.SetParent(transform.parent);

            Tween.Scale(insideBlock, transform.localScale, duration: 0.3f)
            .OnComplete(() =>
            {
                Collider[] colliders = insideBlock.GetComponents<Collider>();

                foreach (var collider in colliders)
                {
                    collider.enabled = true;
                }
            });
        }
    }
    #endregion
}
