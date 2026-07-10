using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;

    private Interactable currentInteractable;

    void Update()
    {
        CheckInteractable();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckInteractable()
    {
        Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            Interactable interactable =
                hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                currentInteractable = interactable;

                ObjectiveUIManager.Instance.ShowInteraction(interactable.interactionName);

                return;
            }
        }

        currentInteractable = null;
        ObjectiveUIManager.Instance.HideInteraction();
    }
}