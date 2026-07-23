using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Zaagiidiwin/zaagiidiwin")]

public class Zaagiidiwin : ScriptableObject
{
    public string zaagiidiwinID; // questID for saving purposes
    public string zaagiidiwinName; // questName that appears in the game
    public string description; // describes quest - for developper, player will not see
    public List<Win> win; // <QuestObjective> objective, 
    public List<ZaagiIdi> idi; //QuestReward, combining morphemes for continuity
    
    // called when scriptable obj is edited
    private void OnValidate()
    {
        if(string.IsNullOrEmpty(zaagiidiwinID))
        {
            zaagiidiwinID = zaagiidiwinName + Guid.NewGuid().ToString();
        }
    }  
}

 [System.Serializable]

    public class Win // quest objective
    {
        public string winID; // match with itemID that you need to collect
        public string description;
        public Zaagi zaagi; // ObjectiveType type
        public int requiredAmount;
        public int currentAmount;

        public bool IsCompleted => currentAmount >= requiredAmount;
    }

    public enum Zaagi { CollectItem, TalkNPC, ReachLocation, Custom}

    [System.Serializable]
    public class ZaagiidiwinProgress
    {
        public Zaagiidiwin zaagiidiwin;
        public List<Win> win;

        public ZaagiidiwinProgress(Zaagiidiwin zaagiidiwin)
        {
            this.zaagiidiwin = zaagiidiwin;
            win = new List<Win>();

            //deep copy mto avoid modifying original
            foreach(var obj in zaagiidiwin.win)
            {
                win.Add(new Win
                {
                    winID = obj.winID,
                    description = obj.description,
                    zaagi = obj.zaagi,
                    requiredAmount = obj.requiredAmount,
                    currentAmount = 0
                });
            }
        }

        public bool isCompleted => win.TrueForAll(o => o.IsCompleted);

        public string ZaagiidiwinID => zaagiidiwin.zaagiidiwinID;
    }

[System.Serializable]

public class ZaagiIdi  // QuestReward, need to combine morphemes, cnanot have Idi alone (will interfere with Idi idi)
{
    public Idi idi; //RewardType type
    public int idiID; // rewardID, unique
    public int amount = 1;
}

public enum Idi { Item, Restore, Custom} //RewardType that appears in Unity Editor 
