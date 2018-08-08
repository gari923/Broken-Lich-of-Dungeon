#region 네임스페이스
using UnityEngine;
#endregion

/// <summary>
/// 방 이동을 관리하는 게임 매니저
/// </summary>
public class GameManager : MonoBehaviour
{
    #region 멤버 변수
    public static GameManager instance;// 싱글톤 변수

    // 각각의 방에 들어갔는지 체크하는 플래그
    bool start_Check = false;
    bool last_Check = false;
    bool enemy_Check = true;
    bool patience_Check = true;
    bool knife_Check = true;
    bool projectile_Check = true;
    bool door_Check = true;

    // 각각의 방
    public GameObject start_Room;
    public GameObject last_Room;
    public GameObject enemy_Room;
    public GameObject patience_Room;
    public GameObject knife_Room;
    public GameObject projectile_Room;
    public GameObject door_Room;

    // 각각의 방의 시작지점
    public Transform start_Room_StartPoint;
    public Transform last_Room_StartPoint;
    public Transform enemy_Room_StartPoint;
    public Transform patience_Room_StartPoint;
    public Transform knife_Room_StartPoint;
    public Transform projectile_Room_StartPoint;
    public Transform door_Room_StartPoint;

    GameObject player;// 플레이어

    // 다른 방으로 갈수 있는지 확인하는 변수(각 방에서 조건을 완료시키면 true)
    public bool rock = false;
    // 플레이어가 이동의 상호작용을 활성화 확인(각 방에서 이동 상호작용을 누르면 true)
    public bool move = false;
    #endregion

    #region 어웨이크 함수
    void Awake()
    {
        if (instance == null)// 싱글톤 할당
        {
            instance = this;
        }
    }
    #endregion

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    #region 업데이트 함수
    void Update()
    {
        // 모든 방이 비활성화일 경우 마지막방을 활성화
        if (start_Check == false
            && last_Check == false
            && enemy_Check == false
            && patience_Check == false
            && knife_Check == false
            && projectile_Check == false
            && door_Check == false)
        {
            last_Check = true;
        }

        // Rock 이 True이고 플레이어가 상호작용키를 눌렀을 경우 방을 랜덤으로 이동
        if (rock == true && move == true)
        {
            int random = Random.Range(0, 7);// 방을 선택할 랜덤 변수

            // 랜덤 변수에 해당하는 방의 좌표로 이동
            switch (random)
            {
                case 0:
                    if (start_Check == true)// 방에 들어갔는지를 체크한다

                    {
                        start_Check = false;// 그 방에 들어갔다고 체크
                        start_Room.SetActive(true);// 해당하는 방을 활성
                        player.transform.position
                            = start_Room_StartPoint.transform.position;// 플레이어를 해당 좌표로 이동
                        rock = false;// 락을 걸기
                        move = false;// 상호작용 완료
                        print(random + "방으로 이동합니다");
                    }
                    break;
                case 1:
                    if (last_Check == true)
                    {
                        last_Check = false;
                        last_Room.SetActive(true);
                        player.transform.position = last_Room_StartPoint.transform.position;
                        rock = false;
                        move = false;
                        print(random + "방으로 이동합니다");
                    }
                    break;
                case 2:
                    if (enemy_Check == true)
                    {
                        enemy_Check = false;
                        enemy_Room.SetActive(true);
                        player.transform.position = enemy_Room_StartPoint.transform.position;
                        rock = false;
                        move = false;
                        print(random + "방으로 이동합니다");
                    }
                    break;
                case 3:
                    if (patience_Check == true)
                    {
                        patience_Check = false;
                        patience_Room.SetActive(true);
                        player.transform.position = patience_Room_StartPoint.transform.position;
                        rock = false;
                        move = false;
                        print(random + "방으로 이동합니다");
                    }
                    break;
                case 4:
                    if (knife_Check == true)
                    {
                        knife_Check = false;
                        knife_Room.SetActive(true);
                        player.transform.position = knife_Room_StartPoint.transform.position;
                        rock = false;
                        move = false;
                        print(random + "방으로 이동합니다");
                    }
                    break;
                case 5:
                    if (projectile_Check == true)
                    {
                        projectile_Check = false;
                        projectile_Room.SetActive(true);
                        player.transform.position = projectile_Room_StartPoint.transform.position;
                        rock = false;
                        move = false;
                        print(random + "방으로 이동합니다");
                    }
                    break;
                case 6:
                    if (door_Check == true)
                    {
                        door_Check = false;
                        door_Room.SetActive(true);
                        player.transform.position = door_Room_StartPoint.transform.position;
                        rock = false;
                        move = false;
                        print(random + "방으로 이동합니다");
                    }
                    break;
            }     
        }
    }
    #endregion
}
