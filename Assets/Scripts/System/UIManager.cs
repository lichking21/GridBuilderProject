using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button[] buildings;
    [SerializeField] private Button placeBtn;
    [SerializeField] private Button deleteBtn;

    void Start()
    {
        TryInit();

        if (BuildSystem.Instance == null)
            SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryInit();

        if (BuildSystem.Instance != null)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void TryInit()
    {
        if (BuildSystem.Instance == null)
        {
            Debug.Log("Couldn't find BuildSystem");
            return;
        }

        if (buildings.Length > 0) buildings[0].onClick.AddListener(() => TrySelectBuilding(0));
        if (buildings.Length > 0) buildings[1].onClick.AddListener(() => TrySelectBuilding(1));
        if (buildings.Length > 0) buildings[2].onClick.AddListener(() => TrySelectBuilding(2));


        placeBtn.onClick.AddListener(() => BuildSystem.Instance.SwitchPlaceBtn());

        Debug.Log("ButtonListeners added");
    }

    private void TrySelectBuilding(int idx)
    {
        if (!BuildSystem.Instance.placeBtn)
            return;

        BuildSystem.Instance.SelectBuilding(idx);
    } 
}
