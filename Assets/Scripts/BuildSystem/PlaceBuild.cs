using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceBuild : MonoBehaviour
{
    [SerializeField] private GameObject building;

    private void PickBuilding()
    {
        building.transform.position = Mouse.current.position.ReadValue();
    }

    private void SetBuilding()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
        }
    }

    void Update()
    {
        PickBuilding();    
    }
}