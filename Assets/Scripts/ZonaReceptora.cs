using UnityEngine;
using UnityEngine.EventSystems;

public class ZonaReceptora : MonoBehaviour, IDropHandler
{
    public int silabasRequeridas;
    private GameManager miManager;

    void Start()
    {
        miManager = FindFirstObjectByType<GameManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            ItemArrastrable ficha = eventData.pointerDrag.GetComponent<ItemArrastrable>();

            if (ficha != null)
            {
                if (ficha.cantidadSilabas == silabasRequeridas)
                {
                    // Ordena la destrucción inmediata del objeto al acertar
                    if (miManager != null) miManager.RegistrarAcierto();
                    Destroy(ficha.gameObject);
                }
                else
                {
                    // Obliga a la imagen a ejecutar su rutina de retorno al fallar
                    if (miManager != null) miManager.RegistrarError();
                    ficha.RegresarAInventario();
                }
            }
        }
    }
}