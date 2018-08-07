#region 네임스페이스
using UnityEngine;
#endregion

#region 몬스터 종류
enum enemy_name { monster1, monster2 }
#endregion

/// <summary>
/// 몬스터 정보를 관리하는 스크립트
/// </summary>
public class Enemy_Manager : MonoBehaviour
{
    #region 멤버 변수
    public static Enemy_Manager instance;// 싱글톤 변수

    enemy_name enemy;// 몬스터의 이름

    public GameManager monster1;
    public GameManager monster2;
    #endregion

    public class enemyInfo
    {
        //몬스터 정보
        float enemy_damage; // 몬스터 공격력
        float enemy_hp; // 몬스터 hp
        float enemy_max_hp; // 몬스터 hp를 초기화
        float enemy_drop_exp; // 몬스터가 주는 경험치
        float enemy_drop_gold; // 몬스터가 주는 골드
        float attack_range; // 몬스터의 공격 범위

        public enemyInfo(float damage, float maxHP, float exp, float gold, float range)
        {
            enemy_damage = damage;
            enemy_hp = maxHP;
            enemy_max_hp = maxHP;
            enemy_drop_exp = exp;
            enemy_drop_gold = gold;
            attack_range = range;
        }

        public float Enemy_damage
        {
            get
            {
                return enemy_damage;
            }

            set
            {
                enemy_damage = value;
            }
        }

        public float Enemy_hp
        {
            get
            {
                return enemy_hp;
            }

            set
            {
                enemy_hp = value;
            }
        }

        public float Enemy_max_hp
        {
            get
            {
                return enemy_max_hp;
            }

            set
            {
                enemy_max_hp = value;
            }
        }

        public float Enemy_drop_exp
        {
            get
            {
                return enemy_drop_exp;
            }

            set
            {
                enemy_drop_exp = value;
            }
        }

        public float Enemy_drop_gold
        {
            get
            {
                return enemy_drop_gold;
            }

            set
            {
                enemy_drop_gold = value;
            }
        }

        public float Attack_range
        {
            get
            {
                return attack_range;
            }

            set
            {
                attack_range = value;
            }
        }
    }

    #region 어웨이크 함수
    void Awake()
    {
        if (instance == null)// 싱글톤 할당
        {
            instance = this;
        }
    }
    #endregion

    public void monsterInfo()
    {
        enemyInfo monster1 = new enemyInfo(5, 20, 30, 100, 2);

        enemyInfo monster2 = new enemyInfo(8, 30, 50, 150, 2);

    }


    #region 업데이트 함수
    void Update()
    {
        // 몬스터 종류에 따른 정보 할당
        //switch (enemy)
        //{
        //    case enemy_name.monster1:// case Enemy.몬스터 이름
        //        enemyInfo monster1 = new enemyInfo(5, 20, 30, 100,2);
        //        break;
        //    case enemy_name.monster2:
        //        enemyInfo monster2 = new enemyInfo(8, 30, 50, 150, 2);
        //        break;
        //}
    }
    #endregion
}
