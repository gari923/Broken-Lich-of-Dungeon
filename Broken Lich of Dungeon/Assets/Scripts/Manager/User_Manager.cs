#region 네임스페이스
using UnityEngine;
#endregion

/// <summary>
/// 플레이어의 정보를 관리하는 스크립트
/// </summary>
public class User_Manager : MonoBehaviour
{
    #region 멤버 변수
    //attack = power*2 + weapon
    //hp = health *20

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
    public static string weapon_slot;// 장비 슬롯
    #endregion

    #region 시작 함수
    void Start()
    {
        gold = 0;//돈을 0으로 초기화
        LV = 1; //LV을 1로 초기화
<<<<<<< HEAD
        hp = max_hp;//hp를 최대치로 초기화
=======
>>>>>>> origin/yudahee
        //스텟 초기화
        power = 5;
        health = 5;
        attack = power * 2 + Item_Manager.weapon_Damage;
        max_hp = health * 20;

        hp = max_hp;//hp를 최대치로 초기화
    }
    #endregion

    #region 업데이트 함수
    void Update()
    {
        
    }
    #endregion
}
