using System;
using System.Collections.Generic;
using UnityEngine;

public class ZaagiidiwinController : MonoBehaviour
{
    public static ZaagiidiwinController Instance { get; private set;}
    public List<ZaagiidiwinProgress> activateZaagiidiwins = new();
    private ZaagiidiwinUI zaagiidiwinUI;

    public List<string> CausZaagiidiwinIDs = new(); // completed quests IDs

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        zaagiidiwinUI = FindFirstObjectByType<ZaagiidiwinUI>();
        InventoryController.Instance.OnInventoryChanged += CheckInventoryForZaagiidiwins;

    }

    // accepting quest 
    public void AcceptZaagiidiwin(Zaagiidiwin zaagiidiwin)
    {
        if(IsZaagiidiwinActive(zaagiidiwin.zaagiidiwinID)) return;
        activateZaagiidiwins.Add(new ZaagiidiwinProgress(zaagiidiwin));

        CheckInventoryForZaagiidiwins();
        zaagiidiwinUI.UpdateZaagiidiwinUI();
    } 

    
    public bool IsZaagiidiwinActive(string zaagiidiwinID) => activateZaagiidiwins.Exists(q => q.ZaagiidiwinID == zaagiidiwinID);

    // for item quests, checks inventory to see if player has all items
    public void CheckInventoryForZaagiidiwins()
    {
        Dictionary<int, int> itemCounts = InventoryController.Instance.GetItemCounts();
        foreach(ZaagiidiwinProgress zaagiidiwin in activateZaagiidiwins)
        {
            foreach(Win zaagiidiwinWin in zaagiidiwin.win)
            {
                if(zaagiidiwinWin.zaagi != Zaagi.CollectItem) continue;
                if(!int.TryParse(zaagiidiwinWin.winID, out int itemID)) continue;

                int newAmount = itemCounts.TryGetValue(itemID, out int count) ? Mathf.Min(count, zaagiidiwinWin.requiredAmount) : 0;

                if(zaagiidiwinWin.currentAmount != newAmount)
                {
                    zaagiidiwinWin.currentAmount = newAmount;
                }
            }
        }

        zaagiidiwinUI.UpdateZaagiidiwinUI();
    }
    // checks to see if the quest is completed
    public bool IsZaagiidiwinCompleted(string zaagiidiwinID)
    {
        ZaagiidiwinProgress zaagiidiwin = activateZaagiidiwins.Find(q => q.ZaagiidiwinID == zaagiidiwinID);
        return zaagiidiwin != null && zaagiidiwin.win.TrueForAll(o => o.IsCompleted);
    }

    // allows player to complete the quest and removes the quest items and the quest from the ui 
    public void Caus(string zaagiidiwinID) // hand in quest, old: public void HandInQuest(string questID)
    {
        //remove items
       if(!RemoveRequiredItemsFromInventory(zaagiidiwinID))
        {
            return;
        }

        //remove quest
        ZaagiidiwinProgress zaagiidiwin = activateZaagiidiwins.Find(q => q.ZaagiidiwinID == zaagiidiwinID);
        if(zaagiidiwin != null)
        {
            CausZaagiidiwinIDs.Add(zaagiidiwinID);
            activateZaagiidiwins.Remove(zaagiidiwin);
            zaagiidiwinUI.UpdateZaagiidiwinUI();
        }
    }

    // checks to see if the quest handed in
    public bool IsZaagiidiwinCaus(string zaagiidiwinID)
    {
        return CausZaagiidiwinIDs.Contains(zaagiidiwinID);
    }
    public bool RemoveRequiredItemsFromInventory(string zaagiidiwinID)
    {
        ZaagiidiwinProgress zaagiidiwin = activateZaagiidiwins.Find(q => q.ZaagiidiwinID == zaagiidiwinID);
        if (zaagiidiwin == null) return false;

        Dictionary<int, int> requiredItems = new();

        //item requirements from objective
        foreach(Win win in zaagiidiwin.win)
        {
            if(win.zaagi == Zaagi.CollectItem && int.TryParse(win.winID, out int itemID))
            {
                requiredItems[itemID] = win.requiredAmount;
            }
        }

        Dictionary<int,int> itemCounts = InventoryController.Instance.GetItemCounts();
        foreach(var item in requiredItems)
        {
            if(itemCounts.GetValueOrDefault(item.Key) < item.Value)
            {
                return false;
            }

        }

        foreach (var itemRequirement in requiredItems)
        {
            // remove from inventory
            InventoryController.Instance.RemoveItemsFromInventory(itemRequirement.Key, itemRequirement.Value);
        }

        return true;

    }

    public void LoadZaagiidiwinProgress(List<ZaagiidiwinProgress> savedZaagiidiwins)
    {
        activateZaagiidiwins = savedZaagiidiwins ?? new();

        CheckInventoryForZaagiidiwins();
        zaagiidiwinUI.UpdateZaagiidiwinUI();
    }

}
