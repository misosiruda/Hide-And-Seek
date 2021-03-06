using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //조작법에 인벤토리 선택 기능 안넣어줌.
    //할아버지의 영혼 형식으로 처음에 간단한 조작법과 목표를 알려준다.
    /// <summary>
    /// 할 : 하루토... 잘 듣거라... 너는 지금 봉인이 풀려난 귀신의 결계 안에 있다. 여기에 흩어져있는 부적 8개를 모아서 토리이를 지나야 한다. 부적을 모으면 모을 수록 귀신은 강해질 것이고, 
    /// 8개를 모으면 귀신은 언제나 너의 위치를 알것이야... 이곳에 숨을 수 있는 공간이 있을게다. 그곳을 이용하여 귀신을 따돌리고 부적 8개를 모아서 토리이를 지나 탈출하거라...
    /// 이후 손전등과 라이터의 작동 방법을 독백으로 알려주고 차이점도 독백으로 알려준다.
    /// 방울을 먹으면 던지면 주의를 끌 수 있을것 같다. 라고 독백
    /// 부적을 모으면 할아버지가 말했던 부적이 이거구나. 라고 독백
    /// </summary>
    //파이널 프레젠 테이션은 PPT 와 용량 최대로 줄인 프로젝트 파일, 빌드한 .exe파일 그리고 그걸 플레이한 동영상 파일 까지 준비해서 오기
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
    public bool isCaught = false;
    public string lightNow = "";
    public float distance = 0f;
    public bool gamePause = false;

    private WaitForSeconds waitGameOver;
    private WaitForSeconds waitText;
    private WaitForFixedUpdate waitFix;
    private WaitForSeconds waitEndGame;
    private WaitForSeconds waitDist;
    public GameObject gameoverUI;
    public GameObject inventoryUI;
    public GameObject loadingUI;
    public GameObject endingUI;
    public GameObject loadingText;
    public GameObject startText;
    public Text endingStory;
    public Text startStory;
    private string startStory_ = "200X년 08월 21일\n나는 하나 남은 가족인 할아버지가 돌아가셨다는 소식을 듣고 장례식을 치르고 할아버지 집에 왔다.\n하지만 집에 들어온 순간 갑자기 기절 했고 깨어 보니 할아버지 집같지만 이상한 장소에 손전등, 라이터, 그리고 이곳에서 나가려면 부적을 8개 찾아 토리이를 통과해라 라는 편지 한장 뿐이였다. ";
    private string endingStory_ = "정신을 차리고 보니 나는 할아버지가 들어가지 말라던 지하실 입구에서 깨어났고 그곳엔 작은 사당이 있었으며 내가 모았던 부적이 덕지덕지 붙어 있었다.\n그리고 거기엔 할아버지의 편지도 있었는데 '미안하다'외에는 흐릿하여 아무것도 보이지 않았다. 왠지 꿈속에서 편지조각들을 봤던거 같은데 기억이 나지 않는다. ";
    public bool ending = false;
    public GameObject ghost;
    public Transform player;

    public Text gameoverText;
    private List<string> tipText;

    public AudioSource playerAudio;
    public AudioClip jumpScare;
    public bool isJumpScared = true;

    public GameObject pauseMenu;
    public GameObject endingCredit;

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
                StartCoroutine(TipTextTyping());
                while(true)
                {
                    if (Input.anyKeyDown) break;
                    yield return null;
                }
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
        player.gameObject.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ending = true;
        StartCoroutine(EndingStory());
    }
    private IEnumerator EndingStory()
    {
        string temp = "";
        for (int i = 0; i < endingStory_.Length; i++)
        {
            temp += endingStory_[i]; 
            if (Input.anyKeyDown)
            {
                endingStory.text = "";
                StartCoroutine(EndingCredit());
                yield break;
            }
            if (endingStory_[i] == ' ')
            {
                endingStory.text += temp;
                temp = "";
                yield return waitText;
            }
            yield return waitFix;
        }
        endingStory.text = "";
        StartCoroutine(EndingCredit());
        yield break;
    }
    private IEnumerator EndingCredit()
    {
        while(true)
        {
            yield return waitFix;
            if(endingCredit.transform.position.y < 1500f)
            {
                endingCredit.GetComponent<RectTransform>().position += new Vector3(0, 1f, 0);
            }
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("MainMenu");
            }
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
        string temp = "";
        for (int i = 0; i < startStory_.Length; i++)
        {
            temp += startStory_[i];
            if (startStory_[i] == ' ')
            {
                startStory.text += temp;
                temp = "";
                yield return waitText;
            }
            if (gamestart) yield break;
        }
        yield break;
    }

    private IEnumerator Distance()
    {
        while(true)
        {
            distance = Vector3.Distance(ghost.transform.position, player.position);
            yield return waitDist;
        }
    }

    private void TipTextInitialize()
    {
        tipText = new List<string>();
        tipText.Add("귀신은 방울을 싫어 합니다. 방울을 던져 귀신의 주의를 끄세요. ");
        tipText.Add("부적을 여덟개 모으게 되면 귀신은 당신의 존재를 어디서든지 알아 챕니다. ");
        tipText.Add("옷장안에 숨어 귀신을 따돌리세요. ");
        tipText.Add("귀신과 충분한 거리를 벌리지 못하고 옷장안에 들어가면 귀신이 알아챌 지 모릅니다. ");
        //tipText.Add("");
    }

    private IEnumerator TipTextTyping()
    {
        int n = Random.Range(0, tipText.Count);
        string temp = "";
        for(int i = 0; i < tipText[n].Length; i++)
        {
            temp += tipText[n][i];
            if (tipText[n][i] == ' ')
            {
                gameoverText.text += temp;
                temp = "";
                yield return waitText;
            }
        }
        yield break;
    }

    public IEnumerator JumpScare()
    {
        if (isJumpScared)
        {
            isJumpScared = false;
            playerAudio.clip = jumpScare;
            playerAudio.Play();
            yield return waitDist;
        }
    }

    public void GamePause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gamePause = true;
        pauseMenu.SetActive(true);
    }
    public void GameResume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gamePause = false;
        pauseMenu.SetActive(false);
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
        waitDist = new WaitForSeconds(0.5f);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        TipTextInitialize();
        StartCoroutine(GameStart());
        StartCoroutine(GameOver());
        StartCoroutine(StartStory());
        StartCoroutine(Distance());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gamePause)
            {
                GameResume();
            }
            else
            {
                GamePause();
            }
        }
    }
}
//귀신 애니메이션 각도 회전 할때 위치 고정하고 각도 바꾸기 그럴려면 네비메쉬랑 연동 필요함
//플레이어가 귀신에게 잡혔을때 좀더 격동적인 무브먼트 필요함