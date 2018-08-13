#region 네임스페이스
using UnityEngine;
#endregion

/// <summary>
/// 아이템의 정보를 관리하는 스크립트
/// </summary>
public class Item_Manager : MonoBehaviour
{
    #region 멤버 변수
    //아이템 이름
    public static string item_name;
    //아이템 타입
    public static string item_type;
    //사용가능 여부
    public static bool can_use;
    //드랍 여부
    public static bool can_drop;
    //상점에서 존재하는 페이지
    public static float shop_page;
    //아이템 가격
    public static float buy_gold;
    //무기 능력치
    public static float weapon_Damage;// 무기 대미지
    //소모품 능력치
    public static float heal_amount;
    //스텟 능력치
    public static float status_amount;
    //정보 보냈는지 확인
    public static bool infomation_check = false;
    //유저 무기슬롯, 무기 데미지, 현재hp량, 스텟, 돈



    #endregion

    #region 시작 함수
    void Start()
    {
        print(item_name);
    }
    #endregion

    #region 업데이트 함수
    void Update()
    {
        
            //아이템 정보
            switch (item_name)
            {
                case "부숴진검": print("검받음"); item_type = "무기"; can_use = true; can_drop = false; shop_page = 1; buy_gold = 1000; weapon_Damage = 3; infomation_check = true; break;
                case "(소형)HP회복": item_type = "HP회복"; can_use = true; can_drop = false; shop_page = 1; buy_gold = 30; heal_amount = 10; break;
                case "(중형)HP회복": item_type = "HP회복"; can_use = true; can_drop = false; shop_page = 1; buy_gold = 70; heal_amount = 30; break;
                case "(대형)HP회복": item_type = "HP회복"; can_use = true; can_drop = false; shop_page = 1; buy_gold = 120; heal_amount = 50; break;
                case "(소형)MP회복": item_type = "MP회복"; can_use = true; can_drop = false; shop_page = 2; buy_gold = 50; heal_amount = 10; break;
                case "힘 증가": item_type = "힘 스텟"; can_use = true; can_drop = false; shop_page = 1; buy_gold = 500 + User_Manager.LV * 300; status_amount = 1; break;
                case "체력 증가": item_type = "체력 스텟"; can_use = true; can_drop = false; shop_page = 1; buy_gold = 500 + User_Manager.LV * 300; status_amount = 1; break;
            }
        
    }
    #endregion
}
