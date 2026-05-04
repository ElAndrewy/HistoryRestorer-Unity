using UnityEngine;

public class BookshelfInteract : MonoBehaviour
{
    [Header("Configuración de la Estantería")]
    [Tooltip("Índice de esta estantería (0, 1, 2...) para el RestorationManager")]
    public int shelfIndex;

    private bool isPlayerInRange = false;
    private bool isChallengeCompleted = false; // Nueva variable de estado

    void Update()
    {
        // Detecta interacción solo si no se ha completado el desafío
        if (isPlayerInRange && !isChallengeCompleted && Input.GetKeyDown(KeyCode.E))
        {
            // En el juego real, esto abriría el minijuego UI
            TriggerChallenge();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isChallengeCompleted)
        {
            isPlayerInRange = true;
            // Aquí podrías encender un pequeño icono de "Presiona E" sobre la cabeza del jugador
            Debug.Log("Jugador en rango. Presiona 'E' para restaurar esta estantería.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void TriggerChallenge()
    {
        Debug.Log($"¡Iniciando desafío en estantería #{shelfIndex}!");

        // --- SIMULACIÓN DEL DESAFÍO ---
        // En el juego final, esta función pausará el movimiento del jugador,
        // abrirá una ventana de UI con el puzzle, y esperará la respuesta.

        // Por ahora, simularemos que el niño respondió bien instantáneamente:
        OnChallengeSuccess();
        // ------------------------------
    }

    // Función que llamará tu sistema de UI/Puzzle al ganar
    public void OnChallengeSuccess()
    {
        if (isChallengeCompleted) return; // Evita restaurar múltiples veces

        isChallengeCompleted = true; // Marcamos como completada

        // Le avisamos al cerebro central (Manager) que encienda esta sección
        if (RestorationManager.Instance != null)
        {
            RestorationManager.Instance.RestoreSection(shelfIndex);
        }
        else
        {
            Debug.LogError("No se encontró el RestorationManager en la escena.");
        }

        // Aquí podrías cambiar el sprite de la estantería de "gris" a "colorido"
        // GetComponent<SpriteRenderer>().color = Color.white; // Si usabas un tono gris previo
    }
}