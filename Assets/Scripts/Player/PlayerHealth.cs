using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("UI")]
    public GameObject youDiedPanel; // Inspector'da "You Died" panelni tashlang

    void Start()
    {
        currentHealth = maxHealth;

        if (youDiedPanel != null)
            youDiedPanel.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // o'lgandan keyin damage olmasin

        currentHealth -= damage;
        Debug.Log("HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player Dead");

        // You Died panelni ochish
        if (youDiedPanel != null)
            youDiedPanel.SetActive(true);

        // O'yinni pauza qilish
        Time.timeScale = 0f;

        // Cursor'ni ochib qo'yish (panel bilan ishlash uchun)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Agar GameManager orqali state boshqarilsa:
        // GameManager.Instance.SetGameStep(GameStep.PlayerDied);
    }
}
