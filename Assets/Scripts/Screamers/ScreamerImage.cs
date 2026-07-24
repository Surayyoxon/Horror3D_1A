using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreamerImage : MonoBehaviour
{
    public Image screamerImage;
    public AudioSource screamSound;
    public float delay = 0f;
    public float flashDuration = 0.3f;   // rasm qancha vaqt ekranda tursin
    public float fadeInTime = 0.03f;     // deyarli oniy paydo bo'lishi
    public float fadeOutTime = 0.15f;

    private bool triggered = false;

    void Start()
    {
        SetAlpha(0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;
        Invoke(nameof(Execute), delay);
    }

    void Execute()
    {
        if (screamSound != null) screamSound.Play();
        StartCoroutine(FlashSequence());
    }

    IEnumerator FlashSequence()
    {
        // Tez paydo bo'lish
        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(0f, 1f, t / fadeInTime));
            yield return null;
        }
        SetAlpha(1f);

        yield return new WaitForSeconds(flashDuration);

        // Tez yo'qolish
        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(1f, 0f, t / fadeOutTime));
            yield return null;
        }
        SetAlpha(0f);
    }

    void SetAlpha(float value)
    {
        Color c = screamerImage.color;
        c.a = value;
        screamerImage.color = c;
    }
}