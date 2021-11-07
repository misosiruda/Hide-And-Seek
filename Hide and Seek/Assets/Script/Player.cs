using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform player;
    public CharacterController plcon;
    public Transform plCamera;
    public float speed;
    private bool isPlaiavle = true;

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
    public void Move()
    {

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Run");


        Vector2 moveInput = new Vector2(hAxis, vAxis);
        bool isMove = moveInput.magnitude != 0;
        //카메라 전면
        Vector3 lookForward = new Vector3(plCamera.forward.x, 0f, plCamera.forward.z).normalized;
        Vector3 lookRight = new Vector3(plCamera.right.x, 0f, plCamera.right.z).normalized;
        Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

        player.forward = lookForward;
        if (moveInput.y > 0)
        {
            moveDir = moveDir.normalized * speed * (wDown ? 0.8f : 0.3f);
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

        if (wDown)
        {
            GameManager.Instance.isLoud = true;
        }
        else
        {
            GameManager.Instance.isLoud = false;
        }
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
                    isDoorMoving = true;
                    time = 0;
                    shojiDoor = hitinfo.transform;
                    if (shojiDoor.localPosition.x == -0.053f)
                    {
                        if (shojiDoor.localPosition.z < 0.85f)
                        {
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
                    isPlaiavle = false;
                    GameManager.Instance.isInCloset = true;
                    while (time<21)
                    {
                        cloDoor.localRotation = Quaternion.Euler(-90, 90 / 18 * time, 0);
                        time++;
                        yield return wait60FPS;
                    }
                    time = 0;
                    RaycastHit hitClo;
                    Physics.SphereCast(player.position - player.forward, 1f, player.forward, out hitClo, 2f, inClosetLM);
                    closet = hitClo.transform;
                    while (time<10)
                    {
                        player.Rotate(0, (180 / 8 * time), 0);
                        player.position = Vector3.Lerp(player.position, new Vector3(closet.position.x, closet.position.y + 1.5f, closet.position.z), 0.3f);
                        time++;
                        yield return wait60FPS;
                    }
                    time = 0;
                    while (time < 21)
                    {
                        cloDoor.localRotation = Quaternion.Euler(-90, 110 -(90 / 18 * time), 0);
                        time++;
                        yield return wait60FPS;
                    }
                    isPlaiavle = true;
                }
            }
        }
        else if(isPlaiavle && Physics.SphereCast(player.position - player.forward, 1f, player.forward, out hitout, 2f, closetOutLM))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isPlaiavle = false;
                time = 0;
                GameManager.Instance.isInCloset = false;
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
                time = 0;
                while (time < 21)
                {
                    cloDoor.localRotation = Quaternion.Euler(-90, 100 - (90 / 18 * time), 0);
                    time++;
                    yield return wait60FPS;
                }
                isPlaiavle = true;
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
                if (chfnier.localPosition.z < 0.45f)
                {
                    time = 0;
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
                        item.SetActive(false);
                    }
                    else
                    {
                        time = 0;
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
                    item.SetActive(false);
                }
            }
        }
    }

    public void InvenToryUse()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            switch(GameManager.Instance.invenSlot)
            {
                case 0:
                    FlashLightUse();
                    break;
                case 1:
                    FireLightUse();
                    break;
                case 2:
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
            flashLight.SetActive(false);
            defaultLight.SetActive(true);
            return;
        }
        flashLight.SetActive(true);
        fireLight.SetActive(false);
        defaultLight.SetActive(false);
    }
    private void FireLightUse()
    {
        if (fireLight.activeSelf)
        {
            fireLight.SetActive(false);
            defaultLight.SetActive(true);
            return;
        }
        flashLight.SetActive(false);
        fireLight.SetActive(true);
        defaultLight.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        gravity = 1000f;
        wait60FPS = new WaitForSeconds(1f / 60f);
        waitFix = new WaitForFixedUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaiavle)
        {
            Move();
        }
        StartCoroutine(DoorInterection());
        StartCoroutine(ClosetInterection());
        StartCoroutine(ChiffonierInterection());
        ItemInterectiom();
        InvenToryUse();
    }
    private void LateUpdate()
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
