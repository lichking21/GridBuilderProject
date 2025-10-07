using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] GameObject highlightSprite;
    [SerializeField] Color baseColor;
    [SerializeField] Color offsetColor;

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
}
