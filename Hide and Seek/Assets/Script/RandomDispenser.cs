using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDispenser : MonoBehaviour
{
    public GameObject parent;
    private GameObject child;
    private WaitForFixedUpdate waitFix = new WaitForFixedUpdate();
    private List<GameObject> childList = new List<GameObject>();

    private RandomSettingManager rsm;
    private RandomItemDispenser rid;

    public IEnumerator Dispenser()
    {
        int num = 1;
        while(true)
        {
            try
            {
                child = parent.transform.Find(parent.name + " (" + num + ")").gameObject;
                childList.Add(child);
                num++;
            }
            catch (Exception ex)
            {
                rsm.RonSet(parent.name);
                break;
            }
            yield return waitFix;
        }
        StartCoroutine(RandomSetting(childList));
    }

    public IEnumerator RandomSetting(List<GameObject> childList)
    {
        int chCount, actCount = 0, num;
        chCount = childList.Count;
        for (int i = 0; i < chCount; i++)
        {
            if (UnityEngine.Random.Range(0, 11) < 5)
            {
                childList[i].SetActive(false);
            }
            else
            {
                actCount++;
            }
            yield return waitFix;
        }
        if (actCount < chCount / 2)
        {
            while (actCount < chCount / 2)
            {
                num = UnityEngine.Random.Range(0, chCount);
                if(!childList[num].activeSelf)
                {
                    childList[num].SetActive(true);
                    actCount++;
                }
                yield return waitFix;
            }
        }
        //작업 끝 알리는 함수 넣어주기
        rsm.RolSet(parent.name);
        if (rsm.isLoaded())
        {
            rid.StartDispenser();
            //작업 끝
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Dispenser()); 
        rsm = new RandomSettingManager();
        rid = this.gameObject.AddComponent<RandomItemDispenser>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
