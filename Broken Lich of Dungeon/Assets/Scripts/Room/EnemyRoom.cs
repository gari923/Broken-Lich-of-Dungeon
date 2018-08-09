using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//룸은 기믹의 작동 여부를 받아서 문을 여는 코드입니다

//필수로 들어가야 하는것!!!
//방의 클리어 조건을 완료하면
//SetActive(false);
//GameManager.instance.rock = true;
//문에 상호작용을 누르면
//GameManager.instance.move = true;

public class EnemyRoom : MonoBehaviour
{

    //public static EnemyRoom instance;
    //텍스트가 작동하고 문이 작동하면 탈출을 할수 있다
    //방의 변수 필요
    //public GameObject start_Room;

    //public RaycastHit obj;

    // 클리어 조건
    public bool door_Action_Check = false;
    public bool enemy_Remove_Check = false;

    float curTime = 0;
    float startTime = 3;
    bool getStart = false;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //}

    List<Transform> spawnPool = new List<Transform>();

    void Start()
    {
        //// 자식 스포너들을 담을 변수
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Spawn"))
            {
                spawnPool.Add(transform.GetChild(i));
            }
        }

        Player.instance.ps = pState.Attack;

    }

    void Update()
    {
        if (getStart == false)
        {
            curTime += Time.deltaTime;

            if(curTime >= startTime)
            {
                curTime = 0;
                getStart = true;
            }
        }

        if (spawnPool.Count == 0 && getStart == true)
        {
            enemy_Remove_Check = true;
            Player.instance.anim.SetTrigger("IdleMode");
            Player.instance.ps = pState.Idle;
        }
        else
        {
            Player.instance.ps = pState.Attack;
            for (int i = 0; i < spawnPool.Count; i++)
            {
                if (spawnPool[i].childCount == 0)
                {
                    spawnPool.RemoveAt(i);
                }
            }
        }


        if (enemy_Remove_Check == true && door_Action_Check == true)
        {
            GameManager.instance.rock = true;
            GameManager.instance.move = true;
            //start_Room.SetActive(false);
            gameObject.SetActive(false);
        }

    }

}
