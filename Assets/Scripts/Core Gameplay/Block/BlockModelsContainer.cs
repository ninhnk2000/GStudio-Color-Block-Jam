using UnityEngine;

[CreateAssetMenu(fileName = "BlockModelsContainer", menuName = "Scriptable Objects/BlockModelsContainer")]
public class BlockModelsContainer : ScriptableObject
{
    [SerializeField] private Mesh[] doubleTileBlockMeshes;
    [SerializeField] private Mesh[] singleTileBlockMeshes;

    public Mesh[] DoubleTileBlockMeshes
    {
        get => doubleTileBlockMeshes;
    }

    public Mesh[] SingleTileBlockMeshes
    {
        get => singleTileBlockMeshes;
    }
}
