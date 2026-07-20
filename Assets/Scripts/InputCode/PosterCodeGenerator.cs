using UnityEngine;

public class PosterCodeGenerator : MonoBehaviour
{
    [Header("Bog'liq skriptlar")]
    public InputCodeManager inputCodeManager;

    private void Start()
    {
        // O'yin boshlanishi bilan darhol yangi kod yaratamiz
        GenerateNewCode();
    }

    public void GenerateNewCode()
    {
        if (inputCodeManager == null)
        {
            Debug.LogError("[PosterCodeGenerator] InputCodeManager biriktirilmagan!");
            return;
        }

        // Tasodifiy kod yaratish
        string newCode = GenerateRandomCode(inputCodeManager.maxDigits);

        // InputCodeManager'ga yangi kodni yuboramiz
        inputCodeManager.SetNewCode(newCode);

        Debug.Log("<color=cyan>[PosterCodeGenerator]</color> O'yin boshida yangi kod yaratildi va tarqatildi: " + newCode);
    }

    private string GenerateRandomCode(int length)
    {
        System.Text.StringBuilder codeBuilder = new System.Text.StringBuilder();
        for (int i = 0; i < length; i++)
        {
            codeBuilder.Append(Random.Range(0, 10));
        }
        return codeBuilder.ToString();
    }
}