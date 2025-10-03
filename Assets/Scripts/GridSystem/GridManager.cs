using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject cell;
    [SerializeField] Transform cellsParent;

    private int width = 16;
    private int height = 9;
    private float cellScale = 1f;

    private void CreateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 cellPos = new Vector2(x * cellScale, y * cellScale);

                var newCell = Instantiate(cell, cellPos, Quaternion.identity, cellsParent);
                newCell.name = $"[{x}, {y}]";

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                newCell.GetComponent<Cell>().Init(isOffset);
            }
        }

        Camera.main.transform.position = new Vector3(
            (float)width / 2 - 0.5f,
            (float)height / 2 - 0.5f,
            -10f
        );
    }

    void Start()
    {
        CreateGrid();
    }
}
