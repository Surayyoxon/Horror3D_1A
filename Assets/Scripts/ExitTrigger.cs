using UnityEngine;
using TMPro;
using System.Collections;

public class ExitTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject objectivePanel;
    public TMP_Text objectiveText;

    [Header("Message")]
    [TextArea(5, 15)]
    public string message =
@"CHIQISH YO'LI BERKILGAN

Chiqish yo'li qulagan toshlar bilan berkilgan.

Kondan qutulish uchun zaxira chiqish eshigini ochishing kerak.

1. Generatorni ishga tushir
2. Panel 01 ni aktivlashtir
3. Panel 02 ni aktivlashtir
4. Keycardni top
5. Eshikni och va qoch";

    [Header("Typing")]
    public float typingSpeed = 0.05f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip typingSound;

    private Coroutine typingCoroutine;

    private void Start()
    {
        objectivePanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        objectivePanel.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        objectiveText.text = "";
        objectivePanel.SetActive(false);
    }

    IEnumerator TypeText()
    {
        objectiveText.text = "";

        int soundCounter = 0;

        foreach (char letter in message)
        {
            objectiveText.text += letter;

            soundCounter++;

            if (soundCounter % 4 == 0 &&
                letter != ' ' &&
                letter != '\n')
            {
                if (audioSource != null &&
                    typingSound != null &&
                    !audioSource.isPlaying)
                {
                    audioSource.pitch = Random.Range(0.95f, 1.05f);
                    audioSource.clip = typingSound;
                    audioSource.Play();
                }
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        // Matn tugagach ovozni to'xtatish
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}