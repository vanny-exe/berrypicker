using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
    Transform originalParent;
    CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; // Save OG parent
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; // Make item semi-transparent
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // Move item with mouse


    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f; // Reset item transparency

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();

        if(dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
            
        }
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if(dropSlot!= null)
        {
            if(dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        
            else
            {
                originalSlot.currentItem = null;
            }
        

        //moveitem into drop slot
        transform.SetParent(dropSlot.transform);
        dropSlot.currentItem = gameObject;
        }
    
    else
    {
        // no slot under drop point 
        transform.SetParent(originalParent);
    }
    GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Reset item position
    }

}
