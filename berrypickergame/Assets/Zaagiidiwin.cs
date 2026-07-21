using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Zaagiidiwin/zaagiidiwin")]

public class Zaagiidiwin : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;
    public List<ZaagiidiwinWin> win;
    // called when scriptable obj is edited
    private void OnValidate()
    {
        if(string.IsNullOrEmpty(questID))
        {
            questID = questName + Guid.NewGuid().ToString();
        }
    }

   
}

 [System.Serializable]

    public class ZaagiidiwinWin
    {
        public string objectiveID; // match with itemID that you need to collect
        public string description;
        public ZaagiType zaagi;
        public int requiredAmount;
        public int currentAmount;

        public bool IsCompleted => currentAmount >= requiredAmount;
    }

    public enum ZaagiType { CollectItem, TalkNPC, ReachLocation, Custom}

    [System.Serializable]
    public class ZaagiidiwinProgress
    {
        public Zaagiidiwin zaagiidiwin;
        public List<ZaagiidiwinWin> win;

        public ZaagiidiwinProgress(Zaagiidiwin zaagiidiwin)
        {
            this.zaagiidiwin = zaagiidiwin;
            win = new List<ZaagiidiwinWin>();

            //deep copy mto avoid modifying original
            foreach(var obj in zaagiidiwin.win)
            {
                win.Add(new ZaagiidiwinWin
                {
                    objectiveID = obj.objectiveID,
                    description = obj.description,
                    zaagi = obj.zaagi,
                    requiredAmount = obj.requiredAmount,
                    currentAmount = 0
                });
            }
        }

        public bool isCompleted => win.TrueForAll(o => o.IsCompleted);

        public string QuestID => zaagiidiwin.questID;
    }
