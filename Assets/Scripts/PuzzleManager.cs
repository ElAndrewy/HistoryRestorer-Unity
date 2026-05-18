using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    [Header("Configuración del Puzzle")]
    public int bookshelfIndex;
    public GameObject puzzlePanel;
    public List<DropSlot> sequenceSlots;

    [Header("Feedback Visual")]
    public TextMeshProUGUI feedbackText;

    // SOLUCIÓN PUNTO 4: El método ClosePuzzle está de vuelta
    public void ClosePuzzle()
    {
        Time.timeScale = 1f;
        puzzlePanel.SetActive(false);
        if (feedbackText != null) feedbackText.text = "";
    }

    public void CheckAnswer()
    {
        int correctPieces = 0;
        List<Image> pieceImages = new List<Image>();

        foreach (DropSlot slot in sequenceSlots)
        {
            if (slot.transform.childCount > 0)
            {
                DraggableItem item = slot.transform.GetChild(0).GetComponent<DraggableItem>();
                if (item != null)
                {
                    pieceImages.Add(item.GetComponent<Image>());

                    if (item.correctSlotID == slot.slotID)
                    {
                        correctPieces++;
                    }
                }
            }
        }

        if (correctPieces == sequenceSlots.Count)
        {
            feedbackText.text = "¡EXCELENTE! Has restaurado la historia.";
            feedbackText.color = Color.green;

            foreach (Image img in pieceImages) img.color = Color.green;

            Invoke("ExecuteWinCondition", 2f);
        }
        else
        {
            feedbackText.text = "Mmm... algo no cuadra. ¡Revisa el orden!";
            feedbackText.color = Color.red;

            // SOLUCIÓN PUNTO 3: Usamos una corrutina para asegurar el cambio de color
            StartCoroutine(ResetColorRoutine(pieceImages));
        }
    }

    // Este es el temporizador seguro que apaga el rojo después de 1 segundo
    private IEnumerator ResetColorRoutine(List<Image> imagesToReset)
    {
        foreach (Image img in imagesToReset) img.color = Color.red;

        // Ajustado a 1.5 segundos exactos según tu requerimiento
        yield return new WaitForSecondsRealtime(1.5f);

        foreach (Image img in imagesToReset)
        {
            if (img != null) img.color = Color.white;
        }

        // NUEVO: Limpiamos el texto de fallo de la pantalla
        if (feedbackText != null) feedbackText.text = "";
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