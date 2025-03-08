using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using Unity.Mathematics;
using UnityEngine;
using static GameEnum;

public class BarricadeFaction : MonoBehaviour
{
    // [SerializeField] private GameFaction faction;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Renderer _outlineSpriteRenderer;

    [SerializeField] private BarricadeServiceLocator barricadeServiceLocator;

    [SerializeField] private BarricadeMaterialsContainer barricadeMaterialsContainer;

    #region PRIVATE FIELD
    [SerializeField] private List<Tween> _tweens;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Renderer _arrowRenderer;
    private MaterialPropertyBlock _propertyBlock;
    private MaterialPropertyBlock _arrowPropertyBlock;
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

        if (_arrowRenderer == null)
        {
            _arrowRenderer = arrow.GetComponent<Renderer>();
        }

        if (_propertyBlock == null)
        {
            _propertyBlock = new MaterialPropertyBlock();
            _arrowPropertyBlock = new MaterialPropertyBlock();
        }
    }

    public void SetFaction(GameFaction faction)
    {
        if (_propertyBlock == null)
        {
            Init();
        }

        Color factionColor = FactionUtility.GetColorForFaction(faction);

        float ratio = 1 - Mathf.Max(Mathf.Max(factionColor.r + factionColor.g), factionColor.b);

        Color outlineColor = FactionUtility.GetHDRColorForFaction(faction);

        outlineColor.r = Mathf.Min(outlineColor.r, 1.8f);
        outlineColor.g = Mathf.Min(outlineColor.g, 1.8f);
        outlineColor.b = Mathf.Min(outlineColor.b, 1.8f);

        // _propertyBlock.SetColor("_Color", factionColor);
        _propertyBlock.SetColor("_OutlineColor", 2.7f * factionColor);
        _propertyBlock.SetFloat("_AdditionPositionX", barricadeServiceLocator.barricade.BarricadeProperty.AdditionalPositionX);

        _arrowPropertyBlock.SetColor("_Color", Color.white);
        _arrowRenderer.SetPropertyBlock(_arrowPropertyBlock);

        if (_outlineSpriteRenderer != null)
        {
            _outlineSpriteRenderer.SetPropertyBlock(_propertyBlock);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            BarricadeTile barricadeTile = transform.GetChild(i).GetComponent<BarricadeTile>();

            if (barricadeTile == null)
            {
                continue;
            }

            ParticleSystem.MainModule mainModule = barricadeTile.DisintegrationFx.main;

            mainModule.startColor = new ParticleSystem.MinMaxGradient(factionColor, ColorUtil.WithAlpha(factionColor * 0.8f, 1));
        }

        barricadeServiceLocator.BarricadeProperty.Faction = faction;



        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (faction == GameFaction.Disabled)
        {
            meshRenderer.enabled = true;

            arrow.gameObject.SetActive(false);

            if (_outlineSpriteRenderer != null)
            {
                _outlineSpriteRenderer.gameObject.SetActive(false);
            }

            _propertyBlock.SetFloat("_OutlineWidth", 0);
            _propertyBlock.SetFloat("_Alpha", 1);
        }
        else
        {
            meshRenderer.enabled = false;

            arrow.gameObject.SetActive(true);

            _propertyBlock.SetFloat("_OutlineWidth", 1.05f);
            _propertyBlock.SetFloat("_Alpha", 0.5f);
        }

        meshRenderer.enabled = true;
        meshRenderer.sharedMaterial = barricadeMaterialsContainer.BarricadeMaterials[(int)faction];

        barricadeServiceLocator.BlockSmasherRenderer.gameObject.SetActive(false);

        _renderer.SetPropertyBlock(_propertyBlock);

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
}
