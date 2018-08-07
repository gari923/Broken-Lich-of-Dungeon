#region 네임스페이스
using UnityEngine;
using UnityEngine.AI;
#endregion

#region 적 상태
public enum EState
{
    Idle,
    Move,
    Attack,
    Warning,
    Dead
}
#endregion

/// <summary>
/// 적의 상태, 움직임, 공격에 관한 스크립트 
/// </summary>
public class Enemy : MonoBehaviour
{
    #region 멤버 변수
    public float moveRange = 3f;// 타겟을 따라가는 범위
    public float viewAngle = 90f;// 시야각
    public float attackRange = 2f;// 공격 범위
    public float pathRange = 0.2f;// 지정한 웨이 포인트와의 거리
    public float idleDelay = 1f;// Idle 상태를 지속할 딜레이
    public float attackDelay = 1f;// 공격 딜레이
    public float stopDelay = 3f;
    public float destinationRange = 1f;
    public float warnignRange = 20;
    // 적의 정보
    public float enemy_damage = 100; // 몬스터 공격력
    public float enemy_hp; // 몬스터 hp
    public float enemy_max_hp = 100; // 몬스터 hp를 초기화
    public float enemy_drop_exp = 100; // 몬스터가 주는 경험치
    public float enemy_drop_gold = 100; // 몬스터가 주는 골드
    public float attack_range = 2; // 몬스터의 공격 범위

    public EState es;// 적 상태

    NavMeshAgent agent;// 네비 메시 에이전트
    Transform target;// 에이전트의 타겟
    Transform wayPoint;// 웨이 포인트 집합
    Transform[] path;// 웨이 포인트 패스

    int randPath;// 랜덤으로 길을 고르는 변수
    float curTime = 0;// 경과 시간
    float mag;
    float temp;
    float temps;
    float radi;
    float pathMag;
    float destinationMag;

    #endregion

    #region 시작 함수
    void Start()
    {
        agent = transform.GetComponent<NavMeshAgent>();// 네브 메시 에이전트 동적 할당
        target = GameObject.FindGameObjectWithTag("Player").transform;// 타겟을 플레이어로 동적 할당
        wayPoint = transform.parent.parent.Find("WayPoint");// 웨이 포인트 집합 동적 할당
        path = new Transform[wayPoint.childCount];// 웨이 포인트 패스들을 동적 할당
        es = EState.Idle;// 적 상태를 idle 상태로 초기화

        // 웨이 포인트 집합에서 웨이 포인트 패스 변수로 할당
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = wayPoint.GetChild(i);
        }

