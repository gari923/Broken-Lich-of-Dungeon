using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Player playerScript;

    Vector3 originSize;
    public GameObject pauseUI;
    float curTime;
    bool paused = false;

    void Start()
    {
        originSize = pauseUI.transform.localScale;
        pauseUI.SetActive(false);
        curTime = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Ray ray = new Ray(Camera.main.transform.position,
            Camera.main.transform.forward);
            RaycastHit hitInfo;
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

        if (paused)
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0F;

            if (playerScript.rayObject == pauseUI.transform.GetChild(0))
            {
                print("resume");
                curTime += Time.deltaTime;
                if (curTime >= 2)
                {
                    Resume();
                }
            }
            else if (playerScript.rayObject == pauseUI.transform.GetChild(1))
            {
                print("exit");
                curTime += Time.deltaTime;
                if (curTime >= 2)
                {
                    Exit();
                }
            }
            else
            {
                curTime = 0;
            }
        }
        else
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1F;
        }
    }

    public void Resume()
    {
        paused = !paused;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
