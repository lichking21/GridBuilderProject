using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject highlightSprite;
    [SerializeField] Color baseColor;
    [SerializeField] Color offsetColor;

    public bool isEmpty;
    public Building currBuilding;

    public void Init(bool isOffset)
    {
        sr.color = isOffset ? offsetColor : baseColor;
        isEmpty = true;
    }

    public void SetBuilding(Building building)
    {
        currBuilding = building;
        isEmpty = false;
    }

    public void ClearBuilding()
    {
        currBuilding = null;
        isEmpty = true;
    }

    public void MouseCellEnter()
    {
        highlightSprite.SetActive(true);
    }

    public void MouseCellExit()
    {
        highlightSprite.SetActive(false);
    }

    public bool IsEmpty() => isEmpty;
}