        randPath = Random.Range(0, path.Length);// 웨이 포인트를 랜덤으로 선택
    }
    #endregion

    #region 업데이트 함수
    void Update()
    {
        // 적 상태에 따른 함수 실행
        switch (es)
        {
            case EState.Idle:
                Idle();
                break;
            case EState.Move:
                Move();
                break;
            case EState.Attack:
                Attack();
                break;
            case EState.Warning:
                Warning();
                break;
            case EState.Dead:
                Dead();
                break;
        }
    }
    #endregion

    #region idle 상태 함수
    void Idle()
    {
        curTime += Time.deltaTime;// 경과 시간 할당

        // idle 딜레이 이후 move 상태로 전환
        if (curTime >= idleDelay)
        {
            curTime = 0;// 경과 시간 초기화
            es = EState.Move;
        }
    }
    #endregion

    #region move 상태 함수
    void Move()
    {
        mag = Vector3.Distance(target.position,
            transform.position);// 플레이어와 적 사이의 거리

        EnemyAngle();

        pathMag = Vector3.Distance(path[randPath].position,
            transform.position);// 선택한 웨이 포인트와 적과의 거리

        destinationMag = Vector3.Distance(agent.pathEndPosition, transform.position);


        // 시야 안쪽
        if (radi <= viewAngle)
        {
            // 거리 안쪽
            if (mag <= moveRange)
            {
                // 장애물 유
                if (float.IsInfinity(agent.remainingDistance))
                {
                    agent.destination = agent.pathEndPosition;
                }
                // 장애물 무
                else
                {
                    // 플레이어를 따라간다.
                    agent.destination = target.position;
                    print(agent.remainingDistance);
                    // 공격 가능 거리에 도달하면 공격으로 상태로 전환
                    if (attackRange >= mag)
                    {
                        curTime = attackDelay;
                        agent.isStopped = true;
                        es = EState.Attack;
                    }
                }
            }
            // 거리 바깥쪽
            else
            {
                // 장애물 유
                if (float.IsInfinity(agent.remainingDistance))
                {
                    agent.destination = agent.pathEndPosition;
                }
                // 장애물 무
                else
                {
                    NewWayPoint();
                }
            }
        }
        // 시야 바깥쪽
        else
        {
            //거리 안쪽
            if (mag <= moveRange)
            {
                // 장애물 안쪽
                if (float.IsInfinity(agent.remainingDistance))
                {
                    agent.destination = agent.pathEndPosition;
                }
                // 장애물 바깥쪽
                else
                {
                    NewWayPoint();
                }
            }
            //거리 바깥쪽
            else
            {
                // 장애물 안쪽
                if (float.IsInfinity(agent.remainingDistance))
                {
                    agent.destination = agent.pathEndPosition;
                }
                //장애물 바깥쪽
                else
                {
                    NewWayPoint();
                }
            }
        }

        //시야 안쪽
        // 거리 안쪽
        // 장애물 유
        // 무
        // 거리 바깥쪽
        // 장애물 유
        // 무

        // 시야 바깥쪽
        // 거리안쪽
        // 장애물 유
        // 무
        // 거리 바깥쪽
        // 장애물 유
        // 무



    }
    #endregion

    void EnemyAngle()
    {
        // 시야 범위 확인/////////////////////////////////////////////////////////////////////////////////
        temp = Mathf.Cos(Mathf.Deg2Rad * viewAngle);// 시야각의 코사인
        temps = Vector3.Dot(transform.forward,
            (target.position - transform.position).normalized);// 적의 시야와, 적과 플레이어와의 방향 내적
        radi = Mathf.Acos(temps) * Mathf.Rad2Deg;// 내적한 각도 계산
                                                 ////////////////////////////////////////////////////////////////////////////////////////////////
    }

    void NewWayPoint()
    {
        // 선택한 웨이 포인트에 접근했을 경우 새로운 웨이 포인트를 선택
        if (pathMag <= pathRange)
        {
            randPath = Random.Range(0, path.Length);

            if (agent.destination == path[randPath].position)
            {
                randPath = Random.Range(0, path.Length);
            }
            // idle
        }
        // 아니면 웨이 포인트에 계속 접근
        else
        {
            agent.destination = path[randPath].position;

        }
    }

    #region attack 상태 함수
    void Attack()
    {
        curTime += Time.deltaTime;// 경과 시간 할당

        mag = Vector3.Distance(target.position,
            transform.position);// 플레이어와 적 사이의 거리

        // 공격 딜레이가 지나면 플레이어를 공격
        if (curTime >= attackDelay)
        {
            print("Attack!");
            curTime = 0;// 경과 시간 초기화
                        // 플레이어에게 데미지를 준다.
            if (attackRange <= mag)
            {
                agent.isStopped = false;
                es = EState.Move;
            }
        }
    }
    #endregion

    #region damaged 상태 함수
    public void Damaged(float damage)
    {
        EnemyAngle();
        if (enemy_hp > 0)
        {
            if (radi >= viewAngle)
            {
                enemy_hp -= damage * 2;
                es = EState.Warning;

                for (int i = 0; i < transform.parent.childCount - 1; i++)
                {
                    float friendMag = (transform.parent.GetChild(i).transform.position - transform.position).magnitude;

                    if (friendMag <= warnignRange)
                    {
                        transform.parent.GetChild(i).GetComponent<Enemy>().es = EState.Warning;
                    }
                }
            }
            else
            {
                enemy_hp -= damage;

            }

            // 만약 시야범위 내에 플레이어가 없는데 공격을 당하면 데미지 두배(백어택)
            // 받은 즉시 플레이어의 위치로 destination을 바꾼다.

            // 피격된 적에서 부터의 범위 내에 다른 적이 있을 시 해당 적의 destination을 플레이어로 바꾼다.
            // move안에 warning 만들기
            // 도착한 뒤에는 move 상태로 바꾼다.(moveMag 안에 들어올 시 move 상태로 전환)
        }
        else
        {
            es = EState.Dead;
        }

    }
    #endregion

    void Warning()
    {
        mag = Vector3.Distance(target.position,
           transform.position);// 플레이어와 적 사이의 거리

        EnemyAngle();

        if (radi <= viewAngle)
        {
            if (mag <= moveRange)
            {
                agent.destination = agent.pathEndPosition;
                es = EState.Move;
                print("move");
            }
        }
        else
        {
            agent.destination = target.position;
        }
    }

    #region dead 상태 함수
    void Dead()
    {

    }
    #endregion
}