using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRay : MonoBehaviour
{
    private Cell lastCell;

    private void CheckMouseOnCell()
    {
        Vector2 mouseCurrPos = Mouse.current.position.ReadValue();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouseCurrPos);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            Cell cell = hit.collider.GetComponent<Cell>();

            if (cell != null & cell != lastCell)
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

    void Update()
    {
        CheckMouseOnCell();
    }
}