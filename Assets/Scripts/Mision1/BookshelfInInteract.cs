using UnityEngine;

public class BookshelfInteract : MonoBehaviour
{
    public int shelfIndex;
    public GameObject puzzleUI; // NUEVO: Asigna el Panel_Desafio1 en el inspector

    private bool isPlayerInRange = false;
    private bool isChallengeCompleted = false;

    void Update()
    {
        // Chivato 1: ¿Sabe que estás ahí?
        if (isPlayerInRange)
        {
            Debug.Log("El jugador está en el rango. Esperando la tecla E...");

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("¡Tecla E presionada!");
                if (!isChallengeCompleted)
                {
                    TriggerChallenge();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Chivato 2: ¿Qué está tocando la estantería?
        Debug.Log("ALGO acaba de tocar el Trigger de la estantería: " + collision.gameObject.name);

        if (collision.CompareTag("Player") && !isChallengeCompleted)
        {
            isPlayerInRange = true;
            Debug.Log("Ese ALGO era el Player. isPlayerInRange ahora es TRUE.");
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
        // Enciende la UI del puzzle y congela el juego
        puzzleUI.SetActive(true);
        Time.timeScale = 0f;
    }
}