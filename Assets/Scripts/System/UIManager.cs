using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button[] buildings;
    [SerializeField] private Button placeBtn;
    [SerializeField] private Button deleteBtn;
    [SerializeField] private GameObject buildingsPanel;

    void Start()
    {
        Init();

        if (BuildSystem.Instance == null)
            SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init();

        if (BuildSystem.Instance != null)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Init()
    {
        if (BuildSystem.Instance == null)
        {
            return;
        }

        if (buildings.Length > 0) buildings[0].onClick.AddListener(() => SelectBuilding(0));
        if (buildings.Length > 0) buildings[1].onClick.AddListener(() => SelectBuilding(1));
        if (buildings.Length > 0) buildings[2].onClick.AddListener(() => SelectBuilding(2));

        placeBtn.onClick.AddListener(() => PlaceBtn());
        deleteBtn.onClick.AddListener(() => BuildSystem.Instance.OnDeleteBtn());

        Debug.Log("ButtonListeners added");
    }

    private void PlaceBtn()
    {
        BuildSystem.Instance.SwitchPlaceBtn();

        if (BuildSystem.Instance.placeBtn)
            buildingsPanel.SetActive(true);
        else 
            buildingsPanel.SetActive(false);
    }

    private void SelectBuilding(int idx)
    {
        if (!BuildSystem.Instance.placeBtn)
            return;

        BuildSystem.Instance.SelectBuilding(idx);
    } 
}
