using UnityEngine;
using UnityEngine.SceneManagement;

// =====================================================================
// MenuManager.cs
// -----------------------------------------------------------------
// Bu script "MenuManager" obyektiga qo'yiladi va barcha menyu
// panellarini (MainMenuPanel, PauseMenu, YouDie, Escaped, GameMenu)
// hamda ulardagi tugmalarni (Play, Quit, Resume va h.k.) boshqaradi.
//
// ISHLASH PRINSIPI:
//  - Har doim faqat BITTA panel ko'rinadi, qolganlari o'chirilgan bo'ladi
//  - Har bir "Show..." funksiyasi avval hammasini o'chiradi,
//    keyin kerakli panelni yoqadi
//  - Tugmalar Inspector'dagi OnClick() orqali shu funksiyalarni chaqiradi
// =====================================================================

public class MenuManager : MonoBehaviour
{
    [Header("Panellar (Hierarchy'dan mos obyektlarni shu yerga tashlang)")]
    [SerializeField] private GameObject mainMenuPanel;   // "MainMenuPanel"
    [SerializeField] private GameObject pauseMenuPanel;  // "PauseMenu"
    [SerializeField] private GameObject youDiePanel;      // "YouDie"
    [SerializeField] private GameObject escapedPanel;     // "Escaped"
    [SerializeField] private GameObject gameMenuPanel;    // "GameMenu" (o'yin ichidagi HUD)

    // Hozir biror menyu ochiqmi yo'qmi - shu bo'yicha cursor holatini har freym tekshirib turamiz
    private bool isMenuCurrentlyOpen = true;

    private void Start()
    {
        // O'yin boshlanganda faqat asosiy menyu ko'rinadi
        ShowMainMenu();
    }

    // (LateUpdate olib tashlandi, chunki u boshqa panellar ochilganda ham sichqonchani majburan yashirib qo'yayotgan edi)

    // ---------------------------------------------------------------
    // SICHQONCHA (CURSOR) HOLATINI BOSHQARISH
    // ---------------------------------------------------------------
    // menuOpen = true  -> sichqoncha ko'rinadi va erkin harakatlanadi (tugmalarni bosish uchun)
    // menuOpen = false -> sichqoncha yashiriladi va ekran markaziga qulflanadi (FPS kamera uchun)
    private void SetCursorState(bool menuOpen)
    {
        isMenuCurrentlyOpen = menuOpen; // Holatni eslab qolamiz, LateUpdate shundan foydalanadi

        if (menuOpen)
        {
            Cursor.lockState = CursorLockMode.None; // Erkin harakat
            Cursor.visible = true;                  // Ko'rinadi
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Ekran markaziga qulflanadi
            Cursor.visible = false;                    // Yashiriladi
        }
    }

    // ---------------------------------------------------------------
    // PANELLARNI ALMASHTIRISH FUNKSIYALARI
    // ---------------------------------------------------------------

    // Barcha panellarni o'chirib qo'yuvchi yordamchi funksiya.
    // "if (x != null)" tekshiruvi bor - shuning uchun agar biror
    // panel Inspector'da hali ulanmagan bo'lsa ham xato chiqmaydi,
    // faqat o'sha panel e'tiborsiz qoldiriladi.
    private void CloseAllPanels()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (youDiePanel != null) youDiePanel.SetActive(false);
        if (escapedPanel != null) escapedPanel.SetActive(false);
        if (gameMenuPanel != null) gameMenuPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        Time.timeScale = 1f; // Vaqtni me'yoriga qaytaramiz
        CloseAllPanels();
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        SetCursorState(true); // Menyu - sichqoncha ko'rinadi
    }


    public void ShowGameMenu()
    {
        Time.timeScale = 1f;
        CloseAllPanels();
        if (gameMenuPanel != null) gameMenuPanel.SetActive(true);
        SetCursorState(false); // O'yin ichida - sichqoncha yashirinadi va qulflanadi
    }

    public void ShowPauseMenu()
    {
        Time.timeScale = 0f; // O'yinni to'xtatib turamiz
        CloseAllPanels();
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        SetCursorState(true); // Menyu - sichqoncha ko'rinadi
    }

    public void ShowYouDie()
    {
        Time.timeScale = 0f;
        CloseAllPanels();
        if (youDiePanel != null) youDiePanel.SetActive(true);
        SetCursorState(true);
    }

    public void ShowEscaped()
    {
        Time.timeScale = 0f;
        CloseAllPanels();
        if (escapedPanel != null) escapedPanel.SetActive(true);
        SetCursorState(true);
    }

    // ---------------------------------------------------------------
    // TUGMALAR UCHUN FUNKSIYALAR (Inspector'da OnClick() ga ulanadi)
    // ---------------------------------------------------------------

    // === MainMenuPanel ichidagi tugmalar ===

    // "Play" tugmasi bosilganda
    public void PlayButton()
    {
        ShowGameMenu(); // O'yin HUD paneli ko'rinadi, o'yin boshlanadi
    }

    // "Quit" tugmasi bosilganda (MainMenuPanel ichida)
    public void QuitButton()
    {
        Debug.Log("O'yindan chiqildi!");
        Application.Quit();

#if UNITY_EDITOR
        // Unity Editor'da "Application.Quit()" ishlamaydi,
        // shuning uchun Play rejimini shu qator to'xtatadi (faqat Editor uchun)
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // === PauseMenu ichidagi tugmalar ===

    // "ResumeButton" bosilganda - o'yinga qaytish
    public void ResumeButton()
    {
        ShowGameMenu();
    }

    // === YouDie ichidagi tugmalar ===

    // "RestartButton" bosilganda - sahnani qaytadan yuklaydi
    public void RestartButton()
    {
        Time.timeScale = 1f; // Vaqtni tiklamasak, qayta yuklangan sahna ham to'xtab qoladi
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // === Escaped ichidagi tugmalar ===

    // "Countine" (Continue) tugmasi bosilganda
    public void ContinueButton()
    {
        // Keyingi bosqich/level logikasi shu yerga yoziladi.
        // Hozircha shunchaki o'yin menyusiga qaytaramiz:
        ShowGameMenu();
    }

    // "QuiteButton" (Quit) - Escaped panelidagi chiqish tugmasi
    // (yuqoridagi QuitButton() bilan bir xil ishlaydi, shuning uchun shunchaki uni chaqiramiz)
    public void QuiteButton()
    {
        QuitButton();
    }

    // === Barcha panellarda takrorlanadigan umumiy tugma ===

    // "MainMenuButton" - PauseMenu, YouDie, Escaped panellarining
    // barchasida bor, hammasi shu bitta funksiyaga ulanadi
    public void MainMenuButton()
    {
        ShowMainMenu();
    }

    private void Update()
    {
        // "P" tugmasi bosilganda pauza menyusini ochamiz
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowPauseMenu(); // Bu funksiya avtomatik ravishda o'yinni to'xtatadi va sichqonchani ko'rsatadi
        }
    }
}
