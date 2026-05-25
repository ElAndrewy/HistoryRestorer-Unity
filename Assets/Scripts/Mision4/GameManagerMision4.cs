using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameManagerMision4 : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    public int bookshelfIndex = 4; // Ajustado a la quinta estantería
    public string palabraSecreta = "JARRON";
    public Sprite[] spritesEstadoReliquia;

    [Header("Referencias de Interfaz")]
    public TextMeshProUGUI textoPalabraOculta;
    public Image imagenReliquia;
    public TextMeshProUGUI textoFeedback;

    [Header("Control del Teclado")]
    [Tooltip("Arrastra aquí tu objeto Contenedor_Botones")]
    public Transform contenedorBotones; // NUEVA VARIABLE PARA EL TECLADO

    [Header("Control de la Misión (UI)")]
    public GameObject panelMisionCompleta;
    public Collider2D zonaInteraccion;

    [Header("Pantallas Finales")]
    public GameObject panelVictoria;
    public GameObject panelDerrota;
    public Button botonReintentar;

    private char[] progresoPalabra;
    private int erroresCometidos = 0;
    private bool juegoTerminado = false;

    void Start()
    {
        InicializarPalabra();

        if (botonReintentar != null)
        {
            botonReintentar.onClick.RemoveAllListeners();
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
            if (char.ToUpper(palabraSecreta[i]) == letraChar && progresoPalabra[i] == '_')
            {
                progresoPalabra[i] = palabraSecreta[i];
                acierto = true;
            }
        }

        if (acierto)
        {
            if (textoFeedback != null)
            {
                textoFeedback.text = "¡Letra correcta!";
                textoFeedback.color = Color.green;
            }
            VerificarVictoria();
        }
        else
        {
            erroresCometidos++;
            if (textoFeedback != null)
            {
                textoFeedback.text = "Esa letra no está en la palabra.";
                textoFeedback.color = Color.red;
            }
            VerificarDerrota();
        }

        ActualizarPantalla();

        if (!juegoTerminado) StartCoroutine(LimpiarFeedback());
    }

    private IEnumerator LimpiarFeedback()
    {
        yield return new WaitForSecondsRealtime(2f);
        if (!juegoTerminado && textoFeedback != null)
        {
            textoFeedback.text = "";
        }
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
        erroresCometidos = 0;
        juegoTerminado = false;

        if (panelDerrota != null) panelDerrota.SetActive(false);
        if (textoFeedback != null) textoFeedback.text = "";

        // NUEVO BLOQUE: Escanea el contenedor y vuelve a encender todos los botones
        if (contenedorBotones != null)
        {
            Button[] botones = contenedorBotones.GetComponentsInChildren<Button>(true);
            foreach (Button btn in botones)
            {
                btn.interactable = true;
            }
        }

        InicializarPalabra();
    }

    IEnumerator CerrarMisionYEncenderLuz()
    {
        yield return new WaitForSecondsRealtime(2.5f);

        Time.timeScale = 1f;

        if (RestorationManager.Instance != null)
        {
            RestorationManager.Instance.RestoreSection(bookshelfIndex);
        }

        if (zonaInteraccion != null) zonaInteraccion.enabled = false;
        if (panelMisionCompleta != null) panelMisionCompleta.SetActive(false);
    }
}