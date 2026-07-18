using UnityEngine;

public class InventoryController : MonoBehaviour
{

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < slotCount;i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newitem = Instantiate(itemPrefab, slot.transform);
                newitem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newitem;
                return true; // Item added successfully
            }
        }
        
        return false; // Inventory full
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
