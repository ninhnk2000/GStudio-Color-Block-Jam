using UnityEngine;

public class WorldGridManager : MonoBehaviour
{
    [SerializeField] private GameObject gridLinePrefab;

    [Header("CUSTOMIZE")]
    [SerializeField] private float tileSize;
    [SerializeField] private int maxRow;

    private void Awake()
    {
        GenerateGrid();
        GenerateGridLeftSide();
        GenerateGridUpSide();
        GenerateGridBottomSide();
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < maxRow; i++)
        {
            GameObject gridLine = Instantiate(gridLinePrefab);

            Vector3 position = gridLine.transform.position;

            position.x = tileSize * i;

            gridLine.transform.position = position;
        }
    }

    private void GenerateGridLeftSide()
    {
        for (int i = 0; i < maxRow; i++)
        {
            GameObject gridLine = Instantiate(gridLinePrefab);

            Vector3 position = gridLine.transform.position;

            position.x = -tileSize * i;

            gridLine.transform.position = position;
        }
    }

    private void GenerateGridUpSide()
    {
        for (int i = 0; i < maxRow; i++)
        {
            GameObject gridLine = Instantiate(gridLinePrefab);

            Vector3 position = gridLine.transform.position;

            position.z = tileSize * i;

            gridLine.transform.position = position;

            gridLine.transform.eulerAngles = new Vector3(90, 90, 0);
        }
    }

    private void GenerateGridBottomSide()
    {
        for (int i = 0; i < maxRow; i++)
        {
            GameObject gridLine = Instantiate(gridLinePrefab);

            Vector3 position = gridLine.transform.position;

            position.z = -tileSize * i;

            gridLine.transform.position = position;

            gridLine.transform.eulerAngles = new Vector3(90, 90, 0);
        }
    }
}
