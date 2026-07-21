using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZaagiidiwinUI : MonoBehaviour
{
    public Transform zaagiidiwinListContent;
    public GameObject zaagiidiwinEntryPrefab;
    public GameObject winTextPrefab;

    public Zaagiidiwin testZaagiidiwin;
    public int testZaagiidiwinAmount;
    private List<ZaagiidiwinProgress> testZaagiidiwins = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < testZaagiidiwinAmount; i++)
        {
            testZaagiidiwins.Add(new ZaagiidiwinProgress(testZaagiidiwin));
        }

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

        //build zaagiidiwin entries
        foreach(var zaagiidiwin in testZaagiidiwins)
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
