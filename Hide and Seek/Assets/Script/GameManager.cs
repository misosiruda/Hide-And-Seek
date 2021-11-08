using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public int invenSlot = 0;
    public bool isLoud;
    public bool isInCloset;
    public bool gameover;
    public float ghostRunSpd = 5;

    public static GameManager Instance 
    {
        get 
        {
            if (!instance)
            {
                if (instance == null)
                {
                    Debug.Log("no Singleton obj");
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("씬에 두 개 이상의 게임 매니저가 존재합니다.");
            Destroy(gameObject); //자기 자신을 삭제
        }
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
