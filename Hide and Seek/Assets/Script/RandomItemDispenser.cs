using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemDispenser : MonoBehaviour
{
    private List<GameObject> charmList = new List<GameObject>();
    private List<GameObject> ringList = new List<GameObject>();
    private WaitForFixedUpdate waitFix = new WaitForFixedUpdate();
    // Start is called before the first frame update
    private IEnumerator Dispenser(List<GameObject> list, string st)
    {
        while (true)
        {
            try
            {
                list.Add(GameObject.Find(st));
            }
            catch (Exception ex)
            {
                break;
            }
            yield return waitFix;
        }
    }
    public void StartDispenser()
    {
        StartCoroutine(Dispenser(charmList, "charm"));
        //Dispenser(ringList, "ring"); 미구현
    }
}
