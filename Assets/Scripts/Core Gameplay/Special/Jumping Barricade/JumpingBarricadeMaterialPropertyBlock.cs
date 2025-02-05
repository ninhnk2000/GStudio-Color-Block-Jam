using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class JumpingBarricadeMaterialPropertyBlock : MonoBehaviour
{
    #region PRIVATE FIELD
    private List<Tween> _tweens;
    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
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

    public float GetPositionXAddition()
    {
        return _renderer.material.GetFloat(GameConstants.ADDITION_POSITION_X);
    }

    public void SetPositionXAddition(float value)
    {
        _propertyBlock.SetFloat(GameConstants.ADDITION_POSITION_X, value);

        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
