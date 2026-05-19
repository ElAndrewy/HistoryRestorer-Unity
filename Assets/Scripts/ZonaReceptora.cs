using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ZonaReceptora : MonoBehaviour, IDropHandler
{
    [Header("Configuración")]
    public int silabasRequeridas;
    public float tiempoEsperaTransicion = 1.5f;

    [Header("Referencias Visuales")]
    public GameObject iconoCheck;
    public GameObject iconoMal;
    private Image imagenFondo;
    private Color colorOriginal;

    private bool estaOcupada = false;

    private void Awake()
    {
        imagenFondo = GetComponent<Image>();
        if (imagenFondo != null) colorOriginal = imagenFondo.color;

        if (iconoCheck) iconoCheck.SetActive(false);
        if (iconoMal) iconoMal.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (estaOcupada || eventData.pointerDrag == null) return;

        DatosFicha ficha = eventData.pointerDrag.GetComponent<DatosFicha>();
        if (ficha != null)
        {
            ValidarFicha(ficha, eventData.pointerDrag.GetComponent<RectTransform>());
        }
    }

    private void ValidarFicha(DatosFicha ficha, RectTransform transformFicha)
    {
        if (ficha.cantidadSilabas == silabasRequeridas)
        {
            // --- ACIERTO ---

            // Hacer la ficha hija de la canasta y centrarla (para la máscara y el diseño)
            transformFicha.SetParent(this.transform);
            transformFicha.localPosition = Vector3.zero;

            // Bloquear el slot
            estaOcupada = true;

            // Actualizar visuales
            if (iconoCheck) iconoCheck.SetActive(true);
            if (iconoMal) iconoMal.SetActive(false);
            if (imagenFondo) imagenFondo.color = Color.green;

            // Desactivar interacciones en la ficha acertada
            transformFicha.GetComponent<CanvasGroup>().blocksRaycasts = false;

            // Llamar al GameManager e iniciar transición
            GameManager.Instance.RegistrarAcierto();
            StartCoroutine(TransicionSiguienteFicha(transformFicha.gameObject));
        }
        else
        {
            // --- FALLO ---

            // Enviar la ficha de vuelta a su posición original
            ArrastrarUI dragScript = transformFicha.GetComponent<ArrastrarUI>();
            if (dragScript != null)
            {
                dragScript.DevolverAlInventario();
            }

            // Mostrar el error de forma temporal
            StartCoroutine(MostrarErrorTemporal());
        }
    }

    private IEnumerator TransicionSiguienteFicha(GameObject fichaCorrecta)
    {
        yield return new WaitForSeconds(tiempoEsperaTransicion);

        // Limpiar la ficha y resetear el slot
        Destroy(fichaCorrecta);

        if (imagenFondo) imagenFondo.color = colorOriginal;
        if (iconoCheck) iconoCheck.SetActive(false);
        estaOcupada = false;

        // Pedir la siguiente pieza
        GameManager.Instance.VerificarProgreso();
    }

    private IEnumerator MostrarErrorTemporal()
    {
        // Activar estado de error
        if (iconoMal) iconoMal.SetActive(true);
        if (imagenFondo) imagenFondo.color = Color.red;

        yield return new WaitForSeconds(1.5f);

        // Limpiar estado de error
        if (iconoMal) iconoMal.SetActive(false);
        if (imagenFondo) imagenFondo.color = colorOriginal;
    }
}