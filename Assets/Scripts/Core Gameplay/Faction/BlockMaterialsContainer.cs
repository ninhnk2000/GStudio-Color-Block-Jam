using UnityEngine;

[CreateAssetMenu(fileName = "BlockMaterialsContainer", menuName = "Scriptable Objects/BlockMaterialsContainer")]
public class BlockMaterialsContainer : ScriptableObject
{
    [SerializeField] private Material[] blockMaterials;

    public Material[] BlockMaterials
    {
        get => blockMaterials;
    }
}
