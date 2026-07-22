using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Zaagiidiwin/zaagiidiwin")]

public class Zaagiidiwin : ScriptableObject
{
    public string zaagiidiwinID; // questID
    public string zaagiidiwinName; // questName
    public string description; // 
    public List<ZaagiidiwinWin> win; // <QuestObjective> objective
    public List<ZaagIdi> idi; //QuestReward
    
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

    public class ZaagiidiwinWin
    {
        public string winID; // match with itemID that you need to collect
        public string description;
        public Zaag zaag; // ObjectiveType type
        public int requiredAmount;
        public int currentAmount;

        public bool IsCompleted => currentAmount >= requiredAmount;
    }

    public enum Zaag { CollectItem, TalkNPC, ReachLocation, Custom}

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
                    winID = obj.winID,
                    description = obj.description,
                    zaag = obj.zaag,
                    requiredAmount = obj.requiredAmount,
                    currentAmount = 0
                });
            }
        }

        public bool isCompleted => win.TrueForAll(o => o.IsCompleted);

        public string ZaagiidiwinID => zaagiidiwin.zaagiidiwinID;
    }

[System.Serializable]

public class ZaagIdi  // QuestReward
{
    public Idi idi; //RewardType type
    public int idiID; // rewardID
    public int amount = 1;
}

public enum Idi { Item, Restore, Custom} //RewardType
