using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class PreviewSnapSpriteMaterialPropertyBlock : MonoBehaviour
{
    #region PRIVATE FIELD
    private List<Tween> _tweens;
    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    private bool _isHighlight;
    #endregion

    private void Awake()
    {
        _tweens = new List<Tween>();

        Init();
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

    public void SetTiling(Vector2 tiling)
    {
        _propertyBlock.SetVector("_Tiling", tiling);
        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
