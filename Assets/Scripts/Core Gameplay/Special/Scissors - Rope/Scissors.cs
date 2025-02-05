using System;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class Scissors : BaseElement
{
    [SerializeField] private Transform halfOne;
    [SerializeField] private Transform halfTwo;

    [SerializeField] private GameFaction faction;

    [SerializeField] private ScissorsMaterialPropertyBlock[] scissorsMaterialPropertyBlock;

    public GameFaction Faction
    {
        get => faction;
    }

    public static event Action<Scissors> cutBlockRopeEvent;

    void Awake()
    {
        for (int i = 0; i < scissorsMaterialPropertyBlock.Length; i++)
        {
            scissorsMaterialPropertyBlock[i].SetFaction(faction);
        }
    }

    void OnValidate()
    {
        if (scissorsMaterialPropertyBlock == null)
        {
            return;
        }

        for (int i = 0; i < scissorsMaterialPropertyBlock.Length; i++)
        {
            scissorsMaterialPropertyBlock[i].SetFaction(faction);
        }
    }

    public override void Use()
    {
        cutBlockRopeEvent?.Invoke(this);
    }

    public void CutAnimation()
    {
        Tween.Rotation(halfOne, new Vector3(0, 0, -16), cycles: -1, cycleMode: CycleMode.Yoyo, duration: 0.2f);
        Tween.Rotation(halfTwo, new Vector3(0, 0, 16), cycles: -1, cycleMode: CycleMode.Yoyo, duration: 0.2f);
    }

    public void Hide()
    {
        Tween.Scale(transform, 0, duration: 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
