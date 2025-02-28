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

    protected List<Tween> _tweens;
    private Rigidbody _blockRigidBody;
    private MeshCollider _blockCollider;
    private Vector3 _targetPosition;
    private bool _isMovingLastFrame;
    private Vector3 _prevTargetPosition;
    private Vector3 _prevDirection;

    [Header("CUSTOMIZE")]
    [SerializeField] private float speedMultiplier;
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
    private Vector3 _lastEnqueuedDestination;
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





    #region TEST MOVE
    private Vector3 _startMovingPosition;
    private Vector3 _startMovingMousePosition;
    private Direction _prevDirectional;
    private Vector3 _safePos;
    private bool _isMoveToSafePos;
    private bool _isInCountdownBounceBack;
    private Queue<Vector3> _destinations;
    private Vector3 _currentDestination;
    private Vector2 _inputDirection;
    private BoxCollider[] _boxColliders;
    private Vector3[] _boxColliderSize;
    #endregion

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

        speedMultiplier = 50f;
        snappingLerpRatio = 1f / 2;

        _tileSize = GamePersistentVariable.tileSize;
        _initialPosition = transform.position;


        _destinations = new Queue<Vector3>();

        if (transform.parent.GetComponent<BaseBlock>() == null)
        {
            transform.localScale = 1.95f * Vector3.one;
        }

        _boxColliders = GetComponents<BoxCollider>();

        _boxColliderSize = new Vector3[_boxColliders.Length];

        PhysicsMaterial physicsMaterial = new PhysicsMaterial();

        physicsMaterial.staticFriction = 0;
        physicsMaterial.dynamicFriction = 0;

        for (int i = 0; i < _boxColliders.Length; i++)
        {
            _boxColliderSize[i] = _boxColliders[i].size;

            _boxColliders[i].material = physicsMaterial;
        }


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
            CheckDisintegration();
        }

        if (_isSnapping)
        {
            transform.position = Vector3.Lerp(transform.position, _snapPosition, snappingLerpRatio);

            if (Vector3.Distance(transform.position, _snapPosition) < GameConstants.TINY_FLOAT_VALUE)
            {
                Vector3 direction;
                float maxDistance;

                // if (transform.position.x > 0)
                // {
                //     direction = Vector3.right;
                // }
                // else
                // {
                //     direction = -Vector3.right;
                // }

                // maxDistance = 0.3f * blockServiceLocator.Size.x;

                // bool IsDisintegrate = blockServiceLocator.blockCollider.CheckDisintegration(direction, maxDistance);

                // if (!IsDisintegrate)
                // {
                //     if (transform.position.z > 0)
                //     {
                //         direction = Vector3.forward;
                //     }
                //     else
                //     {
                //         direction = -Vector3.forward;
                //     }

                //     maxDistance = 0.3f * blockServiceLocator.Size.z;

                //     blockServiceLocator.blockCollider.CheckDisintegration(direction, maxDistance);
                // }

                CheckDisintegration();

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

        if (blockProperty.IsMoving && !_isMoveToSafePos)
        {
            // if (_isMovingLastFrame && (_targetPosition - transform.position).magnitude < 1f)
            // {
            //     _blockRigidBody.linearVelocity = Vector3.zero;

            //     _prevTargetPosition = _targetPosition;

            //     return;
            // }

            // if (Mathf.Abs(_moveDirection.x) > Mathf.Abs(_moveDirection.z))
            // {
            //     _moveDirection.z = 0;
            // }
            // else
            // {
            //     _moveDirection.x = 0;
            // }


            Vector3 expectedDestination;

            expectedDestination.x = _startMovingPosition.x + (Input.mousePosition.x - _startMovingMousePosition.x) * 0.03f;
            expectedDestination.y = transform.position.y;
            expectedDestination.z = _startMovingPosition.z + (Input.mousePosition.y - _startMovingMousePosition.y) * 0.03f;


            Vector3 lastMoveDirection = _moveDirection;

            Vector3 expectedMoveDirection = expectedDestination - transform.position;
            _moveDirection = expectedMoveDirection;

            float maxVelocity = 85;

            // if (!_isMovingLastFrame)
            // {
            //     if (Mathf.Abs(_moveDirection.x) > Mathf.Abs(_moveDirection.z))
            //     {
            //         _prevDirectional = Direction.Right;
            //     }
            //     else
            //     {
            //         _prevDirectional = Direction.Up;
            //     }
            // }
            // else
            // {
            //     if (modulusX < 0.3f && modulusZ < 0.3f)
            //     {
            //         if (Mathf.Abs(_moveDirection.x) > Mathf.Abs(_moveDirection.z))
            //         {
            //             _prevDirectional = Direction.Right;
            //         }
            //         else
            //         {
            //             _prevDirectional = Direction.Up;
            //         }
            //     }
            // }

            // if (Mathf.Abs(_inputDirection.x) > Mathf.Abs(_inputDirection.y))
            // {
            //     _moveDirection.z = 0;
            // }
            // else
            // {
            //     _moveDirection.x = 0;
            // }

            // if (lastMoveDirection.x > 0 && transform.position.x < _prevPosition.x + 0.1f)
            // {
            //     _moveDirection.x = 0;

            // }
            // if (lastMoveDirection.x < 0 && transform.position.x > _prevPosition.x - 0.1f)
            // {
            //     _moveDirection.x = 0;

            // }
            // if (lastMoveDirection.z < 0 && transform.position.z > _prevPosition.z - 0.1f)
            // {
            //     _moveDirection.z = 0;

            // }
            // if (lastMoveDirection.z < 0 && transform.position.z > _prevPosition.z - 0.1f)
            // {
            //     _moveDirection.z = 0;

            // }

            Vector3 velocity = speedMultiplier * _moveDirection;

            velocity.x = Mathf.Clamp(velocity.x, -maxVelocity, maxVelocity);
            velocity.z = Mathf.Clamp(velocity.z, -maxVelocity, maxVelocity);

            if (!_isMovingLastFrame)
            {
                _isMovingLastFrame = true;
            }
            else
            {

            }

            _blockRigidBody.linearVelocity = velocity;

            _prevTargetPosition = _targetPosition;
            _prevDirection = _moveDirection;
            _prevPosition = transform.position;
        }
    }
    #endregion

    private void BounceBack(Collision other)
    {
        if (blockProperty.IsMoving && !_isMoveToSafePos && !_isInCountdownBounceBack)
        {
            Vector3 bounceBackDirection = transform.position - other.transform.position;
            bounceBackDirection.y = 0;

            if (Mathf.Abs(bounceBackDirection.x) > Mathf.Abs(bounceBackDirection.z))
            {
                bounceBackDirection.z = 0;
            }
            else
            {
                bounceBackDirection.x = 0;
            }

            _safePos = transform.position + 0.25f * GamePersistentVariable.tileSize * bounceBackDirection.normalized;

            _isMoveToSafePos = true;
            _isInCountdownBounceBack = true;

            _blockRigidBody.linearVelocity = Vector3.zero;

            _tweens.Add(Tween.Position(transform, _safePos, duration: 0.3f).OnComplete(() =>
            {
                _isMoveToSafePos = false;

                _isInCountdownBounceBack = true;

                _startMovingPosition = transform.position;

                _tweens.Add(Tween.Delay(1.5f).OnComplete(() =>
                {
                    _isInCountdownBounceBack = false;
                }));
            }));
        }
    }


    // void OnCollisionStay(Collision other)
    // {
    //     if (blockProperty.IsMoving)
    //     {
    //         Vector3 bounceBackDirection = transform.position - other.transform.position;
    //         bounceBackDirection.y = 0;

    //         bounceBackDirection = bounceBackDirection.normalized;

    //         if (Mathf.Abs(bounceBackDirection.x) > Mathf.Abs(bounceBackDirection.z))
    //         {
    //             bounceBackDirection.z = 0;
    //         }
    //         else
    //         {
    //             bounceBackDirection.x = 0;
    //         }

    //         Vector3 bounceDistance = bounceBackDirection;

    //         bounceDistance.x = Mathf.Clamp(bounceDistance.x, -0.002f * GamePersistentVariable.tileSize, 0.002f * GamePersistentVariable.tileSize);
    //         bounceDistance.z = Mathf.Clamp(bounceDistance.z, -0.002f * GamePersistentVariable.tileSize, 0.002f * GamePersistentVariable.tileSize);

    //         _safePos = transform.position + bounceDistance;

    //         _isMoveToSafePos = true;
    //         // _isInCountdownBounceBack = true;

    //         _blockRigidBody.linearVelocity = Vector3.zero;

    //         transform.position = _safePos;
    //     }
    // }

    // void OnCollisionExit(Collision other)
    // {
    //     _isMoveToSafePos = false;
    // }

    private void OnCollisionEnter(Collision other)
    {
        // BounceBack(other);
    }

    private void CheckDisintegration()
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

        if (blockProperty.isCheckDisintegrationBothRightLeft)
        {
            if (!IsDisintegrate)
            {
                IsDisintegrate = blockServiceLocator.blockCollider.CheckDisintegration(-direction, maxDistance);
            }
        }

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




            _startMovingPosition = transform.position;
            _startMovingMousePosition = Input.mousePosition;

            for (int i = 0; i < _boxColliders.Length; i++)
            {
                _boxColliders[i].size = 0.99f * _boxColliders[i].size;
            }
        }

        _moveDirection = new Vector3(direction.x, 0, direction.y);



        Vector3 expectedDestination;

        expectedDestination.x = _startMovingPosition.x + (Input.mousePosition.x - _startMovingMousePosition.x) * 0.03f;
        expectedDestination.y = transform.position.y;
        expectedDestination.z = _startMovingPosition.z + (Input.mousePosition.y - _startMovingMousePosition.y) * 0.03f;

        if (Vector3.Distance(expectedDestination, _lastEnqueuedDestination) > 1f)
        {
            _destinations.Enqueue(expectedDestination);

            _lastEnqueuedDestination = expectedDestination;
        }

        _inputDirection = direction;
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

        _destinations.Clear();
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

        SoundManager.Instance.PlaySoundBreakObject();

        // KEY
        UnlockKey();
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
