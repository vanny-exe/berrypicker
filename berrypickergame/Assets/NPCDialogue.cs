using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public string[] dialogueLines;
    public bool [] autoProgressLines;
    public bool [] endDialogueLines;
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.05f;

    public DialogueChoice[] choices;

    public int zaagiidiwinInProgressIndex;
    public int zaagiidiwinCompletedIndex;
    public Zaagiidiwin zaagiidiwin;
    
}

[System.Serializable]

public class DialogueChoice
{
    public int dialogueIndex;
    public string[] choices; // choices for player response
    public int[] nextDialogueIndexes; // where choice leads
    public bool[] givesZaagiidiwin; // if choice gives quest
}
