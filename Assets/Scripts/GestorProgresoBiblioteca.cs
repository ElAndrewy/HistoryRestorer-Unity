using UnityEngine;

public class GestorProgresoBiblioteca : MonoBehaviour
{
    [Header("Indicadores de Progreso Visual")]
    public GameObject luzMision4; // El objeto físico de la luz (foco, destello o partículas)

    void Start()
    {
        // Al cargar la biblioteca, revisamos la memoria del dispositivo
        // Si la clave no existe, por defecto devolverá 0 (Falso)
        int estadoMision4 = PlayerPrefs.GetInt("Mision4Completada", 0);

        if (estadoMision4 == 1)
        {
            // Si el valor es 1, la misión fue hecha: encendemos el objeto de la luz
            if (luzMision4 != null) luzMision4.SetActive(true);
        }
        else
        {
            // Si es 0, el jugador no la ha completado: mantenemos la luz apagada
            if (luzMision4 != null) luzMision4.SetActive(false);
        }
    }

    // Función opcional por si quieren un botón en el menú para borrar el progreso y probar desde cero
    public void BorrarProgresoParaPruebas()
    {
        PlayerPrefs.DeleteKey("Mision4Completada");
        Debug.Log("Progreso reseteado para pruebas.");
    }
}