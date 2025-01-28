using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BlockMaterialPropertyBlock : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;

    [Header("CUSTOMIZE")]
    [SerializeField] private string alphaValueReference;

    #region PRIVATE FIELD
    [SerializeField] private List<Tween> _tweens;
    [SerializeField] private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    private GameFaction _cachedFaction;
    private bool _isInTransition;
    #endregion

    private void Awake()
    {
        _tweens = new List<Tween>();

        Init();

        SetFaction(_cachedFaction);
    }

    void OnDestroy()
    {
        CommonUtil.StopAllTweens(_tweens);
    }

    private void Init()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }

        if (_propertyBlock == null)
        {
            _propertyBlock = new MaterialPropertyBlock();
        }
    }

    public void SetFaction(GameFaction faction)
    {
        if (_propertyBlock == null)
        {
            Init();
        }

        _propertyBlock.SetColor("_Color", FactionUtility.GetColorForFaction(faction));

        _renderer.SetPropertyBlock(_propertyBlock);

        _cachedFaction = faction;
    }

    public void Disintegrate(Direction direction)
    {
        if (direction == Direction.Up)
        {
            _propertyBlock.SetFloat("_WeightClipZ", -1);
        }
        else if (direction == Direction.Down)
        {
            _propertyBlock.SetFloat("_WeightClipZ", 1);
        }
        else if (direction == Direction.Right)
        {
            _propertyBlock.SetFloat("_WeightClipX", -1);
        }
        else if (direction == Direction.Left)
        {
            _propertyBlock.SetFloat("_WeightClipX", 1);
        }

        Vector3 meshSizeWorld = meshFilter.mesh.bounds.size;

        if (direction == Direction.Up || direction == Direction.Down)
        {
            _propertyBlock.SetFloat("_LowestPosition", -0.5f * meshSizeWorld.y);
            _propertyBlock.SetFloat("_HighestPosition", 0.5f * meshSizeWorld.y);
        }
        else
        {
            _propertyBlock.SetFloat("_LowestPosition", -0.5f * meshSizeWorld.x);
            _propertyBlock.SetFloat("_HighestPosition", 0.5f * meshSizeWorld.x);
        }

        _tweens.Add(Tween.Custom(0, 1, duration: 1f, onValueChange: newVal =>
        {
            _propertyBlock.SetFloat("_ClipValue", newVal);
            _renderer.SetPropertyBlock(_propertyBlock);
        }));
    }

    public void ShowOutline(bool isShow)
    {
        if (_isInTransition)
        {
            return;
        }
        else
        {
            _isInTransition = true;
        }

        float startValue = 1;
        float endValue = 1.1f;

        if (!isShow)
        {
            startValue = 1.1f;
            endValue = 1;
        }

        _tweens.Add(Tween.Custom(startValue, endValue, duration: 0.3f, onValueChange: newVal =>
        {
            _propertyBlock.SetFloat("_OutlineWidth", newVal);
            _renderer.SetPropertyBlock(_propertyBlock);
        })
            .OnComplete(() =>
            {
                _isInTransition = false;
            })
        );
    }
}
