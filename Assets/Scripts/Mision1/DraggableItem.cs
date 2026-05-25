using UnityEngine;
using UnityEngine.EventSystems;

// CanvasGroup es vital para gestionar si el ratón "atraviesa" el objeto al arrastrarlo
[RequireComponent(typeof(CanvasGroup))]
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Tooltip("El número de orden correcto de este fragmento (0, 1, 2...)")]
    public int correctSlotID;

    [HideInInspector] public Transform parentAfterDrag;
    private CanvasGroup canvasGroup;

    private Canvas myCanvas; // Referencia a su propio canvas (Sub-Canvas)
    private const int SORT_ORDER_DRAGGING = 9999; // Un número masivo
    private const int SORT_ORDER_REST = 0; // Estándar al soltar
    void Start()
    {
        // ... (lógica existente si hay)

        // Obtenemos la referencia a su propio componente Canvas
        myCanvas = GetComponent<Canvas>();

        // Por seguridad, aseguramos que Override Sorting esté activo
        if (myCanvas != null)
        {
            myCanvas.overrideSorting = true;
            myCanvas.sortingOrder = SORT_ORDER_REST; // Empieza en 0
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;

        // Mueve el objeto a la raíz del Canvas para que se renderice por encima de todo
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        // SOLUCIÓN TÉCNICA DEFINITIVA (Punto 2 de tus errores):
        // Activamos un Sub-Canvas Z-Sorting masivo durante el arrastre
        if (myCanvas != null)
        {
            myCanvas.sortingOrder = SORT_ORDER_DRAGGING;
        }

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // El objeto sigue al cursor
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);

        // SOLUCIÓN TÉCNICA DEFINITIVA:
        // Restauramos el sorting estándar para que las máscaras estándar y Layout Groups funcionen a rest
        if (myCanvas != null)
        {
            myCanvas.sortingOrder = SORT_ORDER_REST;
        }

        canvasGroup.blocksRaycasts = true;
    }
}