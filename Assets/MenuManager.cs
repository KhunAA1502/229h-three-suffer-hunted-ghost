using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject mainMenuPanel;
    ///[SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button startButton;
    //[SerializeField] private Button settingsButton;
    //[SerializeField] private Button quitButton;
    //[SerializeField] private Button backButton;

    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "GameScene";

    private void Start()
    {
        // ตั้งค่าปุ่มต่างๆ
        startButton.onClick.AddListener(StartGame);
       //settingsButton.onClick.AddListener(ShowSettings);
        //quitButton.onClick.AddListener(QuitGame);
        //backButton.onClick.AddListener(ShowMainMenu);

        // เริ่มต้นแสดงหน้าเมนูหลัก
        ShowMainMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    /*public void ShowSettings()
    {
        mainMenuPanel.SetActive(false);
        //settingsPanel.SetActive(true);
    }*/

    public void ShowMainMenu()
    {
        //settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    /*public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }*/
}
