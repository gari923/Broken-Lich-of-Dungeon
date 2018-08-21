using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//필수로 들어가야 하는것!!!
//이방에 들어가면 플레이어가 무슨 상태로 변하는지 값을 보낸다
//방의 클리어 조건을 완료하면
//GameManager.instance.rock = true;
//문에 상호작용을 누르면
//SetActive(false);
//GameManager.instance.move = true;

/// <summary>
///룸은 기믹의 작동 여부를 받아서 문을 여는 코드
/// </summary>
public class Start_Room : MonoBehaviour
{
    #region 멤버 변수
    //방의 변수 필요
    public GameObject start_Room;

    // 클리어 조건
    public bool door_Action_Check = false;

    SaveClass saveData;// 세이브 데이터 클래스
    #endregion

    #region 시작 함수
    void Start()
    {
        saveData = (SaveClass)AssetDatabase.// 세이브 데이터 파일 불러오기
                LoadAssetAtPath("Assets/Data/SaveData.asset", typeof(SaveClass));

        Player.instance.ps = pState.Idle;//플레이어의 상태를 아이들 상태로 만든다
        GameManager.instance.rock = true;//방의 클리어 조건을 완료시킨다
    }
    #endregion

    #region 업데이트 함수
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
<<<<<<< HEAD
            StartCoroutine("DoorOpen");

            door_Action_Check = false;
            GameManager.instance.move = true;

            // 세이브 데이터////////////////////////////////////////////
            print("저장할것이다");
            saveData.savedGold = User_Manager.gold;
            saveData.savedLV = User_Manager.LV;
            saveData.savedAttack = User_Manager.attack;
            saveData.savedHP = User_Manager.hp;
            saveData.savedMaxHP = User_Manager.max_hp;
            saveData.savedPower = User_Manager.power;
            saveData.savedHealth = User_Manager.health;
            saveData.savedLeftWeapon = User_Manager.left_weapon_slot;
            saveData.savedRightWeapon = User_Manager.right_weapon_slot;
            saveData.savedRightNumber = User_Manager.right_weapon_num;
            saveData.savedLeftNumber = User_Manager.left_weapon_num;
            saveData.savedWeaponDamage = User_Manager.weapon_Damage;
            saveData.savedWeaponHealth = User_Manager.weapon_Health;
            print("저장된것이다");
            ///////////////////////////////////////////////////////////
        }

    }
    #endregion
    IEnumerator DoorOpen()
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
        while (curTime <= openTime)
        {
            curTime += Time.deltaTime;
            doorY.y += Mathf.Lerp(-45, 0, Time.deltaTime);
            door.eulerAngles = doorY;
            yield return new WaitForEndOfFrame();
=======
            door_Action_Check = false;
            GameManager.instance.move = true;
>>>>>>> parent of 3329688... gari
        }
    }
}
