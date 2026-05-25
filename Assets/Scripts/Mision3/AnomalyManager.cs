using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class AnomalyManager : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    public int bookshelfIndex = 2; // Índice 2 = Tercera estantería (Roma)
    public GameObject puzzlePanel;
    public int totalAnomalies = 4;

    [Header("Puntuación")]
    public int scorePerHit = 10;
    public int penaltyPerMiss = 5;
    private int currentScore = 0;
    private int anomaliesFound = 0;

    [Header("UI Elementos")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI feedbackText;

    [Header("Sistema de Pistas")]
    public string[] writtenHints;
    private int currentHintIndex = 0;

    void Start()
    {
        UpdateScoreUI();
        if (feedbackText != null)
        {
            feedbackText.text = "Encuentra los objetos que no pertenecen a la Antigua Roma.";
            feedbackText.color = Color.white;
        }
    }

    public void ClickOnAnomaly(Button clickedButton)
    {
        anomaliesFound++;
        currentScore += scorePerHit;
        UpdateScoreUI();

        clickedButton.gameObject.SetActive(false);

        if (feedbackText != null)
        {
            feedbackText.text = "¡Correcto! Ese objeto no existía en esta época.";
            feedbackText.color = Color.green;
        }

        if (anomaliesFound >= totalAnomalies)
        {
            ExecuteWinCondition();
        }
    }

    public void ClickOnBackground()
    {
        if (currentScore >= penaltyPerMiss)
        {
            currentScore -= penaltyPerMiss;
        }
        else
        {
            currentScore = 0;
        }

        UpdateScoreUI();

        if (feedbackText != null)
        {
            feedbackText.text = "Ese lugar está bien. ¡Sigue buscando el anacronismo!";
            feedbackText.color = Color.red;
        }
    }

    public void UseHint()
    {
        if (currentHintIndex < writtenHints.Length)
        {
            if (feedbackText != null)
            {
                feedbackText.text = "Pista: " + writtenHints[currentHintIndex];
                feedbackText.color = Color.yellow;
            }
            currentHintIndex++;
        }
        else
        {
            if (feedbackText != null)
            {
                feedbackText.text = "Ya no te quedan más pistas.";
                feedbackText.color = Color.gray;
            }
        }
    }

    public void ClosePuzzle()
    {
        Time.timeScale = 1f;
        if (puzzlePanel != null) puzzlePanel.SetActive(false);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + currentScore;
        }
    }

    private void ExecuteWinCondition()
    {
        if (feedbackText != null)
        {
            feedbackText.text = "¡Línea temporal restaurada! Puntuación: " + currentScore;
            feedbackText.color = Color.cyan;
        }

        // CORRECCIÓN: Forma segura de desactivar clics sin romper el juego
        GraphicRaycaster raycaster = GetComponentInChildren<GraphicRaycaster>();
        if (raycaster != null)
        {
            raycaster.enabled = false;
        }

        StartCoroutine(WinDelay());
    }

    private IEnumerator WinDelay()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        Time.timeScale = 1f;

        // CORRECCIÓN DE ORDEN: 1. Primero se enciende la luz
        if (RestorationManager.Instance != null)
        {
            RestorationManager.Instance.RestoreSection(bookshelfIndex);
        }

        // CORRECCIÓN DE ORDEN: 2. El cierre automático se hace estrictamente al final
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(false);
        }
    }
}