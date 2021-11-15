using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public bool gamestart = false;
    public bool isPlaiable = false;
    public bool endGame = false;

    private WaitForSeconds waitGameOver;
    private WaitForSeconds waitText;
    private WaitForFixedUpdate waitFix;
    private WaitForSeconds waitEndGame;
    public GameObject gameoverUI;
    public GameObject inventoryUI;
    public GameObject loadingUI;
    public GameObject loadingText;
    public GameObject startText;
    public Text startStory;
    private string startStory_ = "200X년 08월 21일\n나는 할아버지의 유품을 어쩌구 저쩌구 이 집에 왔다.\n하지만 집에 들어온 순간 갑자기 기절 했고 깨어 보니 할아버지 집같지만 이상한 장소에\n손전등, 라이터, 그리고 이곳에서 나가려면 부적을 8개 찾아 토리이를 통과해라 라는 편지 한장 뿐이였다.";


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
                loadingText.SetActive(false);
                startText.SetActive(true);
                if (Input.anyKeyDown)
                {
                    gamestart = true;
                    isPlaiable = true;
                    inventoryUI.SetActive(true);
                    loadingUI.SetActive(false);
                    yield break;
                }
            }
            yield return waitFix;
        }   
    }

    private IEnumerator StartStory()
    {
        for (int i = 0; i < startStory_.Length; i++)
        {
            startStory.text += startStory_[i];
            if (gamestart) yield break;
            yield return waitText;
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
    }
    // Start is called before the first frame update
    void Start()
    {
        invenSlot = 0;
        isLoud = false;
        isInCloset = false;
        getOutCloset = false;
        gameover = false;
        ghostRunSpd = 5;
        ghostRoamingDistance = 50f;
        bellCount = 0;
        charmCount = 0;
        catBell = new List<GameObject>();
        randomDispensEnd = false;
        gamestart = false;
        isPlaiable = false;
        endGame = false;

        waitFix = new WaitForFixedUpdate();
        waitGameOver = new WaitForSeconds(2f);
        waitEndGame = new WaitForSeconds(3f);
        waitText = new WaitForSeconds(0.1f);
        StartCoroutine(GameStart());
        StartCoroutine(GameOver());
        StartCoroutine(StartStory());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
//귀신 애니메이션 각도 회전 할때 위치 고정하고 각도 바꾸기 그럴려면 네비메쉬랑 연동 필요함
//플레이어가 귀신에게 잡혔을때 좀더 격동적인 무브먼트 필요함