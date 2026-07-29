using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SlidePuzzleManager : MonoBehaviour
{
    public GameObject puzzleCanvas;
    public Button[] tileButtons = new Button[16];
    public Sprite[] numberSprites = new Sprite[15];
    public Sprite emptySprite;

    [Header("Which Panel Is This Puzzle For?")]
    public PanelTwo panelTwo; // Agar bu Panel 2 uchun bo'lsa, shu yerga bog'lang
    public bool isPanelOne = false; // Agar bu Panel 1 uchun bo'lsa, true qiling

    private int[] tiles = new int[16];
    private int emptyIndex;

    private void Awake()
    {
        for (int i = 0; i < tileButtons.Length; i++)
        {
            int index = i;
            tileButtons[i].onClick.AddListener(() => OnTileClicked(index));
        }
    }

    private void OnEnable()
    {
        SetupPuzzle();
    }

    private void SetupPuzzle()
    {
        for (int i = 0; i < 15; i++) tiles[i] = i + 1;
        tiles[15] = 0;
        emptyIndex = 15;
        ShuffleBySwaps(200);
        RefreshUI();
    }

    private void ShuffleBySwaps(int moves)
    {
        for (int i = 0; i < moves; i++)
        {
            List<int> validMoves = GetValidNeighborIndexes(emptyIndex);
            int randomIndex = validMoves[Random.Range(0, validMoves.Count)];
            SwapTiles(randomIndex, emptyIndex);
            emptyIndex = randomIndex;
        }
    }

    private List<int> GetValidNeighborIndexes(int index)
    {
        List<int> neighbors = new List<int>();
        int row = index / 4;
        int col = index % 4;
        if (row > 0) neighbors.Add(index - 4);
        if (row < 3) neighbors.Add(index + 4);
        if (col > 0) neighbors.Add(index - 1);
        if (col < 3) neighbors.Add(index + 1);
        return neighbors;
    }

    private void SwapTiles(int a, int b)
    {
        int temp = tiles[a];
        tiles[a] = tiles[b];
        tiles[b] = temp;
    }

    private void RefreshUI()
    {
        for (int i = 0; i < 16; i++)
        {
            Image img = tileButtons[i].image;
            if (tiles[i] == 0)
            {
                img.sprite = emptySprite;
                img.color = new Color(1, 1, 1, 0);
                tileButtons[i].interactable = false;
            }
            else
            {
                img.sprite = numberSprites[tiles[i] - 1];
                img.color = Color.white;
                tileButtons[i].interactable = true;
            }
        }
    }

    public void OnTileClicked(int index)
    {
        List<int> validMoves = GetValidNeighborIndexes(emptyIndex);
        if (validMoves.Contains(index))
        {
            SwapTiles(index, emptyIndex);
            emptyIndex = index;
            RefreshUI();
            CheckWin();
        }
    }

    private void CheckWin()
    {
        for (int i = 0; i < 15; i++)
        {
            if (tiles[i] != i + 1) return;
        }
        StartCoroutine(CompletePuzzle());
    }

    private IEnumerator CompletePuzzle()
    {
        yield return new WaitForSecondsRealtime(0.8f);
        puzzleCanvas.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // O'ZGARTIRILGAN QISM: qaysi panel ekaniga qarab to'g'ri metodni chaqiradi
        if (isPanelOne)
        {
            GameManager.Instance.OnPanel01Activated();
        }
        else if (panelTwo != null)
        {
            panelTwo.OnPuzzleCompleted();
        }
    }
}