using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrimeTween;
using Saferio.Util.SaferioTween;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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
    private int _countdownPreviewSnapping;
    private List<BoardTileMaterialPropertyBlock> _prevPreviewSnappingTiles;
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
    public static event Action disableHighlightTilesEvent;





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
    private Vector3[] _boundSize;
    #endregion

    #region SNAP PREVIEW
    private Transform _snapPreviewSprite;
    private Vector3 _snapPreviewPosition;
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

        speedMultiplier = 15f;
        snappingLerpRatio = 1f / 2;

        _destinations = new Queue<Vector3>();

        if (transform.parent.GetComponent<BaseBlock>() == null)
        {
            transform.position = transform.position.ChangeY(1.3f);
            transform.localScale = 1.95f * Vector3.one;
        }

        _tileSize = GamePersistentVariable.tileSize;
        _initialPosition = transform.position;

        _boxColliders = GetComponents<BoxCollider>();

        _boxColliderSize = new Vector3[_boxColliders.Length];
        _boundSize = new Vector3[_boxColliders.Length];

        // Physics Material
        PhysicsMaterial physicsMaterial = new PhysicsMaterial();

        physicsMaterial.staticFriction = 0;
        physicsMaterial.dynamicFriction = 0;
        physicsMaterial.frictionCombine = PhysicsMaterialCombine.Minimum;
        physicsMaterial.bounceCombine = PhysicsMaterialCombine.Minimum;

        for (int i = 0; i < _boxColliders.Length; i++)
        {
            _boxColliders[i].size = _boxColliders[i].size.ChangeZ(1.3f * _boxColliders[i].size.z);
            _boxColliderSize[i] = _boxColliders[i].size;
            _boundSize[i] = _boxColliders[i].bounds.size;

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

                CheckDisintegration();

                _isSnapping = false;
            }
        }

        if (blockProperty.IsMoving)
        {
            _snapPreviewSprite.position = Vector3.Lerp(_snapPreviewSprite.position, _snapPreviewPosition, 1 / 3f);
        }
        else
        {
            if (_snapPreviewSprite != null)
            {
                _snapPreviewSprite.gameObject.SetActive(false);
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


            Vector3 expectedDestination = Vector3.zero;

            // expectedDestination.x = _startMovingPosition.x + (Input.mousePosition.x - _startMovingMousePosition.x) * 0.03f;
            // expectedDestination.y = transform.position.y;
            // expectedDestination.z = _startMovingPosition.z + (Input.mousePosition.y - _startMovingMousePosition.y) * 0.03f;



            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out RaycastHit hit, 999, LayerMask.GetMask("Background"));

                if (hit.collider != null)
                {
                    expectedDestination = hit.point.ChangeY(transform.position.y);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

            if (!_isMovingLastFrame)
            {
                _isMovingLastFrame = true;
            }
            else
            {
                if (Vector3.Distance(expectedDestination, _prevPosition) > 0.13f)
                {
                    Vector3 expectedMoveDirection = expectedDestination - transform.position;
                    _moveDirection = expectedMoveDirection;

                    float maxVelocity = 75;

                    Vector3 velocity = speedMultiplier * _moveDirection;

                    velocity.x = Mathf.Clamp(velocity.x, -maxVelocity, maxVelocity);
                    velocity.z = Mathf.Clamp(velocity.z, -maxVelocity, maxVelocity);

                    _blockRigidBody.linearVelocity = velocity;
                }
                else
                {
                    _blockRigidBody.linearVelocity = Vector2.zero;
                }

                if (Vector3.Distance(expectedDestination, transform.position) < 1f)
                {
                    Vector3 expectedMoveDirection = expectedDestination - transform.position;

                    _blockRigidBody.linearVelocity = expectedMoveDirection;
                }

                if (Vector3.Distance(expectedDestination, transform.position) < 0.1f)
                {
                    _blockRigidBody.linearVelocity = Vector2.zero;
                }
            }

            // PreviewSnapping();

            _prevTargetPosition = _targetPosition;
            _prevDirection = _moveDirection;
            _prevPosition = expectedDestination;
            // _prevPosition = transform.position;
        }
    }
    #endregion

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

        CreateSnapPreviewSprite();
    }
    #endregion 

    #region PRVIEW SNAPPING
    private void CreateSnapPreviewSprite()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(GameConstants.SNAP_PREVIEW_SPRITE);

        handle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                _snapPreviewSprite = Instantiate(op.Result, transform).transform;

                _snapPreviewSprite.localScale = new Vector3(2 * blockProperty.NumTileX, 2 * blockProperty.NumTileZ, 1);
                _snapPreviewSprite.eulerAngles = new Vector3(90, 0, 0);

                PreviewSnapSpriteMaterialPropertyBlock materialPropertyBlock = _snapPreviewSprite.GetComponent<PreviewSnapSpriteMaterialPropertyBlock>();

                materialPropertyBlock.SetTiling(new Vector2(2 * blockProperty.NumTileX, 2 * blockProperty.NumTileZ));

                _snapPreviewSprite.gameObject.SetActive(false);
            }
        };
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
                _boxColliders[i].size = 0.99f * _boxColliderSize[i];
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

        blockProperty.IsMoving = false;

        _isMovingLastFrame = false;

        _destinations.Clear();
    }

    private void PreviewSnapping()
    {
        if (_countdownPreviewSnapping != 0)
        {
            _countdownPreviewSnapping--;

            return;
        }
        else
        {
            _countdownPreviewSnapping = 8;
        }

        // List<BoardTileMaterialPropertyBlock> disableHighlightTiles = _prevPreviewSnappingTiles;
        // List<BoardTileMaterialPropertyBlock> duplicateHighlightTiles = new List<BoardTileMaterialPropertyBlock>();

        // _prevPreviewSnappingTiles = new List<BoardTileMaterialPropertyBlock>();

        // for (int i = 0; i < _boxColliders.Length; i++)
        // {
        //     Vector3 halfExtent = 0.5f * new Vector3(_boundSize[i].x, 0.2f, _boundSize[i].z);

        //     RaycastHit[] hits = Physics.BoxCastAll(
        //         transform.position + _boxColliders[i].center, halfExtent, Vector3.down, Quaternion.identity, 10);

        //     for (int j = 0; j < hits.Length; j++)
        //     {
        //         BoardTileMaterialPropertyBlock boardTileMaterialPropertyBlock = hits[j].collider.GetComponent<BoardTileMaterialPropertyBlock>();

        //         if (boardTileMaterialPropertyBlock != null)
        //         {
        //             // boardTileMaterialPropertyBlock.Highlight(true);

        //             _prevPreviewSnappingTiles.Add(boardTileMaterialPropertyBlock);
        //         }
        //     }
        // }

        // Dictionary<BoardTileMaterialPropertyBlock, Vector3> someList = new Dictionary<BoardTileMaterialPropertyBlock, Vector3>();

        // List<BoardTileMaterialPropertyBlock> finalList = new List<BoardTileMaterialPropertyBlock>();

        // for (int i = 0; i < _prevPreviewSnappingTiles.Count; i++)
        // {
        //     if (!someList.ContainsKey(_prevPreviewSnappingTiles[i]))
        //     {
        //         someList.Add(_prevPreviewSnappingTiles[i], _prevPreviewSnappingTiles[i].transform.position - transform.position);
        //     }
        // }



        float tileDistance = GamePersistentVariable.tileDistance;

        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5, layerMaskCheckTile);

        if (hit.collider != null)
        {
            _snapPreviewPosition = hit.collider.transform.position;

            if (_snapPreviewPosition.x > transform.position.x)
            {
                _snapPreviewPosition.x -= (BlockProperty.NumTileX - 1) / 2f * tileDistance;
            }
            else
            {
                _snapPreviewPosition.x += (BlockProperty.NumTileX - 1) / 2f * tileDistance;
            }

            if (_snapPreviewPosition.z > transform.position.z)
            {
                _snapPreviewPosition.z -= (BlockProperty.NumTileZ - 1) / 2f * tileDistance;
            }
            else
            {
                _snapPreviewPosition.z += (BlockProperty.NumTileZ - 1) / 2f * tileDistance;
            }

            if (BlockProperty.NumTileX % 2 == 1)
            {
                _snapPreviewPosition.x = hit.collider.transform.position.x;
            }

            if (BlockProperty.NumTileZ % 2 == 1)
            {
                _snapPreviewPosition.z = hit.collider.transform.position.z;
            }

            _snapPreviewPosition.y = transform.position.y - 0.1f;

            if (!_snapPreviewSprite.gameObject.activeSelf)
            {
                _snapPreviewSprite.gameObject.SetActive(true);
            }
        }


        // if (blockProperty.NumTileZ > blockProperty.NumTileX)
        // {
        //     List<BoardTileMaterialPropertyBlock> orderedListHorizontal = someList
        //         .OrderBy(item => Mathf.Abs(item.Value.z))
        //         .OrderBy(item => Mathf.Abs(item.Value.x)).Select(item => item.Key).ToList();

        //     for (int i = 0; i < orderedListHorizontal.Count; i++)
        //     {
        //         if (i / blockProperty.NumTileZ <= blockProperty.NumTileX - 1)
        //         {
        //             finalList.Add(orderedListHorizontal[i]);
        //         }
        //         else
        //         {
        //             _prevPreviewSnappingTiles.Remove(orderedListHorizontal[i]);
        //         }
        //     }
        // }
        // else if (blockProperty.NumTileZ < blockProperty.NumTileX)
        // {
        //     List<BoardTileMaterialPropertyBlock> orderedListVertical = someList
        //         .OrderBy(item => Mathf.Abs(item.Value.x))
        //         .OrderBy(item => Mathf.Abs(item.Value.z)).Select(item => item.Key).ToList();

        //     for (int i = 0; i < orderedListVertical.Count; i++)
        //     {
        //         if (i / blockProperty.NumTileX <= blockProperty.NumTileZ - 1)
        //         {
        //             finalList.Add(orderedListVertical[i]);
        //         }
        //         else
        //         {
        //             _prevPreviewSnappingTiles.Remove(orderedListVertical[i]);
        //         }
        //     }
        // }
        // else
        // {

        // }

        // for (int i = 0; i < finalList.Count; i++)
        // {
        //     finalList[i].Highlight(true);

        //     if (disableHighlightTiles == null)
        //     {
        //         continue;
        //     }

        //     for (int k = 0; k < disableHighlightTiles.Count; k++)
        //     {
        //         if (disableHighlightTiles[k].GetInstanceID() == finalList[i].GetInstanceID())
        //         {
        //             duplicateHighlightTiles.Add(finalList[i]);
        //         }
        //     }
        // }

        // if (disableHighlightTiles == null)
        // {
        //     return;
        // }

        // for (int i = 0; i < duplicateHighlightTiles.Count; i++)
        // {
        //     disableHighlightTiles.Remove(duplicateHighlightTiles[i]);
        // }

        // for (int i = 0; i < disableHighlightTiles.Count; i++)
        // {
        //     disableHighlightTiles[i].Highlight(false);
        // }
    }

    private void DisablePreviewSnapping()
    {
        if (_prevPreviewSnappingTiles == null)
        {
            return;
        }

        for (int i = 0; i < _prevPreviewSnappingTiles.Count; i++)
        {
            _prevPreviewSnappingTiles[i].Highlight(false);
        }
    }

    public void Snap()
    {
        float tileDistance = GamePersistentVariable.tileDistance;

        DisablePreviewSnapping();

        blockServiceLocator.blockMaterialPropertyBlock.ShowOutline(false);

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

        blockServiceLocator.blockMaterialPropertyBlock.DisableOutline();

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
