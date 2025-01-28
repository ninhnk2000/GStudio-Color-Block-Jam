using System;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BaseBlock : MonoBehaviour
{
    [SerializeField] private BlockServiceLocator blockServiceLocator;
    [SerializeField] private BlockProperty blockProperty;
    [SerializeField] private GameObject tilePrefab;

    private Rigidbody _blockRigidBody;
    private MeshCollider _blockCollider;

    [SerializeField] private float speedMultiplier;
    [SerializeField] private float snappingLerpRatio;

    #region PRIVATE FIELD
    private Vector3 _snapPosition;
    private bool _isSnapping;
    private float _tileSize;
    #endregion

    public BlockProperty BlockProperty
    {
        get => blockProperty;
    }

    public GameFaction Faction
    {
        get => blockProperty.Faction;
    }

    public static event Action disintegrateBlockEvent;

    private void Awake()
    {
        _tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size.x;

        _blockRigidBody = GetComponent<Rigidbody>();
        _blockCollider = GetComponent<MeshCollider>();

        _blockRigidBody.isKinematic = true;

        speedMultiplier = 20;
    }

    private void OnValidate()
    {
        blockServiceLocator.Init();

        blockServiceLocator.blockMaterialPropertyBlock.SetFaction(blockProperty.Faction);
    }

    private void Update()
    {
        if (blockProperty.IsDisintegrating)
        {
            return;
        }

        if (_isSnapping)
        {
            transform.position = Vector3.Lerp(transform.position, _snapPosition, snappingLerpRatio);

            if (Vector3.Distance(transform.position, _snapPosition) < GameConstants.TINY_FLOAT_VALUE)
            {
                // _blockRigidBody.isKinematic = false;

                _isSnapping = false;
            }
        }

    }

    public void Move(Vector2 inputDirection)
    {
        if (blockProperty.IsMoving)
        {
            // return;
        }
        else
        {
            blockServiceLocator.blockMaterialPropertyBlock.ShowOutline(true);

            _blockRigidBody.isKinematic = false;

            blockProperty.IsMoving = true;
        }

        _blockRigidBody.linearVelocity = speedMultiplier * new Vector3(inputDirection.x, 0, inputDirection.y);
    }

    public void Stop()
    {
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
        _blockCollider.enabled = false;

        blockServiceLocator.blockMaterialPropertyBlock.ShowOutline(false);

        Snap();

        await Task.Delay(200);

        if (blockProperty.IsPreventDisintegrating)
        {
            return;
        }

        blockProperty.IsReadyTriggerDisintegrateFx = true;

        disintegrateBlockEvent?.Invoke();

        Vector3 raycastDirection = Vector3.right;

        if (direction == Direction.Right)
        {
            Tween.PositionX(transform, transform.position.x + blockProperty.NumTileX * _tileSize, duration: 1f);

            raycastDirection = Vector3.right;
        }
        if (direction == Direction.Left)
        {
            Tween.PositionX(transform, transform.position.x - blockProperty.NumTileX * _tileSize, duration: 1f);

            raycastDirection = -Vector3.right;
        }
        if (direction == Direction.Up)
        {
            Tween.PositionZ(transform, transform.position.z + blockProperty.NumTileZ * _tileSize, duration: 1f);

            raycastDirection = Vector3.forward;
        }
        else if (direction == Direction.Down)
        {
            Tween.PositionZ(transform, transform.position.z - blockProperty.NumTileZ * _tileSize, duration: 1f);

            raycastDirection = -Vector3.forward;
        }

        blockServiceLocator.blockMaterialPropertyBlock.Disintegrate(direction);

        blockServiceLocator.block.BlockProperty.IsDisintegrating = true;
    }
}
