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

    private WaitForSeconds waitSec;
    private Transform shojiDoor;
    private float time;
    private static WaitForSeconds wait60FPS;
    private static bool isDoorMoving = false;

    private static Vector3 roamingPos;
    private static WaitForSeconds waitRoamTerm;

    private void OnTriggerStay(Collider other)
    {
        if(other.transform == target)
        {
            if(GameManager.Instance.isLoud || IsInSight(target))
            {
                isChacing = true;
                if (GameManager.Instance.isInCloset)
                {
                    GameManager.Instance.gameover = true;
                }
            }
        }
    }

    private bool IsInSight(Transform target)
    {
        Vector3 trDir = (target.position - ghost.position).normalized;
        float dot = Vector3.Dot(ghost.forward, trDir);
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (theta <= 70f) return true;
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
            Vector3 pos = ghost.position;
            roamingPos = new Vector3();
            roamingPos.x = Random.Range(pos.x - 10f, pos.x + 10f);
            roamingPos.y = pos.y;
            roamingPos.z = Random.Range(pos.z - 10f, pos.z + 10f);
            Debug.Log("new Roaming Pos");
            yield return waitRoamTerm;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Charactor").GetComponent<Transform>();
        waitSec = new WaitForSeconds(1f);
        wait60FPS = new WaitForSeconds(1f / 60f);
        waitRoamTerm = new WaitForSeconds(6f);
        StartCoroutine(RoamingPos());
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(OpenDoor());
        if (isChacing)
        {
            agent.speed = GameManager.Instance.ghostRunSpd;
            agent.destination = target.position;
            if (GameManager.Instance.isInCloset)
            {
                isChacing = false;
            }
        }
        else //로밍중
        {
            agent.speed = 2f;
            agent.destination = roamingPos;
        }
    }
}
