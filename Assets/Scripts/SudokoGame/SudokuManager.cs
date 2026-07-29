using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SudokuManager : MonoBehaviour
{
    public GameObject puzzleCanvas;
    public TMP_InputField[] cells = new TMP_InputField[16]; // 4x4 = 16 katak
    public TMP_Text feedbackText;

    [Header("Qiyinlik")]
    public int cellsToRemove = 8; // Nechta katak bo'sh qoldirilsin (16 tadan)

    private int[] puzzle = new int[16];
    private int[] solution = new int[16];

    // Bazaviy (o'zgarmas) to'g'ri 4x4 sudoku yechimi (2x2 bloklar bilan)
    private readonly int[] baseSolution = new int[16]
    {
        1, 2, 3, 4,
        3, 4, 1, 2,
        2, 1, 4, 3,
        4, 3, 2, 1
    };

    private void OnEnable()
    {
        GenerateNewPuzzle();
        SetupBoard();
    }

    // ------------------- Tasodifiy 4x4 Sudoku generatsiyasi -------------------
    private void GenerateNewPuzzle()
    {
        int[] grid = (int[])baseSolution.Clone();

        // 1) Qatorlarni band ichida (2 qatorlik guruh) aralashtirish
        grid = ShuffleRowsWithinBands(grid);

        // 2) Ustunlarni band ichida (2 ustunlik guruh) aralashtirish
        grid = ShuffleColsWithinBands(grid);

        // 3) Qator-bandlarni o'zaro almashtirish
        grid = ShuffleRowBands(grid);

        // 4) Ustun-bandlarni o'zaro almashtirish
        grid = ShuffleColBands(grid);

        // 5) Raqamlarni tasodifiy almashtirish (1-4 aralashmasi)
        grid = RemapNumbers(grid);

        solution = grid;

        // 6) Puzzle hosil qilish: tasodifiy kataklarni bo'sh (0) qilib qo'yish
        puzzle = (int[])solution.Clone();
        List<int> indexes = new List<int>();
        for (int i = 0; i < 16; i++) indexes.Add(i);

        for (int i = 0; i < cellsToRemove && indexes.Count > 0; i++)
        {
            int randPos = Random.Range(0, indexes.Count);
            puzzle[indexes[randPos]] = 0;
            indexes.RemoveAt(randPos);
        }
    }

    private int[] ShuffleRowsWithinBands(int[] grid)
    {
        int[] result = (int[])grid.Clone();
        for (int band = 0; band < 2; band++) // 2 ta band, har biri 2 qatordan
        {
            int r0 = band * 2;
            int r1 = band * 2 + 1;
            if (Random.Range(0, 2) == 0)
            {
                SwapRows(result, r0, r1);
            }
        }
        return result;
    }

    private int[] ShuffleColsWithinBands(int[] grid)
    {
        int[] result = (int[])grid.Clone();
        for (int band = 0; band < 2; band++) // 2 ta band, har biri 2 ustundan
        {
            List<int> cols = new List<int> { band * 2, band * 2 + 1 };
            Shuffle(cols);
            for (int r = 0; r < 4; r++)
            {
                result[r * 4 + band * 2 + 0] = grid[r * 4 + cols[0]];
                result[r * 4 + band * 2 + 1] = grid[r * 4 + cols[1]];
            }
        }
        return result;
    }
    private int[] ShuffleRowBands(int[] grid)
    {
        if (Random.Range(0, 2) == 0) return grid;

        int[] result = (int[])grid.Clone();
        for (int rr = 0; rr < 2; rr++)
        {
            for (int c = 0; c < 4; c++)
            {
                // Top va Bottom bandlarni almashtirish (0,1 qatorlar <-> 2,3 qatorlar)
                result[(rr + 2) * 4 + c] = grid[rr * 4 + c];
                result[rr * 4 + c] = grid[(rr + 2) * 4 + c];
            }
        }
        return result;
    }

    private int[] ShuffleColBands(int[] grid)
    {
        if (Random.Range(0, 2) == 0) return grid;

        int[] result = (int[])grid.Clone();
        for (int cc = 0; cc < 2; cc++)
        {
            for (int r = 0; r < 4; r++)
            {
                // Chap va O'ng bandlarni almashtirish (0,1 ustunlar <-> 2,3 ustunlar)
                result[r * 4 + (cc + 2)] = grid[r * 4 + cc];
                result[r * 4 + cc] = grid[r * 4 + (cc + 2)];
            }
        }
        return result;
    }

    private int[] RemapNumbers(int[] grid)
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4 };
        Shuffle(numbers);

        int[] result = new int[16];
        for (int i = 0; i < 16; i++)
        {
            result[i] = numbers[grid[i] - 1];
        }
        return result;
    }

    private void SwapRows(int[] grid, int r0, int r1)
    {
        for (int c = 0; c < 4; c++)
        {
            int temp = grid[r0 * 4 + c];
            grid[r0 * 4 + c] = grid[r1 * 4 + c];
            grid[r1 * 4 + c] = temp;
        }
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    // ------------------------------------------------------------------------------

    private void SetupBoard()
    {
        for (int i = 0; i < 16; i++)
        {
            var cell = cells[i];
            if (cell == null) continue;

            cell.text = "";
            cell.characterLimit = 1;
            cell.contentType = TMP_InputField.ContentType.IntegerNumber;

            if (puzzle[i] != 0)
            {
                cell.text = puzzle[i].ToString();
                cell.interactable = false;
            }
            else
            {
                cell.text = "";
                cell.interactable = true;
            }
        }

        if (feedbackText != null)
            feedbackText.text = "";
    }

    public void OnCheckButtonPressed()
    {
        for (int i = 0; i < 16; i++)
        {
            if (!int.TryParse(cells[i].text, out int value) || value != solution[i])
            {
                if (feedbackText != null)
                    feedbackText.text = "Incorrect. Try again.";
                return;
            }
        }

        if (feedbackText != null)
            feedbackText.text = "Access Granted!";

        StartCoroutine(CompletePuzzle());
    }

    private IEnumerator CompletePuzzle()
    {
        yield return new WaitForSecondsRealtime(1f);

        if (puzzleCanvas != null)
            puzzleCanvas.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPanel02Activated();
        }
    }
}
