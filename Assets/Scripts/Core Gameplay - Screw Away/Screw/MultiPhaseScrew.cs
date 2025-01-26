using System;
using PrimeTween;
using Saferio.Util.SaferioTween;
using UnityEngine;

public class MultiPhaseScrew : BasicScrew
{
    [SerializeField] private int phase;

    public static event Action<MultiPhaseScrew> manageScrewEvent;

    public int Phase
    {
        get => phase;
        set => phase = value;
    }

    protected override void MoreLogicInAwake()
    {
        if (phase != 0)
        {
            // screwServiceLocator.screwMaterialPropertyBlock.SetColor(Color.black);

            _initialScale = transform.localScale;

            transform.localScale = transform.localScale.ChangeZ(0);

            _isLocked = true;
        }
    }

    protected override void RegisterMoreEvent()
    {
        MultiPhaseLevelManager.switchPhaseEvent += OnPhaseSwitched;
    }

    protected override void UnregisterMoreEvent()
    {
        MultiPhaseLevelManager.switchPhaseEvent -= OnPhaseSwitched;
    }

    private void Start()
    {
        manageScrewEvent?.Invoke(this);
    }

    public override void Loose(int screwId, GameEnum.GameFaction faction, ScrewBoxSlot screwBoxSlot)
    {
        if (_isLocked)
        {
            return;
        }

        base.Loose(screwId, faction, screwBoxSlot);
    }

    private void OnPhaseSwitched(int phase)
    {
        if (phase == this.phase)
        {
            // screwServiceLocator.screwFaction.SetColorByFaction();

            Tween.Scale(transform, _initialScale, duration: 0.3f);

            _isLocked = false;
        }
    }
}
