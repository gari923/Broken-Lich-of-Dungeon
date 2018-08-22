#region 네임스페이스
using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
#endregion

/// <summary>
/// 플레이어의 정보를 관리하는 스크립트
/// </summary>
public class User_Manager : MonoBehaviour
{
    #region 멤버 변수
    //attack = power*2 + weapon_Damage
    //hp = health *20 + weapon_Health

    public static User_Manager instance;// 싱글톤 변수

    public static float gold = 0;//유저의 소지 골드

    //유저의 정보
    public static float LV = 1;// 유저 레벨

    //외부 요인 따라 변하는 유저의 정보
    public static float attack;// 공격력
    public static float hp;// 체력
    public static float max_hp;// 최대 체력

    //스텟
    public static float power = 5;// 힘
    public static float health = 5;// 헬스

    //유저의 장비
    public static string right_weapon_slot = "스틸 대거";// 오른손 장비 슬롯
    public static string left_weapon_slot = "버클러";// 왼손 장비 슬롯
    public static int right_weapon_num;
    public static int left_weapon_num;

    //장착한 장비의 데미지
    public static float weapon_Damage;// 무기 대미지
    public static float weapon_Health;// 방패 체력

    //유저가 생존했는지 확인하는 변수
    public static bool alive = true;

    // 스탯 표시
    public GameObject UI;
    public Text text_lv;
    public Text text_gold;
    public Text text_hp;
    public Text text_power;
    public Text text_health;
    public Text text_right_weapon;
    public Text text_left_weapon;
    public Text text_attackPoint;

    public GameObject damagedFX;
    public Image hpFX;

    Color tempColor;

    public GameObject player_right_hand;
    public GameObject player_left_hand;
    #endregion

    #region 어웨이크 함수
    void Awake()
    {
        // 싱글톤 초기화
        if (instance == null)
        {
            instance = this;
        }
        
        gold = PlayerPrefs.GetFloat("savedGold", 0);
        LV = PlayerPrefs.GetFloat("savedLV", 1);
        attack = PlayerPrefs.GetFloat("savedAttack", 5 * 2);
        max_hp = PlayerPrefs.GetFloat("savedHP", 5 * 20);
        power = PlayerPrefs.GetFloat("savedPower", 5);
        health = PlayerPrefs.GetFloat("savedHealth", 5);
        left_weapon_slot = PlayerPrefs.GetString("savedLeftWeapon", "버클러");
        right_weapon_slot = PlayerPrefs.GetString("savedRightWeapon", "스틸 대거");
        left_weapon_num = PlayerPrefs.GetInt("savedLeftNumber", 0);
        right_weapon_num = PlayerPrefs.GetInt("savedRightNumber", 0);
        weapon_Damage = PlayerPrefs.GetFloat("savedWeaponDamage", 0);
        weapon_Health= PlayerPrefs.GetFloat("savedWeaponHealth", 0);

        // 세이브 파일이 없을 경우 파일 생성
        //if (!File.Exists("Assets/Data/SaveData.asset"))
        //{
        //    SaveClass asset = SaveClass.CreateInstance<SaveClass>();
        //    AssetDatabase.CreateAsset(asset, "Assets/Data/SaveData.asset");
        //    AssetDatabase.SaveAssets();

        //    EditorUtility.FocusProjectWindow();

        //    Selection.activeObject = asset;
        //}
        // 세이브 파일이 있을 경우 불러오기
        //else
        //{
        //    SaveClass saveData = (SaveClass)AssetDatabase.
        //        LoadAssetAtPath("Assets/Data/SaveData.asset", typeof(SaveClass));

        //    gold = saveData.savedGold;
        //    LV = saveData.savedLV;
        //    attack = saveData.savedAttack;
        //    hp = saveData.savedHP;
        //    max_hp = saveData.savedMaxHP;
        //    power = saveData.savedPower;
        //    health = saveData.savedHealth;
        //    left_weapon_slot = saveData.savedLeftWeapon;
        //    right_weapon_slot = saveData.savedRightWeapon;
        //    weapon_Damage = saveData.savedWeaponDamage;
        //    weapon_Health = saveData.savedWeaponHealth;
        //    left_weapon_num = saveData.savedLeftNumber;
        //    right_weapon_num = saveData.savedRightNumber;
        //}
    }
    #endregion

    #region 시작 함수
    void Start()
    {
        max_hp = health * 20 + weapon_Health;// 최대 체력 계산
        attack = power * 2 + weapon_Damage;// 공격력 계산
        hp = max_hp;// hp 초기화

        alive = true;

        tempColor = new Color();

        GameObject currentWeapon = GameObject.FindWithTag("weapon");// 무기태그를 가진 오브젝트를 찾는다
        currentWeapon.SetActive(false);// 무기태그를 가진 오브젝트를 비활성화 시킨다.
        GameObject currentShield = GameObject.FindWithTag("shield");// 무기태그를 가진 오브젝트를 찾는다
        currentShield.SetActive(false);// 무기태그를 가진 오브젝트를 비활성화 시킨다.
        player_right_hand.transform.GetChild(right_weapon_num).gameObject.SetActive(true);// 플레이어의 오른손에 있는 무기를 무기번호에따라 활성화
        player_left_hand.transform.GetChild(left_weapon_num).gameObject.SetActive(true);// 플레이어의 오른손에 있는 무기를 무기번호에따라 활성화
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

        // hp에 따른 화면 어둡게 하기
        if (hp >= max_hp / 20)
        {
            tempColor = hpFX.color;
            tempColor.a = Mathf.Lerp(tempColor.a, 1 - hp / max_hp, Time.deltaTime);//1 - hp / max_hp;
            hpFX.color = tempColor;
        }

        // 죽음 상태 플래그
        if (hp <= 0)
        {
            alive = false;
        }

        // 최대 hp를 넘었을 때
        if (hp > max_hp)
        {
            tempColor = hpFX.color;
            tempColor.a = 255;
            hpFX.color = tempColor;
        }
    }
    #endregion

    #region 대미지 받기
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
    #endregion
}
