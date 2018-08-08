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
   Transform player;
    public Transform dustSmoke;
    public Transform explosion;
    public Transform shockWave;
    public ParticleSystem tornado;

    public float apearRotate = 2F;// 대기상태 딜레이
    public float maxHP = 100F;// 최대 HP
    public float hP = 0F;// HP
    public float idle2CombatTime = 2F;// idle 상태에서 combat 상태로 바뀌는 시간
    public float lookPlayerSpeed = 2F;// 플레이어를 바라보는 속도
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
        isInvincible = true;
        hP = maxHP;
        StartCoroutine("Appear");
        animator.SetTrigger("idle");
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    #endregion

    #region 등장
    IEnumerator Appear()
    {
        print("Appear");//////
        tornado.gameObject.SetActive(true);
        tornado.Play();
        while (curTime < apearRotate)
        {
            transform.Rotate(new Vector3(0, 360 * Time.deltaTime / apearRotate, 0));
            yield return new WaitForEndOfFrame();
        }
        tornado.Stop();
        yield return new WaitForSeconds(1.5F);
        curTime = 0;
        while (curTime < 1F)
        {
            transform.Translate(-transform.forward * 0.5F);
            yield return new WaitForEndOfFrame();
        }
        tornado.gameObject.SetActive(false);
        while (transform.position.y >= 0)
        {
            transform.Translate(-transform.up * 0.3F);
            yield return new WaitForEndOfFrame();
        }
        curTime = 0;
        bossState = BossState.Idle;
    }
    #endregion

    #region 업데이트 함수
    void Update()
    {
        curTime += Time.deltaTime;

        //// 무적 상태가 아니고, 플레이어가 대미지를 주면 피격상태
        if (!isInvincible)
        {
            curTime = 0;
            bossState = BossState.Damaged;
        }

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

        StartCoroutine("LookPlayer");// 플레이어를 바라본기

        // 일정 시간이 지나면 뭔가 한다
        if (curTime >= 3F)
        {
            StopCoroutine("LookPlayer");// 바라보기 중단

            // 랜덤으로 3가지 패턴 실행
            //switch (UnityEngine.Random.Range(0, 3))
            switch (0)
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
        yield return new WaitForSeconds(0.5F);
        StartCoroutine("LookPlayer");// 플레이어를 바라본다
        yield return new WaitForSeconds(1);

        curTime = 0;

        // 플레이어 추적
        while (curTime <= 8F)
        {
            TracePlayer();// 플레이어를 추적

            // 플레이어에게 다가가면 반복문 종료
            if (agent.isStopped)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        StopTrace();// 추적 중단
        StopCoroutine("LookPlayer");// 플레이어를 바라보기

        yield return new WaitForSeconds(0.3F);
        animator.SetTrigger("attack");// 공격모션
        yield return new WaitForSeconds(3F);

        curTime = 0;// 시간 초기화
        animator.ResetTrigger("attack");
        animator.ResetTrigger("combat");
        bossState = BossState.Idle;// 대기 상태로
    }

    // 원거리 마법 공격
    IEnumerator Pattern2()
    {
        print("Pattern2");//////
        yield return new WaitForSeconds(3);
        StartCoroutine("LookPlayer");// 플레이어를 바라본다
        yield return new WaitForSeconds(3);
        StopCoroutine("LookPlayer");

        for (int i = 0; i < 3; i++)
        {
            Vector3 targetpoint = player.position;
            dustSmoke.gameObject.SetActive(true);
            dustSmoke.position = targetpoint + new Vector3(0, 0.5F, 0);
            dustSmoke.GetComponentInChildren<ParticleSystem>().Play();
            yield return new WaitForSeconds(1);
            dustSmoke.GetComponentInChildren<ParticleSystem>().Stop();
            animator.SetTrigger("attack");
            print("마법 공격");// 마법 공격
            yield return new WaitForSeconds(2);
        }
        curTime = 0;
        animator.ResetTrigger("attack");
        animator.ResetTrigger("combat");
        bossState = BossState.Idle;
    }

    //
    IEnumerator Pattern3()
    {
        print("Pattern3");//////
        yield return new WaitForSeconds(3);
        StartCoroutine("LookPlayer");// 플레이어를 바라본다
        yield return new WaitForSeconds(3);

        transform.position += transform.forward * Time.deltaTime * 10F;

        curTime = 0;
        animator.ResetTrigger("attack");
        animator.ResetTrigger("combat");
        bossState = BossState.Idle;
    }
    #endregion

    #region 피격 상태
    void Damaged()
    {
        print("Damaged");//////
        animator.SetTrigger("damage");

        // HP가 0이 되면 죽음 상태
        if (hP <= 0)
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
        isRunning = true;
        transform.Translate(transform.forward * moveSpeed);
        animator.SetTrigger("move");
    }
    #endregion

    #region 추격 상태
    void Chase()
    {
        print("Chase");//////
        isRunning = true;
        StartCoroutine("LookPlayer");
        transform.Translate(transform.forward * chaseSpeed);
        animator.SetTrigger("move");
        animator.SetTrigger("fast");
    }
    #endregion

    #region 죽음 상태
    void Dead()
    {
        print("Dead");//////
        animator.SetTrigger("dead");

        if (curTime >= 4F)
        {
            animator.SetTrigger("destroy");
            //Destroy(gameObject);
        }
    }
    #endregion

    IEnumerator LookPlayer()
    {
        print("LookPlayer");//////
        while (true)
        {
            transform.forward = Vector3.Slerp(transform.forward,
    (player.position - transform.position).normalized,
    Time.deltaTime * lookPlayerSpeed);

            yield return new WaitForEndOfFrame();
        }
    }

    void TracePlayer()
    {
        agent.enabled = true;// 에이전트 켜기
        agent.SetDestination(player.position);// 플레이어를 따라가도록
    }

    void StopTrace()
    {
        agent.enabled = false;// 에이전트 끄기
    }
}