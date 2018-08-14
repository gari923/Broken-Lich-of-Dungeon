#region 네임스페이스
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
#endregion

/// <summary>
/// 플레이어의 정보를 관리하는 스크립트
/// </summary>
public class User_Manager : MonoBehaviour
{
    #region 멤버 변수
    //attack = power*2 + weapon
    //hp = health *20

    public static User_Manager instance;

    public static float gold;//유저의 소지 골드

    //유저의 정보
    public static float LV;// 유저 레벨
    //외부 요인 따라 변하는 유저의 정보
    public static float attack;// 공격력
    public static float hp;// 체력
    public static float max_hp;// 최대 체력

    //스텟
    public static float power;// 힘
    public static float health;// 헬스

    //유저의 장비
    public static string right_weapon_slot;// 장비 슬롯
    //장착한 장비의 데미지
    public static float weapon_Damage;

    //유저가 생존했는지 확인하는 변수
    public static bool alive = true;

    public GameObject UI;
    public Text text_lv;
    public Text text_gold;
    public Text text_hp;
    public Text text_power;
    public Text text_health;
    public Text text_right_weapon;
    public Text text_attackPoint;
    #endregion

    public GameObject damagedFX;
    public Image hpFX;

    Color tempColor;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }


    #region 시작 함수
    void Start()
    {
        gold = 0;//돈을 0으로 초기화
        LV = 1; //LV을 1로 초기화
        //스텟 초기화
        power = 5;
        health = 5;
        right_weapon_slot = "스틸 대거";
        //스텟공식
        attack = power * 2 + weapon_Damage;
        max_hp = health * 20;

        hp = max_hp;//hp를 최대치로 초기화

        alive = true;

        tempColor = new Color();
    }
    #endregion

    #region 업데이트 함수
    void Update()
    {
        print(gold);
        //플레이어의 스테이터스
        if (Player.instance.status == true && Shop_Manager.instance.shop_state == false)
        {
            text_lv.text = "LV " + LV;
            text_gold.text = "GOLD : " + gold;
            text_hp.text = "hp : " + hp + " / " + max_hp;
            text_power.text = "힘 : " + power;
            text_health.text = "체력 : " + health;
            text_right_weapon.text = "오른손 장비 : " + right_weapon_slot;
            text_attackPoint.text = "공격력 : " + attack;
            UI.SetActive(true);
        }
        else if (Player.instance.status == false)
        {
            UI.SetActive(false);
        }
        if (hp<0)
        {
            alive = false;
        }

        if (hp >= max_hp / 20)
        {
            tempColor = hpFX.color;
            tempColor.a = 1 - hp / max_hp;
            hpFX.color = tempColor;
        }

        if (hp <= 0)
        {
            alive = false;
        }

        if(hp > max_hp)
        {
            tempColor = hpFX.color;
            tempColor.a = 255;
            hpFX.color = tempColor;
        }
    }
    #endregion

    public void Damaged()
    {
        //print(hp);
        //print(tempColor.a);

        StartCoroutine("DamageProcess");
    }

    IEnumerator DamageProcess()
    {
        damagedFX.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        damagedFX.SetActive(false);
    }
    
}
