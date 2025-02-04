using UnityEngine;

public class BlockConnector : MonoBehaviour
{
    [SerializeField] private BlockServiceLocator blockServiceLocator;

    [Header("CUSTOMIZE")]
    [SerializeField] private GameObject pairBlock;

    private int _pairBlockInstanceId;

    void Awake()
    {
        BaseBlock.movePairedBlock += MoveInPair;

        _pairBlockInstanceId = pairBlock.GetInstanceID();
    }

    void OnDestroy()
    {
        BaseBlock.movePairedBlock -= MoveInPair;
    }

    private void MoveInPair(int instanceId, bool isMove)
    {
        if (instanceId == _pairBlockInstanceId)
        {
            blockServiceLocator.block.BlockRigidbody.isKinematic = !isMove;

            if (!isMove)
            {
                blockServiceLocator.block.Snap();
            }
        }
    }
}
