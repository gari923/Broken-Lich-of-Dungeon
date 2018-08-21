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
    //방의 변수 필요
    public GameObject start_Room;
    public Transform door;

    Vector3 doorY;
    float openTime = 2;
    float curTime = 0;
    // 클리어 조건
    public bool door_Action_Check = false;

    void Start()
    {
        Player.instance.ps = pState.Idle;//플레이어의 상태를 아이들 상태로 만든다
        GameManager.instance.rock = true;//방의 클리어 조건을 완료시킨다
        doorY = door.eulerAngles;
    }

    void Update()
    {
        Player.instance.anim.SetTrigger("IdleMode");//플레이어의 이이들 애니메이션을 실행시킨다
        Player.instance.ps = pState.Idle;//플레이어의 상태를 아이들 상태로 만든다
        GameManager.instance.rock = true;//방의 클리어 조건을 완료시킨다

        //만약 버튼이 클릭되었고 레이가 door에 닿았다면 문을연다
        if (Player.instance.buttonClicked)
        {
            if (Player.instance.rayObjectclick)
            {
                switch (Player.instance.rayObjectclick.name)
                {
                    case "Door":
                        door_Action_Check = true;
                        break;
                }
            }
        }

        //방의 클리어 조건을 완료했고 문을 동작시켰다면
        if (GameManager.instance.rock == true && door_Action_Check == true)
        {
            StartCoroutine("doorOpen");

            door_Action_Check = false;
            GameManager.instance.move = true;

        }
    }

    IEnumerator doorOpen()
    {
        curTime = 0;
        while (curTime <= openTime)
        {
            curTime += Time.deltaTime;
            doorY.y += Mathf.Lerp(0, -45, Time.deltaTime);
            door.eulerAngles = doorY;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);
        curTime = 0;
        while(curTime <= openTime)
        {
            curTime += Time.deltaTime;
            doorY.y += Mathf.Lerp(-45, 0, Time.deltaTime);
            door.eulerAngles = doorY;
            yield return new WaitForEndOfFrame();
        }

    }

}
