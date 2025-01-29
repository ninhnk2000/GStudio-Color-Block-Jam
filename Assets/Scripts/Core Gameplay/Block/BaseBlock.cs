using System;
using System.Collections.Generic;
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

    [SerializeField] private float speedMultiplier;
    [SerializeField] private float snappingLerpRatio;

    #region PRIVATE FIELD
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

    public GameFaction Faction
    {
        get => blockProperty.Faction;
    }

    public static event Action disintegrateBlockEvent;
    public static event Action blockCompletedEvent;


    #region LIFE CYCLE
    private void Awake()
    {
        _tweens = new List<Tween>();

        _tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size.x;

        _blockRigidBody = GetComponent<Rigidbody>();
        _blockCollider = GetComponent<MeshCollider>();

        _blockRigidBody.isKinematic = true;

        speedMultiplier = 25;
    }

    private void OnValidate()
    {
        blockServiceLocator.Init();

        blockServiceLocator.blockMaterialPropertyBlock.SetFaction(blockProperty.Faction);
    }

    private void OnDestroy()
    {
        CommonUtil.StopAllTweens(_tweens);
    }

    private void Update()
    {
        if (_isSnapping)
        {
            transform.position = Vector3.Lerp(transform.position, _snapPosition, snappingLerpRatio);

            if (Vector3.Distance(transform.position, _snapPosition) < GameConstants.TINY_FLOAT_VALUE)
            {
                // _blockRigidBody.isKinematic = false;

                _isSnapping = false;
            }
        }

        if (blockProperty.IsMoving)
        {
            _blockRigidBody.linearVelocity = speedMultiplier * (_targetPosition - transform.position);
        }
    }
    #endregion

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

            _isSnapping = false;

            blockProperty.IsMoving = true;
        }

        _targetPosition = targetPosition.ChangeY(transform.position.y);

        // _blockRigidBody.linearVelocity = speedMultiplier * new Vector3(inputDirection.x, 0, inputDirection.y);
    }

    public void Stop()
    {
        if (blockProperty.IsDisintegrating)
        {
            return;
        }

        _blockRigidBody.isKinematic = true;

        Snap();

        blockServiceLocator.blockMaterialPropertyBlock.ShowOutline(false);

        blockProperty.IsMoving = false;
    }

    private float GetTileDistance()
    {
        return 1.05f * tilePrefab.GetComponent<MeshRenderer>().bounds.size.x;
    }

    private void Snap()
    {
        float tileDistance = GetTileDistance();

        Vector3 position = transform.position;

        Vector2 coordinator;

        // covert to bottom-right position
        Vector3 bottomRightPosition = new Vector3();

        bottomRightPosition.x = transform.position.x + (BlockProperty.NumTileX - 1) / 2f * tileDistance;
        bottomRightPosition.z = transform.position.z - (BlockProperty.NumTileZ - 1) / 2f * tileDistance;

        coordinator.x = Mathf.Round(bottomRightPosition.x / tileDistance);
        coordinator.y = Mathf.Round(bottomRightPosition.z / tileDistance);

        Vector3 finalPosition = new Vector3(0, 2, 0);

        finalPosition.x = coordinator.x * tileDistance - (BlockProperty.NumTileX - 1) / 2f * tileDistance;
        finalPosition.z = coordinator.y * tileDistance + (BlockProperty.NumTileZ - 1) / 2f * tileDistance;

        _snapPosition = finalPosition;

        _blockRigidBody.isKinematic = true;

        _isSnapping = true;
    }

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

        await Task.Delay(200);

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
    }

    public void StopDisintegrating()
    {
        CommonUtil.StopAllTweens(_tweens);

        blockServiceLocator.blockMaterialPropertyBlock.StopDisintegrating();

        blockProperty.IsDisintegrating = false;
    }

    public void Break()
    {
        Tween.ShakeScale(transform, 1.5f * Vector3.one, duration: 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);

            blockProperty.IsDone = true;

            blockCompletedEvent?.Invoke();
        });
    }

    public void InvokeBlockCompletedEvent()
    {
        blockCompletedEvent?.Invoke();
    }
}
