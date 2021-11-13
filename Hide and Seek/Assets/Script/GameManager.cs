using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public int invenSlot = 0;
    public bool isLoud = false;
    public bool isInCloset = false;
    public bool getOutCloset = false;
    public bool gameover = false;
    public float ghostRunSpd = 5;
    public float ghostRoamingDistance = 50f;
    public int bellCount = 0;
    public int charmCount = 0;
    public List<GameObject> catBell = new List<GameObject>();
    public bool randomDispensEnd = false;
    public bool isPlaiable = false;
    public bool endGame = false;

    private WaitForSeconds waitGameOver;
    private WaitForFixedUpdate waitFix;
    private WaitForSeconds waitEndGame;
    public GameObject gameoverUI;
    public GameObject inventoryUI;
    public GameObject loadingUI;

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

    public void ItemGet(GameObject item)
    {
        switch (item.tag)
        {
            case "Charm":
                charmCount++;
                switch (charmCount)
                {
                    case 4:
                        ghostRunSpd++;
                        break;
                    case 6:
                        ghostRunSpd++;
                        break;
                    case 7:
                        ghostRunSpd++;
                        break;
                }
                break;
            case "Bell":
                bellCount++;
                break;
        }
    }

    private IEnumerator GameOver()
    {
        while(true)
        {
            if (gameover)
            {
                yield return waitGameOver;
                inventoryUI.SetActive(false);
                gameoverUI.SetActive(false);
                yield return waitGameOver;
                SceneManager.LoadScene("MainMenu");
                yield break;
            }
            yield return waitFix;
        }
    }
    private IEnumerator EndGame()
    {
        while(true)
        {
            if(endGame)
            {
                //엔딩 스토리 설명과 크레딧
            }
            yield return waitFix;
        }
    }

    private IEnumerator GameStart()
    {
        while(true)
        {
            if(randomDispensEnd)
            {
                isPlaiable = true;
                inventoryUI.SetActive(true);
                loadingUI.SetActive(false);
                yield break;
            }
            yield return waitFix;
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
        //DontDestroyOnLoad(this.gameObject);
        StartCoroutine(GameStart());
    }
    // Start is called before the first frame update
    void Start()
    {
        waitFix = new WaitForFixedUpdate();
        waitGameOver = new WaitForSeconds(2f);
        waitEndGame = new WaitForSeconds(3f);
        StartCoroutine(GameOver());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
