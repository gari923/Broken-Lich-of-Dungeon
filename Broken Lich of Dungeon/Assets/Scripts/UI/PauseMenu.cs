using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Player playerScript;
    public GameObject pauseUI;

    pState curState;
    Vector3 originSize;
    
    bool paused = false;

    void Start()
    {
        originSize = pauseUI.transform.localScale;
        pauseUI.SetActive(false);        
    }

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position,
            Camera.main.transform.forward);
        RaycastHit hitInfo;

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
            if (paused)
            {
                print("paused2play");
                Player.instance.ps = curState;
            }
            else
            {
                print("play2paused");
                Player.instance.ps = pState.Idle;
                pState curState = Player.instance.ps;
            }

            Vector3 pos;
            if (Physics.Raycast(ray, out hitInfo, 100))
            {
                print("inside");
                pos = (hitInfo.point + transform.position) / 2;

                pauseUI.transform.forward = Camera.main.transform.forward;
                pauseUI.transform.position = pos;
                pauseUI.transform.localScale = originSize * hitInfo.distance / 2;
            }
            else
            {
                print("outside");
                pauseUI.transform.forward = Camera.main.transform.forward;
                pauseUI.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
                pauseUI.transform.localScale = originSize;
            }
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
