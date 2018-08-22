using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;
    public TextMesh t1, t2;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        t2.text = "";
    }

    public void OnGameStart()
    {
        StartCoroutine("LoadScene");
    }

    public void OnGameExit()
    {
        Application.Quit();
    }

    IEnumerator LoadScene()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(1);
        ao.allowSceneActivation = false;
        
        for (int i = 0; i < t1.text.Length; i++)
        {
            t2.text += t1.text[i];
            print(t2.text.Length + " > " + t1.text.Length * ao.progress + 1);
            yield return new WaitWhile(() => t2.text.Length > t1.text.Length * ao.progress + 1);
            print(ao.isDone);
            if (ao.progress >= 0.8F)
            {
                print("complete");
                // 강제로 진행 바를 100%까지 조정
                t2.text = "Broken Lich Of Old Dungeon";

                ao.allowSceneActivation = true;// 숨겨왔던 씬을 보여주기
                break;
            }
        }
        print("good");
    }
}