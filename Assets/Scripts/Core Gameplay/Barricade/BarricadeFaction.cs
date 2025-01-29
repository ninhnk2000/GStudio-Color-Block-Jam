using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BarricadeFaction : MonoBehaviour
{
    // [SerializeField] private GameFaction faction;
    [SerializeField] private BarricadeServiceLocator barricadeServiceLocator;

    #region PRIVATE FIELD
    [SerializeField] private List<Tween> _tweens;
    [SerializeField] private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    #endregion

    // public GameFaction Faction
    // {
    //     get => faction;
    // }

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

        for (int i = 0; i < transform.childCount; i++)
        {
            BarricadeTile barricadeTile = transform.GetChild(i).GetComponent<BarricadeTile>();

            if (barricadeTile == null)
            {
                continue;
            }

            barricadeTile.DisintegrationFx.GetComponent<ParticleSystemRenderer>().SetPropertyBlock(_propertyBlock);
        }

        barricadeServiceLocator.BarricadeProperty.Faction = faction;
        Debug.Log(barricadeServiceLocator.BarricadeProperty.Faction);
    }

    public void SetFaction()
    {
        if (_propertyBlock == null)
        {
            Init();
        }

        _propertyBlock.SetColor("_Color", FactionUtility.GetColorForFaction(barricadeServiceLocator.BarricadeProperty.Faction));

        _renderer.SetPropertyBlock(_propertyBlock);

        for (int i = 0; i < transform.childCount; i++)
        {
            BarricadeTile barricadeTile = transform.GetChild(i).GetComponent<BarricadeTile>();

            if (barricadeTile == null)
            {
                return;
            }

            barricadeTile.DisintegrationFx.GetComponent<ParticleSystemRenderer>().SetPropertyBlock(_propertyBlock);
        }
    }
}
