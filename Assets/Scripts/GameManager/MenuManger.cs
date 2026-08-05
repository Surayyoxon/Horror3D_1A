using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Panellar (Hierarchy'dan mos obyektlarni shu yerga tashlang)")]
    [SerializeField] private GameObject mainMenuPanel;   // "MainMenuPanel"
    [SerializeField] private GameObject pauseMenuPanel;  // "PauseMenu"
    [SerializeField] private GameObject youDiePanel;      // "YouDie"
    [SerializeField] private GameObject escapedPanel;     // "Escaped"
    [SerializeField] private GameObject gameMenuPanel;    // "GameMenu" (o'yin ichidagi HUD)

    public bool isPuzzleOpen = false;
    private bool isMenuCurrentlyOpen = true;

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
        ShowMainMenu();
    }

    private void LateUpdate()
    {
        if (isPuzzleOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            SetCursorState(isMenuCurrentlyOpen);
        }
    }

    private void SetCursorState(bool menuOpen)
    {
        isMenuCurrentlyOpen = menuOpen;

        if (menuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void CloseAllPanels()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (youDiePanel != null) youDiePanel.SetActive(false);
        if (escapedPanel != null) escapedPanel.SetActive(false);
        if (gameMenuPanel != null) gameMenuPanel.SetActive(false);
    }

    private void ToggleObjectiveCanvas(bool show)
    {
        if (ObjectiveUIManager.Instance != null)
        {
            Canvas objCanvas = ObjectiveUIManager.Instance.GetComponent<Canvas>();
            if (objCanvas != null)
            {
                objCanvas.enabled = show;
            }
        }
    }

    public void ShowMainMenu()
    {
        Time.timeScale = 1f;
        CloseAllPanels();
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        SetCursorState(true);
        ToggleObjectiveCanvas(false);
    }


    public void ShowGameMenu()
    {
        Time.timeScale = 1f;
        CloseAllPanels();
        if (gameMenuPanel != null) gameMenuPanel.SetActive(true);
        SetCursorState(false);
        ToggleObjectiveCanvas(true);
    }

    public void ShowPauseMenu()
    {
        Debug.Log("ShowPauseMenu chaqirildi!");
        Time.timeScale = 0f;
        CloseAllPanels();
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        SetCursorState(true);
        ToggleObjectiveCanvas(false);
    }

    public void ShowYouDie()
    {
        Time.timeScale = 0f;
        CloseAllPanels();
        if (youDiePanel != null) youDiePanel.SetActive(true);
        SetCursorState(true);
        ToggleObjectiveCanvas(false);
    }

    public void ShowEscaped()
    {
        Time.timeScale = 0f;
        CloseAllPanels();
        if (escapedPanel != null) escapedPanel.SetActive(true);
        SetCursorState(true);
        ToggleObjectiveCanvas(false);
    }

    // === TUGMALAR UCHUN FUNKSIYALAR ===

   public void PlayButton()
{
    ShowGameMenu();

    if (GameManager.Instance != null && GameManager.Instance.currentStep == GameManager.GameStep.FindExit)
    {
        GameManager.Instance.StartGameObjectives();
    }
}

    public void QuitButton()
    {
        Debug.Log("O'yindan chiqildi!");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ResumeButton()
    {
        ShowGameMenu();
    }


    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ContinueButton()
    {
        ShowGameMenu();
    }

    public void QuiteButton()
    {
        QuitButton();
    }

    public void MainMenuButton()
    {
        ShowMainMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMenuCurrentlyOpen && !isPuzzleOpen)
            {
                SetCursorState(true); // Faqat kursorni ko'rsatadi, panel ochmaydi
            }
        }
    }
}
