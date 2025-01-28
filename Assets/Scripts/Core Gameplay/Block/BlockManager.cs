using System;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private BaseBlock[] _blocks;

    public static event Action winLevelEvent;

    private void Awake()
    {
        LevelLoader.sendLevelBaseBlocksDataEvent += ReceiveBaseBlockesData;
        BaseBlock.blockCompletedEvent += OnBlockCompleted;
    }

    private void OnDestroy()
    {
        LevelLoader.sendLevelBaseBlocksDataEvent -= ReceiveBaseBlockesData;
        BaseBlock.blockCompletedEvent -= OnBlockCompleted;
    }

    private void ReceiveBaseBlockesData(BaseBlock[] blocks)
    {
        _blocks = blocks;
    }

    private void OnBlockCompleted()
    {
        int remainingBlocks = 0;

        for (int i = 0; i < _blocks.Length; i++)
        {
            if (!_blocks[i].BlockProperty.IsDone)
            {
                remainingBlocks++;
            }
        }

        if (remainingBlocks == 0)
        {
            winLevelEvent?.Invoke();
        }
    }
}
