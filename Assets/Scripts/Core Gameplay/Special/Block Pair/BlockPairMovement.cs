// using UnityEngine;

// public class BlockPairMovement : MonoBehaviour
// {
//     #region MOVEMENT
//     public void Move(Vector3 targetPosition)
//     {
//         if (blockProperty.IsDisintegrating)
//         {
//             return;
//         }

//         if (blockProperty.IsMoving)
//         {
//             // return;
//         }
//         else
//         {
//             blockServiceLocator.blockMaterialPropertyBlock.ShowOutline(true);

//             _blockRigidBody.isKinematic = false;

//             _isSnapping = false;

//             blockProperty.IsMoving = true;
//         }

//         _targetPosition = targetPosition.ChangeY(1.2f * _initialPosition.y);
//     }

//     public void Stop()
//     {
//         if (blockProperty.IsDisintegrating)
//         {
//             return;
//         }

//         _blockRigidBody.isKinematic = true;

//         Snap();

//         blockServiceLocator.blockMaterialPropertyBlock.ShowOutline(false);

//         blockProperty.IsMoving = false;
//     }

//     private void Snap()
//     {
//         float tileDistance = GetTileDistance();

//         Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5, layerMaskCheckTile);

//         if (hit.collider != null)
//         {
//             _snapPosition = hit.collider.transform.position;

//             if (_snapPosition.x > transform.position.x)
//             {
//                 _snapPosition.x -= (BlockProperty.NumTileX - 1) / 2f * tileDistance;
//             }
//             else
//             {
//                 _snapPosition.x += (BlockProperty.NumTileX - 1) / 2f * tileDistance;
//             }

//             if (_snapPosition.z > transform.position.z)
//             {
//                 _snapPosition.z -= (BlockProperty.NumTileZ - 1) / 2f * tileDistance;
//             }
//             else
//             {
//                 _snapPosition.z += (BlockProperty.NumTileZ - 1) / 2f * tileDistance;
//             }

//             if (BlockProperty.NumTileX % 2 == 1)
//             {
//                 _snapPosition.x = hit.collider.transform.position.x;
//             }

//             if (BlockProperty.NumTileZ % 2 == 1)
//             {
//                 _snapPosition.z = hit.collider.transform.position.z;
//             }

//             // _snapPosition.x -= (BlockProperty.NumTileX - 1) / 2f * tileDistance;
//             // _snapPosition.z += (BlockProperty.NumTileZ - 1) / 2f * tileDistance;

//             _snapPosition.y = _initialPosition.y;

//             _blockRigidBody.isKinematic = true;

//             _isSnapping = true;
//         }

//         // _snapPosition = finalPosition;

//         // _blockRigidBody.isKinematic = true;

//         // _isSnapping = true;
//     }
//     #endregion
// }
