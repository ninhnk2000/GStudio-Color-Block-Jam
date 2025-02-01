using UnityEngine;

public class BlockServiceLocator : MonoBehaviour
{
    public BaseBlock block;
    public BlockFaction blockFaction;
    public BlockCollider blockCollider;
    public BlockMaterialPropertyBlock blockMaterialPropertyBlock;

    public MeshFilter meshFilter;

    public Vector3 MeshSize
    {
        get => meshFilter.mesh.bounds.size;
    }

    public Vector3 Size
    {
        get => meshFilter.mesh.bounds.size * transform.localScale.x;
    }

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        block = GetComponent<BaseBlock>();
        blockFaction = GetComponent<BlockFaction>();
        blockCollider = GetComponent<BlockCollider>();
        blockMaterialPropertyBlock = GetComponent<BlockMaterialPropertyBlock>();
        meshFilter = GetComponent<MeshFilter>();
    }
}
