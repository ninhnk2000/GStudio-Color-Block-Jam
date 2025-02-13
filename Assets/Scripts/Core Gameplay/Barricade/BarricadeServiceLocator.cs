using UnityEngine;

public class BarricadeServiceLocator : MonoBehaviour
{
    public BaseBarricade barricade;
    public BarricadeFaction barricadeFaction;
    public BarricadeProperty BarricadeProperty
    {
        get => barricade.BarricadeProperty;
    }
    public MeshRenderer BlockSmasherRenderer
    {
        get => barricade.BlockSmasherRenderer;
    }
}
