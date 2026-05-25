using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameManagerMision4 : MonoBehaviour
{
    [Header("Referencias de Interfaz")]
    public TextMeshProUGUI textoPalabraOculta;
    public Image imagenReliquia;

    [Header("Control de la Misión (UI)")]
    public GameObject panelMisionCompleta;
    public GameObject luzBiblioteca;
    public Collider2D zonaInteraccion;

    [Header("Pantallas Finales")]
    public GameObject panelVictoria;
    public GameObject panelDerrota;
    public Button botonReintentar;

    [Header("Configuración del Nivel")]
    public Sprite[] spritesEstadoReliquia;
    public string palabraSecreta = "JARRON";

    private char[] progresoPalabra;
    private int erroresCometidos = 0;
    private bool juegoTerminado = false;

    void Start()
    {
        InicializarPalabra();

        if (botonReintentar != null)
        {
            botonReintentar.onClick.AddListener(ReiniciarNivel);
        }
    }

    void InicializarPalabra()
    {
        progresoPalabra = new char[palabraSecreta.Length];
        for (int i = 0; i < progresoPalabra.Length; i++)
        {
            progresoPalabra[i] = '_';
        }
        ActualizarPantalla();
    }

    void ActualizarPantalla()
    {
        textoPalabraOculta.text = string.Join(" ", progresoPalabra);
        if (spritesEstadoReliquia.Length > 0 && erroresCometidos < spritesEstadoReliquia.Length)
        {
            imagenReliquia.sprite = spritesEstadoReliquia[erroresCometidos];
        }
    }

    public void ProcesarLetra(string letra)
    {
        if (juegoTerminado) return;

        bool acierto = false;
        char letraChar = char.ToUpper(letra[0]);

        for (int i = 0; i < palabraSecreta.Length; i++)
        {
            if (char.ToUpper(palabraSecreta[i]) == letraChar)
            {
                progresoPalabra[i] = palabraSecreta[i];
                acierto = true;
            }
        }

        if (acierto)
        {
            VerificarVictoria();
        }
        else
        {
            erroresCometidos++;
            VerificarDerrota();
        }

        ActualizarPantalla();
    }

    void VerificarVictoria()
    {
        string palabraActual = new string(progresoPalabra);
        if (palabraActual == palabraSecreta)
        {
            juegoTerminado = true;
            if (panelVictoria != null) panelVictoria.SetActive(true);

            StartCoroutine(CerrarMisionYEncenderLuz());
        }
    }

    void VerificarDerrota()
    {
        if (erroresCometidos >= spritesEstadoReliquia.Length - 1)
        {
            juegoTerminado = true;
            if (panelDerrota != null) panelDerrota.SetActive(true);
        }
    }

    public void ReiniciarNivel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // --- AQUÍ ESTÁ EL CAMBIO CLAVE ---
    IEnumerator CerrarMisionYEncenderLuz()
    {
        // Usamos Realtime para que los 3 segundos cuenten aunque el juego esté pausado
        yield return new WaitForSecondsRealtime(3f);

        if (luzBiblioteca != null) luzBiblioteca.SetActive(true);

        if (zonaInteraccion != null) zonaInteraccion.enabled = false;

        if (panelMisionCompleta != null) panelMisionCompleta.SetActive(false);

        // Descongelamos el reloj del motor por si el script de la estantería lo pausó
        Time.timeScale = 1f;
    }
}