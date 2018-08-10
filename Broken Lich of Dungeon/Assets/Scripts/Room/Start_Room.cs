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
    public bool door_Action_Check = false;

    void Start()
    {
        Player.instance.ps = pState.Idle;
        GameManager.instance.rock = true;//방의 클리어 조건을 완료시킨다
    }

    void Update()
    {
        //방의 기믹들을 작동시키고 싶다
        //만약 버튼이 클릭되었고 레이가 door에 닿았다면
        if (Player.instance.buttonClicked)
        {
            if (Player.instance.rayObjectclick)
            {
                switch (Player.instance.rayObjectclick.name)
                {
                    case "Door":
                        OpenDoor();
                        break;
                }
            }
        }

        //방의 클리어 조건을 완료했고 문을 동작시켰다면
        if (GameManager.instance.rock == true && door_Action_Check == true)
        {
            GameManager.instance.move = true;
            start_Room.SetActive(false);
        }
    }
    
    void OpenDoor()
    { 
            door_Action_Check = true;
    }
}
