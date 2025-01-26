using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform tileContainer;

    [Header("CUSTOMIZE")]
    [SerializeField] private int numRow;
    [SerializeField] private int numColumn;
    [SerializeField] private float distanceRatio;

    private GameObject[] tiles;
    private int _numTile;
    private Vector3 _tileSize;

    private void Awake()
    {
        _numTile = numRow * numColumn;

        _tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;

        Generate();
    }

    private void Generate()
    {
        tiles = new GameObject[_numTile];

        for (int i = 0; i < _numTile; i++)
        {
            tiles[i] = Instantiate(tilePrefab, tileContainer);
        }

        Vector3 position = new Vector3();

        for (int i = 0; i < numRow; i++)
        {
            for (int j = 0; j < numColumn; j++)
            {
                int tileIndex = j + i * numColumn;

                position.x = (-(numColumn - 1) / 2 + (j + distanceRatio)) * _tileSize.x;
                position.z = ((numRow - 1) / 2 - (i + distanceRatio)) * _tileSize.z;

                tiles[tileIndex].transform.position = position;
            }
        }
    }
}
