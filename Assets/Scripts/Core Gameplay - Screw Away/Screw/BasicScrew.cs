using System;
using System.Threading.Tasks;
using CandyCoded.HapticFeedback;
using PrimeTween;
using UnityEngine;

public class BasicScrew : BaseScrew
{
    // private Vector3 hitPoint;
    // private GameObject obstacle;

    private Vector3 _obstacleHitPointWorld;

    [SerializeField] private LayerMask checkObstacleLayer;

    private void Update()
    {
        if (_isRotating)
        {
            transform.RotateAround(transform.forward, rotateSpeed);
        }

        if (_isRotatingToCorrectAngleOnScrewBox)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(new Vector3(-30, 180, 0)), 1f / 3);

            if (transform.localRotation == Quaternion.Euler(new Vector3(-30, 180, 0)))
            {
                transform.localRotation = Quaternion.Euler(new Vector3(-30, 180, 0));

                _isRotatingToCorrectAngleOnScrewBox = false;
            }
        }
    }

    // private void OnDrawGizmos()
    // {
    //     // Vector3 pointOne = transform.position + transform.lossyScale.z * 0.75f * meshFilter.mesh.bounds.size.z * transform.forward;
    //     // Vector3 pointTwo = pointOne + 10f * transform.forward;

    //     // Gizmos.DrawLine(pointOne, pointTwo);

    //     if (obstacle != null)
    //     {
    //         // Gizmos.DrawWireSphere(transform.position + 0.75f * meshFilter.mesh.bounds.size.z * transform.forward, transform.lossyScale.x * 0.5f * meshFilter.mesh.bounds.size.x);
    //         Gizmos.DrawWireSphere(_obstacleHitPointWorld, 0.1f);
    //     }
    // }

    public override async void Loose(int screwId, GameEnum.GameFaction faction, ScrewBoxSlot screwBoxSlot)
    {
        Loose(screwId, faction, screwBoxSlot, isCheckObstacles: true);
    }

    public override async void Loose(int screwId, GameEnum.GameFaction faction, ScrewBoxSlot screwBoxSlot, bool isCheckObstacles)
    {
        if (screwId == this.screwId)
        {
            if (!_isInteractable)
            {
                return;
            }
            else
            {
                _isInteractable = false;
            }

            if (_isDone)
            {
                return;
            }

            if (isCheckObstacles)
            {
                CountBlockingObjects();
            }
            else
            {
                _numberBlockingObjects = 0;
            }

            // BLOCKED
            if (_numberBlockingObjects > 0)
            {
                FakeScrew fakeScrew = ObjectPoolingEverything.GetFromPool<FakeScrew>(GameConstants.FAKE_SCREW);

                fakeScrew.CloneFromScrew(this);

                fakeScrew.transform.SetParent(transform);

                GetComponent<MeshRenderer>().enabled = false;

                fakeScrew.IsRotating = true;

                SoundManager.Instance.PlaySoundLoosenScrewFail();

                HapticFeedback.MediumFeedback();

                Vector3 hitPointFakeScrewSpace =
                    fakeScrew.transform.InverseTransformPoint(_obstacleHitPointWorld - transform.lossyScale.z * 1f * meshFilter.mesh.bounds.size.z * transform.forward);

                _tweens.Add(Tween.LocalPosition(fakeScrew.transform, 1.3f * hitPointFakeScrewSpace.z * (fakeScrew.transform.localRotation * Vector3.forward),
                    cycles: 2, cycleMode: CycleMode.Yoyo, duration: 0.5f)
                .OnComplete(() =>
                {
                    fakeScrew.IsRotating = false;

                    ObjectPoolingEverything.ReturnToPool(GameConstants.FAKE_SCREW, fakeScrew.gameObject);
                    // fakeScrew.gameObject.SetActive(false);

                    GetComponent<MeshRenderer>().enabled = true;

                    _isInteractable = true;
                }));

                return;
            }

            if (joint != null)
            {
                Destroy(joint);
                // joint.breakForce = 0;

                InvokeBreakJointEvent();
            }

            screwBoxSlot.Fill(this);

            UnscrewAnimationAsync(screwBoxSlot);

            SoundManager.Instance.PlaySoundLoosenScrew();

            HapticFeedback.MediumFeedback();
        }
    }

    public int CountBlockingObjectsPrefabMode(bool isIncludeHiddenScrew)
    {
        const int MAX_OBSTACLE_ARRAY_SIZE = 10;

        Vector3 start = transform.position + transform.forward;

        Collider[] obstacles = new Collider[MAX_OBSTACLE_ARRAY_SIZE];

        // int numObstacle = gameObject.scene.GetPhysicsScene().OverlapCapsule(start, start + 10 * transform.forward, 0.2f, obstacles);

        RaycastHit hit;

        gameObject.scene.GetPhysicsScene().CapsuleCast(start, start + 0.1f * transform.forward, 0.2f, transform.forward, out hit);

        int number = 0;

        if (hit.collider != null)
        {
            if (hit.transform == transform.parent)
            {
                return 0;
            }

            if (hit.transform.GetComponent<IObjectPart>() != null)
            {
                float distance = Vector3.Distance(hit.point, transform.position);

                if (distance < 0.9f)
                {
                    number++;
                }
            }
        }

        // for (int i = 0; i < numObstacle; i++)
        // {

        // }

        return number;
    }

    public override int CountBlockingObjects(bool isIncludeHiddenScrew)
    {
        float screwRadius = transform.lossyScale.x * 0.45f * meshFilter.mesh.bounds.size.x;

        Vector3 pointOne = transform.position + transform.lossyScale.z * 1f * meshFilter.mesh.bounds.size.z * transform.forward;
        Vector3 pointTwo = pointOne + 10f * transform.forward;

        Collider[] hits = Physics.OverlapCapsule(pointOne, pointTwo, screwRadius, checkObstacleLayer);

        int number = 0;

        if (hits != null)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform == transform.parent)
                {
                    continue;
                }

                IObjectPart objectPart = hits[i].transform.GetComponent<IObjectPart>();

                if (objectPart != null)
                {
                    if (isIncludeHiddenScrew)
                    {
                        number++;

                        continue;
                    }

                    Vector3 hitPos = hits[i].ClosestPoint(pointOne);

                    // Concave
                    if (objectPart.IsConcave)
                    {
                        RaycastHit hit;

                        float radius = transform.lossyScale.x * 0.45f * meshFilter.mesh.bounds.size.x;

                        Physics.CapsuleCast(
                            pointOne - radius * transform.forward, pointOne - radius * transform.forward + 0.01f * transform.forward,
                            transform.lossyScale.x * 0.45f * meshFilter.mesh.bounds.size.x, transform.forward, out hit, 10, checkObstacleLayer);

                        if (hit.collider != null)
                        {
                            hitPos = hit.point;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    // Perpendicular distance
                    Vector3 positionInScrewSpace = transform.InverseTransformPoint(hitPos);

                    float distance = positionInScrewSpace.z - transform.lossyScale.z * 1f * meshFilter.mesh.bounds.size.z;

                    distance = Mathf.Max(distance, 0);

                    if (distance < transform.lossyScale.z * 0.5f * meshFilter.mesh.bounds.size.z)
                    {
                        if (Mathf.Abs(positionInScrewSpace.x) > screwRadius || Mathf.Abs(positionInScrewSpace.y) > screwRadius)
                        {
                            continue;
                        }

                        number++;

                        _obstacleHitPointWorld = hitPos;

                        // hitPoint = positionInScrewSpace;
                        // obstacle = hits[i].gameObject;
                    }
                }
            }
        }

        _numberBlockingObjects = number;

        return number;
    }

    // public override int CountBlockingObjects(bool isIncludeHiddenScrew)
    // {
    //     // Vector3 pointOne = transform.position + 0.5f * meshRenderer.bounds.size.z * transform.forward;
    //     Vector3 pointOne = transform.position;
    //     Vector3 pointTwo = pointOne + 0.01f * transform.forward;

    //     // DON'T cast too big capsule --> point 2 param is start + 0.1f * transform.forward
    //     RaycastHit[] hits = Physics.CapsuleCastAll(pointOne, pointTwo, transform.lossyScale.x * 0.5f * meshRenderer.bounds.size.x, transform.forward, 10);

    //     int number = 0;

    //     float minBlockedDistance = float.MaxValue;

    //     if (hits != null)
    //     {
    //         for (int i = 0; i < hits.Length; i++)
    //         {
    //             if (hits[i].transform == transform.parent)
    //             {
    //                 continue;
    //             }

    //             if (hits[i].transform.GetComponent<IObjectPart>() != null)
    //             {
    //                 if (isIncludeHiddenScrew)
    //                 {
    //                     number++;

    //                     continue;
    //                 }

    //                 // Perpendicular distance
    //                 Vector3 positionInScrewSpace = transform.InverseTransformPoint(hits[i].point);

    //                 if (positionInScrewSpace.z < 0)
    //                 {
    //                     continue;
    //                 }

    //                 float distance = Mathf.Abs(positionInScrewSpace.z);

    //                 // float distance = Vector3.Distance(hits[i].point, transform.position);

    //                 if (distance < minBlockedDistance)
    //                 {
    //                     minBlockedDistance = distance;

    //                     hitPoint = hits[i].point;
    //                     obstacle = hits[i].collider.gameObject;
    //                 }

    //                 if (distance < transform.lossyScale.x * 1.75f * meshRenderer.bounds.size.z)
    //                 {
    //                     number++;

    //                     hitPoint = hits[i].point;
    //                     obstacle = hits[i].collider.gameObject;
    //                 }
    //             }
    //         }
    //     }

    //     _numberBlockingObjects = number;
    //     _distanceBlocked = minBlockedDistance;

    //     return number;
    // }
}
