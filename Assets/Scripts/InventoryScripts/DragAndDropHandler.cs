using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private GameObject draggedItem;

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedItem = gameObject;
        originalParent = transform.parent;
        transform.SetParent(GameObject.Find("Canvas").transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);

        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("SuraSlot"))
        {
            transform.SetParent(eventData.pointerEnter.transform);
        }
    }
}