using UnityEngine;
using UnityEngine.EventSystems;

public class ZonaReceptora : MonoBehaviour, IDropHandler
{
    [Header("Configuración del Slot")]
    [Tooltip("Establece si este slot recibe palabras de 1 o 2 sílabas")]
    public int silabasRequeridas;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            ArrastrarFicha scriptArrastre = eventData.pointerDrag.GetComponent<ArrastrarFicha>();
            DatosFicha datosFicha = eventData.pointerDrag.GetComponent<DatosFicha>();

            if (scriptArrastre != null && datosFicha != null)
            {
                // Validación analítica de los datos
                if (datosFicha.cantidadSilabas == silabasRequeridas)
                {
                    // RESPUESTA CORRECTA: El nuevo padre de la ficha es esta canasta
                    scriptArrastre.padreAlSoltar = this.transform;

                    // Aquí es donde se deben instanciar los efectos visuales de éxito
                    EfectoAcierto();
                }
                else
                {
                    // RESPUESTA INCORRECTA: No modificamos 'padreAlSoltar'
                    // El script ArrastrarFicha detectará esto y la devolverá al inventario
                    EfectoError();
                }
            }
        }
    }

    private void EfectoAcierto()
    {
        Debug.Log($"¡Correcto! Ficha guardada en el slot de {silabasRequeridas} sílabas.");
        // Próximo paso: Instanciar prefab de chispitas/confeti aquí
    }

    private void EfectoError()
    {
        Debug.LogWarning("Respuesta incorrecta. La ficha regresa al inventario.");
        // Próximo paso: Reproducir sonido 'Boing' aquí
    }
}