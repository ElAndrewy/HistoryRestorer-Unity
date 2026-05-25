using UnityEngine;
using System.Collections;
using TMPro;

public class ControladorMision : MonoBehaviour
{
    [Header("Configuración de la Misión")]
    public int bookshelfIndex = 1;
    public GameObject puzzlePanel;
    public TextMeshProUGUI textoFeedback;

    [Header("EMERGENCIA - LUZ DIRECTA")]
    public GameObject luzFaroDirecta;

    [Header("Referencias a los Contenedores")]
    public Transform canasta1Silaba;
    public Transform canasta2Silabas;

    private int totalFichasEnNivel = 4;
    private bool juegoTerminado = false;

    void Start()
    {
        // AUTOMATIZACIÓN INMUNE A ERRORES: Cuenta cuántas fichas existen en el panel automáticamente
        DatosFicha[] fichas = GetComponentsInChildren<DatosFicha>(true);
        if (fichas.Length > 0)
        {
            totalFichasEnNivel = fichas.Length;
        }
    }

    void Update()
    {
        // TECLA DE PÁNICO SECRETA: Si estás probando el juego o exponiendo y la luz no prende,
        // presiona la tecla '9' de tu teclado alfanumérico y ganarás la misión al instante.
        if (!juegoTerminado && Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.LogWarning("¡Victoria forzada por tecla de emergencia!");
            ForzarVictoriaSistema();
        }
    }

    public void ValidarRespuestasAutomatico()
    {
        if (juegoTerminado) return;
        if (canasta1Silaba == null || canasta2Silabas == null) return;

        bool nivelAprobado = true;
        int fichasCorrectas = 0;
        int fichasTotalesColocadas = 0;

        foreach (Transform hijo in canasta1Silaba)
        {
            DatosFicha datos = hijo.GetComponent<DatosFicha>();
            if (datos != null)
            {
                fichasTotalesColocadas++;
                if (datos.cantidadSilabas == 1)
                {
                    fichasCorrectas++;
                    SetFichaFeedback(hijo, Color.white);
                }
                else
                {
                    nivelAprobado = false;
                    SetFichaFeedback(hijo, Color.red);
                }
            }
        }

        foreach (Transform hijo in canasta2Silabas)
        {
            DatosFicha datos = hijo.GetComponent<DatosFicha>();
            if (datos != null)
            {
                fichasTotalesColocadas++;
                if (datos.cantidadSilabas == 2)
                {
                    fichasCorrectas++;
                    SetFichaFeedback(hijo, Color.white);
                }
                else
                {
                    nivelAprobado = false;
                    SetFichaFeedback(hijo, Color.red);
                }
            }
        }

        if (textoFeedback != null)
        {
            if (fichasTotalesColocadas > 0 && fichasCorrectas < totalFichasEnNivel)
            {
                textoFeedback.text = $"Fichas correctas: {fichasCorrectas} de {totalFichasEnNivel}";
                textoFeedback.color = Color.white;
            }
        }

        // Si todo está correcto, ejecuta la victoria normal
        if (nivelAprobado && fichasCorrectas == totalFichasEnNivel)
        {
            ForzarVictoriaSistema();
        }
    }

    private void ForzarVictoriaSistema()
    {
        juegoTerminado = true;
        if (textoFeedback != null)
        {
            textoFeedback.text = "¡Excelente! Todo el conocimiento ha sido restaurado.";
            textoFeedback.color = Color.green;
        }
        StartCoroutine(WinDelay());
    }

    private void SetFichaFeedback(Transform ficha, Color colorIndicador)
    {
        UnityEngine.UI.Image img = ficha.GetComponent<UnityEngine.UI.Image>();
        if (img != null) img.color = colorIndicador;
        StartCoroutine(ComicBounceRoutine(ficha));
    }

    private IEnumerator ComicBounceRoutine(Transform obj)
    {
        Vector3 escalaBase = Vector3.one;
        obj.localScale = escalaBase * 1.25f;
        yield return new WaitForSecondsRealtime(0.1f);
        obj.localScale = escalaBase * 0.85f;
        yield return new WaitForSecondsRealtime(0.1f);
        obj.localScale = escalaBase;
    }

    private IEnumerator WinDelay()
    {
        yield return new WaitForSecondsRealtime(2.2f);
        Time.timeScale = 1f;

        // ENCENDIDO FORZADO DIRECTO
        if (luzFaroDirecta != null)
        {
            luzFaroDirecta.SetActive(true);
        }

        if (puzzlePanel != null) puzzlePanel.SetActive(false);
    }
}