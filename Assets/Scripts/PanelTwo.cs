using UnityEngine;

public class PanelTwo : Interactable
{
    private void Start()
    {
        interactionName = "Activate Panel 02";
    }

    public override void Interact()
    {
        // Faqat Panel 1 yoqqanidan keyin ishlaydi
        if (GameManager.Instance.currentStep == GameManager.GameStep.Panel01Active)
        {
            // Bu yerda ikkinchi puzzle o'yinini ochasiz
            // Puzzle yutilgandan keyin:
            GameManager.Instance.OnPanel02Activated();
            ObjectiveUIManager.Instance.HideInteraction();
            this.enabled = false;
        }
    }
}