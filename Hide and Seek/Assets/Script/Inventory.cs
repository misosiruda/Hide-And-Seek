using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<Toggle> invenList = new List<Toggle>();
    public GameObject parent;
    public Text bellText;
    public Text charmText;

    private int prNum = 0;

    private void SettingInven()
    {
        for(int i=0;i<4;i++)
        {
            invenList.Add(parent.transform.GetChild(i).gameObject.GetComponent<Toggle>());
        }
    }
    
    private void ChangeSlot(int a)
    {
        if (a == prNum) return;
        invenList[prNum].isOn = false;
        invenList[a].isOn = true;
        prNum = a;
        GameManager.Instance.invenSlot = prNum;
    }
    private void InventoryChane()
    {
        switch(Input.inputString)
        {
            case "1":
                ChangeSlot(0);
                break;
            case "!":
                ChangeSlot(0);
                break;
            case "2":
                ChangeSlot(1);
                break;
            case "@":
                ChangeSlot(1);
                break;
            case "3":
                ChangeSlot(2);
                break;
            case "#":
                ChangeSlot(2);
                break;
            case "4":
                ChangeSlot(3);
                break;
            case "$":
                ChangeSlot(3);
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SettingInven();
    }

    // Update is called once per frame
    void Update()
    {
        InventoryChane();
        bellText.text = "x" + GameManager.Instance.bellCount.ToString();
        charmText.text = "x" + GameManager.Instance.charmCount.ToString();
    }
}
