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

        // Apaga la detección de rayos para que el ratón detecte el 'DropSlot' que está debajo
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // El objeto sigue al cursor
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Retorna a su contenedor padre (ya sea el original o un nuevo DropSlot)
        transform.SetParent(parentAfterDrag);
        canvasGroup.blocksRaycasts = true;
    }
}