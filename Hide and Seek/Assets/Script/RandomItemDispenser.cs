﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemDispenser : MonoBehaviour
{
    private static List<GameObject> charmList = new List<GameObject>();
    private static List<GameObject> bellList = new List<GameObject>();
    private static WaitForFixedUpdate waitFix = new WaitForFixedUpdate();
    private static GameObject item;

    private RandomSettingManager rsm;
    private IEnumerator Dispenser(List<GameObject> list, string st, int max)
    {
        while (true)
        {
            try
            {
                item = GameObject.Find(st);
                Debug.Log(item.name);
                list.Add(item);
                item.SetActive(false);
            }
            catch (Exception ex)
            {
                break;
            }
            yield return waitFix;
        }
        StartCoroutine(RandomItemSetting(list, max));
    }

    private IEnumerator RandomItemSetting(List<GameObject> list, int max)
    {
        int num = 0;
        while(num < max)
        {
            list[UnityEngine.Random.Range(0, list.Count)].SetActive(true);
            num++;
            yield return waitFix;
        }
        rsm.RilSet();
    }
    public void StartDispenser()
    {
        rsm = new RandomSettingManager();
        rsm.RinSet();
        StartCoroutine(Dispenser(charmList, "charm", 8));
        rsm.RinSet();
        StartCoroutine(Dispenser(bellList, "bell", 30));
    }
}
