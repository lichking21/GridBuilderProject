using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Color baseColor;
    [SerializeField] private Color darkColor;
    [SerializeField] private SpriteRenderer sr;

    public void Init(bool isOffset)
    {
        sr.color = isOffset ? darkColor : baseColor;
    }
}
