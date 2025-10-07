using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRay : MonoBehaviour
{
    private Cell lastCell;

    private void CheckMouseOnCell()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        Vector2 mousePos = new Vector2(mouseWorld.x, mouseWorld.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            Cell cell = hit.collider.GetComponent<Cell>();

            if (cell != null && cell != lastCell)
            {
                if (lastCell != null)
                    lastCell.MouseCellExit();

                cell.MouseCellEnter();
                lastCell = cell;
            }
        }
        else
        {
            if (lastCell != null)
            {
                lastCell.MouseCellExit();
                lastCell = null;
            }
        }
    }

    public Cell GetMouseCell()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, 10f));
        Vector2 mousePos = new Vector2(mouseWorld.x, mouseWorld.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        Cell cell = (hit.collider != null) ? hit.collider.GetComponent<Cell>() : null;

        return cell;
    }

    void Update()
    {
        CheckMouseOnCell();
    }
}