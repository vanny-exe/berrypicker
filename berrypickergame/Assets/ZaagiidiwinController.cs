using System;
using System.Collections.Generic;
using UnityEngine;

public class ZaagiidiwinController : MonoBehaviour
{
    public static ZaagiidiwinController Instance { get; private set;}
    public List<ZaagiidiwinProgress> activateZaagiidiwins = new();
    private ZaagiidiwinUI zaagiidiwinUI;

    public List<string> Caus = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        zaagiidiwinUI = FindFirstObjectByType<ZaagiidiwinUI>();
        InventoryController.Instance.OnInventoryChanged += CheckInventoryForZaagiidiwins;

    }

    public void AcceptZaagiidiwin(Zaagiidiwin zaagiidiwin)
    {
        if(IsZaagiidiwinActive(zaagiidiwin.zaagiidiwinID)) return;
        activateZaagiidiwins.Add(new ZaagiidiwinProgress(zaagiidiwin));

        CheckInventoryForZaagiidiwins();
        zaagiidiwinUI.UpdateZaagiidiwinUI();
    } 

    public bool IsZaagiidiwinActive(string zaagiidiwinID) => activateZaagiidiwins.Exists(q => q.ZaagiidiwinID == zaagiidiwinID);

    public void CheckInventoryForZaagiidiwins()
    {
        Dictionary<int, int> itemCounts = InventoryController.Instance.GetItemCounts();
        foreach(ZaagiidiwinProgress zaagiidiwin in activateZaagiidiwins)
        {
            foreach(ZaagiidiwinWin zaagiidiwinWin in zaagiidiwin.win)
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

    public bool IsZaagiidiwinCompleted(string zaagiidiwinID)
    {
        ZaagiidiwinProgress zaagiidiwin = activateZaagiidiwins.Find(q => q.ZaagiidiwinID == zaagiidiwinID);
        return zaagiidiwin != null && zaagiidiwin.win.TrueForAll(o => o.IsCompleted);
    }

    public void HandInZaagiidiwin(string zaagiidiwinID)
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
            Caus.Add(zaagiidiwinID);
            activateZaagiidiwins.Remove(zaagiidiwin);
            zaagiidiwinUI.UpdateZaagiidiwinUI();
        }
    }

    public bool IsZaagiidiwinHandedIn(string zaagiidiwinID)
    {
        return Caus.Contains(zaagiidiwinID);
    }
    public bool RemoveRequiredItemsFromInventory(string zaagiidiwinID)
    {
        ZaagiidiwinProgress zaagiidiwin = activateZaagiidiwins.Find(q => q.ZaagiidiwinID == zaagiidiwinID);
        if (zaagiidiwin == null) return false;

        Dictionary<int, int> requiredItems = new();

        //item requirements from objective
        foreach(ZaagiidiwinWin win in zaagiidiwin.win)
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
