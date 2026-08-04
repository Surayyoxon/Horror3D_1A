using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Panellar")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject youDiePanel;
    [SerializeField] private GameObject pausaPanel;
    [SerializeField] private GameObject escapedPanel;
    [SerializeField] private GameObject gameMenuPanel;

    private void Start()
    {
        // O'yin boshlanganda asosiy menuni ko'rsatamiz
        ShowMainMenu();
    }

    // --- PANELLARNI ALASHTIRISH METODLARI ---

    // Barcha panellarni o'chirib qo'yuvchi yordamchi funksiya
    private void CloseAllPanels()
    {
        mainMenuPanel.SetActive(false);
        youDiePanel.SetActive(false);
        pausaPanel.SetActive(false);
        escapedPanel.SetActive(false);
        gameMenuPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        Time.timeScale = 1f; // Vaqtni me'yoriga keltirish
        CloseAllPanels();
        mainMenuPanel.SetActive(true);
    }

    public void ShowGameMenu()
    {
        Time.timeScale = 1f;
        CloseAllPanels();
        gameMenuPanel.SetActive(true);
    }

    public void ShowPause()
    {
        Time.timeScale = 0f; // O'yinni to'xtatib turish
        CloseAllPanels();
        pausaPanel.SetActive(true);
    }

    public void ShowYouDie()
    {
        Time.timeScale = 0f;
        CloseAllPanels();
        youDiePanel.SetActive(true);
    }

    public void ShowEscaped()
    {
        Time.timeScale = 0f;
        CloseAllPanels();
        escapedPanel.SetActive(true);
    }

    // --- TUGMALAR UCHUN METODLAR (OnClick) ---

    // 1. MainMenu tugmalari
    public void PlayButton()
    {
        ShowGameMenu(); // O'yinni boshlash (HUD paneli namoyish etiladi)
    }

    // 2. YouDie tugmalari
    public void RestartButton()
    {
        Time.timeScale = 1f;
        // Hozirgi sahna qayta yuklanadi
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 3. Pausa tugmalari
    public void ResumeButton()
    {
        ShowGameMenu(); // O'yinga qaytish
    }

    // 4. Escaped tugmalari
    public void ContinueButton()
    {
        // Keyingi bosqichga o'tish yoki davom ettirish mantiqini shu yerga yozishingiz mumkin
        ShowGameMenu();
    }

    // Common: Barcha panellardagi MainMenu va Quit tugmalari uchun
    public void MainMenuButton()
    {
        ShowMainMenu();
    }

    public void QuitButton()
    {
        Debug.Log("O'yindan chiqildi!");
        Application.Quit(); // O'yindan chiqish
    }
}
