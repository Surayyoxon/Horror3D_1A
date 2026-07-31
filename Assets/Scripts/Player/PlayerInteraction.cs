using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;
    private Interactable currentInteractable;
    private Camera playerCamera;

    void Start()
    {
        // 1. Kamerani topishni xavfsizroq qilish
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            ClearCurrentInteractable();
            return;
        }

        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;

                    // 1. Standart E tugmasi matni
                    ObjectiveUIManager.Instance.ShowInteraction(currentInteractable.interactionName);

                    // 2. YANGI: Agar bu obyekt GENERATOR bo'lsa, Generator UI paneli ham ochiladi!
                    Generator generator = interactable.GetComponent<Generator>();
                    if (generator != null)
                    {
                        ObjectiveUIManager.Instance.ShowGeneratorInfo(generator.currentFuses, generator.requiredFuses);
                    }
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentInteractable.Interact();
                }
            }
            else
            {
                ClearCurrentInteractable();
            }
        }
        else
        {
            ClearCurrentInteractable();
        }
    }

    // Yordamchi metod: UI vizual matnlarni yopish va obyektni tozalash
    private void ClearCurrentInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable = null;
            if (ObjectiveUIManager.Instance != null)
            {
                ObjectiveUIManager.Instance.HideInteraction();
                ObjectiveUIManager.Instance.HideGeneratorInfo();
            }
        }
    }
}