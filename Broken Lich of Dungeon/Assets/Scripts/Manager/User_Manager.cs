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
    //attack = power*2 + weapon_Damage
    //hp = health *20 + weapon_Health

    public static User_Manager instance;

    public static float gold = 0;//유저의 소지 골드

    //유저의 정보
    public static float LV = 1;// 유저 레벨
    
    //스텟
    public static float power = 5;// 힘
    public static float health = 5;// 헬스

    //외부 요인 따라 변하는 유저의 정보
    public static float attack = power * 2 + weapon_Damage;// 공격력
    public static float max_hp = health * 20 + weapon_Health;// 최대 체력
    public static float hp = max_hp;// 체력

    //유저의 장비
    public static string right_weapon_slot = "스틸 대거";// 오른손 장비 슬롯
    public static string left_weapon_slot = "버클러";// 왼손 장비 슬롯
    //장착한 장비의 데미지
    public static float weapon_Damage;
    public static float weapon_Health;

    //유저가 생존했는지 확인하는 변수
    public static bool alive = true;

    public GameObject UI;
    public Text text_lv;
    public Text text_gold;
    public Text text_hp;
    public Text text_power;
    public Text text_health;
    public Text text_right_weapon;
    public Text text_left_weapon;
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
        hp = max_hp;

        alive = true;

        tempColor = new Color();
    }
    #endregion

    #region 업데이트 함수
    void Update()
    {
        //플레이어의 스테이터스
        if (Player.instance.status == true && Shop_Manager.instance.shop_state == false)
        {
            text_lv.text = "LV " + LV;
            text_gold.text = "GOLD : " + gold;
            text_hp.text = "hp : " + hp + " / " + max_hp;
            text_power.text = "힘 : " + power;
            text_health.text = "체력 : " + health;
            text_right_weapon.text = "오른손 장비 : " + right_weapon_slot;
            text_left_weapon.text = "왼손 장비 : " + left_weapon_slot;
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
            tempColor.a = Mathf.Lerp(tempColor.a, 1 - hp / max_hp, Time.deltaTime);//1 - hp / max_hp;
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
