using UnityEngine;

public class BlockServiceLocator : MonoBehaviour
{
    public BaseBlock block;
    public BlockFaction blockFaction;
    public BlockMaterialPropertyBlock blockMaterialPropertyBlock;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        block = GetComponent<BaseBlock>();
        blockFaction = GetComponent<BlockFaction>();
        blockMaterialPropertyBlock = GetComponent<BlockMaterialPropertyBlock>();
    }
}
