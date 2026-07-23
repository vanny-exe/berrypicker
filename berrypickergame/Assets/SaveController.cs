using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Unity.Cinemachine;


public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private Chest[] chests;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Define save location 
        InitializeComponents();
        LoadGame();
       

    }

    private void InitializeComponents()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindFirstObjectByType<InventoryController>();
        chests = FindObjectsByType<Chest>(FindObjectsSortMode.InstanceID);
        
        
    }
    // Update is called once per frame
    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundry = FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems(), 
            chestSaveData = GetChestsState(),
            zaagiidiwinProgressData = ZaagiidiwinController.Instance.activateZaagiidiwins,
            CausZaagiidiwinIDs = ZaagiidiwinController.Instance.CausZaagiidiwinIDs
           
            
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

   private List<ChestSaveData> GetChestsState()
    {
        List<ChestSaveData> chestStates = new List<ChestSaveData>();

        foreach(ChestSaveData chest in chestStates)
        {
            ChestSaveData chestSaveData = new ChestSaveData
            {
                chestID = chest.chestID,
                isOpened = chest.isOpened
            };
            chestStates.Add(chestSaveData);
        }
        return chestStates;
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D = GameObject.Find(saveData.mapBoundry).GetComponent<PolygonCollider2D>();

            inventoryController.SetInventoryItems(saveData.inventorySaveData);
            LoadChestStates(saveData.chestSaveData);

            ZaagiidiwinController.Instance.LoadZaagiidiwinProgress(saveData.zaagiidiwinProgressData);
            ZaagiidiwinController.Instance.CausZaagiidiwinIDs = saveData.CausZaagiidiwinIDs;

            
        }
        else
        {
            SaveGame();
        }
    }

    private void LoadChestStates(List<ChestSaveData> chestStates)
    {
        foreach(Chest chest in chests)
        {
            ChestSaveData chestSaveData = chestStates.FirstOrDefault(c => c.chestID == chest.ChestID);

            if (chestSaveData != null)
            {
                chest.SetOpened(chestSaveData.isOpened);
            }
        }
    }

    
}
