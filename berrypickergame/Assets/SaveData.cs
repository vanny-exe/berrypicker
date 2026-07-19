using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public string mapBoundry; // the boundry name for the map

    public List<InventorySaveData> inventorySaveData; 
}
