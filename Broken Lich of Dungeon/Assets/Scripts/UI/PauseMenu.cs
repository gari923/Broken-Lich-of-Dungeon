using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 일시정지 클래스
/// </summary>
public class PauseMenu : MonoBehaviour
{
    #region 멤버 변수
    public Player playerScript;// 플레이어의 스크립트
    public GameObject pauseUI;// 일시정지 UI 오브젝트    

    bool paused = false;// 일시정지 확인 플래그
    #endregion

    #region 시작 함수
    void Start()
    {
        pauseUI.SetActive(false);// 시작할 때는 안보이게
    }
    #endregion

    #region 업데이트 함수
    void Update()
    {
        // 일시정지 상태에 따른 실행
        if (paused)
        {
            pauseUI.SetActive(true);

            // esc로 종료, 다른 키로 게임 재개
            if (Input.GetKeyDown(KeyCode.Escape))
            {
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
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // UI를 카메라 앞에 위치
            pauseUI.transform.forward = Camera.main.transform.forward;
            pauseUI.transform.position = Camera.main.transform.position + Camera.main.transform.forward;

            // 일시정지 상태 체크 토글
            paused = !paused;

            // 게임상의 시간 토글
            Time.timeScale = 1 - Time.timeScale;
        }
    }
    #endregion

    #region 게임 재게 함수
    public void Resume()
    {
        paused = !paused;
    }
    #endregion

    #region 타이틀 화면으로 함수
    public void Exit()
    {
        print("타이틀로");
        SceneManager.LoadScene("RobbyScene");
    }
    #endregion
}
