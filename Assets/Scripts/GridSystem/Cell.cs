using UnityEngine;
using UnityEngine.InputSystem;

public class Cell : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject highlightSprite;
    [SerializeField] Color baseColor;
    [SerializeField] Color offsetColor;

    private Cell lastCell;
    private MouseRay mouseRay;
    private RaycastHit2D hit;

    public void Init(bool isOffset)
    {
        sr.color = isOffset ? offsetColor : baseColor;
    }

    public void MouseCellEnter()
    {
        highlightSprite.SetActive(true);
    }

    public void MouseCellExit()
    {
        highlightSprite.SetActive(false);
    }

    private void CheckMouseOnCell()
    {
        hit = mouseRay.GetRay();

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

    void Start()
    {
        mouseRay = new MouseRay();
    }

    void Update()
    {
        CheckMouseOnCell();
    }
}
