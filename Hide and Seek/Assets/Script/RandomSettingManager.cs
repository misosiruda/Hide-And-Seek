using System.Collections;
using System.Collections.Generic;

public class RandomSettingManager
{
    private static int randObjectNum = 0;
    private static List<string> randObectName = new List<string>();
    public int RonGet() { return randObjectNum; }
    public void RonSet(string st)
    {
        randObjectNum++;
        if (randObectName == null) randObectName.Add(st);
        else
        {
            foreach (string elem in randObectName)
            {
                if (st == elem)
                {
                    isSame = true;
                    break;
                }
            }
            if (!isSame) randObectName.Add(st);
            isSame = false;
        }
    }
    private static int randObjectLoad = 0;
    private static List<string> randObectLName = new List<string>();
    private bool isSame = false;
    public int RolGet() { return randObjectLoad; }
    public void RolSet(string st)
    {
        randObjectLoad++;
        if (randObectLName == null) randObectLName.Add(st);
        else
        {
            foreach(string elem in randObectLName)
            {
                if (st == elem)
                {
                    isSame = true;
                    break;
                }
            }
            if (!isSame) randObectLName.Add(st);
            isSame = false;
        }
    }
    public bool isLoaded()
    {
        foreach (string elem in randObectName)
        {
            foreach(string elem_ in randObectLName)
            {
                if(elem == elem_)
                {
                    isSame = true;
                    break;
                }
                isSame = false;
            }
        }
        if (randObectLName.Count < 4) return false;
        return isSame;
    }
    private static int randItemNum = 0;
    public int RinGet() { return randItemNum; }
    public void RinSet() { randItemNum++; }
    private static int randItemLoad = 0;
    public int RilGet() { return randItemLoad; }
    public void RilSet() { randItemLoad++; }
}
