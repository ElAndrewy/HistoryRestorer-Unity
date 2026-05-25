using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BotonLetra : MonoBehaviour
{
    private Button miBoton;
    private TextMeshProUGUI textoBoton;
    private GameManagerMision4 gameManager;
    private string letraAsignada;

    void Start()
    {
        miBoton = GetComponent<Button>();
        textoBoton = GetComponentInChildren<TextMeshProUGUI>();
        gameManager = FindFirstObjectByType<GameManagerMision4>();

        if (textoBoton != null)
        {
            letraAsignada = textoBoton.text.ToUpper();
        }

        miBoton.onClick.AddListener(AlPresionarBoton);
    }

    void AlPresionarBoton()
    {
        if (gameManager != null)
        {
            gameManager.ProcesarLetra(letraAsignada);
            miBoton.interactable = false;
        }
    }
}