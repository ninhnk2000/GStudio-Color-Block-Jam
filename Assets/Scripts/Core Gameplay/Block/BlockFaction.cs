using System.Linq;
using UnityEngine;
using static GameEnum;

public class BlockFaction : MonoBehaviour
{
    [SerializeField] private Renderer blockRenderer;

    [SerializeField] private GameFaction faction;

    [SerializeField] private Material[] materials;

    public GameFaction Faction
    {
        get => faction;
    }

    private void OnValidate()
    {
        SetFaction();
    }

    public void SetFaction()
    {
        if (blockRenderer == null)
        {
            return;
        }

        blockRenderer.material = materials[(int)faction];
    }
}
