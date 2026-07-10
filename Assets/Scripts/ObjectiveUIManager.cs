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

    [Header("Typing")]
    public float typingSpeed = 0.03f;

    [Header("Audio")]
    public AudioSource typingAudio;
    public AudioClip typingClip;

    Coroutine typingRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        exitTriggerPanel.SetActive(false);
        interactionPanel.SetActive(false);
        generatorPanel.SetActive(false);
        reminderPanel.SetActive(false);
    }

    #region Exit Panel

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
    }

    #endregion

    #region Interaction

    public void ShowInteraction(string action)
    {
        interactionPanel.SetActive(true);

        interactionText.text =
            $"Press <color=yellow><b>E</b></color> to {action}";
    }

    public void HideInteraction()
    {
        interactionPanel.SetActive(false);
    }

    #endregion

    #region Generator

    public void ShowGeneratorInfo(int currentFuse, int requiredFuse)
    {
        generatorPanel.SetActive(true);

        generatorText.text =
$@"GENERATOR OFFLINE

The generator requires {requiredFuse} Fuses.

Fuse : {currentFuse}/{requiredFuse}";
    }

    public void UpdateFuseUI(int currentFuse, int requiredFuse)
    {
        generatorText.text =
$@"GENERATOR OFFLINE

The generator requires {requiredFuse} Fuses.

Fuse : {currentFuse}/{requiredFuse}";
    }

    public void HideGeneratorInfo()
    {
        generatorPanel.SetActive(false);
    }

    #endregion

    #region Reminder

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

    IEnumerator TypeMessage(TMP_Text textUI, string message)
    {
        textUI.text = "";

        int soundCounter = 0;

        foreach (char c in message)
        {
            textUI.text += c;

            soundCounter++;

            if (typingAudio != null &&
                typingClip != null &&
                soundCounter % 4 == 0 &&
                c != ' ' &&
                c != '\n')
            {
                typingAudio.pitch = Random.Range(0.95f, 1.05f);
                typingAudio.PlayOneShot(typingClip, 0.2f);
            }

            yield return new WaitForSeconds(typingSpeed);
        }
    }
}