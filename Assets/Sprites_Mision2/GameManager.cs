using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Sistema de Puntos")]
    public int puntosPorAcierto = 10;
    public int puntosParaGanar = 40;
    public TextMeshProUGUI textoPuntaje;

    [Header("Sistema de Errores")]
    public int limiteErrores = 3;
    private int erroresActuales = 0;

    [Header("Referencias de Cierre (OBLIGATORIO)")]
    public GameObject panelMisionCompleta;
    public GameObject luzBiblioteca;
    public Collider2D zonaInteraccion;

    [Header("Pantallas Finales")]
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    private int puntajeTotal = 0;
    private bool juegoTerminado = false;

    void Start()
    {
        ActualizarPantalla();
    }

    public void RegistrarAcierto()
    {
        if (juegoTerminado) return;

        puntajeTotal += puntosPorAcierto;
        ActualizarPantalla();

        if (puntajeTotal >= puntosParaGanar)
        {
            Victoria();
        }
    }

    public void RegistrarError()
    {
        if (juegoTerminado) return;

        erroresActuales++;
        if (erroresActuales >= limiteErrores)
        {
            Derrota();
        }
    }

    void Victoria()
    {
        juegoTerminado = true;
        if (panelVictoria != null) panelVictoria.SetActive(true);
        StartCoroutine(CerrarMision());
    }

    void Derrota()
    {
        juegoTerminado = true;
        if (panelDerrota != null) panelDerrota.SetActive(true);
    }

    void ActualizarPantalla()
    {
        if (textoPuntaje != null)
        {
            textoPuntaje.text = "Puntos: " + puntajeTotal.ToString();
        }
    }

    IEnumerator CerrarMision()
    {
        yield return new WaitForSecondsRealtime(3f);

        if (luzBiblioteca != null) luzBiblioteca.SetActive(true);
        if (zonaInteraccion != null) zonaInteraccion.enabled = false;
        if (panelMisionCompleta != null) panelMisionCompleta.SetActive(false);

        Time.timeScale = 1f;
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
}