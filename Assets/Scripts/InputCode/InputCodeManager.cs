using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputCodeManager : MonoBehaviour
{
    [Header("Kod")]
    public string correctCode = "123456";
    private string currentInput = "";
    public int maxDigits = 6;

    [Header("UI Referenslar")]
    public GameObject codePanel;
    public TMP_Text codeText;
    public Button[] numberButtons;
    public Button clearButton;
    public Button enterButton;

    [Header("Bog'liq tizimlar")]
    public ScreamerController screamer;

    private int wrongAttempts = 0;

    private void Awake()
    {
        for (int i = 0; i < numberButtons.Length; i++)
        {
            int digit = i;
            numberButtons[i].onClick.RemoveAllListeners();
            numberButtons[i].onClick.AddListener(() => AddDigit(digit));
        }

        if (clearButton != null)
        {
            clearButton.onClick.RemoveAllListeners();
            clearButton.onClick.AddListener(ClearInput);
        }

        if (enterButton != null)
        {
            enterButton.onClick.RemoveAllListeners();
            enterButton.onClick.AddListener(SubmitCode);
        }
    }

    private void OnEnable()
    {
        ClearInput();
    }

    // YANGI FUNKSIYA: Kodni o'rnatadi va Posterdagi raqamni ham o'zi yangilaydi!
    public void SetNewCode(string newCode)
    {
        correctCode = newCode;
        Debug.Log("<color=cyan>[InputCodeManager]</color> Yangi kod qabul qilindi: " + correctCode);

        // Poster textini ham shu yerning o'zida yangilab qo'yamiz
        if (ObjectiveUIManager.Instance != null)
        {
            ObjectiveUIManager.Instance.SetPosterCode(correctCode);
        }
        else
        {
            Debug.LogWarning("[InputCodeManager] ObjectiveUIManager topilmadi!");
        }
    }

    public void AddDigit(int digit)
    {
        if (currentInput.Length >= maxDigits) return;
        currentInput += digit.ToString();
        UpdateDisplay();
    }

    public void ClearInput()
    {
        currentInput = "";
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (codeText != null)
        {
            codeText.text = currentInput;
        }
    }

    public void SubmitCode()
    {
        if (currentInput == correctCode)
        {
            OnCorrectCode();
        }
        else
        {
            OnWrongCode();
        }
    }

    void OnCorrectCode()
    {
        Debug.Log("<color=green>[InputCodeManager]</color> KOD TO'G'RI!");
        ClosePanel();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPanel01Activated();
        }
    }

    void OnWrongCode()
    {
        Debug.LogWarning("<color=red>[InputCodeManager]</color> XATO KOD!");
        wrongAttempts++;
        ClearInput();

        if (screamer != null)
        {
            screamer.TriggerScreamer(wrongAttempts);
        }
    }

    public void ClosePanel()
    {
        if (codePanel != null)
        {
            codePanel.SetActive(false);
        }
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}