using UnityEngine;

public class RestorationManager : MonoBehaviour
{
    public static RestorationManager Instance;

    [Header("Funcionalidad Principal: Luces y Faros")]
    [Tooltip("Arrastra aquí los objetos de luz o faros correspondientes a cada misión (Elemento 0 = Misión 1, etc.)")]
    public GameObject[] lucesEstanterias;

    [Header("Funcionalidad Secundaria: Color de Estanterías (Opcional)")]
    [Tooltip("Arrastra aquí los SpriteRenderers si deseas alterar su tinte. Si se deja vacío, no romperá el juego.")]
    public SpriteRenderer[] estanteriasSprites;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestoreSection(int index)
    {
        // 1. REINSTAURACIÓN DEL FARO: Se ejecuta de manera prioritaria
        if (index >= 0 && index < lucesEstanterias.Length)
        {
            if (lucesEstanterias[index] != null)
            {
                lucesEstanterias[index].SetActive(true);
                Debug.Log("Funcionalidad del faro activada para el índice: " + index);
            }
            else
            {
                Debug.LogWarning("El objeto de luz en el índice " + index + " está vacío en el Inspector.");
            }
        }
        else
        {
            Debug.LogError("Índice de misión (" + index + ") fuera de rango en la lista de lucesEstanterias.");
        }

        // 2. CONTROL DE COLOR: Protegido para que no interrumpa la lógica del faro si falla
        if (estanteriasSprites != null && index >= 0 && index < estanteriasSprites.Length)
        {
            if (estanteriasSprites[index] != null)
            {
                estanteriasSprites[index].color = Color.white;
            }
        }
    }
}