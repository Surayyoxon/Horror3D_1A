using UnityEngine;

public class KeycardItem : Interactable
{
    private void Start()
    {
        interactionName = "Pick up Keycard";
    }

    public override void Interact()
    {
        if (GameManager.Instance.currentStep == GameManager.GameStep.Panel02Active)
        {
            GameManager.Instance.OnKeycardCollected();
            ObjectiveUIManager.Instance.HideInteraction();
            Destroy(gameObject); // Olinganidan keyin xonadan yo'qoladi
        }
    }
}