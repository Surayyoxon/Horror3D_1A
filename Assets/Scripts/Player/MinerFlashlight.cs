using UnityEngine;

// =====================================================================
// MinerFlashlight.cs
// -----------------------------------------------------------------
// Bu script konchining peshonasidagi fonarni boshqaradi.
// Fonar - bu Unity'ning "Spot Light" (yo'naltirilgan yorug'lik) komponenti.
// Fonar Camera (yoki bosh) ostiga joylashtiriladi, shunda u qayerga
// qaraса o'sha tomonga yoritadi.
//
// FUNKSIYALARI:
//  1) F tugmasi bilan fonarni yoqish/o'chirish (toggle)
//  2) Fonar quvvati (battery) vaqt o'tishi bilan kamayadi (ixtiyoriy)
//  3) Fonar yorug'ligi silliq (flicker) effektsiz, barqaror yonadi
//  4) Boshqa scriptlar (generator, keycard skaneri) fonar yoniq/
//     o'chiqligini bilishi uchun ochiq (public) o'zgaruvchi bor
// =====================================================================

[RequireComponent(typeof(Light))] // Bu script albatta Light komponenti bilan birga bo'lishi kerak
public class MinerFlashlight : MonoBehaviour
{
    [Header("Asosiy sozlamalar")]
    [Tooltip("Fonarni yoqish/o'chirish uchun tugma")]
    public KeyCode toggleKey = KeyCode.F;

    [Tooltip("O'yin boshlanganda fonar yonikmi yoki yo'qmi")]
    public bool startsOn = true;

    [Header("Yorug'lik sozlamalari (Spot Light)")]
    [Tooltip("Fonarning yorug'lik radiusi (masofasi)")]
    public float lightRange = 12f;

    [Tooltip("Fonar konusining kengligi (gradus)")]
    [Range(1f, 179f)]
    public float spotAngle = 45f;

    [Tooltip("Yorug'lik kuchi (yorqinligi)")]
    public float lightIntensity = 3f;

    [Header("Batareya (ixtiyoriy) - agar kerak bo'lmasa useBattery = false qiling")]
    public bool useBattery = false;
    [Tooltip("Batareyaning to'liq quvvati (sekundlarda)")]
    public float maxBattery = 120f;
    private float currentBattery;

    [Header("Ovoz effektlari (ixtiyoriy)")]
    public AudioSource audioSource;      // Fonar tugmasi bosilganda ovoz chiqarish uchun
    public AudioClip toggleSound;        // Klik ovozi

    // ----- Ichki o'zgaruvchilar -----
    private Light flashlightLight;   // Light komponentiga havola
    public bool IsOn { get; private set; } // Boshqa scriptlar fonar holatini shu yerdan bilib oladi

    void Awake()
    {
        // Light komponentini avtomatik topib olamiz
        flashlightLight = GetComponent<Light>();

        // Light turini albatta "Spot" qilib qo'yamiz (fonarga mos)
        flashlightLight.type = LightType.Spot;
        flashlightLight.range = lightRange;
        flashlightLight.spotAngle = spotAngle;
        flashlightLight.intensity = lightIntensity;

        // Soya (shadow) yoqilgan bo'lsa, shaxta ancha real ko'rinadi
        flashlightLight.shadows = LightShadows.Soft;

        currentBattery = maxBattery;
    }

    void Start()
    {
        // O'yin boshida fonar holatini sozlaymiz
        IsOn = startsOn;
        flashlightLight.enabled = IsOn;
    }

    void Update()
    {
        // 1) Tugma bosilganini tekshiramiz
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight();
        }

        // 2) Agar batareya tizimi yoqilgan bo'lsa - quvvatni kamaytiramiz
        if (useBattery && IsOn)
        {
            currentBattery -= Time.deltaTime;

            if (currentBattery <= 0f)
            {
                currentBattery = 0f;
                SetFlashlight(false); // Batareya tugasa fonar avtomatik o'chadi
            }
        }
    }

    // Fonarni yoqish/o'chirish (toggle) funksiyasi
    private void ToggleFlashlight()
    {
        // Agar batareya tugagan bo'lsa, yoqishga urinish befoyda
        if (useBattery && currentBattery <= 0f && !IsOn)
            return;

        SetFlashlight(!IsOn);
    }

    // Fonar holatini aniq belgilash uchun (masalan generator uni majburiy o'chirib qo'yishi mumkin)
    public void SetFlashlight(bool state)
    {
        IsOn = state;
        flashlightLight.enabled = IsOn;

        // Ovoz chiqarish (agar sozlangan bo'lsa)
        if (audioSource != null && toggleSound != null)
        {
            audioSource.PlayOneShot(toggleSound);
        }
    }

    // Batareyani to'ldirish uchun (masalan generator yonida batareya topilsa chaqiriladi)
    public void RechargeBattery(float amount)
    {
        currentBattery = Mathf.Clamp(currentBattery + amount, 0f, maxBattery);
    }

    // Batareya foizini boshqa scriptlarda (masalan UI'da) ko'rsatish uchun
    public float GetBatteryPercent()
    {
        if (!useBattery) return 1f; // Agar batareya tizimi o'chiq bo'lsa, doim 100% deb hisoblaymiz
        return currentBattery / maxBattery;
    }
}