using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Manager : MonoBehaviour
{
    //싱글톤
    public static Shop_Manager instance;

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

    //리스트 변수
    public Text list1;
    public Text list2;
    public Text list3;
    public Text page;

    //페이지 변수
    int page_num;
    //리스트 변수
    int list_num;
    //현재 리스트가 무슨 리스트인지 확인하는 변수
    string current_list;

    //상점에서 부르는값
    string item_name;
    float buy_gold;
    float amount;
    string weapon_type; //무기 타입
    string expendables_type; //소모품 타입
    string state_type; //스텟 타입

    //대화하기 창이 열려있는지 확인하는 변수
    bool dialog_state = false;
    //상점창이 열려있는지 확인하는 변수
    public bool shop_state = false;

    #region 어웨이크
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    #region 스타트
    void Start()
    {
        //유니티짱이 바라보는 기본 방향
        baseRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
    #endregion

    void Update()
    {
        #region 유니티짱 행동
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
        #endregion

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
            #region 목록 버튼
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
            #endregion

            #region 페이지 업/다운 버튼
            //만약 페이지 업 버튼을 클릭했다면
            if (Player.instance.rayObject.name == "btn_Page_Up" && Player.instance.buttonClicked == true)
            {
                //페이지 수가 9보다 작을경우
                if (page_num < 9)
                {
                    page_num++;//페이지 수를 1 증가 시킨다
                    page.text = page_num.ToString();//페이지 텍스트를 현재 페이지 넘버로 바꾼다
                    //현재 리스트에 따라 맞는 페이지를 띄운다
                    switch (current_list)
                    {
                        case "무기":
                            for (int i = 1; i < 4; i++)
                            {
                                list_num = i;
                                OnBtn_Weapon_List();
                            }
                            break;
                        case "소모품":
                            for (int i = 1; i < 4; i++)
                            {
                                list_num = i;
                                OnBtn_Expendables_List();
                            }
                            break;
                        case "스텟":
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
                //페이지 수가 1보다 큰경우
                if (page_num > 1)
                {
                    page_num--;//페이지 수를 1 감소 시킨다
                    page.text = page_num.ToString();//페이지 텍스트를 현재 페이지 넘버로 바꾼다
                    //현재 리스트에 따라 맞는 페이지를 띄운다
                    switch (current_list)
                    {
                        case "무기":
                            for (int i = 1; i < 4; i++)
                            {
                                list_num = i;
                                OnBtn_Weapon_List();
                            }
                            break;
                        case "소모품":
                            for (int i = 1; i < 4; i++)
                            {
                                list_num = i;
                                OnBtn_Expendables_List();
                            }
                            break;
                        case "스텟":
                            for (int i = 1; i < 4; i++)
                            {
                                list_num = i;
                                OnBtn_State_List();
                            }
                            break;
                    }
                }
            }
            #endregion

            //만약 리스가 무기이고 페이지의 버튼을눌렀다면
            switch (current_list)
            {
                case "무기":
                    //1번째 리스트 버튼을 눌렀을때
                    if (Player.instance.rayObject.name == "btn_List1" && Player.instance.buttonClicked == true)
                    {
                        list_num = 1;
                        OnBtn_Weapon_List();
                        //유저가 가진 돈이 아이템의 가격보다 많다면
                        if (User_Manager.gold >= buy_gold)
                        {
                            User_Manager.gold -= Item_Manager.buy_gold;//유저의 돈에서 아이템 가격을 뺸다
                            gold.text = User_Manager.gold.ToString();//상점의 골드 텍스트를 유저의 골드 텍스트로 바꾼다
                            User_Manager.right_weapon_slot = item_name;
                            User_Manager.weapon_Damage = amount; //유저에게 무기데미지를 전해준다
                            User_Manager.attack = User_Manager.power * 2 + User_Manager.weapon_Damage; //유저의 공격데미지를 공식에 맞춰준다
                        }
                    }
                    //2번째 리스트 버튼을 눌렀을때
                    else if (Player.instance.rayObject.name == "btn_List2" && Player.instance.buttonClicked == true)
                    {
                        list_num = 2;
                        OnBtn_Weapon_List();
                    }
                    break;
                case "소모품":
                    //1번째 리스트 버튼을 눌렀을때
                    if (Player.instance.rayObject.name == "btn_List1" && Player.instance.buttonClicked == true)
                    {
                        list_num = 1;
                        OnBtn_Expendables_List();
                        //유저가 가진 돈이 아이템의 가격보다 많다면
                        if (User_Manager.gold >= buy_gold)
                        {
                            User_Manager.gold -= Item_Manager.buy_gold;//유저의 돈에서 아이템 가격을 뺸다
                            gold.text = User_Manager.gold.ToString();//상점의 골드 텍스트를 유저의 골드 텍스트로 바꾼다

                            //현재 hp와 회복량을 더한값이 max_hp값을 넘어간다면
                            if (User_Manager.hp + amount > User_Manager.max_hp)
                            {
                                User_Manager.hp = User_Manager.max_hp;
                            }
                            //현재 hp와 회복량을 더한값이 max_hp값보다 작거나 같을경우
                            else if (User_Manager.hp + amount <= User_Manager.max_hp)
                            {
                                User_Manager.hp += amount;
                            }
                        }
                    }
                    //2번째 리스트 버튼을 눌렀을때
                    else if (Player.instance.rayObject.name == "btn_List2" && Player.instance.buttonClicked == true)
                    {
                        list_num = 2;
                        OnBtn_Expendables_List();
                        //유저가 가진 돈이 아이템의 가격보다 많다면
                        if (User_Manager.gold >= buy_gold)
                        {
                            User_Manager.gold -= Item_Manager.buy_gold;//유저의 돈에서 아이템 가격을 뺸다
                            gold.text = User_Manager.gold.ToString();//상점의 골드 텍스트를 유저의 골드 텍스트로 바꾼다

                            //현재 hp와 회복량을 더한값이 max_hp값을 넘어간다면
                            if (User_Manager.hp + amount > User_Manager.max_hp)
                            {
                                User_Manager.hp = User_Manager.max_hp;
                            }
                            //현재 hp와 회복량을 더한값이 max_hp값보다 작거나 같을경우
                            else if (User_Manager.hp + amount <= User_Manager.max_hp)
                            {
                                User_Manager.hp += amount;
                            }
                        }
                    }
                    //3번째 리스트 버튼을 눌렀을때
                    if (Player.instance.rayObject.name == "btn_List3" && Player.instance.buttonClicked == true)
                    {
                        list_num = 3;
                        OnBtn_Expendables_List();
                        //유저가 가진 돈이 아이템의 가격보다 많다면
                        if (User_Manager.gold >= buy_gold)
                        {
                            User_Manager.gold -= Item_Manager.buy_gold;//유저의 돈에서 아이템 가격을 뺸다
                            gold.text = User_Manager.gold.ToString();//상점의 골드 텍스트를 유저의 골드 텍스트로 바꾼다

                            //현재 hp와 회복량을 더한값이 max_hp값을 넘어간다면
                            if (User_Manager.hp + amount > User_Manager.max_hp)
                            {
                                User_Manager.hp = User_Manager.max_hp;
                            }
                            //현재 hp와 회복량을 더한값이 max_hp값보다 작거나 같을경우
                            else if (User_Manager.hp + amount <= User_Manager.max_hp)
                            {
                                User_Manager.hp += amount;
                            }
                        }
                    }
                    break;
                case "스텟":
                    if (Player.instance.rayObject.name == "btn_List1" && Player.instance.buttonClicked == true)
                    {
                        //1번째 리스트 버튼을 눌렀을때
                        if (Player.instance.rayObject.name == "btn_List1" && Player.instance.buttonClicked == true)
                        {
                            list_num = 1;
                            OnBtn_State_List();

                            //유저가 가진 돈이 아이템의 가격보다 많다면
                            if (User_Manager.gold >= buy_gold)
                            {
                                User_Manager.gold -= Item_Manager.buy_gold;//유저의 돈에서 아이템 가격을 뺸다
                                gold.text = User_Manager.gold.ToString();//상점의 골드 텍스트를 유저의 골드 텍스트로 바꾼다
                                User_Manager.LV += 1; //유저의 LV을 올린다
                                switch (state_type)
                                {
                                    case "힘":
                                        User_Manager.power += 1;
                                        break;
                                    case "체력":
                                        User_Manager.health += 1;
                                        break;
                                }
                            }
                        }
                        //2번째 리스트 버튼을 눌렀을때
                        if (Player.instance.rayObject.name == "btn_List1" && Player.instance.buttonClicked == true)
                        {
                            list_num = 2;
                            OnBtn_State_List();

                            //유저가 가진 돈이 아이템의 가격보다 많다면
                            if (User_Manager.gold >= buy_gold)
                            {
                                User_Manager.gold -= Item_Manager.buy_gold;//유저의 돈에서 아이템 가격을 뺸다
                                gold.text = User_Manager.gold.ToString();//상점의 골드 텍스트를 유저의 골드 텍스트로 바꾼다
                                User_Manager.LV += 1; //유저의 LV을 올린다
                                switch (state_type)
                                {
                                    case "힘":
                                        User_Manager.power += 1;
                                        break;
                                    case "체력":
                                        User_Manager.health += 1;
                                        break;
                                }
                            }
                        }

                    }
                    break;
            }
        }
    }

    //무기 목록 버튼을 눌렀을때 실행할 메소드
    void OnBtn_Weapon_List()
    {
        current_list = "무기";//현재의 리스트를 알려준다
        switch (page_num)//페이지 번호에 따른 리스트를 보여준다
        {
            case 1:
                switch (list_num)//리스트 번호에 따른 리스트를 보여준다
                {
                    case 1:
                        item_name = "부숴진검";
                        buy_gold = 1000;
                        amount = 3;
                        list1.text = item_name + " / 공격력 + " + amount + " / " + buy_gold + " Gold";
                        break;
                    case 2:
                        list2.text = "";
                        break;
                    case 3:
                        list3.text = "";
                        break;
                }
                break;
            default: //빈 페이지일 경우 공백을 띄운다
                switch (list_num)//리스트 번호에 따른 리스트를 보여준다
                {
                    case 1:
                        list1.text = "";
                        break;
                    case 2:
                        list2.text = "";
                        break;
                    case 3:
                        list3.text = "";
                        break;
                }
                break;
        }
    }
    //소비창 리스트 버튼을 눌렀을때 실행할 메소드
    void OnBtn_Expendables_List()
    {
        current_list = "소모품";//현재의 리스트를 알려준다
        switch (page_num)//페이지 번호에 따른 리스트를 보여준다
        {
            case 1:
                switch (list_num)//리스트 번호에 따른 리스트를 보여준다
                {
                    case 1:
                        item_name = "(소형)HP회복";
                        buy_gold = 30;
                        amount = 10;
                        list1.text = item_name + " / hp + " + amount + " / " + buy_gold + " Gold";
                        break;
                    case 2:
                        item_name = "(중형)HP회복";
                        buy_gold = 70;
                        amount = 30;
                        list2.text = item_name + " / hp + " + amount + " / " + buy_gold + " Gold";
                        break;
                    case 3:
                        item_name = "(대형)HP회복";
                        buy_gold = 120;
                        amount = 50;
                        list3.text = item_name + " / hp + " + amount + " / " + buy_gold + " Gold";
                        break;
                }
                break;
            case 2:
                switch (list_num)//리스트 번호에 따른 리스트를 보여준다
                {
                    case 1:
                        list1.text = "";
                        break;
                    case 2:
                        list2.text = "";
                        break;
                    case 3:
                        list3.text = "";
                        break;
                }
                break;
            default: //빈 페이지일 경우 공백을 띄운다
                switch (list_num)//리스트 번호에 따른 리스트를 보여준다
                {
                    case 1:
                        list1.text = "";
                        break;
                    case 2:
                        list2.text = "";
                        break;
                    case 3:
                        list3.text = "";
                        break;
                }
                break;
        }
    }
    //스텟창 리스트 버튼을 눌렀을때 실행할 메소드
    void OnBtn_State_List()
    {
        current_list = "스텟";//현재의 리스트를 알려준다
        switch (page_num)//페이지 번호에 따른 리스트를 보여준다
        {
            case 1:
                switch (list_num)//리스트 번호에 따른 리스트를 보여준다
                {
                    case 1:
                        item_name = "힘 증가";
                        buy_gold = 200 + User_Manager.LV * 300;
                        amount = 1;
                        state_type = "힘";
                        list1.text = item_name + " / 힘 + " + amount + " / " + buy_gold + " Gold";
                        break;
                    case 2:
                        item_name = "체력 증가";
                        buy_gold = 200 + User_Manager.LV * 300;
                        amount = 1;
                        state_type = "체력";
                        list2.text = item_name + " / 체력 + " + amount + " / " + buy_gold + " Gold";
                        break;
                    case 3:
                        list3.text = "";
                        break;
                }
                break;
            default: //빈 페이지일 경우 공백을 띄운다
                switch (list_num)//리스트 번호에 따른 리스트를 보여준다
                {
                    case 1:
                        list1.text = "";
                        break;
                    case 2:
                        list2.text = "";
                        break;
                    case 3:
                        list3.text = "";
                        break;
                }
                break;
        }
    }
}