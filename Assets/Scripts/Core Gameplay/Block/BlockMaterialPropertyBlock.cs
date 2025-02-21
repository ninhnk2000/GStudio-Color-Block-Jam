using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BlockMaterialPropertyBlock : MonoBehaviour
{
    [SerializeField] private BlockServiceLocator blockServiceLocator;
    [SerializeField] private MeshFilter meshFilter;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private BlockMaterialsContainer blockMaterialsContainer;

    [Header("OUTLINE")]
    [SerializeField] private Outline outlineComponent;

    [Header("CUSTOMIZE")]
    [SerializeField] private string alphaValueReference;
    private const float DEFAULT_OUTLINE_WIDTH = 2f;
    private Color DEFAULT_OUTLINE_COLOR = ColorUtil.WithAlpha(0.05f * Color.white, 1);

    #region PRIVATE FIELD
    [SerializeField] private List<Tween> _tweens;
    [SerializeField] private List<Tween> _outlineTweens;
    [SerializeField] private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    private GameFaction _cachedFaction;
    private bool _isInTransition;
    #endregion

    private void Awake()
    {
        LevelLoader.updateBoundEvent += SetMaskingBound;

        _tweens = new List<Tween>();
        _outlineTweens = new List<Tween>();

        Init();

        SetFaction(blockServiceLocator.block.BlockProperty.Faction);

        SetDefaultOutline();

        SetTextureRotation();
    }

    private void Start()
    {
        SetFaction(blockServiceLocator.block.BlockProperty.Faction);
    }

    void OnDestroy()
    {
        LevelLoader.updateBoundEvent -= SetMaskingBound;

        CommonUtil.StopAllTweens(_tweens);
        CommonUtil.StopAllTweens(_outlineTweens);
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
        // if (_propertyBlock == null)
        // {
        //     Init();
        // }

        // _propertyBlock.SetColor("_Color", FactionUtility.GetColorForFaction(faction));

        // _renderer.SetPropertyBlock(_propertyBlock);

        _renderer.sharedMaterial = blockMaterialsContainer.BlockMaterials[(int)faction];

        _cachedFaction = faction;
    }

    public void SetFaction()
    {
        // if (_propertyBlock == null)
        // {
        //     Init();
        // }

        // _propertyBlock.SetColor("_Color", FactionUtility.GetColorForFaction(blockServiceLocator.block.BlockProperty.Faction));

        // _renderer.SetPropertyBlock(_propertyBlock);

        _renderer.sharedMaterial = blockMaterialsContainer.BlockMaterials[(int)blockServiceLocator.block.BlockProperty.Faction];
    }

    public void SetTextureRotation()
    {
        _propertyBlock.SetFloat("_Rotation", -transform.eulerAngles.y);

        _renderer.SetPropertyBlock(_propertyBlock);
    }

    private void SetMaskingBound(float rightBound, float leftBound, float topBound, float bottomBound)
    {
        _propertyBlock.SetFloat("_BoundRight", rightBound);
        _propertyBlock.SetFloat("_BoundLeft", leftBound);
        _propertyBlock.SetFloat("_BoundTop", topBound);
        _propertyBlock.SetFloat("_BoundBottom", bottomBound);
    }

    public void SetMaskingRight(float value)
    {
        _propertyBlock.SetFloat("_BoundRight", value);
    }

    public void SetMaskingLeft(float value)
    {
        _propertyBlock.SetFloat("_BoundLeft", value);
    }

    public void SetMaskingTop(float value)
    {
        _propertyBlock.SetFloat("_BoundTop", value);
    }

    public void SetMaskingBottom(float value)
    {
        _propertyBlock.SetFloat("_BoundBottom", value);
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

        HideOutlineCompletely();
    }

    public void StopDisintegrating()
    {
        CommonUtil.StopAllTweens(_tweens);
    }

    private void SetDefaultOutline()
    {
        outlineComponent.OutlineColor = DEFAULT_OUTLINE_COLOR;
        outlineComponent.OutlineWidth = DEFAULT_OUTLINE_WIDTH;
    }

    public void ShowOutline(bool isShow)
    {
        // Color outlineColor = _propertyBlock.GetColor("_OutlineColor");

        Color outlineColor = FactionUtility.GetColorForFaction(blockServiceLocator.block.BlockProperty.Faction) * 3f;

        Color startValue = DEFAULT_OUTLINE_COLOR;
        Color endValue = Color.white;

        if (!isShow)
        {
            endValue = DEFAULT_OUTLINE_COLOR;
        }

        CommonUtil.StopAllTweens(_outlineTweens);

        _outlineTweens.Add(Tween.Custom(startValue, endValue, duration: 0.3f, onValueChange: newVal =>
        {
            outlineComponent.OutlineColor = newVal;

            // _propertyBlock.SetColor("_OutlineColor", ColorUtil.WithAlpha(outlineColor, newVal));
            // _renderer.SetPropertyBlock(_propertyBlock);
        })
            .OnComplete(() =>
            {
                _isInTransition = false;
            })
        );

        float startWidthValue = DEFAULT_OUTLINE_WIDTH;
        float endWidthValue = 4;

        if (!isShow)
        {
            endWidthValue = DEFAULT_OUTLINE_WIDTH;
        }

        _outlineTweens.Add(Tween.Custom(startWidthValue, endWidthValue, duration: 0.3f, onValueChange: newVal =>
        {
            outlineComponent.OutlineWidth = newVal;
        }));
    }

    public void HideOutlineCompletely()
    {
        CommonUtil.StopAllTweens(_outlineTweens);
        
        // outlineComponent.OutlineColor = ColorUtil.WithAlpha(0.1f * Color.white, 0);
        outlineComponent.OutlineWidth = 0;
        
        // _tweens.Add(Tween.Custom(outlineComponent.OutlineColor, ColorUtil.WithAlpha(0.1f * Color.white, 0), duration: 0.1f, onValueChange: newVal =>
        // {
        //     outlineComponent.OutlineColor = newVal;
        // }));
    }
}
