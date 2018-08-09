#region 네임스페이스
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
#endregion

#region 보스몹의 상태
enum BossState
{
    Idle, Combat, Damaged, Move, Chase, Dead, Wait
}
#endregion

/// <summary>
/// 보스의 상태와 공격패턴에 관한 스크립트
/// </summary>
public class SampleBoss : MonoBehaviour
{
    #region 멤버 변수
    Transform player;// 플레이어
    public Transform dustSmoke;// 폭파 예측 위치 이펙트
    public Transform explosion;// 폭파 이펙트
    public Transform shockWave;// 점프 공격 이펙트
    public ParticleSystem tornado;// 등장, 퇴장 토네이도 이펙트

    public float apearRotate = 2F;// 대기상태 딜레이
    public float maxHP = 100F;// 최대 HP
    public float hP = 0F;// HP
    public float idle2CombatTime = 2F;// idle 상태에서 combat 상태로 바뀌는 시간
    public float combatChooseTime = 1.5F;// 공격 패턴을 선택하는 시간
    public float attackRange = 2F;// 공격 범위
    public float lookPlayerSpeed = 2F;// 플레이어를 바라보는 속도
    public float jumpHight = 20F;// 점프 높이
    public float jumpSpeed = 5F;// 점프 스피드
    public float moveSpeed = 3F;// 움직이는 속도
    public float chaseSpeed = 6F;// 쫒아가는 속도

    public bool isInvincible = false;// 무적 판정


    Animator animator;// 애니메이터
    NavMeshAgent agent;// 내브 메시 에이전트
    BossState bossState;// 현재 보스 상태
    float curTime;// 경과 시간
    float damagedCool;// 대미지를 받고 기다리는 쿨타임
    bool isRunning;// 달리고 있는지
    #endregion

