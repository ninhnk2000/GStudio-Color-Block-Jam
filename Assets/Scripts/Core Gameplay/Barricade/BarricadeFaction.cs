using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BarricadeFaction : MonoBehaviour
{
    // [SerializeField] private GameFaction faction;
    [SerializeField] private BarricadeServiceLocator barricadeServiceLocator;

    [SerializeField] private BarricadeMaterialsContainer barricadeMaterialsContainer;

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

        Color factionColor = FactionUtility.GetColorForFaction(faction);

        _propertyBlock.SetColor("_Color", factionColor);

        _renderer.SetPropertyBlock(_propertyBlock);

        for (int i = 0; i < transform.childCount; i++)
        {
            BarricadeTile barricadeTile = transform.GetChild(i).GetComponent<BarricadeTile>();

            if (barricadeTile == null)
            {
                continue;
            }

            ParticleSystem.MainModule mainModule = barricadeTile.DisintegrationFx.main;

            mainModule.startColor = new ParticleSystem.MinMaxGradient(factionColor, ColorUtil.WithAlpha(factionColor * 0.8f, 1));

            // barricadeTile.DisintegrationFx.GetComponent<ParticleSystemRenderer>().SetPropertyBlock(_propertyBlock);
        }

        barricadeServiceLocator.BarricadeProperty.Faction = faction;



        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (faction == GameFaction.Disabled)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }

        meshRenderer.enabled = true;
        meshRenderer.sharedMaterial = barricadeMaterialsContainer.BarricadeMaterials[(int)faction];

        barricadeServiceLocator.BlockSmasherRenderer.gameObject.SetActive(false);

        return;

        BarricadeTile[] barricadeTiles = TransformUtil.GetComponentsFromAllChildren<BarricadeTile>(transform).ToArray();

        bool isEnable = false;

        if (faction == GameFaction.Disabled)
        {
            isEnable = false;
        }
        else
        {
            isEnable = true;
        }

        if (isEnable)
        {
            barricadeServiceLocator.BlockSmasherRenderer.gameObject.SetActive(true);

            barricadeServiceLocator.BlockSmasherRenderer.sharedMaterial = barricadeMaterialsContainer.BarricadeMaterials[(int)faction];
        }
        else
        {
            barricadeServiceLocator.BlockSmasherRenderer.gameObject.SetActive(false);
        }

        // foreach (var barricadeTile in barricadeTiles)
        // {
        //     MeshRenderer[] meshRenderers = TransformUtil.GetComponentsFromAllChildren<MeshRenderer>(barricadeTile.transform).ToArray();

        //     for (int i = 0; i < meshRenderers.Length; i++)
        //     {
        //         meshRenderers[i].enabled = isEnable;

        //         if (isEnable)
        //         {
        //             meshRenderers[i].sharedMaterial = barricadeMaterialsContainer.BarricadeMaterials[(int)faction];
        //         }
        //     }
        // }
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
