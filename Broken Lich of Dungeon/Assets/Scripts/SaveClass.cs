using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "SaveData", order = 1)]
public class SaveClass : ScriptableObject
{
    public float savedGold = 0;
    public float savedLV = 1;
    public float savedPower = 5;
    public float savedHealth = 5;
    public string savedRightWeapon;
    public string savedLeftWeapon;

    void Awake()
    {
        Debug.Log("ready2play");
    }    
}
