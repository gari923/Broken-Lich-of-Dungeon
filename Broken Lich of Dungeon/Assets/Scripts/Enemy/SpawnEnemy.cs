#region 네임스페이스
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// 일정한 시간 단위로 적을 생성하는 스크립트
/// </summary>
public class SpawnEnemy : MonoBehaviour
{
    #region 멤버 변수
    public GameObject enemyPref;// 적 프리팹
    public float spawnTime = 3f;// 스폰 간격 시간
    public int poolSize = 3;// 풀 사이즈(적 갯수)

    List<GameObject> enemyActive = new List<GameObject>();// 비활성화된 적을 담을 리스트

    float curTime = 0;// 경과 시간
    #endregion

    #region 시작 함수
    void Start()
    {

        // 풀에 적 프리팹을 할당하고, 비활성화 리스트에 넣기
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPref, transform.position, transform.rotation, transform);
            enemy.SetActive(false);
            enemyActive.Add(enemy);
        }
    }
    #endregion

    #region 업데이트 함수
    void Update()
    {
        curTime += Time.deltaTime;// 경과 시간 할당

        // 스폰 간격마다 적을 스폰
        if (curTime >= spawnTime)
        {
            if (enemyActive.Count > 0)
            {
                enemyActive[0].SetActive(true);
                enemyActive.RemoveAt(0);
            }
            curTime = 0;
        }
    }
    #endregion
}
