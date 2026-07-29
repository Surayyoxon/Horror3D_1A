using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject loadingMenu;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject youDiedMenu;
    [SerializeField] private GameObject escapedMenu;

    [Header("UI Canvas (Objective)")]
    // Ekranda menyu turganda topshiriq yozuvi ko'rinmasligi uchun:
    [SerializeField] private GameObject objectiveCanvas;

    [Header("Player Scripts Control")]
    // Player obyektingizdagi PlayerMovement va MouseLook skriptlarini shu yerga ulaysiz:
    [SerializeField] private MonoBehaviour playerMovementScript;
    [SerializeField] private MonoBehaviour playerMouseLookScript;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // O'yin boshlanganda faqat MainMenu ko'rsatiladi
        ShowMainMenu();
    }

    private void Update()
    {
        // ESC tugmasi bosilganda pauza menyusini ochish/yopish
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance != null &&
                GameManager.Instance.currentStep != GameManager.GameStep.Escaped &&
                GameManager.Instance.currentStep != GameManager.GameStep.Died)
            {
                if (pauseMenu != null && pauseMenu.activeSelf)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }

    #region Panel Control
    private void CloseAllPanels()
    {
        if (mainMenu) mainMenu.SetActive(false);
        if (loadingMenu) loadingMenu.SetActive(false);
        if (gameMenu) gameMenu.SetActive(false);
        if (pauseMenu) pauseMenu.SetActive(false);
        if (youDiedMenu) youDiedMenu.SetActive(false);
        if (escapedMenu) escapedMenu.SetActive(false);
    }

    private void OpenPanel(GameObject panel)
    {
        CloseAllPanels();
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    private void SetCursorState(bool isMenuOpen)
    {
        if (isMenuOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void SetPlayerControl(bool state)
    {
        if (playerMovementScript != null) playerMovementScript.enabled = state;
        if (playerMouseLookScript != null) playerMouseLookScript.enabled = state;
    }
    #endregion

    #region Button Actions
    public void ShowMainMenu()
    {
        Time.timeScale = 0f; // Menyu turganda o'yin to'xtab turadi
        SetCursorState(true);
        OpenPanel(mainMenu);

        if (objectiveCanvas != null) objectiveCanvas.SetActive(false);
        SetPlayerControl(false); // Player harakati va kamerasini o'chirish
    }

    public void PlayButton()
    {
        Time.timeScale = 1f; // O'yin vaqtini yurgizish
        SetCursorState(false); // Sichqoncha kursorini yashirish va bloklash
        OpenPanel(gameMenu); // HUD (interfeys)ni ochish

        if (objectiveCanvas != null) objectiveCanvas.SetActive(true);
        SetPlayerControl(true); // Player harakati va kamerasini yoqish
    }

    public void ContinueButton()
    {
        PlayButton();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        SetCursorState(true);
        OpenPanel(pauseMenu);

        SetPlayerControl(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        SetCursorState(false);
        OpenPanel(gameMenu);

        SetPlayerControl(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButton()
    {
        Debug.Log("O'yindan chiqildi!");
        Application.Quit();
    }

    public void ShowEscapedMenu()
    {
        Time.timeScale = 0f;
        SetCursorState(true);
        OpenPanel(escapedMenu);

        SetPlayerControl(false);
    }

    public void ShowYouDiedMenu()
    {
        Time.timeScale = 0f;
        SetCursorState(true);
        OpenPanel(youDiedMenu);

        SetPlayerControl(false);
    }
    #endregion
}