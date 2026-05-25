using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    [Tooltip("El número de orden que representa este espacio (0 para el inicio, 1 para el siguiente...)")]
    public int slotID;

    public void OnDrop(PointerEventData eventData)
    {
        // Detecta qué objeto fue soltado sobre este espacio
        GameObject dropped = eventData.pointerDrag;
        if (dropped != null)
        {
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();

            // Si es un objeto arrastrable y este espacio está vacío, lo acepta
            if (draggableItem != null && transform.childCount == 0)
            {
                draggableItem.parentAfterDrag = transform;
            }
        }
    }
}