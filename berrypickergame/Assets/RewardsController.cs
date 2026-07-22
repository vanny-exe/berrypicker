using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class RewardsController : MonoBehaviour
{
    public static RewardsController Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GiveZaagiIdi(Zaagiidiwin zaagiidiwin)
    {
        if (zaagiidiwin?.idi == null) return;

        foreach(var reward in zaagiidiwin.idi)
        {
            switch (reward.idi)
            {
                case Idi.Item:
                GiveItemReward(reward.idiID, reward.amount);
                break;
                case Idi.Restore:
                break;
                case Idi.Custom:
                break;

            }
        }
    }

    public void GiveItemReward(int itemID, int amount)
    {
        var itemPrefab = FindAnyObjectByType<ItemDictionary>()?.GetItemPrefab(itemID);

        if(itemPrefab == null) return;
        for(int i = 0; i < amount; i++)
        {
            if(!InventoryController.Instance.AddItem(itemPrefab))
            {
                GameObject dropITem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
            }
            else
            {
                itemPrefab.GetComponent<Item>().ShowPopup();
            }
        }
    }

}
