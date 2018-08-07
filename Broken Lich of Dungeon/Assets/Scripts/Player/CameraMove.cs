#region 네임스페이스
using UnityEngine;
#endregion

/// <summary>
/// 카메라가 플레이어를 따라다니게 만드는 스크립트
/// </summary>
public class CameraMove : MonoBehaviour
{
    #region 멤버 변수
    public float CameraYPos = 1f;// 카메라 y축 이동

    Transform PlayerPos;// 플레이어 위치
    #endregion

    #region 시작 함수
    void Start()
    {
        PlayerPos = GameObject.FindGameObjectWithTag("Player").transform;// 플레이어 동적 할당
    }
    #endregion

    #region 업데이트 함수
    void Update()
    {
        Vector3 dir = PlayerPos.position;// 카메라의 위치를 플레이어의 위치로

        dir.y += CameraYPos;// 카메라의 위치를 플레이어 머리로
        transform.position = dir;// 위치 설정
    }
    #endregion
}
