using System.Collections.Generic;
using PrimeTween;
using Saferio.Util.SaferioTween;
using Unity.VisualScripting;
using UnityEngine;
using static GameEnum;

public class OneFactionTile : BoardTile
{
    [Header("ELEVATED TILE")]
    [SerializeField] private SpriteRenderer factionOverlay;
    [SerializeField] private Rigidbody tileRigidBody;
    [SerializeField] private GameFaction faction;

    [SerializeField] private float velocityMultiplier;

    private List<Tween> _tweens;
    private float _initialPositionY;
    private bool _isBounceBack;

    void Awake()
    {
        _tweens = new List<Tween>();

        _initialPositionY = transform.position.y;
    }

    void Update()
    {
        if (_isBounceBack)
        {
            tileRigidBody.linearVelocity = velocityMultiplier * Vector3.up;

            if (transform.position.y > _initialPositionY)
            {
                transform.position = transform.position.ChangeY(_initialPositionY);

                tileRigidBody.isKinematic = true;

                _isBounceBack = false;
            }
        }
    }

    void OnValidate()
    {
        factionOverlay.color = FactionUtility.GetColorForFaction(faction);
    }

    void OnDestroy()
    {
        CommonUtil.StopAllTweens(_tweens);
    }

    void OnCollisionEnter(Collision other)
    {
        BaseBlock baseBlock = other.gameObject.GetComponent<BaseBlock>();

        if (baseBlock != null)
        {
            if (baseBlock.BlockProperty.Faction == faction)
            {
                if (!_isBounceBack)
                {
                    CommonUtil.StopAllTweens(_tweens);

                    tileRigidBody.isKinematic = false;

                    _tweens.Add(
                        Tween.LocalPositionY(transform, _initialPositionY - 3, duration: 0.5f)
                        .OnComplete(() =>
                        {
                            _tweens.Add(Tween.Delay(0.5f).OnComplete(() =>
                            {
                                _isBounceBack = true;
                            }));
                        })
                    );
                }
                else
                {

                }
            }
        }
    }

    // void OnCollisionExit(Collision other)
    // {
    //     CommonUtil.StopAllTweens(_tweens);

    //     _tweens.Add(Tween.LocalPositionY(transform, _initialPositionY, duration: 0.3f));
    // }
}
