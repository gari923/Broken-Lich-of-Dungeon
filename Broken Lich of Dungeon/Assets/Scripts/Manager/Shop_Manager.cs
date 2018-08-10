using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Manager : MonoBehaviour
{
    public static Shop_Manager instance;

    //대화하기 창 변수
    public GameObject dialog;
    //상점 리스트 창 변수
    public GameObject shop;
    //상점 충돌 창 변수
    public GameObject shop_collider;
    //버튼의 오브젝트를 받을 변수
    public GameObject btn_Weapon_list;
    public GameObject btn_Expendables_list;
    public GameObject btn_buy_state;

    //소지 골드 Text
    public Text gold;

    //플레이어 변수
    public Transform player;
    //유니티짱의 변수
    public Transform unitychan;
    //유니티짱의 기본 방향 변수
    Quaternion baseRotation;
    //상점의 게임 오브젝트

    //리스트 변수
    public Text list1;
    public Text list2;
    public Text list3;
    public Text page;

    //페이지 변수
    int page_num;
    //리스트 변수
    int list_num;

    //list mode 변수(현재 리스트가 무슨 리스트인지 확인하는 변수)
    int list_mode;

    //상점에서 부르는값
    public static string buy_name;

    //대화하기 창이 열려있을때 상호작용키를 누르면 상점창을 띄운다
    //대화하기 창이 열려있는지 확인하는 변수
    bool dialog_state = false;
    public bool shop_state = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        //유니티짱이 바라보는 기본 방향
        baseRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    void Update()
    {
        #region 상점 행동
        //유니티짱에 레이가 닿았을 경우 유니티짱이 플레이어를 바라보고 대화하기 창을 활성화 시키고 싶다
        //플레이어 사거리 안에 아무것도 없을경우
        if (Player.instance.rayObject == null && shop_state == false)
        {
            dialog.SetActive(false);//대화하기 창을 비활성화 시킨다
            dialog_state = false;//대화하기 창이 안열려있다고 알려준다

            unitychan.rotation = Quaternion.Slerp(unitychan.rotation, baseRotation, 2 * Time.deltaTime);//(0,0,0)방향을 바라본다
        }
        //플레이어 사거리안에 무언가 있을경우
        else if (Player.instance.rayObject != null && shop_state == false)
        {
            //만약 플레이어 사거리안에 유니티짱이 있다면
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
                    page_num = 1;//페이지를 기본값으로 만든다
                    page.text = page_num.ToString();//페이지 텍스트를 현재 페이지 넘버로 바꾼다
                    for (int i = 1; i < 4; i++)
                    {
                        list_num = i;
                        OnBtn_Weapon_List();//리스트를 기본값으로 만든다
                    }
                    dialog.SetActive(false);//대화하기 창을 종료한다
                    shop_collider.SetActive(true);//상점 충돌을 활성화 시킨다
                    shop.SetActive(true);//상점을 연다
                    shop_state = true;//상점이 열려있다고 알려준다
                }
            }
            //아니라면
            else
            {
                dialog.SetActive(false);//대화하기 창을 비활성화 시킨다
                dialog_state = false;//대화하기 창이 안열려있다고 알려준다

                unitychan.rotation = Quaternion.Slerp(unitychan.rotation, baseRotation, 2 * Time.deltaTime);//(0,0,0)방향을 바라본다
            }
        }

        //만약 상점창이 열려있고 플레이어 사거리에 상점 리스트가 없으면
        if (shop_state == true && (Player.instance.rayObject == null || Player.instance.rayObject.tag != "shop"))
        {
            shop_collider.SetActive(false);//상점 충돌을 비활성화 시킨다
            shop.SetActive(false);//상점을 닫는다
            shop_state = false;//상점이 닫혀있다고 알려준다
        }
        //만약 상점창이 열려있고 플레이어 사거리안에 상점 리스트가 있으면 
        else if (shop_state == true && (Player.instance.rayObject != null || Player.instance.rayObject.tag == "shop"))
        {
            //만약 레이가 무기 리스트에 닿았고 클릭했다면
            if (Player.instance.rayObject.name == "btn_Weapon_list" && Player.instance.buttonClicked == true)
            {
                page_num = 1;//페이지를 기본값으로 만든다
                page.text = page_num.ToString();//페이지 텍스트를 현재 페이지 넘버로 바꾼다
                for (int i = 1; i < 4; i++)
                {
                    list_num = i;
                    OnBtn_Weapon_List();//무기 목록을 띄운다
                }
            }
            //만약 레이가 소비품 리스트에 닿았고 클릭했다면
            else if (Player.instance.rayObject.name == "btn_Expendables_list" && Player.instance.buttonClicked == true)
            {
                page_num = 1;//페이지를 기본값으로 만든다
                page.text = page_num.ToString();//페이지 텍스트를 현재 페이지 넘버로 바꾼다
                for (int i = 1; i < 4; i++)
                {
                    list_num = i;
                    OnBtn_Expendables_List();//소비품 리스트를 띄운다
                }
            }
            //만약 레이가 스텟 리스트에 닿았고 클릭했다면
            else if (Player.instance.rayObject.name == "btn_buy_state" && Player.instance.buttonClicked == true)
            {
                page_num = 1;//페이지를 기본값으로 만든다
                page.text = page_num.ToString();//페이지 텍스트를 현재 페이지 넘버로 바꾼다
                for (int i = 1; i < 4; i++)
                {
                    list_num = i;
                    OnBtn_State_List();//스텟 리스트를 띄운다
                }
            }

            //만약 페이지 업 버튼을 클릭했다면
            if (Player.instance.rayObject.name == "btn_Page_Up" && Player.instance.buttonClicked == true)
            {
                if (page_num < 99)//페이지 수가 99보다 작을경우
                {
                    page_num++;//페이지 수를 1 증가 시킨다
                    page.text = page_num.ToString();//페이지 텍스트를 현재 페이지 넘버로 바꾼다
                    switch (list_mode)//현재 리스트에 따라 맞는 페이지를 띄운다
                    {
                        case 1://1번일 경우 무기 목록을 보여준다
                            for (int i = 1; i < 4; i++)
                            {
                                list_num = i;
                                OnBtn_Weapon_List();
                            }
                            break;
                        case 2://2번일 경우 소비품 목록을 보여준다
                            for (int i = 1; i < 4; i++)
                            {
                                list_num = i;
                                OnBtn_Expendables_List();
                            }
                            break;
                        case 3://3번일 경우 스텟 목록을 보여준다
                            for (int i = 1; i < 4; i++)
                            {
                                list_num = i;
                                OnBtn_State_List();
                            }
                            break;
                    }
                }
            }
            //만약 페이지 다운 버튼을 클릭했다면
            else if (Player.instance.rayObject.name == "btn_Page_Down" && Player.instance.buttonClicked == true)
            {
                if (page_num > 1)//페이지 수가 1보다 큰경우
                {
                    page_num--;//페이지를 감소시킨다
                    page.text = page_num.ToString();//페이지 텍스트를 현재 페이지 넘버로 바꾼다
                    switch (list_mode)//현재 리스트에 따라 맞는 페이지를 띄운다
                    {
                        case 1://1번일 경우 무기 목록을 보여준다
                            for (int i = 1; i < 4; i++)
                            {
                                list_num = i;
                                OnBtn_Weapon_List();
                            }
                            break;
                        case 2://2번일 경우 소비품 목록을 보여준다
                            for (int i = 1; i < 4; i++)
                            {
                                list_num = i;
                                OnBtn_Expendables_List();
                            }
                            break;
                        case 3://3번일 경우 스텟 목록을 보여준다
                            for (int i = 1; i < 4; i++)
                            {
                                list_num = i;
                                OnBtn_State_List();
                            }
                            break;
                    }
                }
            }

            //만약 리스트를 클릭했다면
            if (Player.instance.rayObject.name == "btn_List1" && Player.instance.buttonClicked == true)
            {
                switch (list_mode)
                {
                    case 1:
                        list_num = 1;
                        OnBtn_Weapon_List();
                        if (Item_Manager.buy_gold <= User_Manager.gold)
                        {
                            User_Manager.gold -= Item_Manager.buy_gold;//유저가 가진돈에서 아이템 가격을 뺀다
                            User_Manager.weapon_slot = buy_name;//유저의 무기 슬롯에 무기를 끼운다
                        }
                        break;
                    case 2:
                        list_num = 1;
                        OnBtn_Expendables_List();
                        if (Item_Manager.buy_gold <= User_Manager.gold)
                        {
                            User_Manager.gold -= Item_Manager.buy_gold;//유저가 가진돈에서 아이템 가격을 뺀다
                            //만약 hp에 회복량을 더한값이 유저의 최대채력을 넘어간다면
                            if (User_Manager.hp + Item_Manager.heal > User_Manager.max_hp)
                            {
                                //유저의 hp를 max로 만든다
                                User_Manager.hp = User_Manager.max_hp;
                            }
                            //아니라면
                            else
                            {
                                User_Manager.hp += Item_Manager.heal;//유저의 현재 HP/MP에 아이템의 회복량만큼 회복을한다
                            }
                        }
                        break;
                    case 3:
                        list_num = 1;
                        OnBtn_State_List();
                        break;
                }
            }
            else if (Player.instance.rayObject.name == "btn_List2" && Player.instance.buttonClicked == true)
            {
                switch (list_mode)
                {
                    case 1:
                        list_num = 2;
                        OnBtn_Weapon_List();
                        if (Item_Manager.buy_gold <= User_Manager.gold)
                        {
                            User_Manager.gold -= Item_Manager.buy_gold;//유저가 가진돈에서 아이템 가격을 뺀다
                            User_Manager.weapon_slot = buy_name;//유저의 무기 슬롯에 무기를 끼운다
                        }
                        break;
                    case 2:
                        list_num = 2;
                        OnBtn_Expendables_List();
                        User_Manager.gold -= Item_Manager.buy_gold;//유저가 가진돈에서 아이템 가격을 뺀다
                        //만약 hp에 회복량을 더한값이 유저의 최대채력을 넘어간다면
                        if (User_Manager.hp + Item_Manager.heal > User_Manager.max_hp)
                        {
                            //유저의 hp를 max로 만든다
                            User_Manager.hp = User_Manager.max_hp;
                        }
                        //아니라면
                        else
                        {
                            User_Manager.hp += Item_Manager.heal;//유저의 현재 HP/MP에 아이템의 회복량만큼 회복을한다
                        }
                        break;
                    case 3:
                        list_num = 2;
                        OnBtn_State_List();
                        break;
                }
            }
            else if (Player.instance.rayObject.name == "btn_List3" && Player.instance.buttonClicked == true)
            {
                switch (list_mode)
                {
                    case 1:
                        list_num = 3;
                        OnBtn_Weapon_List();
                        if (Item_Manager.buy_gold <= User_Manager.gold)
                        {
                            User_Manager.gold -= Item_Manager.buy_gold;//유저가 가진돈에서 아이템 가격을 뺀다
                            User_Manager.weapon_slot = buy_name;//유저의 무기 슬롯에 무기를 끼운다
                        }
                        break;
                    case 2:
                        list_num = 3;
                        OnBtn_Expendables_List();
                        //만약 hp에 회복량을 더한값이 유저의 최대채력을 넘어간다면
                        if (User_Manager.hp + Item_Manager.heal > User_Manager.max_hp)
                        {
                            //유저의 hp를 max로 만든다
                            User_Manager.hp = User_Manager.max_hp;
                        }
                        //아니라면
                        else
                        {
                            User_Manager.hp += Item_Manager.heal;//유저의 현재 HP/MP에 아이템의 회복량만큼 회복을한다
                        }
                        break;
                    case 3:
                        list_num = 3;
                        OnBtn_State_List();
                        break;
                }
            }
        }
        #endregion
    }

    //웨폰 리스트 버튼을 눌렀을때 실행할 메소드
    void OnBtn_Weapon_List()
    {
        list_mode = 1;
        switch (page_num)
        {
            case 1:
                switch (list_num)
                {
                    case 1:
                        buy_name = "부숴진검";
                        list1.text = buy_name + " / 공격력 " + Item_Manager.weapon_Damage + " / " + Item_Manager.buy_gold + " Gold";
                        list1.text = buy_name + " / 공격력 3 / 1000 Gold";
                        break;
                    case 2:
                        buy_name = "주먹";
                        list2.text = "";
                        break;
                    case 3:
                        buy_name = "주먹";
                        list3.text = "";
                        break;
                }

                break;
            default: //빈 페이지일 경우 공백을 띄운다
                switch (list_num)
                {
                    case 1:
                        buy_name = "주먹";
                        list1.text = "";
                        break;
                    case 2:
                        buy_name = "주먹";
                        list2.text = "";
                        break;
                    case 3:
                        buy_name = "주먹";
                        list3.text = "";
                        break;
                }
                break;
        }
    }
    //소비창 리스트 버튼을 눌렀을때 실행할 메소드
    void OnBtn_Expendables_List()
    {
        list_mode = 2;
        switch (page_num)
        {
            case 1:
                switch (list_num)
                {
                    case 1:
                        buy_name = "(소형)HP회복";
                        //list1.text = buy_name + " / 회복량 " + Item_Manager.heal + " / " + Item_Manager.buy_gold + " Gold";
                        list1.text =buy_name + " / 회복량 10 / 30 Gold";
                        break;
                    case 2:
                        buy_name = "(중형)HP회복";
                        //list2.text = buy_name + " / 회복량 " + Item_Manager.heal + " / " + Item_Manager.buy_gold + " Gold"; ;
                        list2.text = buy_name + " / 회복량 30 / 70 Gold";
                        break;
                    case 3:
                        buy_name = "(대형)HP회복";
                        list3.text = buy_name + " / 회복량 50 / 120 Gold";
                        //list3.text = buy_name + " / 회복량 " + Item_Manager.heal + " / " + Item_Manager.buy_gold + " Gold"; ;
                        break;
                }
                break;
            case 2:
                switch (list_num)
                {
                    case 1:
                        buy_name = "(소형)MP회복";
                        //list1.text = buy_name + " / 회복량 " + Item_Manager.heal + " / " + Item_Manager.buy_gold + " Gold";
                        list1.text = buy_name + " / 회복량 10 / 50 Gold";
                        break;
                    case 2:
                        buy_name = "";
                        list2.text = "";
                        break;
                    case 3:
                        buy_name = "";
                        list3.text = "";
                        break;
                }
                break;
            default: //빈 페이지일 경우 공백을 띄운다
                switch (list_num)
                {
                    case 1:
                        buy_name = "";
                        list1.text = "";
                        break;
                    case 2:
                        buy_name = "";
                        list2.text = "";
                        break;
                    case 3:
                        buy_name = "";
                        list3.text = "";
                        break;
                }
                break;

        }
    }
    //스텟창 리스트 버튼을 눌렀을때 실행할 메소드
    void OnBtn_State_List()
    {
        list_mode = 3;
        switch (page_num)
        {
            case 1:
                switch (list_num)
                {
                    case 1:
                        buy_name = "";
                        list1.text = "힘 스텟 / 힘 +1 / 500 Gold";
                        break;
                    case 2:
                        buy_name = "";
                        //list2.text = "";
                        list1.text = "체력 스텟 / 체력 +1 / 500 Gold";
                        break;
                    case 3:
                        buy_name = "";
                        list3.text = "";
                        break;
                }
                break;
            default: //빈 페이지일 경우 공백을 띄운다
                switch (list_num)
                {
                    case 1:
                        buy_name = "";
                        list1.text = "";
                        break;
                    case 2:
                        buy_name = "";
                        list2.text = "";
                        break;
                    case 3:
                        buy_name = "";
                        list3.text = "";
                        break;
                }
                break;
        }
    }
}
