using UnityEngine;

public class PanelTwo : Interactable
{
    public GameObject puzzleCanvas; // SudokuCanvas yoki istalgan boshqa o'yin canvasi shu yerga biriktiriladi

    private void Start()
    {
        interactionName = "Activate Panel 02";
    }

    public override void Interact()
    {
        if (GameManager.Instance.currentStep == GameManager.GameStep.Panel01Active)
        {
            puzzleCanvas.SetActive(true);
            ObjectiveUIManager.Instance.HideInteraction();
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Puzzle o'yini (Sudoku, Memory Game, yoki istalgan boshqasi) yechilganda
    // shu metodni chaqirish kifoya
    public void OnPuzzleCompleted()
    {
        puzzleCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager.Instance.OnPanel02Activated();
    }
}