using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellSound : MonoBehaviour
{
    public List<AudioClip> bellList;
    void OnCollisionEnter(Collision col)
    {
        GetComponent<AudioSource>().clip = bellList[Random.Range(0, 7)];
        GetComponent<AudioSource>().Play();
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
