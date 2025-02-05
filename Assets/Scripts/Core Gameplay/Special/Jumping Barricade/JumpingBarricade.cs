using System.Collections.Generic;
using PrimeTween;
using Saferio.Util.SaferioTween;
using Unity.VisualScripting;
using UnityEngine;

public class JumpingBarricade : MonoBehaviour
{
    [SerializeField] private Direction jumpDirection;

    [SerializeField] private BaseBarricade _currentBarricade;

    private BaseBarricade[] _barricades;
    private List<BaseBarricade> _checkedBarricades;
    private Direction _currentDirection;
    private JumpingBarricadeMaterialPropertyBlock _jumpingBarricadeMaterialPropertyBlock;

    private float rightBound;
    private float leftBound;
    private float topBound;
    private float bottomBound;

    void Awake()
    {
        BaseBlock.blockCompletedEvent += JumpNext;

        Init();
    }

    void OnDestroy()
    {
        BaseBlock.blockCompletedEvent -= JumpNext;
    }

    private void Init()
    {
        _barricades = FindObjectsOfType<BaseBarricade>();

        _checkedBarricades = new List<BaseBarricade>();

        _jumpingBarricadeMaterialPropertyBlock = GetComponent<JumpingBarricadeMaterialPropertyBlock>();

        _checkedBarricades.Add(_currentBarricade);

        List<BaseBarricade> validBarricades = new List<BaseBarricade>();

        for (int i = 0; i < _barricades.Length; i++)
        {
            if (_barricades[i].BarricadeProperty.Faction != GameEnum.GameFaction.Disabled)
            {
                validBarricades.Add(_barricades[i]);
            }

            float distanceX = Mathf.Abs(_barricades[i].transform.position.x);
            float distanceZ = Mathf.Abs(_barricades[i].transform.position.z);

            if (distanceX > rightBound)
            {
                rightBound = distanceX;
            }

            if (distanceZ > topBound)
            {
                topBound = distanceZ;
            }
        }

        leftBound = -rightBound;
        bottomBound = -topBound;

        _barricades = validBarricades.ToArray();
    }

