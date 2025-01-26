using System;
using UnityEngine;
using static GameEnum;

public class ScrewBoxFaction : MonoBehaviour
{
    [SerializeField] private GameFaction faction;
    [SerializeField] private SpriteRenderer boxSpriteRenderer;
    [SerializeField] private SpriteRenderer boxLidSpriteRenderer;

    [SerializeField] private ScrewBoxServiceLocator screwBoxServiceLocator;

    [SerializeField] private ScrewBoxFactionData[] screwBoxFactionsData;

    public GameFaction Faction
    {
        get => faction;
        set => SetFaction(value);
    }

    void Awake()
    {
        // screwBoxServiceLocator.screwBoxMaterialPropertyBlock.SetColor(FactionUtility.GetColorForFaction(faction));
    }

    private void SetFaction(GameFaction faction)
    {
        this.faction = faction;

        boxSpriteRenderer.sprite = screwBoxFactionsData[(int)faction].BoxSprite;
        boxLidSpriteRenderer.sprite = screwBoxFactionsData[(int)faction].BoxLidSprite;

        // screwBoxServiceLocator.screwBoxMaterialPropertyBlock.SetColor(FactionUtility.GetColorForFaction(faction));
    }
}

[Serializable]
public class ScrewBoxFactionData
{
    [SerializeField] private GameFaction faction;
    [SerializeField] private Sprite boxSprite;
    [SerializeField] private Sprite boxLid;

    public Sprite BoxSprite
    {
        get => boxSprite;
    }

    public Sprite BoxLidSprite
    {
        get => boxLid;
    }
}
