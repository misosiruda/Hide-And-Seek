using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject keybindPanel;
    public GameObject title;
    public void GameStart()
    {
        SceneManager.LoadScene("InGame");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void KeyBind()
    {
        keybindPanel.SetActive(true);
        title.SetActive(false);
    }

    public void KeyBindClose()
    {
        keybindPanel.SetActive(false);
        title.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