    #region 시작 함수
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        bossState = BossState.Wait;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isInvincible = true;// 등장할 때는 무적
        hP = maxHP;// 체력 채워넣기
        StartCoroutine("Appear");// 등장
        animator.SetTrigger("idle");
    }
    #endregion

    #region 등장
    IEnumerator Appear()
    {
        yield return new WaitForSeconds(0.1F);
        print("Appear");//////
        tornado.gameObject.SetActive(true);
        tornado.Play();// 토네이도
        // 한 바퀴 돌기
        while (curTime < apearRotate)
        {
            transform.Rotate(new Vector3(0, 360 * Time.deltaTime / apearRotate, 0));
            yield return new WaitForEndOfFrame();
        }

        transform.eulerAngles = new Vector3(0, 180F, 0);// 방향 리셋
        tornado.Stop();
        yield return new WaitForSeconds(1.5F);

        // 앞으로 다가가기/////////////////////
        curTime = 0;
        while (curTime < 1F)
        {
            transform.Translate(-transform.forward * 0.5F);
            yield return new WaitForEndOfFrame();
        }
        tornado.gameObject.SetActive(false);
        ////////////////////////////////////

        // 내려가기
        while (transform.position.y >= 0)
        {
            transform.Translate(-transform.up * 0.3F);
            yield return new WaitForEndOfFrame();
        }

        // 초기화 작업///////////////
        curTime = 0;
        bossState = BossState.Idle;
        ///////////////////////////
    }
    #endregion

    #region 업데이트 함수
    void Update()
    {
        curTime += Time.deltaTime;

        // 무적 상태가 아니고, 플레이어가 대미지를 주면 피격상태
        if (!isInvincible)
        {
            curTime = 0;
            bossState = BossState.Damaged;
        }

        // 상태에 따른 함수 실행
        switch (bossState)
        {
            case BossState.Idle:// 플레이어가 방에 등장하면 Appear상태로 전환
                Idle();
                break;
            case BossState.Combat:// 기본 상태: 딜레이 후 플레이어를 바라보기
                Combat();
                break;
            case BossState.Damaged:// 무적 상태를 제외하고 어떤 상태든 대미지 입힐 수 있음
                Damaged();
                break;
            case BossState.Move:// 움직이기
                Move();
                break;
            case BossState.Chase:// 쫒아가기
                Chase();
                break;
            case BossState.Dead:// HP가 0이 되면 죽음
                Dead();
                break;
            case BossState.Wait:// 아무것도 하지않는 상태
                break;
        }
    }
    #endregion

    #region 대기 상태
    void Idle()
    {
        print("Idle");//////
        StopAllCoroutines();
        animator.SetTrigger("idle");

        if (curTime >= idle2CombatTime)
        {
            animator.ResetTrigger("idle");
            animator.SetTrigger("combat");
            bossState = BossState.Combat;
            curTime = 0;
        }
    }
    #endregion

    #region 공격 상태
    void Combat()
    {
        print("combat");//////
        animator.SetTrigger("combat");

        StartCoroutine("LookPlayer");// 플레이어 바라보기

        // 일정 시간이 지나면 공격 패턴 실행
        if (curTime >= combatChooseTime)
        {
            StopCoroutine("LookPlayer");// 바라보기 중단

            // 랜덤으로 3가지 패턴 실행
            switch (UnityEngine.Random.Range(0, 3))
            {
                case 0:
                    StartCoroutine("Pattern1");
                    break;
                case 1:
                    StartCoroutine("Pattern2");
                    break;
                case 2:
                    StartCoroutine("Pattern3");
                    break;
                default:
                    break;
            }
            bossState = BossState.Wait;
            curTime = 0;
        }
    }

    // 플레이어를 따라가서 근접공격
    IEnumerator Pattern1()
    {
        print("Pattern1");//////
        StartCoroutine("LookPlayer");// 플레이어를 바라본다
        yield return new WaitForSeconds(1);



        // 8초간 플레이어 추적//////
        curTime = 0;
        while (curTime <= 8F)
        {
            TracePlayer();
            yield return new WaitForEndOfFrame();
        }
        /////////////////////////

        StopTrace();// 추적 중단
        StopCoroutine("LookPlayer");// 플레이어를 바라보기

        animator.SetTrigger("attack");// 공격모션
        yield return new WaitForSeconds(2F);

        // 초기화 작업////////////////////
        curTime = 0;
        animator.ResetTrigger("attack");
        animator.ResetTrigger("combat");
        bossState = BossState.Idle;
        ////////////////////////////////
    }

    // 원거리 마법 공격
    IEnumerator Pattern2()
    {
        print("Pattern2");//////
        StartCoroutine("LookPlayer");// 플레이어 바라보기
        yield return new WaitForSeconds(1.5F);
        StopCoroutine("LookPlayer");// 바라보기 중단

        // 5회 마법 공격
        for (int i = 0; i < 5; i++)
        {
            animator.SetTrigger("attack");
            Vector3 targetpoint = player.position;// 플레이어 위치를 타겟으로
            dustSmoke.gameObject.SetActive(true);
            dustSmoke.position = targetpoint + new Vector3(0, 0.5F, 0);
            dustSmoke.GetComponentInChildren<ParticleSystem>().Play();
            yield return new WaitForSeconds(1);
            dustSmoke.GetComponentInChildren<ParticleSystem>().Stop();
            StopCoroutine("LookPlayer");
            explosion.gameObject.SetActive(true);
            explosion.position = targetpoint + new Vector3(0, 0.5F, 0);

            foreach (var item in explosion.GetComponentsInChildren<ParticleSystem>())
            {
                item.Play();
            }

            print("마법 공격");// 마법 공격

            yield return new WaitForSeconds(1.5F);
            foreach (var item in explosion.GetComponentsInChildren<ParticleSystem>())
            {
                item.Stop();
            }
            StartCoroutine("LookPlayer");// 플레이어 바라보기
            animator.ResetTrigger("attack");
            animator.SetTrigger("combat");
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.5F);

        // 초기화 작업//////////////////////////
        dustSmoke.gameObject.SetActive(false);
        explosion.gameObject.SetActive(false);
        curTime = 0;
        animator.ResetTrigger("combat");
        bossState = BossState.Idle;
        //////////////////////////////////////
    }

    // 점프 공격
    IEnumerator Pattern3()
    {
        print("Pattern3");//////
        StartCoroutine("LookPlayer");// 플레이어 바라보기
        yield return new WaitForSeconds(2);

        StopCoroutine("LookPlayer");// 바라보기 중단

        // 점프///////////////////////////////////////////////////////////////////
        Vector3 jumpPosition = transform.position + new Vector3(0, jumpHight, 0);
        while (transform.position.y < jumpPosition.y - 0.1F)
        {
            transform.position = Vector3.Slerp(transform.position,
                jumpPosition, Time.deltaTime * jumpSpeed);
            yield return new WaitForEndOfFrame();
        }
        /////////////////////////////////////////////////////////////////////////

        // 플레이어 따라가기/////////////////////
        curTime = 0;
        while (curTime < 10F)
        {
            Vector3 target = getTargetPosition();
            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * Time.deltaTime * chaseSpeed;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.5F);
        //////////////////////////////////////

        // 낙하 공격/////////////////////////////////////
        Vector3 downVelocity = Vector3.zero;// 낙하 속도
        float downAcceleration = 100F;// 낙하 가속도
        while (transform.position.y > 0)
        {
            downVelocity += -transform.up * Time.deltaTime * downAcceleration;
            transform.position += downVelocity * Time.deltaTime;
            print("force down");
            yield return new WaitForEndOfFrame();
        }
        ///////////////////////////////////////////////

        // 쇼크 임펄스//////////////////////////////////////////////////////
        shockWave.gameObject.SetActive(true);
        shockWave.position = transform.position + new Vector3(0, 0.5F, 0);
        shockWave.GetComponentInChildren<ParticleSystem>().Play();
        yield return new WaitForSeconds(1);
        shockWave.GetComponentInChildren<ParticleSystem>().Stop();
        yield return new WaitForSeconds(2);
        //////////////////////////////////////////////////////////////////

        // 초기화 작업//////////////////////////
        curTime = 0;
        shockWave.gameObject.SetActive(false);
        animator.ResetTrigger("combat");
        bossState = BossState.Idle;
        //////////////////////////////////////
    }
    #endregion

    #region 피격 상태
    void Damaged()
    {
        print("Damaged");//////
        animator.SetTrigger("damage");
        isInvincible = true;// 무적상태로 만들기
        // HP가 0이 되면 죽음 상태
        if (curTime >= 2F && hP <= 0)
        {
            curTime = 0;
            bossState = BossState.Dead;
        }
    }
    #endregion

    #region 이동상태
    void Move()
    {
        print("move");//////
        isRunning = true;// 움직인다고 알리기
        transform.Translate(transform.forward * moveSpeed);// 움직이기
        animator.SetTrigger("move");
    }
    #endregion

    #region 추격 상태
    void Chase()
    {
        print("Chase");//////
        isRunning = true;// 움직인다고 알리기
        transform.Translate(transform.forward * chaseSpeed);// 움직이기
        animator.SetTrigger("move");
        animator.SetTrigger("fast");
    }
    #endregion

    #region 죽음 상태
    void Dead()
    {
        print("Dead");//////
        tornado.gameObject.SetActive(true);
        tornado.Play();// 토네이토
        animator.SetTrigger("dead");

        // 5초 후 삭제
        if (curTime >= 5F)
        {
            animator.SetTrigger("destroy");
            //Destroy(gameObject);
        }
    }
    #endregion

    #region 플레이어 바라보기
    IEnumerator LookPlayer()
    {
        print("LookPlayer");//////

        // 계속 플레이어쪽을 바라보기
        while (true)
        {
            transform.forward = Vector3.Slerp(transform.forward,
    (getTargetPosition() - transform.position).normalized,
    Time.deltaTime * lookPlayerSpeed);

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region 플레이어 추적
    void TracePlayer()
    {
        agent.enabled = true;// 에이전트 켜기
        agent.SetDestination(player.position);// 플레이어를 따라가도록
    }
    #endregion

    #region 추적 중단
    void StopTrace()
    {
        agent.enabled = false;// 에이전트 끄기
    }
    #endregion

    #region 높이 무시하고 플레이어 위치 타겟팅
    Vector3 getTargetPosition()
    {
        return new Vector3(player.position.x,
                transform.position.y, player.position.z);
    }
    #endregion
}