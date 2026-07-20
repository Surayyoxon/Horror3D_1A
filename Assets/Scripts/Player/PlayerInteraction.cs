using UnityEngine;
public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;
    private Interactable currentInteractable;
    private Camera playerCamera;              // <-- 1) SHU QATORNI QO'SHING

    void Start()                               // <-- 2) SHU METODNI QO'SHING
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);  // <-- 3) SHU QATORNI ALMASHTIRING
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    ObjectiveUIManager.Instance.ShowInteraction(currentInteractable.interactionName);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentInteractable.Interact();
                    currentInteractable = null;
                }
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable = null;
                ObjectiveUIManager.Instance.HideInteraction();
                ObjectiveUIManager.Instance.HideGeneratorInfo();
            }
        }
    }
}