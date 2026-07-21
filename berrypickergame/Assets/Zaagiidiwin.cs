using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Zaagiidiwin/zaagiidiwin")]

public class Zaagiidiwin : ScriptableObject
{
    public string zaagiidiwinID;
    public string zaagiidiwinName;
    public string description;
    public List<ZaagiidiwinWin> win;
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
        public Zaagi zaagi;
        public int requiredAmount;
        public int currentAmount;

        public bool IsCompleted => currentAmount >= requiredAmount;
    }

    public enum Zaagi { CollectItem, TalkNPC, ReachLocation, Custom}

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
                    zaagi = obj.zaagi,
                    requiredAmount = obj.requiredAmount,
                    currentAmount = 0
                });
            }
        }

        public bool isCompleted => win.TrueForAll(o => o.IsCompleted);

        public string ZaagiidiwinID => zaagiidiwin.zaagiidiwinID;
    }
