using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    [SerializeField] private BlockServiceLocator blockServiceLocator;

    private void OnCollisionEnter(Collision other)
    {
        if (blockServiceLocator.block.BlockProperty.IsDisintegrating)
        {
            return;
        }

        BarricadeTile barricadeTile = other.gameObject.GetComponent<BarricadeTile>();

        if (barricadeTile != null)
        {
            if (barricadeTile.Faction == blockServiceLocator.block.BlockProperty.Faction)
            {
                blockServiceLocator.block.Disintegrate(barricadeTile.Direction);
            }
        }
    }
}
