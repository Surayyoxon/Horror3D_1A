using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameStep
    {
        FindExit,
        BlockedExitSeen,
        NeedFuses,
        GeneratorActive,
        Panel01Active,
        Panel02Active,
        HasKeycard,
        Escaped
    }

    public GameStep currentStep = GameStep.FindExit;

    [Header("Game Balances")]
    public int totalFusesRequired = 3;
    public int currentFusesFound = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        ObjectiveUIManager.Instance.SetReminder("Objective: Find a way out of the mine.");
    }

    public void OnReachBlockedExit()
    {
        if (currentStep == GameStep.FindExit)
        {
            currentStep = GameStep.BlockedExitSeen;

            string mainTaskText = "EXIT BLOCKED\n\n" +
                                  "The exit is blocked by fallen rocks.\n" +
                                  "To escape the mine, you must open the emergency exit.\n\n" +
                                  "Tasks:\n" +
                                  "1. Start the Generator\n" +
                                  "2. Activate Panel 01\n" +
                                  "3. Activate Panel 02\n" +
                                  "4. Find the Keycard\n" +
                                  "5. Open the Exit Door and Escape";

            ObjectiveUIManager.Instance.ShowExitObjective(mainTaskText);
            ObjectiveUIManager.Instance.SetReminder("Objective: Start the Generator.");
        }
    }

    public void OnInteractWithGenerator()
    {
        // 1-HOLAT: Agar o'yinchi bloklangan chiqishni ko'rib, birinchi marta generatorga kelgan bo'lsa
        if (currentStep == GameStep.BlockedExitSeen)
        {
            currentStep = GameStep.NeedFuses;
            ObjectiveUIManager.Instance.ShowGeneratorInfo(currentFusesFound, totalFusesRequired);
            return; // Kod shu yerda to'xtaydi, keyingi shartga o'tib ketmaydi!
        }

        // 2-HOLAT: O'yinchi vazifani biladi va generator bilan qayta interaksiya qilyapti
        if (currentStep == GameStep.NeedFuses)
        {
            // Agar fuselar yetarli bo'lsa, generatorni yoqamiz
            if (currentFusesFound >= totalFusesRequired)
            {
                currentStep = GameStep.GeneratorActive;
                ObjectiveUIManager.Instance.HideGeneratorInfo(); // 3 ta fuse top degan panel yopiladi
                ObjectiveUIManager.Instance.SetReminder("Objective: Activate Panel 01.");

                // Chiroqlar va effektlar shu yerda yoqiladi
                Debug.Log("Lights ON! Generator is running.");
            }
            else
            {
                // Agar hali ham fuse yetarli bo'lmasa, shunchaki panelni qayta yangilab ko'rsatamiz
                ObjectiveUIManager.Instance.ShowGeneratorInfo(currentFusesFound, totalFusesRequired);
            }
        }
    }

    public void FindFuse()
    {
        currentFusesFound++;

        ObjectiveUIManager.Instance.UpdateFuseUI(currentFusesFound, totalFusesRequired);

        if (currentStep == GameStep.NeedFuses)
        {
            ObjectiveUIManager.Instance.SetReminder($"Objective: Start the Generator ({currentFusesFound}/{totalFusesRequired} Fuses).");
        }
    }

    public void OnPanel01Activated()
    {
        if (currentStep == GameStep.GeneratorActive)
        {
            currentStep = GameStep.Panel01Active;
            ObjectiveUIManager.Instance.SetReminder("Objective: Activate Panel 02 (Door opened).");
        }
    }

    public void OnPanel02Activated()
    {
        if (currentStep == GameStep.Panel01Active)
        {
            currentStep = GameStep.Panel02Active;
            ObjectiveUIManager.Instance.SetReminder("Objective: Find the Keycard.");
        }
    }

    public void OnKeycardCollected()
    {
        if (currentStep == GameStep.Panel02Active)
        {
            currentStep = GameStep.HasKeycard;
            ObjectiveUIManager.Instance.SetReminder("Objective: Escape through the Emergency Exit!");
        }
    }

    // AYNAN SHU FUNKSIYA ENG OXIRIGA QO'SHILDI
    public void OnEscape()
    {
        if (currentStep == GameStep.HasKeycard)
        {
            currentStep = GameStep.Escaped;
            ObjectiveUIManager.Instance.HideReminder();
            ObjectiveUIManager.Instance.HideInteraction();

            // Ekranga g'alaba matnini chiqarish
            ObjectiveUIManager.Instance.ShowExitObjective("YOU ESCAPED!\n\nYou survived the dark mine.");

            // O'yin tugaganligi haqida konsolga xabar
            Debug.Log("Player has escaped successfully! Level Complete.");
        }
    }
}