    private void JumpNext()
    {
        float minDistance = float.MaxValue;
        float minDistanceRight = float.MaxValue;
        float minDistanceLeft = float.MaxValue;
        float minDistanceUp = float.MaxValue;
        float minDistanceDown = float.MaxValue;

        BaseBarricade nextBarricade = null;
        BaseBarricade nearestRightBarricade = null;
        BaseBarricade nearestLeftBarricade = null;
        BaseBarricade nearestUpBarricade = null;
        BaseBarricade nearestDownBarricade = null;

        if (_checkedBarricades.Count == _barricades.Length)
        {
            _checkedBarricades.Clear();
        }

        _currentDirection = GetDirection(_currentBarricade.transform.position);

        // if (transform.eulerAngles == new Vector3(90, 0, 0))
        // {
        //     _currentDirection = Direction.Right;
        // }
        // else if (transform.eulerAngles == new Vector3(90, 0, 180))
        // {
        //     _currentDirection = Direction.Left;
        // }
        // else if (transform.eulerAngles == new Vector3(90, 0, 90))
        // {
        //     _currentDirection = Direction.Up;
        // }
        // else if (transform.eulerAngles == new Vector3(90, 0, -90))
        // {
        //     _currentDirection = Direction.Down;
        // }

        for (int i = 0; i < _barricades.Length; i++)
        {
            if (_barricades[i].Faction == GameEnum.GameFaction.Disabled)
            {
                continue;
            }

            if (_checkedBarricades.Contains(_barricades[i]))
            {
                continue;
            }

            if (_barricades[i] != _currentBarricade)
            {
                if (_currentDirection == Direction.Right)
                {
                    if (_barricades[i].transform.position.x < _currentBarricade.transform.position.x &&
                        _barricades[i].transform.position.z == _currentBarricade.transform.position.z)
                    {
                        continue;
                    }
                }
                if (_currentDirection == Direction.Left)
                {
                    if (_barricades[i].transform.position.x > _currentBarricade.transform.position.x &&
                        _barricades[i].transform.position.z == _currentBarricade.transform.position.z)
                    {
                        continue;
                    }
                }
                if (_currentDirection == Direction.Up)
                {
                    if (_barricades[i].transform.position.z < _currentBarricade.transform.position.z &&
                        _barricades[i].transform.position.x == _currentBarricade.transform.position.x)
                    {
                        continue;
                    }
                }
                if (_currentDirection == Direction.Down)
                {
                    if (_barricades[i].transform.position.z > _currentBarricade.transform.position.z &&
                        _barricades[i].transform.position.x == _currentBarricade.transform.position.x)
                    {
                        continue;
                    }
                }

                float distanceX = _barricades[i].transform.position.x - _currentBarricade.transform.position.x;
                float distanceZ = _barricades[i].transform.position.z - _currentBarricade.transform.position.z;

                if (distanceX > 0)
                {
                    if (distanceX <= minDistanceRight)
                    {
                        minDistanceRight = distanceX;

                        nearestRightBarricade = _barricades[i];
                    }
                }
                else
                {
                    if (Mathf.Abs(distanceX) < minDistanceRight)
                    {
                        minDistanceLeft = Mathf.Abs(distanceX);

                        nearestLeftBarricade = _barricades[i];
                    }
                }
                if (distanceZ > 0)
                {
                    if (distanceZ < minDistanceUp)
                    {
                        minDistanceUp = distanceZ;

                        nearestUpBarricade = _barricades[i];
                    }
                }
                else
                {
                    if (Mathf.Abs(distanceZ) < minDistanceDown)
                    {
                        minDistanceDown = Mathf.Abs(distanceZ);

                        nearestDownBarricade = _barricades[i];
                    }
                }

                float distance = Vector3.Distance(_barricades[i].transform.position, _currentBarricade.transform.position);

                if (distance < minDistance)
                {
                    nextBarricade = _barricades[i];

                    minDistance = distance;
                }
            }
        }

        if (_currentDirection == Direction.Right)
        {
            nextBarricade = nearestRightBarricade;
        }
        else if (_currentDirection == Direction.Left)
        {
            nextBarricade = nearestLeftBarricade;
        }
        else if (_currentDirection == Direction.Up)
        {
            nextBarricade = nearestUpBarricade;
        }
        else if (_currentDirection == Direction.Down)
        {
            nextBarricade = nearestDownBarricade;
        }

        Direction nextDirection = GetDirection(nextBarricade.transform.position);

        Vector3 nextPosition = nextBarricade.transform.position.ChangeY(transform.position.y);

        Tween.Position(transform, nextPosition, duration: 0.5f);
        Tween.Rotation(transform, GetAngleAtDirection(nextDirection), duration: 0.5f);

        int maxTile = Mathf.Max(nextBarricade.BarricadeProperty.NumTileX, nextBarricade.BarricadeProperty.NumTileZ);

        float length = 1f * (maxTile - 1);

        _jumpingBarricadeMaterialPropertyBlock.SetPositionXAddition(length);

        _currentBarricade = nextBarricade;
        _currentDirection = nextDirection;

        _checkedBarricades.Add(nextBarricade);
    }

    private Direction GetDirection(Vector3 barricadePosition)
    {
        if (barricadePosition.x == rightBound)
        {
            return Direction.Down;
        }
        else if (barricadePosition.x == leftBound)
        {
            return Direction.Up;
        }
        else if (barricadePosition.z == topBound)
        {
            return Direction.Right;
        }
        else if (barricadePosition.z == bottomBound)
        {
            return Direction.Left;
        }

        return Direction.Right;
    }

    private Vector3 GetAngleAtDirection(Direction direction)
    {
        if (direction == Direction.Right)
        {
            return new Vector3(90, 0, 0);
        }
        else if (direction == Direction.Left)
        {
            return new Vector3(90, 0, 180);
        }
        else if (direction == Direction.Up)
        {
            return new Vector3(90, 0, 90);
        }
        else
        {
            return new Vector3(90, 0, -90);
        }
    }
}
