using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;
    private bool isChacing = false;
    public Transform ghost;
    public LayerMask doorLM;

    private Transform shojiDoor;
    private float time;
    private static WaitForSeconds wait60FPS;
    private static bool isDoorMoving = false;

    private static Vector3 roamingPos;
    private static WaitForSeconds waitRoamTerm;

    private GameObject temp;
    private static WaitForFixedUpdate waitFix;

    public Animator ghostAni;

    public Transform plCamera;
    private bool moshindeiru;
    public Transform ghostFace;

    private void OnTriggerStay(Collider other)
    {
        if(GameManager.Instance.catBell.Count == 0)
        {
            if (other.transform == target && (GameManager.Instance.isLoud || IsInSight(target)))
            {
                isChacing = true;
                if (GameManager.Instance.isInCloset)
                {
                    isChacing = false;
                    moshindeiru = true;
                }
            }
        }
    }

    private bool IsInSight(Transform target)
    {
        if (GameManager.Instance.catBell.Count == 0)
        {
            Vector3 trDir = (target.position - ghost.position).normalized;
            float dot = Vector3.Dot(ghost.forward, trDir);
            float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;
            RaycastHit hitinfo;
            Physics.Raycast(ghost.position, target.position - ghost.position, out hitinfo);
            Debug.DrawRay(ghost.position, target.position - ghost.position, Color.red);
            if (theta <= 70f && hitinfo.collider.gameObject.transform == target) return true;
            else return false;
        }
        else return false;
    }

    private IEnumerator OpenDoor()
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(ghost.position + ghost.up, ghost.forward, out hitinfo, 2f, doorLM))
        {
            //yield return waitSec;
            if (!isDoorMoving)
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

    private IEnumerator RoamingPos()
    {
        while(true)
        {
            if(!isChacing && GameManager.Instance.catBell.Count == 0)
            {
                Vector3 pos = target.position;
                roamingPos = new Vector3();
                roamingPos.x = Random.Range(pos.x - GameManager.Instance.ghostRoamingDistance, pos.x + GameManager.Instance.ghostRoamingDistance);
                roamingPos.y = pos.y;
                roamingPos.z = Random.Range(pos.z - GameManager.Instance.ghostRoamingDistance, pos.z + GameManager.Instance.ghostRoamingDistance);
                if (agent.velocity.magnitude < 0.3f)
                {
                    yield return waitFix;
                    continue;
                }
                yield return waitRoamTerm;
            }
            yield return waitFix;
        }
    }

    private IEnumerator SpotCatBell()
    {
        while(true)
        {
            yield return wait60FPS;
            if (GameManager.Instance.catBell.Count != 0)
            {
                ghostAni.SetBool("isWalk", false);
                ghostAni.SetBool("isRun", true);
                while (true)
                {
                    agent.speed = GameManager.Instance.ghostRunSpd;
                    agent.destination = GameManager.Instance.catBell[0].transform.position;
                    yield return waitFix;
                    if (Vector3.Distance(ghost.position, GameManager.Instance.catBell[0].transform.position) < 0.5f)
                    {
                        agent.speed = 2f;
                        temp = GameManager.Instance.catBell[0];
                        GameManager.Instance.catBell.Remove(temp);
                        Destroy(temp);
                        break;
                    }
                }
            }
        }
    }

    private IEnumerator Moshindeiru()
    {
        ghostAni.SetBool("isWalk", false);
        ghostAni.SetBool("isRun", false);
        GameManager.Instance.gameover = true;
        ghostAni.SetTrigger("scream");
        ghost.position = target.position + (target.forward * 0.5f) - new Vector3(0, 1.5f, 0);
        yield return waitFix;
        ghost.LookAt(new Vector3(plCamera.position.x, ghost.position.y, plCamera.position.z));
        yield return waitFix;
        plCamera.LookAt(ghostFace.position);
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Charactor").GetComponent<Transform>();
        wait60FPS = new WaitForSeconds(1f / 60f);
        waitRoamTerm = new WaitForSeconds(6f);
        waitFix = new WaitForFixedUpdate();
        StartCoroutine(RoamingPos());
        StartCoroutine(SpotCatBell());
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.gamestart)
        {
            //StartCoroutine(OpenDoor());
            if (isChacing && GameManager.Instance.catBell.Count == 0)//쫒는중
            {
                ghostAni.SetBool("isWalk", false);
                ghostAni.SetBool("isRun", true);
                agent.speed = GameManager.Instance.ghostRunSpd;
                agent.destination = target.position;
                if (GameManager.Instance.isInCloset && !moshindeiru)
                {
                    isChacing = false;
                }
                if (Vector3.Distance(ghost.position, target.position) < 2f && !GameManager.Instance.isInCloset)//잡힘
                {
                    ghostAni.SetBool("isWalk", false);
                    ghostAni.SetBool("isRun", false);
                    ghostAni.SetTrigger("scream");
                    ghost.LookAt(new Vector3(plCamera.position.x, ghost.position.y, plCamera.position.z));
                    GameManager.Instance.gameover = true;
                    plCamera.LookAt(ghostFace.position);
                }
            }
            else if (GameManager.Instance.catBell.Count == 0)//로밍중
            {
                agent.speed = 2f;
                agent.angularSpeed = 150f;
                ghostAni.SetBool("isWalk", true);
                ghostAni.SetBool("isRun", false);
                agent.destination = roamingPos;
                if (agent.velocity.magnitude < 0.3f)
                {
                    ghostAni.SetBool("isWalk", false);
                    ghostAni.SetBool("isRun", false);
                    agent.angularSpeed = 0f;
                }
            }
            if (moshindeiru && GameManager.Instance.getOutCloset)//옷장 후 갑툭튀
            {
                StartCoroutine(Moshindeiru());
            }
        }
    }
}
