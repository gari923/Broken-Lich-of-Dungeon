#region 네임스페이스
using UnityEngine;
#endregion

#region 아이템 종류
public enum Weapon { fist, old_sword }
public enum Expendables { basic,small_hp, medium_hp, large_hp, small_mp }
#endregion

/// <summary>
/// 아이템의 정보를 관리하는 스크립트
/// </summary>
public class Item_Manager : MonoBehaviour
{

    #region 멤버 변수
    public static float buy_gold;//가격
    //무기 능력치
    public static float weapon_Damage;// 무기 대미지
    //소모품 능력치
    public static float heal;//회복량

    Weapon weapon;//무기 이름
    Expendables expendables;//소모품 이름
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
        if (Shop_Manager.instance.shop_state == false)
        {
            switch (User_Manager.weapon_slot)
            {
                case "주먹":
                    weapon = Weapon.fist;
                    User_Manager.weapon_Damage = Item_Manager.weapon_Damage;//유저의 무기데미지를 현재 아이템 데미지로 바꾼다
                    break;
                case "부숴진검":
                    weapon = Weapon.old_sword;
                    User_Manager.weapon_Damage = Item_Manager.weapon_Damage;//유저의 무기데미지를 현재 아이템 데미지로 바꾼다
                    break;
            }
        }
        //상점과 아이템 연동
        else
        {
            switch (Shop_Manager.buy_name)
            {
                case "부숴진검":
                    weapon = Weapon.old_sword;
                    break;
                case "(소형)HP회복":
                    expendables = Expendables.small_hp;
                    break;
                case "(중형)HP회복":
                    expendables = Expendables.medium_hp;
                    break;
                case "(대형)HP회복":
                    expendables = Expendables.large_hp;
                    break;
                case "(소형)MP회복":
                    expendables = Expendables.small_mp;
                    break;
                //case "":
                //    weapon = Weapon.fist;
                //    expendables = Expendables.basic;
                //    break;
            }
        }

        //무기의 값
        switch (weapon)
        {
            case Weapon.fist:
                weapon_Damage = 0;
                buy_gold = 0;
                break;
            case Weapon.old_sword:
                weapon_Damage = 3;
                buy_gold = 1000;
                break;
        }
        //소비품의 값
        switch (expendables)
        {
            case Expendables.basic:
                heal = 0;
                buy_gold = 0;
                break;
            case Expendables.small_hp:
                heal = 10;
                buy_gold = 30;
                break;
            case Expendables.medium_hp:
                heal = 30;
                buy_gold = 70;
                break;
            case Expendables.large_hp:
                heal = 50;
                buy_gold = 120;
                break;
            case Expendables.small_mp:
                heal = 10;
                buy_gold = 50;
                break;
        }
    }
    #endregion
}
