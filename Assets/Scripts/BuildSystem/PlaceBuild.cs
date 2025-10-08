using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceBuild : MonoBehaviour
{
    [SerializeField] private GameObject building;
    [SerializeField] private GameObject buildingPreviewPrefab;
    private GameObject buildingPreviewObj;

    [SerializeField] private InputActionAsset inputs;
    private MouseRay mouseRay;

    private InputAction moveAction;
    private InputAction buildAction;

    private Vector2 currPreviewPos;
    private int cellStep = 1;
    private bool isUsingKeyboard;

    void OnEnable()
    {
        moveAction?.Enable();
        moveAction.performed += KeyboardPreviewMove;

        buildAction?.Enable();
        buildAction.performed += SetBuilding;
    }
    void OnDisable()
    {
        moveAction.performed -= KeyboardPreviewMove;
        moveAction?.Disable();

        buildAction.performed -= SetBuilding;
        buildAction?.Disable();
    }

    // move preview
    private void KeyboardPreviewMove(InputAction.CallbackContext context)
    {
        isUsingKeyboard = true;
        if (buildingPreviewObj == null)
        {
            Debug.Log("There is no building preview instance");
            return;
        }

        Vector2 input = (moveAction != null) ? context.ReadValue<Vector2>() : Vector2.zero;
        Vector2 move = new Vector2(input.x, input.y);

        if (input.x > 0) move.x = 1;
        else if (input.x < 0) move.x = -1;

        if (input.y > 0) move.y = 1;
        else if (input.y < 0) move.y = -1;

        currPreviewPos += move * cellStep;
        currPreviewPos.x = Mathf.Round(currPreviewPos.x / cellStep) * cellStep;
        currPreviewPos.y = Mathf.Round(currPreviewPos.y / cellStep) * cellStep;

        buildingPreviewObj.transform.position = currPreviewPos;
    }
    private void MousePreviewDrag(Vector2 mousePos)
    {
        if (buildingPreviewObj == null)
        {
            Debug.Log("There is no building preview instance");
            return;
        }
        
        buildingPreviewObj.transform.position = mousePos;
    }

    private void SetBuilding(InputAction.CallbackContext context)
    {
        Vector2 placePos;
        Cell currCell = (mouseRay != null) ? mouseRay.GetMouseCell() : null;

        if (currCell != null && !isUsingKeyboard)
        {
            placePos = currCell.transform.position;
        }
        else
        {
            placePos = currPreviewPos;
        }

        // if mouse on cell and cell is free
        if (building != null)
            Instantiate(
                building,
                placePos,
                Quaternion.identity
            );
    }

    void Awake()
    {
        if (inputs != null)
        {
            moveAction = inputs.FindActionMap("Player").FindAction("Move");
            buildAction = inputs.FindActionMap("Player").FindAction("Build");
        }    
    }

    void Start()
    {
        mouseRay = FindAnyObjectByType<MouseRay>();
        if (mouseRay == null)
            Debug.Log("MouseRay does not exist");

        // if (buildingPreviewPrefab != null)
        //     buildingPreviewObj = Instantiate(buildingPreviewPrefab);

        if (buildingPreviewPrefab != null)
            buildingPreviewObj = Instantiate(buildingPreviewPrefab, new Vector2(0, 0), Quaternion.identity);
        currPreviewPos = buildingPreviewObj.transform.position;
    }

    void Update()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, 10f));
        Vector2 mousePos = new Vector2(mouseWorld.x, mouseWorld.y);

        // switch preview move
        Vector2 mouseMove = Mouse.current.delta.ReadValue();
        if (mouseMove.sqrMagnitude > 0.01f) isUsingKeyboard = false;
        //if (moveAction != null & moveAction.ReadValue<Vector2>() != Vector2.zero) isUsingKeyboard = true;
        if (!isUsingKeyboard)
        {
            MousePreviewDrag(mousePos);
            currPreviewPos = mousePos;
        }
    }
}