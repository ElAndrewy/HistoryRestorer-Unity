using UnityEngine;
using UnityEngine.EventSystems;

public class ArrastrarFicha : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform padreAlSoltar;

    private Transform padreOriginal;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        // El inventario inicial se guarda como su base segura permanente
        padreOriginal = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Por defecto, si el usuario la suelta en el aire, regresa al inventario
        padreAlSoltar = padreOriginal;

        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling(); // Se dibuja al frente de todo durante el arrastre

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // Se asigna al padre definitivo (La canasta si acertó, o el inventario si falló)
        transform.SetParent(padreAlSoltar);

        // Si regresa al inventario, el Layout Group del inventario la reubicará automáticamente
    }
}