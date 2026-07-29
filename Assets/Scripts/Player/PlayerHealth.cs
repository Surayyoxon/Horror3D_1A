using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // O'lgandan keyin qayta damage olmasligi uchun

        currentHealth -= damage;
        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player Dead!");

        // 1. GameManager'ga o'yinchi o'lganini xabar qilamiz
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDied();
        }
        else if (MenuManager.Instance != null)
        {
            // Agar GameManager bo'lmasa, to'g'ridan-to'g'ri MenuManager orqali ochamiz
            MenuManager.Instance.ShowYouDiedMenu();
        }

        // 2. O'yinchi harakatini yoki skriptlarini o'chirish (ixtiyoriy)
        // MonoBehaviour playerController = GetComponent<PlayerController>();
        // if (playerController != null) playerController.enabled = false;
    }

    // O'yin qayta boshlanganda salomatlikni tiklash uchun metod
    public void HealFull()
    {
        currentHealth = maxHealth;
        isDead = false;
    }
}