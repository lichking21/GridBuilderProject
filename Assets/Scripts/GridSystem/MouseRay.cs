using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRay : MonoBehaviour
{
    public RaycastHit2D GetRay()
    {
        Vector2 mouseCurrPos = Mouse.current.position.ReadValue();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouseCurrPos);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        return hit;
    }
}