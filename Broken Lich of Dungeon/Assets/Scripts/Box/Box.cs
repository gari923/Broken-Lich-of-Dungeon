using UnityEngine;

enum BoxSpecies
{
    gold, item, damage, opened
}

public class Box : MonoBehaviour
{
    public bool isOpened = false;// 상자가 열렸는 지 확인하는 함수
    public int giveGoldMin = 1;
    public int giveGoldMax = 100;

    BoxSpecies boxSpecies;// 상자의 종류

    void Start()
    {
        boxSpecies = (BoxSpecies)Random.Range(0, 3);// 상자의 종류를 랜덤하게 선택
    }

    void Update()
    {
        // 상자가 열려있는 함수
        if (isOpened)
        {
            boxSpecies = BoxSpecies.opened;
        }

        // 상자의 종류에 따른 함수 실행
        switch (boxSpecies)
        {
            case BoxSpecies.gold:
                GoldBox();
                break;
            case BoxSpecies.item:
                ItemBox();
                break;
            case BoxSpecies.damage:
                DamageBox();
                break;
            case BoxSpecies.opened:
                Opened();
                break;
            default:
                break;
        }
    }

    // 골드 박스
    void GoldBox()
    {
        if (Player.instance.buttonClicked &&
            Player.instance.rayObject == this)
        {
            isOpened = true;
            User_Manager.gold += GiveGold();
        }
    }

    // 아이템 박스
    void ItemBox()
    {
        if (Player.instance.buttonClicked &&
            Player.instance.rayObject == this)
        {
            isOpened = true;
            User_Manager.right_weapon_slot = "fist";
        }
    }

    // 피격 박스
    void DamageBox()
    {
        if (Player.instance.buttonClicked &&
            Player.instance.rayObject == this)
        {
            isOpened = true;
            User_Manager.hp -= Random.Range(1, 51);
        }
    }

    // 열린 상자
    void Opened()
    {
        // 상자가 열려있는 모습으로 바꾸기
        // 이미 열려있는 상자라고 표시해주기
        return;
    }

    // 랜덤하게 골드 주기
    int GiveGold()
    {
        return Random.Range(giveGoldMin, giveGoldMax + 1) * 100;
    }       

    // 상자에서 랜덤한 것을 선택
    void SelectRandomItem()
    {

    }
}
