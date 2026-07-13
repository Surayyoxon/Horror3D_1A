using UnityEngine;
using TMPro;
using System.Collections;

public class ObjectiveUIManager : MonoBehaviour
{
    public static ObjectiveUIManager Instance;

    [Header("Panels")]
    public GameObject exitTriggerPanel;
    public GameObject interactionPanel;
    public GameObject generatorPanel;
    public GameObject reminderPanel;

    [Header("Texts")]
    public TMP_Text exitTriggerText;
    public TMP_Text interactionText;
    public TMP_Text generatorText;
    public TMP_Text reminderText;

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;

    [Header("Audio")]
    public AudioSource typingAudio;
    public AudioClip typingClip;

    private Coroutine typingRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // O'yin boshida hamma panellarni yopamiz
        exitTriggerPanel.SetActive(false);
        interactionPanel.SetActive(false);
        generatorPanel.SetActive(false);
        reminderPanel.SetActive(false);
    }

    #region Exit & Main Objective Panel

    public void ShowExitObjective(string message)
    {
        exitTriggerPanel.SetActive(true);

        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        typingRoutine = StartCoroutine(TypeMessage(exitTriggerText, message));
    }

    public void HideExitObjective()
    {
        exitTriggerPanel.SetActive(false);

        // Player triggerdan chiqsa yozishni ham, audioni ham darhol to'xtatamiz
        if (typingRoutine != null)
        {
            StopCoroutine(typingRoutine);
            typingRoutine = null;
        }

        if (typingAudio != null && typingAudio.isPlaying)
        {
            typingAudio.Stop();
        }
    }

    #endregion

    #region Interaction (E tugmasi)

    public void ShowInteraction(string action)
    {
        interactionPanel.SetActive(true);
        interactionText.text = $"Press <color=yellow><b>E</b></color> to {action}";
    }

    public void HideInteraction()
    {
        interactionPanel.SetActive(false);
    }

    #endregion

    #region Generator Info

    public void ShowGeneratorInfo(int currentFuse, int requiredFuse)
    {
        generatorPanel.SetActive(true);
        UpdateFuseUI(currentFuse, requiredFuse);
    }

    public void UpdateFuseUI(int currentFuse, int requiredFuse)
    {
        generatorText.text = $"<color=red>GENERATOR OFFLINE</color>\n\nThe generator requires {requiredFuse} Fuses.\n\nFuses Found: <color=yellow>{currentFuse}/{requiredFuse}</color>";
    }

    public void HideGeneratorInfo()
    {
        generatorPanel.SetActive(false);
    }

    #endregion

    #region Dynamic Reminder (Ekranning burchagidagi doimiy eslatma)

    // AYNAN SHU ERGA JOYLASHTIRILDI!
    public void SetReminder(string reminder)
    {
        reminderPanel.SetActive(true);
        reminderText.text = reminder;
    }

    public void HideReminder()
    {
        reminderPanel.SetActive(false);
    }

    #endregion

    #region Typing Effect Coroutine (Butun audio uchun)

    IEnumerator TypeMessage(TMP_Text textUI, string message)
    {
        textUI.text = "";

        // Matn yozilishi boshlanganda audioni bir marta chalish
        if (typingAudio != null && typingClip != null)
        {
            typingAudio.clip = typingClip;
            typingAudio.loop = false;
            typingAudio.volume = 0.5f;
            typingAudio.Play();
        }

        foreach (char c in message)
        {
            textUI.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Matn yakunlangach audioni to'xtatish
        if (typingAudio != null && typingAudio.isPlaying)
        {
            typingAudio.Stop();
        }

        typingRoutine = null;
    }

    #endregion
}