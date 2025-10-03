using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemLogic : MonoBehaviour
{
    private string mainScene = "MainScene";

    [SerializeField] private GameObject uiManager;
    [SerializeField] private GameObject gridManager;
    [SerializeField] private Camera cameraPrefab;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        Camera cam = Instantiate(cameraPrefab);
        DontDestroyOnLoad(cam);

        if (FindAnyObjectByType<UIManager>() == null && uiManager != null)
        {
            var UI = Instantiate(uiManager);

            DontDestroyOnLoad(UI);
            Debug.Log("UIManager instantiated and moved to DontDestroyOnLoad");
        }
        else if (uiManager == null) Debug.Log("UIManager prefab is not assigned");
        else Debug.Log("UIManager exists or prefab is missing");

        if (FindAnyObjectByType<GridManager>() == null && gridManager != null)
        {
            var grid = Instantiate(gridManager);
            grid
                .GetComponent<GridManager>()
                .InitCam(cam);

            DontDestroyOnLoad(grid);
            Debug.Log("GridManager instantiated and moved to DontDestroyOnLoad");
        }
        else if (gridManager == null) Debug.Log("GridManager prefab is not assigned");
        else Debug.Log("GridManager exists or prefab is missing");
    }

    void Start()
    {
        SceneManager.LoadSceneAsync(mainScene);
    }
}
