using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform player;
    public CharacterController plcon;
    public Transform plCamera;
    public float speed;

    private float hAxis;
    private float vAxis;
    private bool wDown;
    private static float gravity;

    public LayerMask doorLM;
    private Transform shojiDoor;
    private float time;
    private static WaitForSeconds wait60FPS;
    private static bool isDoorMoving = false;

    public LayerMask closetLM;
    public LayerMask inClosetLM;
    public LayerMask closetOutLM;
    private static Transform cloDoor;
    private Transform closet;
    private Transform outClo;
    private static bool isInCloset = false;

    public LayerMask chiffonierLM;
    private Transform chfnier;

    private static WaitForFixedUpdate waitFix;
    public LayerMask itemLM;
    private GameObject item;

    public GameObject flashLight;
    public GameObject fireLight;
    public GameObject defaultLight;

    public Transform aim;
    public GameObject catBell;

    public AudioClip walk_01;
    public AudioClip walk_02;
    public AudioClip walk_03;
    public AudioClip walk_04;
    public AudioClip walk_05;
    public AudioClip walk_06;
    public AudioClip walk_07;
    public AudioClip walk_08;
    public AudioClip walk_09;
    public AudioClip walk_10;
    public AudioClip run_01;
    public AudioClip run_02;
    public AudioClip run_03;
    public AudioClip run_04;
    public AudioClip run_05;
    public AudioClip run_06;
    public AudioClip run_07;
    public AudioClip run_08;
    public AudioClip run_09;
    public AudioClip run_10;

    public AudioSource footStep;
    private List<AudioClip> walkFXList;
    private List<AudioClip> runFXList;
    private WaitForSeconds waitWalk;
    private WaitForSeconds waitRun;

    public AudioClip openDoor;
    public AudioClip closeDoor;
    public AudioClip openChiniffonier;
    public AudioClip closeChiniffonier;
    public AudioClip openCloset;
    public AudioClip closeCloset;
    public AudioClip pickBell;
    public AudioClip throwBell;
    public AudioClip pickCharm;
    private AudioSource objectFX;
    public AudioSource pickItem;

    public AudioClip jumpScare;
    public AudioClip growling;
    private List<AudioClip> BGMList;
    public AudioClip BGM1;
    public AudioClip BGM2;
    public AudioClip BGM3;
    public AudioSource BGM;
    public Transform ghost;
    private WaitForSeconds waitBGM;
    private bool isJumpScared = false;

    public void Move()
    {

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Run");


        Vector2 moveInput = new Vector2(hAxis, vAxis);
        //카메라 전면
        Vector3 lookForward = new Vector3(plCamera.forward.x, 0f, plCamera.forward.z).normalized;
        Vector3 lookRight = new Vector3(plCamera.right.x, 0f, plCamera.right.z).normalized;
        Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

        player.forward = lookForward;
        GameManager.Instance.isLoud = false;
        if (moveInput.y > 0)
        {
            moveDir = moveDir.normalized * speed * (wDown ? 0.8f : 0.3f);
            if (wDown)
            {
                GameManager.Instance.isLoud = true;
            }
            else
            {
                GameManager.Instance.isLoud = false;
            }
        }
        else
        {
            moveDir = moveDir.normalized * speed * 0.3f;
        }
        // 캐릭터에 중력 적용.
        moveDir.y += -gravity * Time.deltaTime;
        gravity += 8f;
        // 캐릭터 움직임.
        plcon.Move(moveDir * Time.deltaTime);
    }

    public IEnumerator DoorInterection()
    {
        if (!isDoorMoving)
        {
            RaycastHit hitinfo;
            if (Physics.Raycast(plCamera.position, plCamera.forward, out hitinfo, 2f, doorLM))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    objectFX = hitinfo.collider.gameObject.GetComponent<AudioSource>();
                    isDoorMoving = true;
                    time = 0;
                    shojiDoor = hitinfo.transform;
                    if (shojiDoor.localPosition.x == -0.053f)
                    {
                        if (shojiDoor.localPosition.z < 0.85f)
                        {
                            objectFX.clip = openDoor;
                            objectFX.Play();
                            while (time < 30)
                            {
                                shojiDoor.localPosition = Vector3.Lerp(shojiDoor.localPosition, new Vector3(-0.053f, 0f, 0.85f), 0.2f);
                                time++;
                                yield return wait60FPS;
                            }
                            shojiDoor.localPosition = new Vector3(-0.053f, 0f, 0.85f);
                        }
                        else
                        {
                            objectFX.clip = closeDoor;
                            objectFX.Play();
                            while (time < 30)
                            {
                                shojiDoor.localPosition = Vector3.Lerp(shojiDoor.localPosition, new Vector3(-0.053f, 0f, 0f), 0.2f);
                                time++;
                                yield return wait60FPS;
                            }
                            shojiDoor.localPosition = new Vector3(-0.053f, 0f, 0f);
                        }
                    }
                    else
                    {
                        if (shojiDoor.localPosition.z > -0.85f)
                        {
                            objectFX.clip = openDoor;
                            objectFX.Play();
                            while (time < 30)
                            {
                                shojiDoor.localPosition = Vector3.Lerp(shojiDoor.localPosition, new Vector3(-0.009f, 0f, -0.85f), 0.2f);
                                time++;
                                yield return wait60FPS;
                            }
                            shojiDoor.localPosition = new Vector3(-0.009f, 0f, -0.85f);
                        }
                        else
                        {
                            objectFX.clip = closeDoor;
                            objectFX.Play();
                            while (time < 30)
                            {
                                shojiDoor.localPosition = Vector3.Lerp(shojiDoor.localPosition, new Vector3(-0.009f, 0f, 0f), 0.2f);
                                time++;
                                yield return wait60FPS;
                            }
                            shojiDoor.localPosition = new Vector3(-0.009f, 0f, 0f);
                        }
                    }
                    isDoorMoving = false;
                }
            }
        }
    }

    public IEnumerator ClosetInterection()
    {
        RaycastHit hitout;
        if (!isInCloset)
        {
            RaycastHit hitDoor;
            if (Physics.Raycast(plCamera.position, plCamera.forward, out hitDoor, 2f, closetLM))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isInCloset = true;
                    time = 0;
                    cloDoor = hitDoor.transform;
                    GameManager.Instance.isPlaiable = false;
                    GameManager.Instance.isInCloset = true;
                    GameManager.Instance.getOutCloset = false;
                    objectFX = cloDoor.parent.GetComponent<AudioSource>();
                    objectFX.clip = openCloset;
                    objectFX.Play();
                    while (time < 21)
                    {
                        cloDoor.localRotation = Quaternion.Euler(-90, 90 / 18 * time, 0);
                        time++;
                        yield return wait60FPS;
                    }
                    time = 0;
                    RaycastHit hitClo;
                    Physics.SphereCast(player.position - player.forward, 1f, player.forward, out hitClo, 2f, inClosetLM);
                    closet = hitClo.transform;
                    while (time < 10)
                    {
                        plCamera.Rotate(0, 18, 0, Space.World);
                        player.position = Vector3.Lerp(player.position, new Vector3(closet.position.x, closet.position.y + 1.5f, closet.position.z), 0.3f);
                        time++;
                        yield return wait60FPS;
                    }
                    time = 0;
                    while (time < 21)
                    {
                        cloDoor.localRotation = Quaternion.Euler(-90, 110 - (90 / 18 * time), 0);
                        time++;
                        yield return wait60FPS;
                    }
                    GameManager.Instance.isPlaiable = true;
                }
            }
        }
        else if (GameManager.Instance.isPlaiable && Physics.SphereCast(player.position - player.forward, 1f, player.forward, out hitout, 2f, closetOutLM))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameManager.Instance.isPlaiable = false;
                time = 0;
                GameManager.Instance.isInCloset = false;
                objectFX = cloDoor.parent.GetComponent<AudioSource>();
                objectFX.clip = closeCloset;
                objectFX.Play();
                while (time < 21)
                {
                    cloDoor.localRotation = Quaternion.Euler(-90, 90 / 18 * time, 0);
                    time++;
                    yield return wait60FPS;
                }
                time = 0;
                outClo = hitout.transform;
                while (time < 10)
                {
                    player.position = Vector3.Lerp(player.position, new Vector3(outClo.position.x, outClo.position.y, outClo.position.z), 0.3f);
                    time++;
                    yield return wait60FPS;
                }
                GameManager.Instance.getOutCloset = true;
                time = 0;
                while (time < 21)
                {
                    cloDoor.localRotation = Quaternion.Euler(-90, 100 - (90 / 18 * time), 0);
                    time++;
                    yield return wait60FPS;
                }
                GameManager.Instance.isPlaiable = true;
                isInCloset = false;
            }
        }
    }

    public IEnumerator ChiffonierInterection()
    {
        RaycastHit hitchfn;
        RaycastHit hititem;
        if (Physics.Raycast(plCamera.position, plCamera.forward, out hitchfn, 2f, chiffonierLM))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                chfnier = hitchfn.transform;
                objectFX = chfnier.gameObject.GetComponent<AudioSource>();
                if (chfnier.localPosition.z < 0.45f)
                {
                    time = 0;
                    objectFX.clip = openChiniffonier;
                    objectFX.Play();
                    while (time < 21)
                    {
                        chfnier.localPosition = Vector3.Lerp(chfnier.localPosition, new Vector3(0f, chfnier.localPosition.y, 1.1f), 0.2f);
                        time++;
                        yield return wait60FPS;
                    }
                    chfnier.localPosition = new Vector3(0f, chfnier.localPosition.y, 1.1f);
                }
                else
                {
                    if ((Physics.Raycast(plCamera.position, plCamera.forward, out hititem, 2f, itemLM)))
                    {
                        item = hititem.transform.gameObject;
                        //아이템 획득
                        switch (item.tag)
                        {
                            case "Charm":
                                pickItem.clip = pickCharm;
                                break;
                            case "Bell":
                                pickItem.clip = pickBell;
                                break;
                        }
                        pickItem.Play();
                        GameManager.Instance.ItemGet(item);
                        item.SetActive(false);
                    }
                    else
                    {
                        time = 0;
                        objectFX.clip = closeChiniffonier;
                        objectFX.Play();
                        while (time < 21)
                        {
                            chfnier.localPosition = Vector3.Lerp(chfnier.localPosition, new Vector3(0f, chfnier.localPosition.y, 0.4f), 0.2f);
                            time++;
                            yield return wait60FPS;
                        }
                        chfnier.localPosition = new Vector3(0f, chfnier.localPosition.y, 0.4f);
                    }
                }
            }
        }
    }
    public void ItemInterectiom()
    {
        RaycastHit hititem;
        if ((Physics.Raycast(plCamera.position, plCamera.forward, out hititem, 2f, itemLM)))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hititem.transform.parent.parent.parent.name == "chiffonier") return;
                else
                {
                    item = hititem.transform.gameObject;
                    //아이템 획득
                    switch (item.tag)
                    {
                        case "Charm":
                            pickItem.clip = pickCharm;
                            break;
                        case "Bell":
                            pickItem.clip = pickBell;
                            break;
                    }
                    pickItem.Play();
                    GameManager.Instance.ItemGet(item);
                    item.SetActive(false);
                }
            }
        }
    }

    public void InvenToryUse()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            switch (GameManager.Instance.invenSlot)
            {
                case 0:
                    FlashLightUse();
                    break;
                case 1:
                    FireLightUse();
                    break;
                case 2:
                    ThrowBell();
                    break;
                case 3:
                    break;
            }
        }
    }
    private void FlashLightUse()
    {
        if (flashLight.activeSelf)
        {
            GameManager.Instance.lightNow = string.Empty;
            flashLight.SetActive(false);
            defaultLight.SetActive(true);
            return;
        }
        flashLight.SetActive(true);
        GameManager.Instance.lightNow = "flash";
        fireLight.SetActive(false);
        defaultLight.SetActive(false);
    }
    private void FireLightUse()
    {
        if (fireLight.activeSelf)
        {
            GameManager.Instance.lightNow = string.Empty;
            fireLight.SetActive(false);
            defaultLight.SetActive(true);
            return;
        }
        GameManager.Instance.lightNow = "fire";
        flashLight.SetActive(false);
        fireLight.SetActive(true);
        defaultLight.SetActive(false);
    }

    private void SoundListInitialize()
    {
        walkFXList = new List<AudioClip>();
        walkFXList.Add(walk_01);
        walkFXList.Add(walk_02);
        walkFXList.Add(walk_03);
        walkFXList.Add(walk_04);
        walkFXList.Add(walk_05);
        walkFXList.Add(walk_06);
        walkFXList.Add(walk_07);
        walkFXList.Add(walk_08);
        walkFXList.Add(walk_09);
        walkFXList.Add(walk_10);
        runFXList = new List<AudioClip>();
        runFXList.Add(run_01);
        runFXList.Add(run_02);
        runFXList.Add(run_03);
        runFXList.Add(run_04);
        runFXList.Add(run_05);
        runFXList.Add(run_06);
        runFXList.Add(run_07);
        runFXList.Add(run_08);
        runFXList.Add(run_09);
        runFXList.Add(run_10);
        BGMList = new List<AudioClip>();
        BGMList.Add(BGM1);
        BGMList.Add(BGM2);
        BGMList.Add(BGM3);
    }

    private IEnumerator FootStepFX()
    {
        while (true)
        {
            if (GameManager.Instance.gameover)
            {
                yield break;
            }
            Vector2 moveInput = new Vector2(hAxis, vAxis);
            if (moveInput.y > 0)
            {
                if (wDown)
                {
                    yield return waitRun;
                    footStep.clip = runFXList[Random.Range(0, 10)];
                    footStep.volume = 1f;
                    footStep.Play();
                }
                else
                {
                    yield return waitWalk;
                    footStep.clip = walkFXList[Random.Range(0, 10)];
                    footStep.volume = 0.4f;
                    footStep.Play();
                }
            }
            else
            {
                yield return waitWalk;
            }
        }
    }

    private void ThrowBell()
    {
        if (GameManager.Instance.bellCount == 0) return;
        GameManager.Instance.bellCount -= 1;
        GameManager.Instance.catBell.Add(Instantiate(catBell, plCamera.position, Quaternion.identity));
        GameManager.Instance.catBell[GameManager.Instance.catBell.Count - 1].GetComponent<Rigidbody>().AddForce((aim.position - plCamera.position) * 10f, ForceMode.Impulse);
    }

    private IEnumerator BGMControl()
    {
        while (true)
        {
            if (GameManager.Instance.isCaught && !isJumpScared)
            {
                BGM.volume = 1f;
                BGM.clip = jumpScare;
                BGM.Play();
                isJumpScared = true;
                yield return waitBGM;
                continue;
            }
            else if (!BGM.isPlaying)
            {
                if (Vector3.Distance(ghost.position, player.position) < 15f)
                {
                    BGM.volume = 1f;
                    BGM.clip = growling;
                    BGM.Play();
                    isJumpScared = false;
                    yield return waitBGM;
                }
                else if (Vector3.Distance(ghost.position, player.position) > 60f)
                {
                    BGM.volume = 0.5f;
                    BGM.clip = BGMList[Random.Range(0, 3)];
                    BGM.Play();
                    isJumpScared = false;
                    yield return waitBGM;
                }
            }
            else
            {
                if (20f < Vector3.Distance(ghost.position, player.position) && Vector3.Distance(ghost.position, player.position) < 50f)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        BGM.volume -= 0.05f;
                        yield return waitRun;
                    }
                    BGM.Pause();
                    yield return waitBGM;
                }
            }
            yield return waitBGM;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gravity = 1000f;
        wait60FPS = new WaitForSeconds(1f / 60f);
        waitFix = new WaitForFixedUpdate();
        waitWalk = new WaitForSeconds(0.5f);
        waitRun = new WaitForSeconds(0.25f);
        waitBGM = new WaitForSeconds(1f);
        SoundListInitialize();
        StartCoroutine(FootStepFX());
        StartCoroutine(BGMControl());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPlaiable && !GameManager.Instance.gameover)
        {
            Move();
        }
        if (GameManager.Instance.gameover || GameManager.Instance.ending)
        {
            footStep.Pause();
        }
        StartCoroutine(DoorInterection());
        StartCoroutine(ClosetInterection());
        StartCoroutine(ChiffonierInterection());
        ItemInterectiom();
        InvenToryUse();
    }
    private void LateUpdate()
    {
        if (!GameManager.Instance.gameover)
        {
            Vector3 vector = player.position;
            vector.y = player.position.y;
            plCamera.position = vector;
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * 2, Input.GetAxis("Mouse Y") * 2);
            Vector3 camAngle = plCamera.rotation.eulerAngles;
            float x = camAngle.x - mouseDelta.y;
            if (x < 180f) { x = Mathf.Clamp(x, -1f, 89f); }
            else { x = Mathf.Clamp(x, 271f, 361f); }
            plCamera.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
        }
    }
}
