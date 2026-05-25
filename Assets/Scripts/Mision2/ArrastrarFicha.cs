using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement == null) layoutElement = gameObject.AddComponent<LayoutElement>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        padreOriginal = transform.parent;
        layoutElement.ignoreLayout = true;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        GameObject objetoTocado = eventData.pointerCurrentRaycast.gameObject;

        // Versión original: Solo verifica si cayó fuera de una zona válida para devolverla.
        // Si cae dentro de la ZonaReceptora, el script de la zona se encarga del resto.
        if (objetoTocado == null || objetoTocado.GetComponentInParent<ZonaReceptora>() == null)
        {
            RegresarAInventario();
        }
    }

    public void RegresarAInventario()
    {
        transform.SetParent(padreOriginal);
        layoutElement.ignoreLayout = false;
    }
}