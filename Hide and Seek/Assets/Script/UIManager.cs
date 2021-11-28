using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void GameResume()
    {
        GameManager.Instance.GameResume();
    }

    public void GamreRestart()
    {
        SceneManager.LoadScene("InGame");
    }

    public void GameEnd()
    {
        SceneManager.LoadScene("MainMenu");
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
