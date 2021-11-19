using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Charactor" && GameManager.Instance.charmCount == 8)
        {
            GameManager.Instance.isPlaiable = false;
            GameManager.Instance.EndGame();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
