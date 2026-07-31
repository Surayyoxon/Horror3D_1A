using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("Menu Panellari (Inspector'da tashlang)")]
    public GameObject mainMenu;
    public GameObject loadingMenu;
    public GameObject gameMenu;
    public GameObject pauseMenu;
    public GameObject youDiedMenu;
    public GameObject escapedMenu;

    [Header("Sozlamalar")]
    public float minLoadingTime = 1.5f; // Loading ekrani kamida shuncha ko'rinadi

    [Header("Main Menu tugmalari")]
    public Button continueButton; // Save bo'lmasa o'chiq turadi (ixtiyoriy)

    private bool isPaused = false;
    private bool inGame = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        ShowMainMenu();
    }

    private void Update()
    {
        // ESC bilan pauza qilish / davom ettirish
        if (inGame && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    // Hamma panellarni yopib, faqat bittasini ochadi
    private void ShowOnly(GameObject panel)
    {
        mainMenu.SetActive(panel == mainMenu);
        loadingMenu.SetActive(panel == loadingMenu);
        gameMenu.SetActive(panel == gameMenu);
        pauseMenu.SetActive(panel == pauseMenu);
        youDiedMenu.SetActive(panel == youDiedMenu);
        escapedMenu.SetActive(panel == escapedMenu);
    }

    private void SetCursor(bool free)
    {
        Cursor.lockState = free ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = free;
    }

    // ================= MAIN MENU =================

    public void ShowMainMenu()
    {
        inGame = false;
        isPaused = false;
        Time.timeScale = 0f;
        SetCursor(true);
        ShowOnly(mainMenu);

        // Save bo'lmasa Continue tugmasi bosilmaydi
        if (continueButton != null)
            continueButton.interactable = PlayerPrefs.HasKey("HasSave");
    }

    // Play tugmasi — yangi o'yin
    public void PlayGame()
    {
        PlayerPrefs.SetInt("HasSave", 1);
        PlayerPrefs.Save();
        StartCoroutine(LoadGameScene());
    }

    // Continue tugmasi — o'yinni davom ettirish
    public void ContinueGame()
    {
        StartCoroutine(LoadGameScene());
    }

    // Quit tugmasi — o'yindan chiqish
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // ================= LOADING =================

    // Sahna yuklanmaydi — hamma narsa shu sahnaning o'zida,
    // shunchaki Loading ko'rsatib, o'yinni boshlaymiz
    private IEnumerator LoadGameScene()
    {
        ShowOnly(loadingMenu);

        yield return new WaitForSecondsRealtime(minLoadingTime);

        StartGameplay();
    }

    // O'yinni boshlash — GameMenu ochiladi, vaqt yuradi, cursor qulflanadi
    private void StartGameplay()
    {
        inGame = true;
        isPaused = false;
        Time.timeScale = 1f;
        SetCursor(false);
        ShowOnly(gameMenu);
    }

    // ================= PAUSE MENU =================

    // PauseButton tugmasi
    public void PauseGame()
    {
        if (!inGame) return;

        isPaused = true;
        Time.timeScale = 0f;
        SetCursor(true);
        ShowOnly(pauseMenu);
    }

    // Resume tugmasi
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SetCursor(false);
        ShowOnly(gameMenu);
    }

    // ================= YOU DIED MENU =================

    // Buni PlayerHealth.Die() ichidan chaqiring: MenuManager.Instance.ShowYouDiedMenu();
    public void ShowYouDiedMenu()
    {
        inGame = false;
        Time.timeScale = 0f;
        SetCursor(true);
        ShowOnly(youDiedMenu);
    }

    // Restart tugmasi — sahnani qayta yuklaydi
    public void RestartGame()
    {
        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        ShowOnly(loadingMenu);

        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        while (!op.isDone)
            yield return null;

        yield return new WaitForSecondsRealtime(minLoadingTime);
        StartGameplay();
    }

    // ================= ESCAPED MENU =================

    // Buni GameManager.OnEscape() ichidan chaqiring: MenuManager.Instance.ShowEscapedMenu();
    public void ShowEscapedMenu()
    {
        inGame = false;
        Time.timeScale = 0f;
        SetCursor(true);
        ShowOnly(escapedMenu);
    }

    // EscapedMenu'dagi Continue tugmasi — o'yinni davom ettirish
    public void ContinueAfterEscape()
    {
        StartGameplay();
    }
}
