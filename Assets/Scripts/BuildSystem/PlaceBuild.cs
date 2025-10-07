using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceBuild : MonoBehaviour
{
    [SerializeField] private GameObject building;
    [SerializeField] private GameObject buildingPreviewPrefab;
    private GameObject buildingPreviewObj;

    private MouseRay mouseRay;

    private void PreviewDrag(Vector2 mousePos, Cell currCell)
    {
        if (buildingPreviewObj == null)
        {
            Debug.Log("There is no building preview instance");
            return;
        }

            if (currCell != null)
                buildingPreviewObj.transform.position = currCell.transform.position;
            else
                buildingPreviewObj.transform.position = mousePos;
    }

    private void SetBuilding(Cell currCell)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // if mouse on cell and cell is free
            if (currCell != null && building != null)
                Instantiate(
                    building,
                    currCell.transform.position,
                    Quaternion.identity
                );
        }
    }

    void Start()
    {
        mouseRay = FindAnyObjectByType<MouseRay>();
        if (mouseRay == null)
            Debug.Log("MouseRay does not exist");

        if (buildingPreviewPrefab != null)
            buildingPreviewObj = Instantiate(buildingPreviewPrefab);
    }

    void Update()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, 10f));
        Vector2 mousePos = new Vector2(mouseWorld.x, mouseWorld.y);

        Cell cell = (mouseRay != null) ? mouseRay.GetMouseCell() : null;

        PreviewDrag(mousePos, cell);
        SetBuilding(cell);
    }
}