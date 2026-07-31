using UnityEngine;

public class Generator : Interactable
{
    // PlayerInteraction.cs qidirayotgan o'zgaruvchilar:
    public int currentFuses = 0;
    public int requiredFuses = 3; // O'zingizga kerakli saqlagichlar (fuse) sonini qo'ying

    private void Start()
    {
        // Ekranning pastida chiquvchi matn
        interactionName = "Start Generator";
    }

    public override void Interact()
    {
        // E tugmasi bosilganda GameManager-ga xabar beramiz
        GameManager.Instance.OnInteractWithGenerator();
    }
}