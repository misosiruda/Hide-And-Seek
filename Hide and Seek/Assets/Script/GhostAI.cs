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
        Vector3 trDir = (ghost.position - target.position).normalized;
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
            yield return waitSec;
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

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Charactor").GetComponent<Transform>();
        waitSec = new WaitForSeconds(1f);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(OpenDoor());
        if (isChacing)
        { 
            agent.destination = target.position;
            if (GameManager.Instance.isInCloset)
            {
                isChacing = false;
            }
        }
        else
        {

        }
    }
}
