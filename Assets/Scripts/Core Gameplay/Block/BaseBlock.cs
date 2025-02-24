using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrimeTween;
using Saferio.Util.SaferioTween;
using Unity.VisualScripting;
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
    private Vector3 _prevDirection;

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



    #region MOVEMENT
    private Vector3 _moveDirection;
    bool _isLockedHorizontal;
    bool _isLockedVertical;
    private float _lockHorizontalTime;
    private float _lockVericalTime;
    private int _horizontalColliderInstanceId;
    private int _verticalColliderInstanceId;
    private Vector3 _prevPosition;
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

        speedMultiplier = 45;
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

    void Update()
    {
        if (blockProperty.IsMoving && !blockProperty.IsDisintegrating)
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

            maxDistance = 0.3f * blockServiceLocator.Size.x;

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

                maxDistance = 0.3f * blockServiceLocator.Size.z;

                IsDisintegrate = blockServiceLocator.blockCollider.CheckDisintegration(direction, maxDistance);
            }
        }

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

                maxDistance = 0.3f * blockServiceLocator.Size.x;

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

                    maxDistance = 0.3f * blockServiceLocator.Size.z;

                    blockServiceLocator.blockCollider.CheckDisintegration(direction, maxDistance);
                }

                _isSnapping = false;
            }
        }

        if (blockProperty.IsRotating)
        {
            transform.RotateAround(Vector3.up, 6);
        }
    }

    private void FixedUpdate()
    {
        if (blockProperty.IsDisintegrating)
        {
            return;
        }

        if (blockProperty.IsMoving)
        {
            if (_isMovingLastFrame && (_targetPosition - transform.position).magnitude < 1f)
            {
                _blockRigidBody.linearVelocity = Vector3.zero;

                return;
            }

            float tileSize = GamePersistentVariable.tileSize;

            if (!_isMovingLastFrame)
            {
                _prevTargetPosition = _targetPosition;
            }

            // Vector3 direction = _targetPosition - transform.position;
            Vector3 direction = _targetPosition - _prevTargetPosition;

            // RaycastHit obstacleHorizontal;
            // RaycastHit obstacleVertical;

            // Vector3 size = blockServiceLocator.Size;

            // int layerMask = 1 << LayerMask.NameToLayer("Default");

            // Physics.BoxCast(transform.position, 0.5f * new Vector3(0.1f * size.x, size.y, size.z),
            //     new Vector3(direction.x, 0, 0).normalized, out obstacleHorizontal, Quaternion.identity, 0.95f * size.x, layerMask);
            // Physics.BoxCast(transform.position, 0.5f * new Vector3(size.x, size.y, 0.1f * size.z),
            //     new Vector3(0, 0, direction.z).normalized, out obstacleVertical, Quaternion.identity, 0.95f * size.z, layerMask);



            // if (obstacleHorizontal.collider != null)
            // {
            //     float distanceX = Mathf.Abs(transform.position.x - obstacleHorizontal.point.x);
            //     float distanceZ = Mathf.Abs(transform.position.z - obstacleHorizontal.point.z);

            //     if (distanceX < 0.5f * size.x)
            //     {
            //         direction.x = 0;
            //     }
            // }
            // if (obstacleVertical.collider != null)
            // {
            //     float distanceX = Mathf.Abs(transform.position.x - obstacleVertical.point.x);
            //     float distanceZ = Mathf.Abs(transform.position.z - obstacleVertical.point.z);

            //     if (distanceZ < 0.5f * size.z)
            //     {
            //         direction.z = 0;
            //     }
            // }

            // Debug.Log("SAFERIO: " + obstacleHorizontal.collider + " / " + obstacleVertical.collider + " / " + direction);

            if (_isMovingLastFrame)
            {
                if (!_isLockedHorizontal && !_isLockedVertical)
                {
                    if (_prevDirection.x > 0 && transform.position.x < _prevPosition.x + 0.1f)
                    {
                        _isLockedHorizontal = true;
                    }
                    if (_prevDirection.x < 0 && transform.position.x > _prevPosition.x - 0.1f)
                    {
                        _isLockedHorizontal = true;
                    }

                    if (_prevDirection.z > 0 && transform.position.z < _prevPosition.z + 0.1f)
                    {
                        _isLockedVertical = true;
                    }
                    if (_prevDirection.z < 0 && transform.position.z > _prevPosition.z - 0.1f)
                    {
                        _isLockedVertical = true;
                    }
                }
            }

            if (_isLockedHorizontal)
            {
                direction.x = 0;

                _lockHorizontalTime += 1;

                if (_lockHorizontalTime > 4)
                {
                    _isLockedHorizontal = false;
                    _lockHorizontalTime = 0;
                }
            }

            if (_isLockedVertical)
            {
                direction.z = 0;

                _lockVericalTime += 1;

                if (_lockVericalTime > 4)
                {
                    _isLockedVertical = false;
                    _lockVericalTime = 0;
                }
            }

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                direction.z = 0;
            }
            else
            {
                direction.x = 0;
            }

            // Debug.Log("SAFERIO " + _isLockedHorizontal + "/" + _isLockedVertical + "/" + direction);

            Vector3 ndirection = _targetPosition - transform.position;

            if (Mathf.Abs(_moveDirection.x) > Mathf.Abs(_moveDirection.z))
            {
                _moveDirection.z = 0;
            }
            else
            {
                _moveDirection.x = 0;
            }

            float maxVelocity = speedMultiplier;

            Vector3 velocity = speedMultiplier * _moveDirection;

            velocity.x = Mathf.Clamp(velocity.x, -maxVelocity, maxVelocity);
            velocity.z = Mathf.Clamp(velocity.z, -maxVelocity, maxVelocity);

            if (!_isMovingLastFrame)
            {
                _isMovingLastFrame = true;
            }
            else
            {
                _blockRigidBody.linearVelocity = velocity;
            }

            _prevTargetPosition = _targetPosition;
            _prevDirection = _moveDirection;
            _prevPosition = transform.position;
        }
    }
    #endregion

    // private void OnCollisionEnter(Collision other)
    // {
    //     if (!_isMovingLastFrame)
    //     {
    //         return;
    //     }

    //     if (_isLockedHorizontal || _isLockedVertical)
    //     {
    //         return;
    //     }

    //     if (other.gameObject.GetComponent<BaseBlock>() == null)
    //     {
    //         return;
    //     }

    //     Debug.Log(_prevDirection);

    //     if (Mathf.Abs(_prevDirection.x) > Mathf.Abs(_prevDirection.z))
    //     {
    //         _isLockedHorizontal = true;

    //         _horizontalColliderInstanceId = other.gameObject.GetInstanceID();
    //     }
    //     else
    //     {
    //         _isLockedVertical = true;

    //         _verticalColliderInstanceId = other.gameObject.GetInstanceID();
    //     }

    //     // if (Mathf.Abs(transform.position.x - other.transform.position.x) > Mathf.Abs(transform.position.z - other.transform.position.z))
    //     // {
    //     //     _isLockedHorizontal = true;
    //     //     _lockHorizontalTime = 0;

    //     //     _horizontalColliderInstanceId = other.gameObject.GetInstanceID();
    //     // }
    //     // else
    //     // {
    //     //     _isLockedVertical = true;
    //     //     _lockVericalTime = 0;

    //     //     _verticalColliderInstanceId = other.gameObject.GetInstanceID();
    //     // }

    //     _prevPosition = transform.position;
    // }

    // void OnCollisionExit(Collision collision)
    // {
    //     if (_isLockedHorizontal)
    //     {
    //         if (collision.gameObject.GetInstanceID() == _horizontalColliderInstanceId)
    //         {
    //             _isLockedHorizontal = false;
    //         }
    //     }

    //     if (_isLockedVertical)
    //     {
    //         if (collision.gameObject.GetInstanceID() == _verticalColliderInstanceId)
    //         {
    //             _isLockedVertical = false;
    //         }
    //     }
    // }

    // On

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

    public virtual void Move(Vector2 direction)
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

        _moveDirection = new Vector3(direction.x, 0, direction.y);
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

        // blockServiceLocator.blockMaterialPropertyBlock.ShowOutline(false);
        blockServiceLocator.blockMaterialPropertyBlock.HideOutlineCompletely();

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

        SoundManager.Instance.PlaySoundBreakObject();

        if (direction == Direction.Right)
        {
            blockServiceLocator.blockMaterialPropertyBlock.SetMaskingRight(transform.position.x + 0.5f * (blockProperty.NumTileX + 1) * _tileSize);

            _tweens.Add(Tween.PositionX(transform, transform.position.x + (blockProperty.NumTileX + 1) * _tileSize, duration: GameGeneralConfiguration.DISINTEGRATION_TIME));
        }
        if (direction == Direction.Left)
        {
            blockServiceLocator.blockMaterialPropertyBlock.SetMaskingLeft(transform.position.x - 0.5f * (blockProperty.NumTileX + 1) * _tileSize);

            _tweens.Add(Tween.PositionX(transform, transform.position.x - (blockProperty.NumTileX + 1) * _tileSize, duration: GameGeneralConfiguration.DISINTEGRATION_TIME));
        }
        if (direction == Direction.Up)
        {
            blockServiceLocator.blockMaterialPropertyBlock.SetMaskingTop(transform.position.z + 0.5f * (blockProperty.NumTileZ + 1) * _tileSize);

            _tweens.Add(Tween.PositionZ(transform, transform.position.z + (blockProperty.NumTileZ + 1) * _tileSize, duration: GameGeneralConfiguration.DISINTEGRATION_TIME));
        }
        else if (direction == Direction.Down)
        {
            blockServiceLocator.blockMaterialPropertyBlock.SetMaskingBottom(transform.position.z - 0.5f * (blockProperty.NumTileZ + 1) * _tileSize);

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
        BoosterHammer hammer = ObjectPoolingEverything.GetFromPool<BoosterHammer>(GameConstants.HAMMER);

        hammer.HitTarget(transform, onCompletedAction: () =>
        {
            Tween.ShakeScale(transform, 1.5f * Vector3.one, duration: 0.3f).OnComplete(() =>
            {
                gameObject.SetActive(false);

                blockProperty.IsDone = true;

                blockCompletedEvent?.Invoke();
            });
        });
    }

    public void Vacumn(Vector3 vacumnPosition)
    {
        Tween.Scale(transform, 0, duration: 0.6f);
        Tween.Position(transform, vacumnPosition, duration: 0.6f)
        .OnComplete(() =>
        {
            gameObject.SetActive(false);

            blockProperty.IsDone = true;

            blockCompletedEvent?.Invoke();
        });

        blockProperty.IsRotating = true;
    }

    public void Vacumn(GameFaction faction, Vector3 vacumnPosition)
    {
        if (blockProperty.IsRotating)
        {
            return;
        }

        if (faction == Faction)
        {
            Vacumn(vacumnPosition);
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
