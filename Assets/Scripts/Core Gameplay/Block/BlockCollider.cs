using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    [SerializeField] private BlockServiceLocator blockServiceLocator;

    private void OnTriggerEnter(Collider other)
    {
        if (blockServiceLocator.block.BlockProperty.IsDisintegrating)
        {
            return;
        }

        BarricadeTile barricadeTile = other.GetComponent<BarricadeTile>();

        if (barricadeTile != null)
        {
            blockServiceLocator.block.Disintegrate(barricadeTile.Direction);
        }
    }
}
