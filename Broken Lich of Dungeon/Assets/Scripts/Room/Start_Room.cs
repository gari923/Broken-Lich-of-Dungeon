using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//룸은 기믹의 작동 여부를 받아서 문을 여는 코드입니다


//필수로 들어가야 하는것!!!
//이방에 들어가면 플레이어가 무슨 상태로 변하는지 값을 보낸다
//
//방의 클리어 조건을 완료하면
//GameManager.instance.rock = true;
//문에 상호작용을 누르면
//SetActive(false);
//GameManager.instance.move = true;

public class Start_Room : MonoBehaviour
{
    //텍스트가 작동하고 문이 작동하면 탈출을 할수 있다
    //방의 변수 필요
    public GameObject start_Room;

    // 클리어 조건
    public bool text_Action_Check = false;
    public bool door_Action_Check = false;

    void Start()
    {
        Player.instance.ps = pState.Idle;

    }

    void Update()
    {
        //방의 기믹들을 작동시키고 싶다
        //만약 버튼이 클릭되었고 레이가 Start_Text에 닿았다면
        if (Player.instance.buttonClicked)
        {
            if (Player.instance.rayObjectclick)
            {
                switch (Player.instance.rayObjectclick.name)
                {
                    case "Start_Text":
                        ViewText();
                        break;
                    case "Door":
                        OpenDoor();
                        break;
                }
            }
        }

        //방의 클리어 조건을 완료했다면
        if (GameManager.instance.rock == true && door_Action_Check == true)
        {
            GameManager.instance.move = true;
            start_Room.SetActive(false);
        }
    }
    void ViewText()
    {
        //메모지를 봅니다.
        text_Action_Check = true;
        print(" 1.방은 렌덤으로 이동할 수 있다.");
        print(" 2.같은 방은 들어갈 수 없다.동작");
        print(" 3.모든 방을 전부 돌면 마지막 방에 도달할 수 있다.");
        print(" 4.각 방의 규칙을 따라 방을 클리어 해야 한다.");
        print(" 5.잃어버린 무언가가 가장 소중한 법이다.");
        //문의 잠금장치를 해제한다
        GameManager.instance.rock = true;
    }
    void OpenDoor()
    {
        //만약 작동했다면 작동했다고 보낸다
        if (text_Action_Check == true)
        {
            door_Action_Check = true;
            print("문을 엽니다.");
        }
        else
        {
            print("잠겨있습니다");
        }
    }
}
