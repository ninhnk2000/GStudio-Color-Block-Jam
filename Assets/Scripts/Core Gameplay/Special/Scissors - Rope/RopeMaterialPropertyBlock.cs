using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class RopeMaterialPropertyBlock : MonoBehaviour
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

    public void SetFaction(GameFaction faction)
    {
        if (_propertyBlock == null)
        {
            Init();
        }

        _propertyBlock.SetColor("_Color", FactionUtility.GetColorForFaction(faction));

        _renderer.SetPropertyBlock(_propertyBlock);
    }

    public void Cut()
    {
        if (_propertyBlock == null)
        {
            Init();
        }

        Tween.Custom(0, 3, duration: 0.3f, onValueChange: newVal =>
        {
            _propertyBlock.SetFloat("_Bound", newVal);

            _renderer.SetPropertyBlock(_propertyBlock);
        });
    }
}
