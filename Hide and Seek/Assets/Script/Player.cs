using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform player;
    public CharacterController plcon;
    public Transform camera;
    public float speed;

    private float hAxis;
    private float vAxis;
    private bool wDown;
    private static float gravity;

    public LayerMask doorLM;
    public Transform shojiDoor;
    private static float time;
    static WaitForSeconds waitDoor;

    public void Move()
    {

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Run");



        Vector2 moveInput = new Vector2(hAxis, vAxis);
        bool isMove = moveInput.magnitude != 0;
        //카메라 전면
        Vector3 lookForward = new Vector3(camera.forward.x, 0f, camera.forward.z).normalized;
        Vector3 lookRight = new Vector3(camera.right.x, 0f, camera.right.z).normalized;
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
    }

    public IEnumerator DoorInterection()
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(player.position, player.forward, out hitinfo, 1f, doorLM))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                time = 0;
                shojiDoor = hitinfo.collider.transform;
                if(shojiDoor.localPosition.x == -0.053f)
                {
                    if (shojiDoor.localPosition.z < 0.85f)
                    {
                        while (time < 30)
                        {
                            shojiDoor.localPosition = Vector3.Lerp(shojiDoor.localPosition, new Vector3(-0.053f, 0f, 0.85f), 0.2f);
                            time++;
                            yield return waitDoor;
                        }
                        shojiDoor.localPosition = new Vector3(-0.053f, 0f, 0.85f);
                    }
                    else
                    {
                        while (time < 30)
                        {
                            shojiDoor.localPosition = Vector3.Lerp(shojiDoor.localPosition, new Vector3(-0.053f, 0f, 0f), 0.2f);
                            time++;
                            yield return waitDoor;
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
                            yield return waitDoor;
                        }
                        shojiDoor.localPosition = new Vector3(-0.009f, 0f, -0.85f);
                    }
                    else
                    {
                        while (time < 30)
                        {
                            shojiDoor.localPosition = Vector3.Lerp(shojiDoor.localPosition, new Vector3(-0.009f, 0f, 0f), 0.2f);
                            time++;
                            yield return waitDoor;
                        }
                        shojiDoor.localPosition = new Vector3(-0.009f, 0f, 0f);
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gravity = 1000f;
        waitDoor = new WaitForSeconds(1f / 60f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        StartCoroutine(DoorInterection());
    }
    private void LateUpdate()
    {
        Vector3 vector = player.position;
        vector.y = player.position.y;
        camera.position = vector; 
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * 2, Input.GetAxis("Mouse Y") * 2);
        Vector3 camAngle = camera.rotation.eulerAngles;
        camera.rotation = Quaternion.Euler(camAngle.x - mouseDelta.y, camAngle.y + mouseDelta.x, camAngle.z);
    }
}
