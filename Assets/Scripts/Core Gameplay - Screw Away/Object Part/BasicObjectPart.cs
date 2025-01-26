using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class BasicObjectPart : MonoBehaviour, IObjectPart
{
    [SerializeField] private Transform levelCenter;
    [SerializeField] private Rigidbody partRigidbody;
    [SerializeField] private MeshCollider meshCollider;

    [SerializeField] private BaseScrew[] attachedScrews;

    [Header("CUSTOMIZE")]
    [SerializeField] private Vector3 throwForceMultiplier = new Vector3(0.003f, 0.003f, 0.003f);
    [SerializeField] private int minJointToDisableKinematic = 1;

    #region PRIVATE FIELD
    private bool _isSelecting;
    private bool _isFree;
    private bool _isImmuneSwipeForce;
    private int _totalJoint;
    private bool _isConcave;
    #endregion

    public bool IsConcave
    {
        get => _isConcave;
    }

    #region EVENT
    public static event Action<int> selectObjectPartEvent;
    public static event Action<int> deselectObjectPartEvent;
    public static event Action<BaseScrew> loosenScrewOnObjectBrokenEvent;
    public static event Action shakeCameraEvent;
    public static event Action<int> dissolveObjectPartEvent;
    #endregion

    #region LIFECYCLE
    void Awake()
    {
        ScrewSelectionInput.mouseUpEvent += Deselect;
        BaseScrew.breakJointEvent += OnJointBreakCallback;

        partRigidbody = GetComponent<Rigidbody>();
        meshCollider = GetComponent<MeshCollider>();

        _totalJoint = GetComponents<HingeJoint>().Length;

        throwForceMultiplier = new Vector3(1200f, 800f, 1200f);

        attachedScrews = new BaseScrew[transform.childCount];

        for (int i = 0; i < attachedScrews.Length; i++)
        {
            attachedScrews[i] = transform.GetChild(i).GetComponent<BaseScrew>();
        }

        // if (_totalJoint > 1)
        // {
        //     partRigidbody.isKinematic = true;
        // }

        partRigidbody.isKinematic = true;

        if (!meshCollider.convex)
        {
            _isConcave = true;
        }
        else
        {
            meshCollider.isTrigger = true;
        }

        partRigidbody.mass = 100;

        minJointToDisableKinematic = 0;

        HingeJoint[] hingeJoints = GetComponents<HingeJoint>();

        for (int i = 0; i < hingeJoints.Length; i++)
        {
            hingeJoints[i].breakForce = 0;
        }
    }

    void OnDestroy()
    {
        ScrewSelectionInput.mouseUpEvent -= Deselect;
        BaseScrew.breakJointEvent -= OnJointBreakCallback;
    }
    #endregion

    public void Select()
    {
        selectObjectPartEvent?.Invoke(gameObject.GetInstanceID());

        _isSelecting = true;

        for (int i = 0; i < attachedScrews.Length; i++)
        {
            if (!attachedScrews[i].IsDone)
            {
                attachedScrews[i].ScrewServiceLocator.screwOutline.ShowOutline();
            }
        }
    }

    private void Deselect()
    {
        if (_isSelecting)
        {
            deselectObjectPartEvent?.Invoke(gameObject.GetInstanceID());

            _isSelecting = false;

            for (int i = 0; i < attachedScrews.Length; i++)
            {
                if (!attachedScrews[i].IsDone)
                {
                    attachedScrews[i].ScrewServiceLocator.screwOutline.HideOutline();
                }
            }
        }
    }

    private async void OnJointBreakCallback(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            await Task.Delay(500);

            _totalJoint--;

            if (partRigidbody.isKinematic)
            {
                if (_totalJoint <= 1 && _totalJoint <= minJointToDisableKinematic)
                {
                    if (meshCollider != null)
                    {
                        if (!meshCollider.convex)
                        {
                            meshCollider.convex = true;
                            meshCollider.isTrigger = true;
                        }
                    }

                    partRigidbody.isKinematic = false;
                }
            }

            if (_totalJoint == 0)
            {
                _isFree = true;

                Throw();

                // dissolveObjectPartEvent?.Invoke(gameObject.GetInstanceID());
            }
        }
    }

    public async void Break(Vector3 touchPosition)
    {
        BoosterHammer hammer = ObjectPoolingEverything.GetFromPool<BoosterHammer>(GameConstants.HAMMER);

        Vector3 position1 = transform.position + 5 * transform.forward;
        Vector3 position2 = transform.position - 5 * transform.forward;

        float expectedYDirection;
        float expectedZDirection;

        Vector3 expectedPosition;



        // hammer.transform.position = transform.position + 5 * expectedZDirection * transform.forward + 0 * transform.up;
        // hammer.transform.rotation = Quaternion.LookRotation(transform.forward);
        // hammer.transform.Rotate(new Vector3(0, 90, 0));

        if (Vector3.Distance(position1, Vector3.zero) < Vector3.Distance(position2, Vector3.zero))
        {
            // expectedPosition = position2 - 0.8f * transform.up;

            expectedZDirection = -1;
        }
        else
        {
            // expectedPosition = position1 - 0.8f * transform.up;

            expectedZDirection = 1;
        }

        // hammer.transform.position = transform.position + 5 * expectedZDirection * transform.forward + 0 * transform.up;

        if (Vector3.Distance(hammer.transform.position + hammer.transform.up, transform.position) < Vector3.Distance(hammer.transform.position - hammer.transform.up, transform.position))
        {
            expectedYDirection = 1;
        }
        else
        {
            expectedYDirection = -1;
        }

        hammer.transform.rotation = Quaternion.Euler(12, 90, 0);
        hammer.transform.position = touchPosition + 4 * hammer.transform.right;

        // Tween.Position(hammer.transform, transform.position + 2f * expectedZDirection * transform.forward + 0 * expectedYDirection * hammer.transform.up, duration: 0.2f)

        // Tween.Position(hammer.transform, touchPosition, duration: 0.2f)
        // .Chain(Tween.Rotation(hammer.transform, hammer.transform.rotation.eulerAngles + new Vector3(0, 0, expectedZDirection * 60), duration: 0.6f))
        // .Chain(Tween.Rotation(hammer.transform, hammer.transform.rotation.eulerAngles + new Vector3(0, 0, -expectedZDirection * 30), duration: 0.2f)
        // .OnComplete(() =>
        // {
        //     Tween.Position(hammer.transform, 50f * expectedZDirection * transform.forward, startDelay: 0.3f, duration: 0.3f);
        // }));

        Tween.Position(hammer.transform, touchPosition + 1f * hammer.transform.right, duration: 0.2f)
        .Chain(Tween.Rotation(hammer.transform, Quaternion.Euler(0, 78, -60), duration: 0.6f))
        .Chain(Tween.Rotation(hammer.transform, Quaternion.Euler(0, 78, 45), duration: 0.2f)
        .OnComplete(() =>
        {
            hammer.Dissolve();
        }));

        await Task.Delay(950);

        SoundManager.Instance.PlaySoundBreakObject();

        // hammer.PlayHitFx();

        shakeCameraEvent?.Invoke();

        // BE CAREFUL OF USING childCount
        List<BaseScrew> screws = new List<BaseScrew>();

        for (int i = 0; i < transform.childCount; i++)
        {
            BaseScrew screw = transform.GetChild(i).GetComponent<BaseScrew>();

            screws.Add(screw);
        }

        for (int i = 0; i < screws.Count; i++)
        {
            BaseScrew screw = screws[i];

            loosenScrewOnObjectBrokenEvent?.Invoke(screw);

            await Task.Delay(33);
        }

        Throw(forceBoost: 2);
    }

    private void Throw(float forceBoost = 1)
    {
        Vector3 direction;

        if (levelCenter != null)
        {
            direction = transform.position - levelCenter.position;
        }
        else
        {
            direction = transform.position - transform.parent.position;
        }

        direction = direction.normalized;

        if (direction.x > 0)
        {
            direction.x = 1;
        }
        else
        {
            direction.x = -1;
        }

        if (direction.y > 0)
        {
            direction.y = 1;
        }
        else
        {
            direction.y = -1;
        }

        if (direction.z > 0)
        {
            direction.z = 1;
        }
        else
        {
            direction.z = -1;
        }

        // // direction.y = Mathf.Abs(direction.y);

        // if (Mathf.Abs(direction.x) < 0.01f)
        // {
        //     direction.x = 1f;
        // }
        // if (Mathf.Abs(direction.y) < 0.01f)
        // {
        //     direction.y = 1f;
        // }
        // if (Mathf.Abs(direction.z) < 0.01f)
        // {
        //     direction.z = 1f;
        // }

        partRigidbody.AddForce(TransformUtil.ComponentWiseMultiply(forceBoost * throwForceMultiplier, direction), ForceMode.Impulse);

        transform.SetParent(LevelGarbageBin.GarbageBin);
    }
}
