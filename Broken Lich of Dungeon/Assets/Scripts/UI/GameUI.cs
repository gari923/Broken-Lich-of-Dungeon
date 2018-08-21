using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    public GameObject GUI;
    public bool gameOver;
    public bool gameClear;
    public SampleBoss sampleBossScript;

    bool locking;
    	
	void Start () {
        GUI.SetActive(false);
        gameOver = false;
        gameClear = false;
        locking = false;
    }
		
	void Update () {
        if (User_Manager.alive == false && !locking)
        {
            gameOver = true;
            locking = true;
        }

        if(sampleBossScript.die && !locking)
        {
            gameClear = true;
            locking = true;
        }
                
        if (gameOver)
        {
            GUI.transform.GetChild(1).GetComponent<Text>().text = "Try Again";
            gameOver = !gameOver;
            StartCoroutine(TheEnd());
        }
        if (gameClear)
        {
            GUI.transform.GetChild(1).GetComponent<Text>().text = "Good";
            gameClear = !gameClear;
            StartCoroutine(TheEnd());
        }
	}

    IEnumerator TheEnd()
    {
        Time.timeScale = 1 - Time.timeScale;
        GUI.SetActive(true);
        yield return new WaitUntil(() => Input.anyKey);
        Time.timeScale = 1 - Time.timeScale;
        GUI.SetActive(false);
    }
}
