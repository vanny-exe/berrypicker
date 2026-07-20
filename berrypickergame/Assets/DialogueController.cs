using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance {get; private set;}
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Update is called once per frame
    public void ShowDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);

    }

    public void SetNPCInfo(string npcName, Sprite portrait)
    {
        nameText.text = npcName; 
        portraitImage.sprite = portrait;
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }
}
