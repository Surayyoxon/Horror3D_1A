using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f; // Qanchalik yaqin kelsa ishlaydi
    public LayerMask interactableLayer;    // Obyektlarni qidirish uchun layer

    private Interactable currentInteractable;

    void Update()
    {
        // O'yinchi ko'zidan (kamerasidan) to'g'riga nur (Raycast) yuboramiz
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            // Nur tekkan obyektda Interactable skripti bormi?
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    // Ekranga o'sha obyektning nomini chiqaramiz (Masalan: Press E to Pick up Fuse)
                    ObjectiveUIManager.Instance.ShowInteraction(currentInteractable.interactionName);
                }

                // Agar o'yinchi E tugmasini bossa
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentInteractable.Interact();
                }
            }
        }
        else
        {
            // Agar hech narsaga qaramayotgan bo'lsa UI'ni yopamiz
            if (currentInteractable != null)
            {
                currentInteractable = null;
                ObjectiveUIManager.Instance.HideInteraction();
                ObjectiveUIManager.Instance.HideGeneratorInfo(); // Generator matni ochilgan bo'lsa yopish uchun
            }
        }
    }
}