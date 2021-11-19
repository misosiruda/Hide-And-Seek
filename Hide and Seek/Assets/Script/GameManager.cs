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
    public string lightNow = "";

    private WaitForSeconds waitGameOver;
    private WaitForSeconds waitText;
    private WaitForFixedUpdate waitFix;
    private WaitForSeconds waitEndGame;
    public GameObject gameoverUI;
    public GameObject inventoryUI;
    public GameObject loadingUI;
    public GameObject endingUI;
    public GameObject loadingText;
    public GameObject startText;
    public Text endingStory;
    public Text startStory;
    private string startStory_ = "200X년 08월 21일\n나는 할아버지의 유품을 어쩌구 저쩌구 이 집에 왔다.\n하지만 집에 들어온 순간 갑자기 기절 했고 깨어 보니 할아버지 집같지만 이상한 장소에 손전등, 라이터, 그리고 이곳에서 나가려면 부적을 8개 찾아 토리이를 통과해라 라는 편지 한장 뿐이였다.";
    private string endingStory_ = "정신을 차리고 보니 나는 할아버지가 들어가지 말라던 지하실 입구에서 깨어났고 그곳엔 작은 사당이 있었으며 내가 모았던 부적이 덕지덕지 붙어 있었다.\n그리고 거기엔 할아버지의 편지도 있었는데 '미안하다'외에는 흐릿하여 아무것도 보이지 않았다. 왠지 꿈속에서 편지조각들을 봤던거 같은데 기억이 나지 않는다.";
    public bool ending = false;
    public GameObject ghost;

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
                gameoverUI.SetActive(true);
                yield return waitGameOver;
                SceneManager.LoadScene("MainMenu");
                yield break;
            }
            yield return waitFix;
        }
    }
    public void EndGame()
    {
        ghost.SetActive(false);
        inventoryUI.SetActive(false);
        endingUI.SetActive(true);
        ending = true;
    }
    private IEnumerator EndingStory()
    {
        while(!ending)
        {
            yield return waitText;
        }
        for (int i = 0; i < endingStory_.Length; i++)
        {
            endingStory.text += endingStory_[i];
            if (Input.anyKeyDown)
            {
                EndingCredit();
                yield break;
            }
            yield return waitText;
        }
    }
    private void EndingCredit()
    {

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
        lightNow = "";

        waitFix = new WaitForFixedUpdate();
        waitGameOver = new WaitForSeconds(2f);
        waitEndGame = new WaitForSeconds(3f);
        waitText = new WaitForSeconds(0.1f);
        StartCoroutine(GameStart());
        StartCoroutine(GameOver());
        StartCoroutine(StartStory());
        StartCoroutine(EndingStory());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
//귀신 애니메이션 각도 회전 할때 위치 고정하고 각도 바꾸기 그럴려면 네비메쉬랑 연동 필요함
//플레이어가 귀신에게 잡혔을때 좀더 격동적인 무브먼트 필요함