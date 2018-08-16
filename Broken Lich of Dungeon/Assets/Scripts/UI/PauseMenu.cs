using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Player playerScript;
    public GameObject pauseUI;

    bool paused = false;

    void Start()
    {

        pauseUI.SetActive(false);
    }

    void Update()
    {

        if (paused)
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0F;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                print("종료한다");
                Exit();
            }
            else if (Input.anyKeyDown)
            {
                Resume();
            }
        }
        else
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1F;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("outside");
            pauseUI.transform.forward = Camera.main.transform.forward;
            pauseUI.transform.position = Camera.main.transform.position + Camera.main.transform.forward;

            paused = !paused;
        }
    }

    public void Resume()
    {
        paused = !paused;
    }

    public void Exit()
    {
        print("앱 종료");
        Application.Quit();
    }
}
