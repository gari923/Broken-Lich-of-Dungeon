#region 네임스페이스
using UnityEngine;
using UnityEngine.UI;
#endregion

#region 플레이어 상태
public enum pState
{
    Idle,
    Attack
}
#endregion

/// <summary>
/// 플레이어의 상태 움직임에 관한 스크립트
/// </summary>
public class Player : MonoBehaviour
{
    #region 멤버 변수
    public static Player instance;// 싱글톤 변수

    public Transform rayObject;// 레이로 가리킨 오브젝트
    public Transform rayObjectclick;// 레이로 가리킨 오브젝트
    public GameObject gvrRectPointer;// idle상태에서 보여줄 클릭용 UI
    public GameObject coolTimeCanvas;// attack 상태에서 보여줄 쿨타임 게이지
    public Animator anim;// 플레이어 애니메이션

    public float moveSpeed = 10;// 이동 속도
    public float rotSpeed = 300;// 회전 속도
    public float rayRadius = 0.1f;// 레이 반경
    public float attackRange = 2f;// 공격 범위
    public float idleRange = 5f;// idle 상태에서의 선택을 위한 거리
    public float siteRange = 4f;// 유니티짱을 인식시키기 위한 시야 거리
    public float attackSpeed = 2f;// 플레이어 공격 속도
    public bool IsLocked = false;// 마우스 락 확인
    public bool buttonClicked = true;// 버튼 클릭 확인
    public bool status = false;// 플레이어의 스테이터스 확인

    CharacterController cc;// 캐릭터 컨트롤러 변수
    Ray ray; // 레이 발사
    RaycastHit hitInfo;// 레이 정보 담기
    Image coolTimeImg;// 쿨타임 이미지 담기

    public pState ps;// 플레이어 상태 enum

    float v;// 수직 움직임
    float h;// 수평 움직임
    float coolTime;
    bool attackCheck;

    #endregion

    #region 어웨이크 함수
    void Awake()
    {
        if (instance == null)// 싱글톤 구현
        {
            instance = this;
        }
    }
    #endregion

    #region 시작 함수
    void Start()
    {
        if (IsLocked)// 마우스 커서 락
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        cc = transform.GetComponent<CharacterController>();// 캐릭터 컨트롤 동적 할당
        anim = transform.GetComponentInChildren<Animator>();
        coolTimeImg = coolTimeCanvas.transform.GetComponentInChildren<Image>();

        ps = pState.Idle;
    }
    #endregion

    #region 업데이트 함수
    void Update()
    {

        transform.forward = new Vector3(Camera.main.transform.forward.x,
            0, Camera.main.transform.forward.z);// 메인 카메라의 방향으로 플레이어 캐릭터의 방향을 전환

        v = Input.GetAxis("Vertical");// 수직 움직임 할당
        h = Input.GetAxis("Horizontal");// 수평 움직임 할당


        Vector3 lv = transform.forward * v;// 수직 움직임을 앞뒤로 할당
        Vector3 lh = transform.right * h;// 수평 움직임을 좌우로 할당

        Vector3 dir = (lv + lh).normalized;// 앞뒤좌우를 방향으로 할당하고 정규화

        cc.SimpleMove(dir * moveSpeed);// 바라보는 방향으로 이동

        ray = new Ray(Camera.main.transform.position,
            Camera.main.transform.forward);// 카메라 방향으로 레이를 쏘기
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        //RaycastHit hitInfo;// 레이 정보

        if (Physics.SphereCast(ray, rayRadius, out hitInfo, siteRange))
        {
            rayObject = hitInfo.transform;
        }
        else
        {
            rayObject = null;
        }


        switch (ps)
        {
            case pState.Idle:
                Idle();
                break;
            case pState.Attack:
                Attack();
                break;
        }

    }

    void Idle()
    {

        gvrRectPointer.SetActive(true);
        coolTimeCanvas.SetActive(false);

        if (v != 0 || h != 0)
        {
            anim.SetTrigger("IdleMove");
        }
        else
        {
            anim.SetTrigger("Idle");
        }

        // Fire1키로 공격/상호작용
        if (Input.GetButtonDown("Fire1"))
        {
            buttonClicked = true;// 버튼을 다운을 했을 때 true
            if (Physics.SphereCast(ray, rayRadius, out hitInfo, idleRange))
            {
                rayObjectclick = hitInfo.transform;
            }

        }
        else
        {
            buttonClicked = false;
        }
        // Jump키를 땠을 때 false
        if (Input.GetButtonUp("Fire1"))
        {
            buttonClicked = false;
        }

        // Jump키를 눌렀을 때 레이 정보를 얻기
        if (Input.GetButtonDown("Jump"))
        {
            status = true;
        }

        // Jump키를 땠을 때 false
        if (Input.GetButtonUp("Jump"))
        {
            status = false;
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");

        gvrRectPointer.SetActive(false);
        coolTimeCanvas.SetActive(true);

        if (attackCheck)
        {
            // Fire1키로 공격/상호작용
            if (Input.GetButtonDown("Fire1"))
            {
                attackCheck = false;
                coolTime = 0;

                anim.SetTrigger("AttackSlash");
                if (Physics.SphereCast(ray, rayRadius, out hitInfo, attackRange))
                {
                    //print(hitInfo.transform.name);
                    if (hitInfo.transform.tag.Equals("Enemy"))
                    {
                        hitInfo.transform.GetComponent<Enemy>().Damaged(User_Manager.attack);
                    }
                }
            }
        }
        else
        {
            coolTime += Time.deltaTime;

            if (coolTime >= attackSpeed)
            {
                attackCheck = true;
            }
            else
            {
                coolTimeImg.fillAmount = coolTime / attackSpeed;
            }
        }

        // Jump키를 눌렀을 때 레이 정보를 얻기
        if (Input.GetButtonDown("Jump"))
        {
            if (Physics.SphereCast(ray, rayRadius, out hitInfo, 1000))
            {
                print(hitInfo.transform.name);
            }
        }

    }
    #endregion
}
