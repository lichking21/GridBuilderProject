using UnityEngine;

public class Building : MonoBehaviour
{
    public Cell cell;

    private void OnDestroy()
    {
        if (cell != null)
            cell.ClearBuilding();
    }
}