using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemLogic : MonoBehaviour
{
    private string mainScene = "MainScene";

    [SerializeField] private GameObject uiManager;
    [SerializeField] private GameObject gridManager;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject saveManager;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // UI
        if (FindAnyObjectByType<UIManager>() == null && uiManager != null)
        {
            var UI = Instantiate(uiManager);

            DontDestroyOnLoad(UI);
            Debug.Log("UIManager instantiated and moved to DontDestroyOnLoad");
        }
        else if (uiManager == null) Debug.Log("UIManager prefab is not assigned");
        else Debug.Log("UIManager exists or prefab is missing");

        // Grid
        if (FindAnyObjectByType<GridManager>() == null && gridManager != null)
        {
            var grid = Instantiate(gridManager);

            DontDestroyOnLoad(grid);
            Debug.Log("GridManager instantiated and moved to DontDestroyOnLoad");
        }
        else if (gridManager == null) Debug.Log("GridManager prefab is not assigned");
        else Debug.Log("GridManager exists or prefab is missing");

        // Camera
        if (FindAnyObjectByType<Camera>() == null && cam != null)
        {
            var camera = Instantiate(cam);

            DontDestroyOnLoad(camera);
            Debug.Log("Camera instantiated and moved to DontDestroyOnLoad");
        }
        else if (gridManager == null) Debug.Log("Camera prefab is not assigned");
        else Debug.Log("Camera exists or prefab is missing");

        // Saves
        if (FindAnyObjectByType<SaveManager>() == null && cam != null)
        {
            var saves = Instantiate(saveManager);

            DontDestroyOnLoad(saves);
            Debug.Log("SaveManager instantiated and moved to DontDestroyOnLoad");
        }
        else if (saveManager == null) Debug.Log("SaveManager prefab is not assigned");
        else Debug.Log("SaveManager exists or prefab is missing");
    }

    void Start()
    {
        SceneManager.LoadSceneAsync(mainScene);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BuildSystem.Instance.Load();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnApplicationQuit()
    {
        BuildSystem.Instance.Save();      
    }
}
