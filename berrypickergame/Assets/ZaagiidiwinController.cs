using System.Collections.Generic;
using UnityEngine;

public class ZaagiidiwinController : MonoBehaviour
{
    public static ZaagiidiwinController Instance { get; private set;}
    public List<ZaagiidiwinProgress> activateZaagiidiwins = new();
    private ZaagiidiwinUI zaagiidiwinUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        zaagiidiwinUI = FindFirstObjectByType<ZaagiidiwinUI>();

    }

    public void AcceptZaagiidiwin(Zaagiidiwin zaagiidiwin)
    {
        if(IsZaagiidiwinActive(zaagiidiwin.zaagiidiwinID)) return;
        activateZaagiidiwins.Add(new ZaagiidiwinProgress(zaagiidiwin));
        zaagiidiwinUI.UpdateZaagiidiwinUI();
    } 

    public bool IsZaagiidiwinActive(string zaagiidiwinID) => activateZaagiidiwins.Exists(q => q.ZaagiidiwinID == zaagiidiwinID);

}
