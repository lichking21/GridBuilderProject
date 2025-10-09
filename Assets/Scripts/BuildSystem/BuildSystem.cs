using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class BuildSystem : MonoBehaviour
{
    public static BuildSystem Instance { get; private set; }

    // Objects
    [SerializeField] BuildingData[] buildings;
    private GameObject currBuilding;
    private GameObject currPreviewPrefab;
    private GameObject buildingPreviewObj;

    private List<BuildingSaveData> buildingSaves = new List<BuildingSaveData>();

    // Actions
    [SerializeField] private InputActionAsset inputs;
    private MouseRay mouseRay;
    private InputAction moveAction;
    private InputAction buildAction;

    // Variables
    private Vector2 mousePos;
    private Vector2 currPreviewPos;
    private int cellStep = 1;
    private bool isUsingKeyboard;
    private bool deleteBtn;
    public bool placeBtn;

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

    // Buildings management
    private void SetBuilding(InputAction.CallbackContext context)
    {
        if (currBuilding == null) return;

        Cell currCell = (mouseRay != null) ? mouseRay.GetMouseCell() : null;
        if (currCell == null) return;

        if (!currCell.isEmpty)
        {
            Debug.Log($"Cell [{currCell}] is not empty");
            return;
        }

        Vector2 placePos;
        if (!isUsingKeyboard)
            placePos = currCell.transform.position;
        else
            placePos = currPreviewPos;

        Building newBuilding = Instantiate(currBuilding, placePos, Quaternion.identity).GetComponent<Building>();
        currCell.SetBuilding(newBuilding);
        newBuilding.cell = currCell;

        BuildingSaveData data = new BuildingSaveData
        {
            id = currBuilding.name,
            position = placePos
        };
        buildingSaves.Add(data);
    }
    
    public void OnDeleteBtn()
    {
        deleteBtn = true;
        buildingPreviewObj.SetActive(false);

        placeBtn = false;
        buildAction?.Disable();
    }
    private void DeleteBuilding()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                var building = hit.collider.GetComponentInParent<Building>();

                if (building != null)
                {
                    Destroy(building.gameObject);
                    Debug.Log("Building DELETED");
                }
            }
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            deleteBtn = false;
        }
    }

    public void SwitchPlaceBtn()
    {
        placeBtn = !placeBtn;
        if (!placeBtn) buildAction?.Disable();
        else buildAction?.Enable();
    }

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


        BuildingData selectedBuilding = buildings[currBuildingIdx];

        currBuilding = selectedBuilding.building;
        currPreviewPrefab = selectedBuilding.buildingPreview;

        if (buildingPreviewObj != null) 
            Destroy(buildingPreviewObj);

        if (currPreviewPrefab != null)
            buildingPreviewObj = Instantiate(
                currPreviewPrefab,
                currPreviewPos,
                Quaternion.identity
            );

        buildingPreviewObj.SetActive(true);

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            placeBtn = false;
            if (buildingPreviewObj != null)
                buildingPreviewObj.SetActive(false);
        }
    }

    // Save - Load builgns
    public void Save()
    {
        SaveManager.Instance.SaveAll(buildingSaves);
    }    

    public void Load()
    {
        List<BuildingSaveData> loadedBuildings = SaveManager.Instance.LoadAll();

        foreach(var b in loadedBuildings)
        {
            var buildingId = Array.Find(buildings, building => building.id == b.id);

            if (buildingId.building != null)
                Instantiate(
                    buildingId.building,
                    b.position,
                    Quaternion.identity
                );
        }
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
        mousePos = new Vector2(mouseWorld.x, mouseWorld.y);

        // switch preview move
        Vector2 mouseMove = Mouse.current.delta.ReadValue();
        if (mouseMove.sqrMagnitude > 0.01f) isUsingKeyboard = false;
        if (!isUsingKeyboard && buildingPreviewObj != null)
        {
            MousePreviewDrag(mousePos);
            currPreviewPos = mousePos;
        }

        // handle delete mode
        if (deleteBtn)
            DeleteBuilding();

        // handle placement mode
        if (placeBtn)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                placeBtn = false;
                buildAction?.Disable();
                if (buildingPreviewObj != null)
                    buildingPreviewObj.SetActive(false);
            }
        }
    }
}