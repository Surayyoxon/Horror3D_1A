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
            numberButtons[i].onClick.AddListener(() => AddDigit(digit));
        }

        clearButton.onClick.AddListener(ClearInput);
        enterButton.onClick.AddListener(SubmitCode);
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
        codeText.text = currentInput;
    }

    public void SubmitCode()
    {
        if (currentInput == correctCode)
            OnCorrectCode();
        else
            OnWrongCode();
    }

    void OnCorrectCode()
    {
        ClosePanel();
        GameManager.Instance.OnPanel01Activated();
    }

    void OnWrongCode()
    {
        wrongAttempts++;
        ClearInput();

        if (screamer != null)
            screamer.TriggerScreamer(wrongAttempts);
    }

    public void ClosePanel()
    {
        codePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}