using UnityEngine;

/// <summary>
/// 게임 데이터 세이브 클래스
/// </summary>
[CreateAssetMenu(fileName = "data", menuName = "SaveData", order = 1)]
public class SaveClass : ScriptableObject
{
    public float savedGold = 0;
    public float savedLV = 1;
    public float savedPower = 5;
    public float savedHealth = 5;
    public float savedMaxHP = 5 * 20;
    public float savedHP = 5 * 20;
    public float savedAttack = 5 * 2;
    public float savedWeaponDamage = 0;
    public float savedWeaponHealth = 0;
    public string savedRightWeapon = "스틸 대거";
    public string savedLeftWeapon = "버클러";
    public int savedRightNumber;
    public int savedLeftNumber;
}
