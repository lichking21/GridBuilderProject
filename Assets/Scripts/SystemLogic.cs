using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemLogic : MonoBehaviour
{
    private string mainScene = "MainScene";

    [SerializeField] private GameObject uiManager;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (FindAnyObjectByType<UIManager>() == null && uiManager != null)
        {
            var UI = Instantiate(uiManager);
            DontDestroyOnLoad(UI);
            Debug.Log("UIManager instantiated and moved to DontDestroyOnLoad");
        }
        else if (uiManager == null) Debug.Log("UIManager prefab is not assigned");
        else Debug.Log("UIManager exists or prefab is missing");
    }

    void Start()
    {
        SceneManager.LoadSceneAsync(mainScene);
    }
}
