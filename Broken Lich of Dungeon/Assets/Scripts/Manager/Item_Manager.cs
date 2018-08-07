#region 네임스페이스
using UnityEngine;
#endregion

#region 아이템 종류
enum Weapon { fist, old_sword, bar, stone }
#endregion

/// <summary>
/// 아이템의 정보를 관리하는 스크립트
/// </summary>
public class Item_Manager : MonoBehaviour
{
    #region 멤버 변수
    //무기 능력치
    public float weapon_Damage;// 무기 대미지
    public float weapon_buy_gold;// 무기 가격

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
            case "bar":
                weapon = Weapon.bar;
                break;
            case "stone":
                weapon = Weapon.stone;
                break;
        }

        //무기의 값
        switch (weapon)
        {
            case Weapon.fist:
                weapon_Damage = 1;
                weapon_buy_gold = 0;
                break;
            case Weapon.old_sword:
                weapon_Damage = 12;
                weapon_buy_gold = 1000;
                break;
            case Weapon.bar:
                weapon_Damage = 5;
                weapon_buy_gold = 500;
                break;
            case Weapon.stone:
                weapon_Damage = 2;
                weapon_buy_gold = 300;
                break;
        }
    }
    #endregion
}
