using UnityEngine;

[CreateAssetMenu(fileName = "BarricadeMaterialsContainer", menuName = "Scriptable Objects/BarricadeMaterialsContainer")]
public class BarricadeMaterialsContainer : ScriptableObject
{
    [SerializeField] private Material[] barricadeMaterials;

    public Material[] BarricadeMaterials
    {
        get => barricadeMaterials;
    }
}
