using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrimeTween;
using Saferio.Util.SaferioTween;
using UnityEngine;
using static GameEnum;

public class BaseBlock : MonoBehaviour
{
    private BlockServiceLocator blockServiceLocator;
    [SerializeField] private BlockProperty blockProperty;

    private List<Tween> _tweens;
    private Rigidbody _blockRigidBody;
    private MeshCollider _blockCollider;
    private Vector3 _targetPosition;
    private bool _isMovingLastFrame;
    private Vector3 _prevTargetPosition;
    private Direction _prevDirection;

    [Header("CUSTOMIZE")]
    [SerializeField] private float speedMultiplier = 15f;
    [SerializeField] private float snappingLerpRatio = 0.2f;
    [SerializeField] private LayerMask layerMaskCheckTile;

    #region PRIVATE FIELD
    private Vector3 _initialPosition;
    private Vector3 _snapPosition;
    private bool _isSnapping;
    private float _tileSize;
    private bool _isLimitMovingSpeedOnNearObstacles;
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

        blockServiceLocator = GetComponent<BlockServiceLocator>();
        _blockRigidBody = GetComponent<Rigidbody>();
        _blockCollider = GetComponent<MeshCollider>();

        _blockRigidBody.constraints |= RigidbodyConstraints.FreezePositionY;
        _blockRigidBody.isKinematic = true;

        speedMultiplier = 90;
        snappingLerpRatio = 1f / 2;

        _tileSize = GamePersistentVariable.tileSize;
        _initialPosition = transform.position;

        ScaleOnLevelStarted();

        InitInsideBlock();

        MoreLogicInAwake();
    }

    protected virtual void MoreLogicInAwake()
    {

    }

    private void OnValidate()
    {
        if (blockServiceLocator == null)
        {
            blockServiceLocator = GetComponent<BlockServiceLocator>();
        }

        blockServiceLocator.Init();

        blockServiceLocator.blockMaterialPropertyBlock.SetFaction(blockProperty.Faction);
    }

    private void OnDestroy()
    {
        BlockSelectionInput.vacumnEvent -= Vacumn;

        CommonUtil.StopAllTweens(_tweens);

        MoreLogicOnDestroy();
    }

    protected virtual void MoreLogicOnDestroy()
    {

    }

    public void TempDisableMovement()
    {
        _isLimitMovingSpeedOnNearObstacles = true;

        _tweens.Add(Tween.Delay(1f).OnComplete(() =>
        {
            _isLimitMovingSpeedOnNearObstacles = false;
        }));
    }

    private void FixedUpdate()
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

                maxDistance = 0.5f * blockServiceLocator.Size.x;

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

                    maxDistance = 0.5f * blockServiceLocator.Size.z;

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
            if (_isMovingLastFrame && (_targetPosition - _prevTargetPosition).magnitude < 0.02f)
            {
                _blockRigidBody.linearVelocity = Vector3.zero;

                return;
            }

            float tileSize = GamePersistentVariable.tileSize;

            // if (Mathf.Abs(_targetPosition.x - transform.position.x) < 0.03f * tileSize)
            // {
            //     _targetPosition = _targetPosition.ChangeX(transform.position.x);
            // }

            // if (Mathf.Abs(_targetPosition.z - transform.position.z) < 0.03f * tileSize)
            // {
            //     _targetPosition = _targetPosition.ChangeZ(transform.position.z);
            // }

            if (!_isMovingLastFrame)
            {
                _prevTargetPosition = _targetPosition;
            }

            Vector3 direction = (_targetPosition - _prevTargetPosition);

            if (!_isMovingLastFrame)
            {
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                {
                    _prevDirection = Direction.Right;
                }
                else
                {
                    _prevDirection = Direction.Up;
                }
            }
            else
            {
                if (_prevDirection == Direction.Right)
                {
                    if (Mathf.Abs(direction.z) > Mathf.Abs(direction.x))
                    {
                        _prevDirection = Direction.Up;
                    }
                }
                else
                {
                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                    {
                        _prevDirection = Direction.Right;
                    }
                }
            }

            if (_prevDirection == Direction.Right)
            {
                direction.z = 0;
            }
            else
            {
                direction.x = 0;
            }

            float maxDistance = 0.5f * tileSize;
            float maxVelocity = 45 * 0.5f * tileSize;

            Vector3 velocity = speedMultiplier * direction;

            velocity.x = Mathf.Clamp(velocity.x, -maxVelocity, maxVelocity);
            velocity.z = Mathf.Clamp(velocity.z, -maxVelocity, maxVelocity);

            _blockRigidBody.linearVelocity = velocity;

            if (!_isMovingLastFrame)
            {
                _isMovingLastFrame = true;
            }

            _prevTargetPosition = _targetPosition;
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
    public virtual void Move(Vector3 targetPosition)
    {
        if (blockProperty.IsDisintegrating)
        {
            return;
        }

        if (_isSnapping)
        {
            return;
        }

        if (!blockProperty.IsMoving)
        {
            blockServiceLocator.blockMaterialPropertyBlock.ShowOutline(true);

            _blockRigidBody.isKinematic = false;

            movePairedBlock?.Invoke(gameObject.GetInstanceID(), true);

            _isSnapping = false;

            blockProperty.IsMoving = true;
        }

        // _targetPosition = targetPosition.ChangeY(1.1f * _initialPosition.y);

        _targetPosition = targetPosition.ChangeY(_initialPosition.y);
    }

    public virtual void Stop()
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

        _isMovingLastFrame = false;
    }

    public void Snap()
    {
        float tileDistance = GamePersistentVariable.tileDistance;

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
            await Task.Delay(2);
        }

        // if (blockProperty.IsPreventDisintegrating)
        // {
        //     blockProperty.IsDisintegrating = false;

        //     return;
        // }

        blockProperty.IsReadyTriggerDisintegrateFx = true;

        disintegrateBlockEvent?.Invoke();

        // _blockCollider.enabled = false;

        CandyCoded.HapticFeedback.HapticFeedback.MediumFeedback();

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

        // KEY
        UnlockKey();

        // ELEMENT
        UseElement();
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

            if (insideBlock.GetComponent<BaseBlock>() == null)
            {
                return;
            }

            insideBlock.SetParent(transform.parent);

            insideBlock.GetComponent<MeshFilter>().mesh = blockServiceLocator.meshFilter.mesh;

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

    #region KEY
    private void UnlockKey()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Key key = transform.GetChild(i).GetComponent<Key>();

                if (key != null)
                {
                    key.transform.SetParent(transform.parent);

                    key.Unlock();
                }
            }
        }
    }
    #endregion

    #region ELEMENTS
    private void UseElement()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                BaseElement element = transform.GetChild(i).GetComponent<BaseElement>();

                if (element != null)
                {
                    element.transform.SetParent(transform.parent);

                    element.Use();
                }
            }
        }
    }
    #endregion
}
