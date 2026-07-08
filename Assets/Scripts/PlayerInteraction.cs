using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;

    [Header("UI")]
    public GameObject interactPanel;
    public TMP_Text interactText;

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

                interactPanel.SetActive(true);
                interactText.text =
                $"Press <color=#FFD700>E</color> to {interactable.interactionName}";

                return;
            }
        }

        currentInteractable = null;
        interactPanel.SetActive(false);
    }
}