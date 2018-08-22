#region 네임스페이스
using System.Collections;
using System.IO;
//using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#endregion

/// <summary>
/// 로비 씬을 관리하는 매니저
/// </summary>
public class GameSceneManager : MonoBehaviour
{
    #region 멤버 변수
    public static GameSceneManager Instance;
    public TextMesh t1, t2;
    public GameObject ne;
    public GameObject cont;
    public GameObject exi;
    #endregion

    #region 어웨이크 함수
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    #region 시작 함수
    void Start()
    {
        //if (!File.Exists("Assets/Data/SaveData.asset"))
        //{
        //    cont.SetActive(false);
        //}

        DontDestroyOnLoad(gameObject);
        t2.text = "";
    }
    #endregion

    #region 게임 시작 함수
    public void OnGameStart()
    {
        StartCoroutine("LoadScene");
    }
    #endregion

    #region 새 게임 함수
    public void OnNewGameStart()
    {
        //if (File.Exists("Assets/Data/SaveData.asset"))
        //{
        //    print("확인");
        //    File.Delete("Assets/Data/SaveData.asset");

        //    SaveClass asset = SaveClass.CreateInstance<SaveClass>();
        //    AssetDatabase.CreateAsset(asset, "Assets/Data/SaveData.asset");
        //    AssetDatabase.SaveAssets();

        //    EditorUtility.FocusProjectWindow();

        //    Selection.activeObject = asset;
        //}

        PlayerPrefs.DeleteAll();

        StartCoroutine("LoadScene");
    }
    #endregion

    #region 게임 종료 함수
    public void OnGameExit()
    {
        Application.Quit();
    }
    #endregion

    #region 메인 씬 로딩
    IEnumerator LoadScene()
    {
        //ne.GetComponent<Button>().interactable = false;
        //cont.GetComponent<Button>().interactable = false;
        //exi.GetComponent<Button>().interactable = false;

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
    #endregion
}