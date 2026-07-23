using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZaagiidiwinUI : MonoBehaviour
{
    public Transform zaagiidiwinListContent;
    public GameObject zaagiidiwinEntryPrefab;
    public GameObject winTextPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateZaagiidiwinUI();
    }
   

    // Update is called once per frame
    public void UpdateZaagiidiwinUI()
    {
        //destroy any existing zaagiidiwin entries
        foreach(Transform child in zaagiidiwinListContent)
        {
            Destroy(child.gameObject);
        }

        //build zaagiidiwin (quest) entries
        foreach(var zaagiidiwin in ZaagiidiwinController.Instance.activateZaagiidiwins)
        {
            GameObject entry = Instantiate(zaagiidiwinEntryPrefab, zaagiidiwinListContent);
            TMP_Text zaagiidiwinNameText = entry.transform.Find("ZaagiidiwinName").GetComponent<TMP_Text>();
            Transform winList = entry.transform.Find("WinList");

            zaagiidiwinNameText.text = zaagiidiwin.zaagiidiwin.name;

            foreach(var win in zaagiidiwin.win)
            {
                GameObject objTextGO = Instantiate(winTextPrefab, winList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{win.description} ({win.currentAmount}/{win.requiredAmount})"; // Collect 1 berry (0/5)
            }
        }
    }
}
