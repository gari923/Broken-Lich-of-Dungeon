using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Manager : MonoBehaviour
{
    //대화하기 창 변수
    public GameObject dialog;
    //상점 리스트 창 변수
    public GameObject shop;
    //상점 충돌 창 변수
    public GameObject shop_collider;
    //소지 골드 Text
    public Text gold;

    //플레이어 변수
    public Transform player;
    //유니티짱의 변수
    public Transform unitychan;
    //유니티짱의 기본 방향 변수
    Quaternion baseRotation;
    //상점의 게임 오브젝트

    //대화하기 창이 열려있을때 상호작용키를 누르면 상점창을 띄운다
    //대화하기 창이 열려있는지 확인하는 변수
    bool dialog_state = false;
    bool shop_state = false;
    // Use this for initialization
    void Start()
    {
        //유니티짱이 바라보는 기본좌표를 (0,0,0) 방향으로 한다
        baseRotation = Quaternion.LookRotation(new Vector3(0, 0, 0));
    }

    void Update()
    {
        //유니티짱에 레이가 닿았을 경우 유니티짱이 플레이어를 바라보고 대화하기 창을 활성화 시키고 싶다
        //유니티짱이 레이에 닿았을경우
        if (Player.instance.rayObject == null && shop_state == false)
        {
            dialog.SetActive(false);//대화하기 창을 비활성화 시킨다
            dialog_state = false;//대화하기 창이 안열려있다고 알려준다

            unitychan.rotation = Quaternion.Slerp(unitychan.rotation, baseRotation, 2 * Time.deltaTime);//(0,0,0)방향을 바라본다
        }
        else if (Player.instance.rayObject != null && shop_state == false)
        {
            if (Player.instance.rayObject.name == "unitychan")
            {
                Quaternion newRotation = Quaternion.LookRotation(player.position - unitychan.position - new Vector3(0, 1, 0));//플레이어의 방향을 가져온다
                unitychan.rotation = Quaternion.Slerp(unitychan.rotation, newRotation, 2 * Time.deltaTime);//플레이어의 방향을 바라본다

                dialog.SetActive(true);//대화하기 창을 활성화 시킨다
                dialog_state = true;//대화하기 창이 열려있다고 알려준다

                //만약 플레이어가 유니티짱을 클릭했고 대화하기 창이 열려있다면
                if (Player.instance.buttonClicked == true && dialog_state == true)
                {
                    gold.text = User_Manager.gold.ToString();//플레이어의 현재 골드를 가져온다
                    dialog.SetActive(false);//대화하기 창을 종료한다
                    shop_collider.SetActive(true);//상점 충돌을 활성화 시킨다
                    shop.SetActive(true);//상점을 연다
                    shop_state = true;//상점이 열려있다고 알려준다
                }
            }
            else
            {
                dialog.SetActive(false);//대화하기 창을 비활성화 시킨다
                dialog_state = false;//대화하기 창이 안열려있다고 알려준다

                unitychan.rotation = Quaternion.Slerp(unitychan.rotation, baseRotation, 2 * Time.deltaTime);//(0,0,0)방향을 바라본다
            }
        }
        //만약 플레이어 사거리에 상점 리스트가 없으면 상점창을 닫는다
        if (shop_state == true && (Player.instance.rayObject == null || Player.instance.rayObject.tag != "shop"))
        {
            shop_collider.SetActive(false);//상점 충돌을 비활성화 시킨다
            shop.SetActive(false);//상점을 닫는다
            shop_state = false;//상점이 닫혀있다고 알려준다
        }
    }
}
