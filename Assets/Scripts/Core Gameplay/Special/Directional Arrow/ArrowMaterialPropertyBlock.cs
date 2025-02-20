using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class ArrowMaterialPropertyBlock : MonoBehaviour
{
    #region PRIVATE FIELD
    private List<Tween> _tweens;
    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    #endregion

    private void Awake()
    {
        LevelLoader.updateBoundEvent += SetMaskingBound;

        _tweens = new List<Tween>();

        Init();
    }

    void OnDestroy()
    {
        LevelLoader.updateBoundEvent -= SetMaskingBound;

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

    public void SetMaskingBound(float rightBound, float leftBound, float topBound, float bottomBound)
    {
        _propertyBlock.SetFloat("_BoundRight", rightBound);
        _propertyBlock.SetFloat("_BoundLeft", leftBound);
        _propertyBlock.SetFloat("_BoundTop", topBound);
        _propertyBlock.SetFloat("_BoundBottom", bottomBound);

        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
