using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Requerido para manipular el Layout

public class ItemArrastrable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int cantidadSilabas;

    private Transform padreOriginal;
    private CanvasGroup canvasGroup;
    private LayoutElement layoutElement;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Busca o inyecta el componente que controla la relación con el inventario
        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement == null) layoutElement = gameObject.AddComponent<LayoutElement>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        padreOriginal = transform.parent;

        // 1. Apaga la fuerza magnética del inventario
        layoutElement.ignoreLayout = true;

        // 2. Saca la imagen del panel temporalmente para que flote libre sobre el Canvas
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Movimiento atado estrictamente al cursor
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        GameObject objetoTocado = eventData.pointerCurrentRaycast.gameObject;

        // Verifica rigurosamente si el impacto fue sobre una zona válida
        if (objetoTocado == null || objetoTocado.GetComponentInParent<ZonaReceptora>() == null)
        {
            RegresarAInventario();
        }
    }

    public void RegresarAInventario()
    {
        // Reconecta la imagen al inventario y reactiva las reglas del Layout Group
        transform.SetParent(padreOriginal);
        layoutElement.ignoreLayout = false;
    }
}