using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    [Header("Configuración")]
    public int bookshelfIndex;
    public GameObject puzzlePanel;
    public List<DropSlot> sequenceSlots;

    [Header("Feedback Visual")]
    public TextMeshProUGUI feedbackText;

    // Método para el botón Cerrar
    public void ClosePuzzle()
    {
        Time.timeScale = 1f;
        puzzlePanel.SetActive(false);
        if (feedbackText != null) feedbackText.text = "";
    }

    // Método para el botón Comprobar
    public void CheckAnswer()
    {
        int correctPieces = 0;

        foreach (DropSlot slot in sequenceSlots)
        {
            if (slot.transform.childCount > 0)
            {
                DraggableItem item = slot.transform.GetChild(0).GetComponent<DraggableItem>();
                if (item != null && item.correctSlotID == slot.slotID)
                {
                    correctPieces++;
                }
            }
        }

        if (correctPieces == sequenceSlots.Count)
        {
            if (feedbackText != null)
            {
                feedbackText.text = "¡Excelente! Has restaurado la historia.";
                feedbackText.color = Color.green;
            }
            Invoke("ExecuteWinCondition", 2f);
        }
        else
        {
            if (feedbackText != null)
            {
                feedbackText.text = "Mmm... algo no cuadra. ¡Revisa el orden e inténtalo de nuevo!";
                feedbackText.color = Color.red;
            }
        }
    }

    private void ExecuteWinCondition()
    {
        Time.timeScale = 1f;
        puzzlePanel.SetActive(false);
        if (feedbackText != null) feedbackText.text = "";

        if (RestorationManager.Instance != null)
        {
            RestorationManager.Instance.RestoreSection(bookshelfIndex);
        }
    }
}