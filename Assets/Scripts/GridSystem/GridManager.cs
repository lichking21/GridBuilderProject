using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Cell cell;
    [SerializeField] private Transform cellsParent;
    private Camera mainCam;

    // grid size
    private int width = 16;
    private int height = 16;

    private void CreateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var newCell = Instantiate(cell, new Vector3(x, y), Quaternion.identity, cellsParent);

                // isOffset = true if (x is even and y is odd ) or (x is odd and y is even)
                // isOffset = false if (x and y both are even or odd) 
                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);

                newCell.name = $"Cell[{x},{y}]";
                newCell.Init(isOffset);
            }
        }

        // Center camera
        mainCam.transform.position = new Vector3(
            width / 2f - 0.5f,
            height / 2f - 0.5f,
            -10f
        );
    }

    public void InitCam(Camera cam)
    {
        mainCam = cam;
        CreateGrid();
    }
}
