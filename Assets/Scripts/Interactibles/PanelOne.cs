using UnityEngine;

public class PanelOne : Interactable
{
    private void Start()
    {
        interactionName = "Activate Panel 01";
    }

    public override void Interact()
    {
        // Agar player generatorni yoqib kelgan bo'lsa
        if (GameManager.Instance.currentStep == GameManager.GameStep.GeneratorActive)
        {
            // Bu yerda sodda puzzle o'yiningizni ishga tushirasiz
            // Puzzle muvaffaqiyatli tugagach quyidagi kodni chaqirasiz:
            GameManager.Instance.OnPanel01Activated();
            ObjectiveUIManager.Instance.HideInteraction();

            // Panel 1 ishga tushgani uchun bu obyektni boshqa bosib bo'lmaydigan qilamiz
            this.enabled = false;
        }
        else
        {
            Debug.Log("Panelga tok kelmayapti. Avval generatorni yoqing!");
        }
    }
}