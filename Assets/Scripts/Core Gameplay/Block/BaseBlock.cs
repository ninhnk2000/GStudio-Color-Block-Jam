using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    [SerializeField] private BlockProperty blockProperty;
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private Rigidbody blockRigidBody;

    [SerializeField] private float speedMultiplier;
    [SerializeField] private float snappingLerpRatio;

    #region PRIVATE FIELD
    private Vector3 _snapPosition;
    private bool _isSnapping;
    #endregion

    public BlockProperty BlockProperty
    {
        get => blockProperty;
    }

    private void Update()
    {
        if (_isSnapping)
        {
            transform.position = Vector3.Lerp(transform.position, _snapPosition, snappingLerpRatio);

            if (Vector3.Distance(transform.position, _snapPosition) < GameConstants.TINY_FLOAT_VALUE)
            {
                _isSnapping = false;
            }
        }

    }

    public void Move(Vector2 inputDirection)
    {
        blockRigidBody.linearVelocity = speedMultiplier * new Vector3(inputDirection.x, 0, inputDirection.y);
    }


    public void Stop()
    {
        Snap();
    }

    private float GetTileDistance()
    {
        return 1.05f * tilePrefab.GetComponent<MeshRenderer>().bounds.size.x;
    }

    private void Snap()
    {
        float tileDistance = GetTileDistance();

        Vector3 position = transform.position;

        Vector2 coordinator;

        // covert to bottom-right position
        Vector3 bottomRightPosition = new Vector3();

        bottomRightPosition.x = transform.position.x + (BlockProperty.NumTileX - 1) / 2f * tileDistance;
        bottomRightPosition.z = transform.position.z - (BlockProperty.NumTileZ - 1) / 2f * tileDistance;

        coordinator.x = Mathf.Round(bottomRightPosition.x / tileDistance);
        coordinator.y = Mathf.Round(bottomRightPosition.z / tileDistance);

        Vector3 finalPosition = new Vector3(0, 2, 0);

        finalPosition.x = coordinator.x * tileDistance - (BlockProperty.NumTileX - 1) / 2f * tileDistance;
        finalPosition.z = coordinator.y * tileDistance + (BlockProperty.NumTileZ - 1) / 2f * tileDistance;

        _snapPosition = finalPosition;

        _isSnapping = true;
    }
}
