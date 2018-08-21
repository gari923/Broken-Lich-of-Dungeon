using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            gameOver = !gameOver;
            StartCoroutine(TheEnd());
        }
        if (gameClear)
        {
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
