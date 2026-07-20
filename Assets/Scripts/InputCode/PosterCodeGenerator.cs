using UnityEngine;
public class PosterCodeGenerator : MonoBehaviour
{
    [Header("Bog'liq skriptlar")]
    public InputCodeManager inputCodeManager;
    private void Start()
    {
        Debug.Log("PosterCodeGenerator Start() ishga tushdi"); // TEST 1
        GenerateNewCode();
    }
    public void GenerateNewCode()
    {
        if (inputCodeManager == null)
        {
            Debug.LogWarning("InputCodeManager biriktirilmagan!");
            return;
        }
        string newCode = GenerateRandomCode(inputCodeManager.maxDigits);
        inputCodeManager.correctCode = newCode;
        Debug.Log("YANGI KOD: " + newCode); // TEST 2

        if (ObjectiveUIManager.Instance != null)
        {
            ObjectiveUIManager.Instance.SetPosterCode(newCode);
            Debug.Log("SetPosterCode chaqirildi: " + newCode); // TEST 3
        }
        else
        {
            Debug.LogWarning("ObjectiveUIManager.Instance NULL!");
        }
    }
    private string GenerateRandomCode(int length)
    {
        string code = "";
        for (int i = 0; i < length; i++)
        {
            code += Random.Range(0, 10).ToString();
        }
        return code;
    }
}