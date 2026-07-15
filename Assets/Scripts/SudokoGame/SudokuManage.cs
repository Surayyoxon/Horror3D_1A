using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SudokuManager : MonoBehaviour
{
    public GameObject puzzleCanvas;
    public TMP_InputField[] cells = new TMP_InputField[36]; // 6x6 = 36 katak
    public TMP_Text feedbackText;

    [Header("Qiyinlik")]
    public int cellsToRemove = 18; // Nechta katak bo'sh qoldirilsin (36 tadan)

    private int[] puzzle = new int[36];
    private int[] solution = new int[36];

    // Bazaviy (o'zgarmas) to'g'ri sudoku yechimi — aralashtirish uchun asos
    private readonly int[] baseSolution = new int[36]
    {
        1, 2, 3, 4, 5, 6,
        4, 5, 6, 1, 2, 3,
        2, 3, 1, 5, 6, 4,
        5, 6, 4, 2, 3, 1,
        3, 1, 2, 6, 4, 5,
        6, 4, 5, 3, 1, 2
    };

    private void OnEnable()
    {
        GenerateNewPuzzle();
        SetupBoard();
    }

    // ------------------- YANGI: Tasodifiy sudoku generatsiya qilish -------------------
    private void GenerateNewPuzzle()
    {
        int[] grid = (int[])baseSolution.Clone();

        // 1) Qatorlarni "band" ichida aralashtirish (har band = 2 qator)
        grid = ShuffleRowsWithinBands(grid);

        // 2) Ustunlarni "band" ichida aralashtirish (har band = 3 ustun)
        grid = ShuffleColsWithinBands(grid);

        // 3) Qator-bandlarni o'zaro almashtirish
        grid = ShuffleRowBands(grid);

        // 4) Ustun-bandlarni o'zaro almashtirish
        grid = ShuffleColBands(grid);

        // 5) Raqamlarni qayta belgilash (1->4, 2->1 kabi tasodifiy almashtirish)
        grid = RemapNumbers(grid);

        solution = grid;

        // 6) Puzzle hosil qilish: tasodifiy kataklarni bo'sh (0) qilib qo'yish
        puzzle = (int[])solution.Clone();
        List<int> indexes = new List<int>();
        for (int i = 0; i < 36; i++) indexes.Add(i);

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
        for (int band = 0; band < 3; band++) // 3 ta band, har biri 2 qatordan
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
        for (int band = 0; band < 2; band++) // 2 ta band, har biri 3 ustundan
        {
            List<int> cols = new List<int> { band * 3, band * 3 + 1, band * 3 + 2 };
            Shuffle(cols);
            for (int r = 0; r < 6; r++)
            {
                result[r * 6 + band * 3 + 0] = grid[r * 6 + cols[0]];
                result[r * 6 + band * 3 + 1] = grid[r * 6 + cols[1]];
                result[r * 6 + band * 3 + 2] = grid[r * 6 + cols[2]];
            }
        }
        return result;
    }

    private int[] ShuffleRowBands(int[] grid)
    {
        if (Random.Range(0, 2) == 0) return grid;

        int[] result = (int[])grid.Clone();
        // 3 ta bandni (har biri 2 qator) o'zaro tasodifiy tartiblash
        List<int> bandOrder = new List<int> { 0, 1, 2 };
        Shuffle(bandOrder);

        for (int b = 0; b < 3; b++)
        {
            int srcBand = bandOrder[b];
            for (int rr = 0; rr < 2; rr++)
            {
                int srcRow = srcBand * 2 + rr;
                int dstRow = b * 2 + rr;
                for (int c = 0; c < 6; c++)
                    result[dstRow * 6 + c] = grid[srcRow * 6 + c];
            }
        }
        return result;
    }

    private int[] ShuffleColBands(int[] grid)
    {
        if (Random.Range(0, 2) == 0) return grid;

        int[] result = (int[])grid.Clone();
        List<int> bandOrder = new List<int> { 0, 1 };
        Shuffle(bandOrder);

        for (int b = 0; b < 2; b++)
        {
            int srcBand = bandOrder[b];
            for (int cc = 0; cc < 3; cc++)
            {
                int srcCol = srcBand * 3 + cc;
                int dstCol = b * 3 + cc;
                for (int r = 0; r < 6; r++)
                    result[r * 6 + dstCol] = grid[r * 6 + srcCol];
            }
        }
        return result;
    }

    private int[] RemapNumbers(int[] grid)
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
        Shuffle(numbers);

        int[] result = new int[36];
        for (int i = 0; i < 36; i++)
        {
            result[i] = numbers[grid[i] - 1];
        }
        return result;
    }

    private void SwapRows(int[] grid, int r0, int r1)
    {
        for (int c = 0; c < 6; c++)
        {
            int temp = grid[r0 * 6 + c];
            grid[r0 * 6 + c] = grid[r1 * 6 + c];
            grid[r1 * 6 + c] = temp;
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
        for (int i = 0; i < 36; i++)
        {
            var cell = cells[i];
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
        for (int i = 0; i < 36; i++)
        {
            if (!int.TryParse(cells[i].text, out int value) || value != solution[i])
            {
                feedbackText.text = "Incorrect. Try again.";
                return;
            }
        }

        feedbackText.text = "Access Granted!";
        StartCoroutine(CompletePuzzle());
    }

    private IEnumerator CompletePuzzle()
    {
        yield return new WaitForSecondsRealtime(1f);

        puzzleCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager.Instance.OnPanel02Activated();
    }
}