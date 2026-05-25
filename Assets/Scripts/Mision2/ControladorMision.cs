using UnityEngine;

public class ControladorMision : MonoBehaviour
{
    // Referencias a los contenedores físicos (Tus paneles)
    public Transform canasta1Silaba;
    public Transform canasta2Silabas;

    // Método público que se ejecutará exclusivamente al presionar el botón
    public void ValidarRespuestas()
    {
        bool nivelAprobado = true;
        int fichasEvaluadas = 0; // Para llevar control de que no presionen comprobar con las cajas vacías

        // Iteración sobre los hijos (fichas) contenidos en la Canasta 1
        foreach (Transform hijo in canasta1Silaba)
        {
            DatosFicha datos = hijo.GetComponent<DatosFicha>();
            if (datos != null)
            {
                fichasEvaluadas++;
                // Cuestionamiento lógico: Si está en la canasta 1, debe tener 1 sílaba.
                if (datos.cantidadSilabas != 1)
                {
                    nivelAprobado = false;
                }
            }
        }

        // Iteración sobre los hijos contenidos en la Canasta 2
        foreach (Transform hijo in canasta2Silabas)
        {
            DatosFicha datos = hijo.GetComponent<DatosFicha>();
            if (datos != null)
            {
                fichasEvaluadas++;
                if (datos.cantidadSilabas != 2)
                {
                    nivelAprobado = false;
                }
            }
        }

        // Validación final
        if (fichasEvaluadas == 0)
        {
            Debug.LogWarning("El jugador presionó comprobar sin colocar fichas.");
            return; // Cortamos la ejecución aquí
        }

        if (nivelAprobado)
        {
            Debug.Log("¡Respuestas correctas! Transición al siguiente nivel.");
            // Aquí en el futuro programarás la pantalla de victoria
        }
        else
        {
            Debug.Log("Hay errores en la clasificación. Retroalimentación para el jugador.");
            // Aquí programarás que las fichas incorrectas regresen al inventario
        }
    }
}