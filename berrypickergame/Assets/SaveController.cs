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
        
        
    }
    // Update is called once per frame
    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundry = FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems(), 
           
            
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

   

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D = GameObject.Find(saveData.mapBoundry).GetComponent<PolygonCollider2D>();

            inventoryController.SetInventoryItems(saveData.inventorySaveData);

            
        }
        else
        {
            SaveGame();
        }
    }

    
}
