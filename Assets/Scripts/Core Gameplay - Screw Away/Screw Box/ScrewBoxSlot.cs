using System;
using UnityEngine;
using static GameEnum;

public class ScrewBoxSlot : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private bool isScrewPort;

    private BaseScrew _screw;
    private bool _isFilled;

    public BaseScrew Screw
    {
        get => _screw;
        set => _screw = value;
    }

    public bool IsFilled
    {
        get => _isFilled;
        set => _isFilled = value;
    }

    public bool IsScrewPort
    {
        get => isScrewPort;
        set => isScrewPort = value;
    }

    #region EVENT
    public static event Action<int> screwBoxCompleteEvent;
    #endregion

    public void Fill(BaseScrew screw)
    {
        _screw = screw;

        _isFilled = true;
    }

    public void CompleteFill()
    {
        if (transform.GetSiblingIndex() == 2)
        {
            screwBoxCompleteEvent?.Invoke(transform.parent.gameObject.GetInstanceID());
        }
    }
}
