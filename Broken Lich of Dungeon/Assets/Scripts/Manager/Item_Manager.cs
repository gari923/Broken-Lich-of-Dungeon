#region 네임스페이스
using UnityEngine;
#endregion

#region 아이템 종류
public enum Weapon { fist, old_sword, bar, stone }
#endregion

/// <summary>
/// 아이템의 정보를 관리하는 스크립트
/// </summary>
public class Item_Manager : MonoBehaviour
{
    
    #region 멤버 변수
    //무기 능력치
    public static float weapon_Damage;// 무기 대미지
    public static float weapon_buy_gold;// 무기 가격

    Weapon weapon;//무기 이름
    #endregion

    #region 시작 함수
    void Start()
    {
        weapon = Weapon.fist;// 기본은 맨손
    }
    #endregion

    #region 업데이트 함수
    void Update()
    {
        //플레이어의 무기와 연동
        switch (User_Manager.weapon_slot)
        {
            case "fist":
                weapon = Weapon.fist;
                break;
            case "old_sword":
                weapon = Weapon.old_sword;
                break;
        }

        //무기의 값
        switch (weapon)
        {
            case Weapon.fist:
                weapon_Damage = 0;
                weapon_buy_gold = 0;
                break;
            case Weapon.old_sword:
                weapon_Damage = 3;
                weapon_buy_gold = 1000;
                break;
        }
    }
    #endregion
}
