using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BaseScrew : MonoBehaviour, IScrew
{
    [SerializeField] protected int screwId;

    [SerializeField] protected ScrewServiceLocator screwServiceLocator;

    [SerializeField] protected MeshFilter meshFilter;
    [SerializeField] protected HingeJoint joint;

    [Header("CUSTOMIZE")]
    [SerializeField] protected float scaleOnScrewBox;
    [SerializeField] private int screwDifficultyModifier;
    [SerializeField] protected float rotateSpeed = 0.3f;

    [Header("CUSTOMIZE - UNSCREW ANIMATION")]
    [SerializeField] private float durationMoveOut = 0.3f;
    [SerializeField] private float durationDelay = 0.1f;
    [SerializeField] private float durationMoveToScrewPort = 0.3f;
    [SerializeField] private float durationTightenScrewPort = 0.3f;

    #region PRIVATE FIELD
    protected List<Tween> _tweens = new List<Tween>();
    protected int _numberBlockingObjects;
    protected Vector3 _initialScale;
    protected float _initialRotateSpeed;
    protected bool _isMoving;
    protected bool _isRotating;
    protected bool _isRotatingToCorrectAngleOnScrewBox;
    protected bool _isInteractable = true;
    protected bool _isDone;
    protected bool _isOutsideWithNoScrewBox;
    protected bool _isLocked;
    protected bool _isInScrewPort;
    protected ScrewBoxSlot _screwBoxSlot;
    protected ScrewData _screwData;
    #endregion

    #region PROPERTY
    public int ScrewId
    {
        get => screwId;
        set => screwId = value;
    }

    public HingeJoint Joint
    {
        set => joint = value;
    }

    public ScrewBoxSlot ScrewBoxSlot
    {
        get => _screwBoxSlot;
    }

    public ScrewData ScrewData
    {
        get => _screwData;
    }

    public ScrewServiceLocator ScrewServiceLocator
    {
        get => screwServiceLocator;
    }

    public GameFaction Faction
    {
        get => screwServiceLocator.screwFaction.Faction;
        set => screwServiceLocator.screwFaction.Faction = value;
    }

    public int NumberBlockingObjects
    {
        get => _numberBlockingObjects;
    }

    public bool IsMoving
    {
        get => _isMoving;
        set => _isMoving = value;
    }

    public bool IsRotating
    {
        get => _isRotating;
        set => _isRotating = value;
    }

    public bool IsDone
    {
        get => _isDone;
        set => _isDone = value;
    }

    public bool IsInteractable
    {
        get => _isInteractable;
        set => _isInteractable = value;
    }

    public bool IsOutsideWithNoScrewBox
    {
        get => _isOutsideWithNoScrewBox;
        set => _isOutsideWithNoScrewBox = value;
    }

    public bool IsLocked
    {
        get => _isLocked;
        set => _isLocked = value;
    }

    public bool IsInScrewPort
    {
        get => _isInScrewPort;
        set => _isInScrewPort = value;
    }

    public int ScrewDifficultyModifier
    {
        get => screwDifficultyModifier;
    }
    #endregion

    #region EVENT
    public static event Action<int, GameFaction> selectScrewEvent;
    public static event Action<BaseScrew> addScrewToListEvent;
    public static event Action<BaseScrew> screwMovedCompletedEvent;
    public static event Action<int> breakJointEvent;
    public static event Action<int> screwStartLoosenedEvent;
    public static event Action screwLoosenedEvent;
    #endregion

    #region LIFE CYCLE
    protected virtual void Awake()
    {
        ScrewBoxManager.looseScrewEvent += Loose;
        ScrewBoxManager.looseScrewWithoutCheckingObstaclesEvent += LooseWithoutCheckingObstacles;

        RegisterMoreEvent();

        AddScrewToList();

        _initialScale = transform.localScale;
        _initialRotateSpeed = rotateSpeed;

        MoreLogicInAwake();
    }

    void OnDestroy()
    {
        ScrewBoxManager.looseScrewEvent -= Loose;
        ScrewBoxManager.looseScrewWithoutCheckingObstaclesEvent -= LooseWithoutCheckingObstacles;

        UnregisterMoreEvent();

        CommonUtil.StopAllTweens(_tweens);
    }

    protected virtual void MoreLogicInAwake()
    {

    }

    protected virtual void RegisterMoreEvent()
    {

    }

    protected virtual void UnregisterMoreEvent()
    {

    }
    #endregion

    public void Select()
    {
        selectScrewEvent?.Invoke(screwId, Faction);
    }

    public virtual void Loose(int screwId, GameFaction faction, ScrewBoxSlot screwBoxSlot)
    {

    }

    public virtual void Loose(int screwId, GameFaction faction, ScrewBoxSlot screwBoxSlot, bool isCheckObstacles)
    {

    }

    public virtual void LooseWithoutCheckingObstacles(int screwId, GameFaction faction, ScrewBoxSlot screwBoxSlot)
    {
        Loose(screwId, faction, screwBoxSlot, isCheckObstacles: false);
    }

    public async Task UnscrewAnimationAsync(ScrewBoxSlot screwBoxSlot, Action onCompleteAction = null, bool isSwitchCamera = true)
    {
        if (_isMoving)
        {
            return;
        }

        screwStartLoosenedEvent?.Invoke(gameObject.GetInstanceID());

        _isMoving = true;
        _isRotating = true;

        _screwBoxSlot = screwBoxSlot;

        Vector3 screwSize = TransformUtil.ComponentWiseMultiply(transform.lossyScale, meshFilter.mesh.bounds.size);

        // // FOR ROTATING
        // await Task.Delay(100);

        _tweens.Add(Tween.Position(transform, transform.position + screwSize.z * transform.forward, duration: durationMoveOut, ease: Ease.Linear).OnComplete(() =>
        {
            // _isRotating = false;

            transform.SetParent(screwBoxSlot.transform);

            if (isSwitchCamera)
            {
                gameObject.layer = LayerMask.NameToLayer("UI");

                foreach (Transform child in transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("UI");
                }

                GameplayCameraManager.ConvertCameraSpaceSmoothly(transform);
            }

            _isRotating = false;

            float timeMultiplier = Vector3.Distance(new Vector3(0, 0.3f, -0.3f), transform.localPosition);
            timeMultiplier = Mathf.Min(timeMultiplier, 8);

            _tweens.Add(Tween.LocalRotation(transform, Quaternion.Euler(new Vector3(-30f, 180 + 0f, 0)), duration: timeMultiplier * durationMoveToScrewPort, ease: Ease.Linear));
            _tweens.Add(Tween.Scale(transform, scaleOnScrewBox * Vector3.one, duration: timeMultiplier * durationMoveToScrewPort, ease: Ease.Linear));
            _tweens.Add(Tween.LocalPosition(transform, new Vector3(0, 0.4f, -1.6f), duration: timeMultiplier * durationMoveToScrewPort, ease: Ease.Linear)
            .OnComplete(() =>
            {
                // _tweens.Add(Tween.LocalRotation(transform, Quaternion.Euler(new Vector3(-30f, 180 + 0f, 0)), duration: durationDelay, ease: Ease.Linear));
                _tweens.Add(Tween.Delay(duration: durationDelay)
                .OnComplete(() =>
                {
                    _isRotating = true;
                    rotateSpeed *= 2;

                    SoundManager.Instance.PlaySoundTightenScrew();

                    _tweens.Add(Tween.Delay(0.9f * durationTightenScrewPort)
                    .OnComplete(() =>
                    {
                        _isRotating = false;
                        rotateSpeed = _initialRotateSpeed;
                        _isRotatingToCorrectAngleOnScrewBox = true;
                    }));
                    // _tweens.Add(Tween.LocalRotation(transform, Quaternion.Euler(new Vector3(-30f, 180 + 0f, 0)), duration: durationTightenScrewPort, ease: Ease.Linear));
                    _tweens.Add(Tween.LocalPosition(transform, new Vector3(0, -0.1f, 0.2f), duration: durationTightenScrewPort, ease: Ease.Linear)
                        .OnComplete(() =>
                        {
                            _isMoving = false;

                            screwMovedCompletedEvent?.Invoke(this);
                        })
                    );

                    screwBoxSlot.CompleteFill();

                    _isDone = true;
                    _isInScrewPort = screwBoxSlot.IsScrewPort;

                    InvokeScrewLoosenedEvent();

                    onCompleteAction?.Invoke();
                }));
            }));
        }));
    }

    private async void AddScrewToList()
    {
        await Task.Delay(200);

        addScrewToListEvent?.Invoke(this);
    }

    public bool IsValidToLoose()
    {
        CountBlockingObjects();

        if (_numberBlockingObjects > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public virtual void ForceUnscrew()
    {
        if (joint != null)
        {
            Destroy(joint);
            // joint.breakForce = 0;

            breakJointEvent?.Invoke(joint.gameObject.GetInstanceID());
        }

        ThrowOutside();

        _isDone = false;
        _isOutsideWithNoScrewBox = true;
    }

    public virtual void ThrowOutside()
    {
        _isRotating = true;

        Tween.Position(transform, transform.position + 3f * transform.forward, duration: 0.3f)
        .OnComplete(() =>
        {
            Vector3 destination = transform.position;

            if (transform.position.x > 0)
            {
                destination.x = 10;
            }
            else
            {
                destination.x = -10;
            }

            destination.y = 0;

            Tween.Position(transform, destination, duration: 0.3f).OnComplete(() =>
            {
                InvokeScrewLoosenedEvent();

                screwServiceLocator.screwMeshRenderer.enabled = false;

                _isRotating = false;
            });
        });
    }

    public virtual int CountBlockingObjects(bool isIncludeHiddenScrew)
    {
        return 0;
    }

    public virtual int CountBlockingObjects()
    {
        return CountBlockingObjects(isIncludeHiddenScrew: false);
    }

    protected void InvokeScrewLoosenedEvent()
    {
        screwLoosenedEvent?.Invoke();
    }

    protected void InvokeBreakJointEvent()
    {
        breakJointEvent?.Invoke(joint.gameObject.GetInstanceID());
    }
}
