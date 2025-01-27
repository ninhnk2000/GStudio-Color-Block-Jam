using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BarricadeFaction : MonoBehaviour
{
    [SerializeField] private GameFaction faction;

    #region PRIVATE FIELD
    [SerializeField] private List<Tween> _tweens;
    [SerializeField] private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    #endregion

    public GameFaction Faction
    {
        get => faction;
    }

    private void Awake()
    {
        _tweens = new List<Tween>();

        Init();
    }

    private void OnValidate()
    {
        SetFaction();
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

    public void SetFaction()
    {
        Init();

        _propertyBlock.SetColor("_Color", FactionUtility.GetColorForFaction(faction));

        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
