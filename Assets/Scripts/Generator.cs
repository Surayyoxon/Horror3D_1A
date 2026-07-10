using UnityEngine;

public class Generator : Interactable
{
    [Header("Generator")]
    public int requiredFuses = 3;
    private bool generatorStarted = false;

    public override void Interact()
    {
        if (generatorStarted)
        {
            Debug.Log("Generator is already running.");
            return;
        }

        // Hozircha test uchun 0 ta fuse deb olamiz
        int currentFuses = 0;

        ObjectiveUIManager.Instance.ShowGeneratorInfo(currentFuses, requiredFuses);

        ObjectiveUIManager.Instance.SetReminder("Find 3 Fuses");
    }

    public void StartGenerator()
    {
        generatorStarted = true;

        Debug.Log("Generator Started!");

        ObjectiveUIManager.Instance.HideGeneratorInfo();
        ObjectiveUIManager.Instance.SetReminder("Activate Panel 01");
    }
}