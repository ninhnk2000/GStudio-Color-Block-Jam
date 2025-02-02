using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BlockMaterialPropertyBlock : MonoBehaviour
{
    [SerializeField] private BlockServiceLocator blockServiceLocator;
    [SerializeField] private MeshFilter meshFilter;

    [Header("OUTLINE")]
    [SerializeField] private Outline outlineComponent;

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
        LevelLoader.updateRightBoundEvent += SetMaskingRightBound;
        LevelLoader.updateTopBoundEvent += SetMaskingTopBound;

        _tweens = new List<Tween>();

        Init();

        SetFaction(blockServiceLocator.block.BlockProperty.Faction);

        outlineComponent.OutlineColor = ColorUtil.WithAlpha(outlineComponent.OutlineColor, 0);
    }

    private void Start()
    {
        SetFaction(blockServiceLocator.block.BlockProperty.Faction);
    }

    void OnDestroy()
    {
        LevelLoader.updateRightBoundEvent -= SetMaskingRightBound;
        LevelLoader.updateTopBoundEvent -= SetMaskingTopBound;

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

        blockServiceLocator = GetComponent<BlockServiceLocator>();
        outlineComponent = GetComponent<Outline>();
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

    public void SetFaction()
    {
        if (_propertyBlock == null)
        {
            Init();
        }

        _propertyBlock.SetColor("_Color", FactionUtility.GetColorForFaction(blockServiceLocator.block.BlockProperty.Faction));

        _renderer.SetPropertyBlock(_propertyBlock);
    }

    private void SetMaskingRightBound(float rightBound)
    {
        _propertyBlock.SetFloat("_BoundLeft", -rightBound);
        _propertyBlock.SetFloat("_BoundRight", rightBound);
    }

    private void SetMaskingTopBound(float topBound)
    {
        _propertyBlock.SetFloat("_BoundTop", topBound);
        _propertyBlock.SetFloat("_BoundBottom", -topBound);
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
            _propertyBlock.SetFloat("_MinPositionY", -0.5f * meshSizeWorld.y);
            _propertyBlock.SetFloat("_MaxPositionY", 0.5f * meshSizeWorld.y);
        }
        else
        {
            _propertyBlock.SetFloat("_MinPositionX", -0.5f * meshSizeWorld.x);
            _propertyBlock.SetFloat("_MaxPositionX", 0.5f * meshSizeWorld.x);
        }

        _tweens.Add(Tween.Custom(0, 1,
            startDelay: 0.1f * GameGeneralConfiguration.DISINTEGRATION_TIME, duration: 0.5f * GameGeneralConfiguration.DISINTEGRATION_TIME, onValueChange: newVal =>
            {
                _propertyBlock.SetFloat("_ClipValue", newVal);
                _renderer.SetPropertyBlock(_propertyBlock);
            })
            .OnComplete(() =>
            {
                gameObject.SetActive(false);

                blockServiceLocator.block.BlockProperty.IsDone = true;

                blockServiceLocator.block.InvokeBlockCompletedEvent();
            })
        );
    }

    public void StopDisintegrating()
    {
        CommonUtil.StopAllTweens(_tweens);
    }

    public void ShowOutline(bool isShow)
    {
        // Color outlineColor = _propertyBlock.GetColor("_OutlineColor");

        Color outlineColor = FactionUtility.GetColorForFaction(blockServiceLocator.block.BlockProperty.Faction) * 3f;

        float startValue = outlineColor.a;
        float endValue = 1;

        if (!isShow)
        {
            endValue = 0;
        }

        CommonUtil.StopAllTweens(_tweens);

        _tweens.Add(Tween.Custom(startValue, endValue, duration: 0.3f, onValueChange: newVal =>
        {
            outlineComponent.OutlineColor = ColorUtil.WithAlpha(outlineColor, newVal);

            // _propertyBlock.SetColor("_OutlineColor", ColorUtil.WithAlpha(outlineColor, newVal));
            // _renderer.SetPropertyBlock(_propertyBlock);
        })
            .OnComplete(() =>
            {
                _isInTransition = false;
            })
        );
    }
}
