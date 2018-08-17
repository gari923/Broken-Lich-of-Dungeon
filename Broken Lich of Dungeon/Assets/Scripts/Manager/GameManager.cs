#region 네임스페이스
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#endregion

/// <summary>
/// 방 이동을 관리하는 게임 매니저
/// </summary>
public class GameManager : MonoBehaviour
{
    #region 멤버 변수
    public static GameManager instance;// 싱글톤 변수

    // 각각의 방에 들어갔는지 체크하는 플래그
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
    GameObject currentRoom;

    // 다른 방으로 갈수 있는지 확인하는 변수(각 방에서 조건을 완료시키면 true)
    public bool rock = false;
    // 플레이어가 이동의 상호작용을 활성화 확인(각 방에서 이동 상호작용을 누르면 true)
    public bool move = false;
    // 방클리어 확인하는 변수
    public bool clear = false;

    bool alivehp = false;

    public GameObject fadeObj;
    public float fadeTime = 1f;
    public float curTime;
    Image fadeImg;
    Color fadeColor;

    bool endPlayerMove = true;

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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fadeImg = fadeObj.transform.GetComponentInChildren<Image>();
        fadeObj.SetActive(false);
        fadeColor = new Color();
        curTime = 0;
        currentRoom = start_Room;
    }

    #region 업데이트 함수
    void Update()
    {
        // 모든 방이 비활성화일 경우 마지막방을 활성화


        // Rock 이 True이고 플레이어가 상호작용키를 눌렀을 경우 방을 랜덤으로 이동
        if (rock == true && move == true)
        {
            if (enemy_Check == false
            && patience_Check == false
            && knife_Check == false
            && projectile_Check == false
            && door_Check == false)
            {
                // 라스트룸으로

                return;
            }

            int random = Random.Range(0, 5);// 방을 선택할 랜덤 변수

            // 랜덤 변수에 해당하는 방의 좌표로 이동
            switch (random)
            {
                case 0:
                    if (enemy_Check == true)
                    {
                        enemy_Check = false;
                        enemy_Room.SetActive(true);
                        StartCoroutine(PlayerMove(enemy_Room_StartPoint, currentRoom));
                        //player.transform.position = enemy_Room_StartPoint.transform.position;
                        rock = false;
                        move = false;
                        print(random + "방으로 이동합니다");
                    }
                    break;
                case 1:
                    if (patience_Check == true)
                    {
                        patience_Check = false;
                        patience_Room.SetActive(true);
                        StartCoroutine(PlayerMove(patience_Room_StartPoint, currentRoom));
                        //player.transform.position = patience_Room_StartPoint.transform.position;
                        rock = false;
                        move = false;
                        print(random + "방으로 이동합니다");
                    }
                    break;
                case 2:
                    if (knife_Check == true)
                    {
                        knife_Check = false;
                        knife_Room.SetActive(true);
                        StartCoroutine(PlayerMove(knife_Room_StartPoint, currentRoom));
                        //player.transform.position = knife_Room_StartPoint.transform.position;
                        rock = false;
                        move = false;
                        print(random + "방으로 이동합니다");
                    }
                    break;
                case 3:
                    if (projectile_Check == true)
                    {
                        projectile_Check = false;
                        projectile_Room.SetActive(true);
                        StartCoroutine(PlayerMove(projectile_Room_StartPoint, currentRoom));
                        //player.transform.position = projectile_Room_StartPoint.transform.position;
                        rock = false;
                        move = false;
                        print(random + "방으로 이동합니다");
                    }
                    break;
                case 4:
                    if (door_Check == true)
                    {
                        door_Check = false;
                        door_Room.SetActive(true);
                        StartCoroutine(PlayerMove(door_Room_StartPoint, currentRoom));
                        //player.transform.position = door_Room_StartPoint.transform.position;
                        rock = false;
                        move = false;
                        print(random + "방으로 이동합니다");
                    }
                    break;
            }
        }

        // 유저가 죽었을경우
        if (User_Manager.alive == false)
        {
            //방을 초기화 한다
            if (start_Room.activeSelf != true)
            {
                start_Room.SetActive(true);
            }
            enemy_Check = true;
            patience_Check = true;
            knife_Check = true;
            projectile_Check = true;
            door_Check = true;
            User_Manager.hp = float.MaxValue; //유저의 피를 최대치로 채운다
            alivehp = true;
            StartCoroutine(PlayerMove(start_Room_StartPoint, currentRoom));
            User_Manager.alive = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            //SceneManager.LoadScene(0);
            //User_Manager.alive = true;
            //User_Manager.hp = User_Manager.max_hp; //유저의 피를 최대치로 채운다
            //player.transform.position
            //                = start_Room_StartPoint.transform.position;// 플레이어를 처음있던 방의 좌표로 이동
        }

        // 방을 클리어했을경우
        if (User_Manager.alive == true && clear == true && endPlayerMove == true)
        {
            if (start_Room.activeSelf != true)
            {
                start_Room.SetActive(true);
            }
            endPlayerMove = false;
            StartCoroutine(PlayerMove(start_Room_StartPoint, currentRoom));
            clear = false;
            //player.transform.position
            //                = start_Room_StartPoint.transform.position;// 플레이어를 처음있던 방의 좌표로 이동
        }
    }

    IEnumerator PlayerMove(Transform room, GameObject curRoom)
    {
        //카메라 앞 이미지 필터를 킨다
        fadeObj.SetActive(true);

        // 정해진 시간동안 화면을 어둡게 한다.
        while (curTime <= fadeTime)
        {
            curTime += Time.deltaTime;
            fadeColor.a = curTime;
            fadeImg.color = fadeColor;
            yield return new WaitForEndOfFrame();
        }
        // 정해진 시간까지 기다리고 나서 실행한다.
        yield return new WaitForSeconds(fadeTime);

        // 게임오버 자리

        // 플레이어를 해당 방으로 이동시킨다.
        player.transform.position = room.transform.position;
        player.transform.rotation = room.transform.rotation;

        currentRoom.SetActive(false);

        //print(currentRoom);
        if (alivehp == true)
        {
            //start_Room.SetActive(true);
            alivehp = false;
            User_Manager.hp = User_Manager.max_hp; //유저의 피를 최대치로 채운다
        }

        clear = false;

        currentRoom = room.parent.gameObject;

        // 시간 초기화
        curTime = 0;

        // 정해진 시간 동안 화면을 밝게 한다.
        while (curTime <= fadeTime)
        {
            curTime += Time.deltaTime;
            fadeColor.a = fadeTime - curTime;
            fadeImg.color = fadeColor;
            yield return new WaitForEndOfFrame();
        }

        // 필터를 끈다.
        fadeObj.SetActive(false);

        // 시간 초기화
        curTime = 0;

        endPlayerMove = true;


    }

    #endregion
}
