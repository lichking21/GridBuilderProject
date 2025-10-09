using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildSystem : MonoBehaviour
{
    public static BuildSystem Instance {  get; private set; }

    // Objects
    [SerializeField] Building[] buildings;

    private GameObject currBuilding;
    private GameObject currPreviewPrefab;
    private GameObject buildingPreviewObj;

    // Actions
    [SerializeField] private InputActionAsset inputs;
    private MouseRay mouseRay;

    private InputAction moveAction;
    private InputAction buildAction;

    // Variables
    private Vector2 currPreviewPos;
    private int cellStep = 1;
    private bool isUsingKeyboard;
    public bool placeBtn = false;

    // Events
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

    // Move building preview
    private void KeyboardPreviewMove(InputAction.CallbackContext context)
    {
        isUsingKeyboard = true;
        if (buildingPreviewObj == null)
        {
            //Debug.Log("There is no building preview instance");
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
            //Debug.Log("There is no building preview instance");
            return;     
        }
        
        buildingPreviewObj.transform.position = mousePos;
    }

    // Building placement
    private void SetBuilding(InputAction.CallbackContext context)
    {
        if (currBuilding == null) return;

        Vector2 placePos;
        Cell currCell = (mouseRay != null) ? mouseRay.GetMouseCell() : null;

        if (currCell != null && !isUsingKeyboard) placePos = currCell.transform.position;
        else placePos = currPreviewPos;

        // if cell is free
        Instantiate(
            currBuilding,
            placePos,
            Quaternion.identity
        );
    }

    // Select building
    public void SwitchPlaceBtn() => placeBtn = !placeBtn;
    public void SelectBuilding(int currBuildingIdx)
    {
        if (buildings == null || buildings.Length == 0)
        {
            Debug.LogError("PlaceBuilding: buildings array is empty!");
            return;
        }

        if (currBuildingIdx < 0 || currBuildingIdx >= buildings.Length)
        {
            Debug.Log($"Wrong building index: {currBuildingIdx}");
            return;
        }


        Building selectedBuilding = buildings[currBuildingIdx];

        currBuilding = selectedBuilding.building;
        currPreviewPrefab = selectedBuilding.buildingPreview;

        if (buildingPreviewObj != null) Destroy(buildingPreviewObj);

        if (currPreviewPrefab != null)
            buildingPreviewObj = Instantiate(
                currPreviewPrefab,
                currPreviewPos,
                Quaternion.identity
            );
    }

    void Awake()
    {
        Instance = this;

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
    }

    void Update()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, 10f));
        Vector2 mousePos = new Vector2(mouseWorld.x, mouseWorld.y);

        // switch preview move
        Vector2 mouseMove = Mouse.current.delta.ReadValue();
        if (mouseMove.sqrMagnitude > 0.01f) isUsingKeyboard = false;
        if (!isUsingKeyboard && buildingPreviewObj != null)
        {
            MousePreviewDrag(mousePos);
            currPreviewPos = mousePos;
        }
    }
}