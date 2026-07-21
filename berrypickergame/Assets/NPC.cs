using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable 
{
    public NPCDialogue dialogueData;
    private DialogueController dialogueUI;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    private enum ZaagiidiwinState { NotStarted, InProgress, Completed }
    private ZaagiidiwinState zaagiidiwinState = ZaagiidiwinState.NotStarted;


    private void Start()

    {
        dialogueUI = DialogueController.instance;
    }
    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if(dialogueData == null || (PauseController.IsGamePaused && !isDialogueActive))
        return;

        if (isDialogueActive)
        {
            NextLine();
        }

        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        //sync in with quest data
        SyncZaagiidiwinState();
        // set dialogue line based on questSTate
        if(zaagiidiwinState == ZaagiidiwinState.NotStarted)
        {
            dialogueIndex = 0;
        }

        else if(zaagiidiwinState == ZaagiidiwinState.InProgress)
        {
            dialogueIndex = dialogueData.zaagiidiwinInProgressIndex;
        }

        else if (zaagiidiwinState == ZaagiidiwinState.Completed)
        {
            dialogueIndex = dialogueData.zaagiidiwinCompletedIndex;
        }



        isDialogueActive = true;
        
        PauseController.SetPause(true);

        dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);
        dialogueUI.ShowDialogueUI(true);
        
        

        DisplayCurrentLine();
    }

    private void SyncZaagiidiwinState()
    {
        if(dialogueData.zaagiidiwin == null) return;
        
        string zaagiidiwinID = dialogueData.zaagiidiwin.zaagiidiwinID;

        //future update add completing quest and handing in
        if(ZaagiidiwinController.Instance.IsZaagiidiwinActive(zaagiidiwinID))
        {
            zaagiidiwinState = ZaagiidiwinState.InProgress;
        }
        else
        {
            zaagiidiwinState = ZaagiidiwinState.NotStarted;
        }
    }

    void NextLine()
    {
        if(isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }

        //clear choice
        dialogueUI.ClearChoices();
        // check end dialogue
        if(dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }
        // check if choices and display 
        foreach(DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if(dialogueChoice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
        }

        if (++dialogueIndex + 1 < dialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {

            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
        if(dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }

    }

    void DisplayChoices(DialogueChoice choice)
    {
        for(int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            bool givesZaagiidiwin = choice.givesZaagiidiwin[i];
            dialogueUI.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex, givesZaagiidiwin));

        }
    }

    void ChooseOption(int nextIndex, bool givesZaagiidiwin)
    {

        if (givesZaagiidiwin)
        {
            ZaagiidiwinController.Instance.AcceptZaagiidiwin(dialogueData.zaagiidiwin);
            zaagiidiwinState = ZaagiidiwinState.InProgress;
        }

        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
        DisplayCurrentLine();
    }
    
    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }
    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);
        PauseController.SetPause(false) ;
    
    }
}
