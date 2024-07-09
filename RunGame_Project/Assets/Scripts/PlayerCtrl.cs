using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    public Button JumpBtn;

    float h = 0.0f;
    float v = 0.0f;
    float moveSpeed = 7.0f;
    Vector3 moveDir = Vector3.zero;

    public float jumpForce = 9;
    private bool isJumping = false;
    int jumpCnt;

    //--- 주인공이 지형 밖으로 나갈 수 없도록 막기 위한 변수
    Vector3 HalfSize = Vector3.zero;
    Vector3 m_CacCurPos = Vector3.zero;
    //--- 주인공이 지형 밖으로 나갈 수 없도록 막기 위한 변수

    //--- 주인공 체력 변수
    float m_hp = 3.0f;
    [HideInInspector] public float m_CurHp = 3.0f;
    public Image[] m_hpImage;
    //--- 주인공 체력 변수

    //--- 쉴드
    float m_SdOnTime = 0.0f;
    float m_SdDuration = 5.0f;
    public GameObject ShieldObj = null;
    //--- 쉴드

    //--- 자석
    [HideInInspector] public bool IsMagnet = false;
    [HideInInspector] public Coin_Ctrl m_RefCoin = null;
    float m_Magnet_OnTime = 0.0f;
    float m_MagnetDur = 10.0f;
    //--- 자석

    //-- 거대화
    float m_Giant_OnTime = 0.0f;
    float m_GiangDur = 8.0f;
    public GameObject PlayerObj = null;
    public GameObject GiantObj = null;
    //-- 거대화

    //--- 부활
    [HideInInspector] public bool IsRevive = false;
    public GameObject Player = null;
    //--- 부활

    //--- 파괴
    public GameObject m_BombPrefab = null;
    //--- 파괴

    //--- 비행
    [HideInInspector] public bool IsFlying = false;
    float m_Fly_OnTime = 0.0f;
    float m_FlyDur = 8.0f;
    //--- 비행

    //--- 스피드업
    [HideInInspector] public bool IsSpeedup = false;
    float m_Speedup_OnTime = 0.0f;
    float m_SpeedupDur = 10.0f;
    //--- 스피드업

    //--- 코인 획득 2배
    [HideInInspector] public bool IsDouble = false;
    float m_DoubleCoin_OnTime = 0.0f;
    float m_DoubleCoin_Dur = 10.0f;
    //--- 코인 획득 2배

    GameObject m_OverlapBlock = null;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f; //원래속도로...

        this.rb = GetComponent<Rigidbody2D>();

        //--- 캐릭터의 가로 반사이즈, 새로 반사이즈 구하기
        //월드에 그려진 스프라이트 사이즈 얻어오기
        SpriteRenderer sprRend = gameObject.GetComponentInChildren<SpriteRenderer>();
        HalfSize.x = sprRend.bounds.size.x / 2.0f - 0.23f;  //여백이 커서 조금 줄임
        HalfSize.y = sprRend.bounds.size.y / 2.0f - 0.05f;
        HalfSize.z = 1.0f;
        //월드에 그려진 스프라이트 사이즈 얻어오기

        this.animator = GetComponent<Animator>();

        Debug.Log("jumpForce" + jumpForce);

        if (JumpBtn != null && JumpBtn.onClick != null)
        {
            JumpBtn.onClick.AddListener(Jump);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        h = Input.GetAxis("Horizontal"); // -1.0f ~ 1.0f
        v = Input.GetAxis("Vertical");

        if (h != 0.0f || v != 0.0f)
        {
            moveDir = new Vector3(h, v, 0);
            if (1.0f < moveDir.magnitude)
                moveDir.Normalize();

            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        Update_Skill();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Tile")
        {
            isJumping = false;
            jumpCnt = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Obstruction")
        {
            if (m_OverlapBlock != coll.gameObject)
            {
                if (m_SdDuration > 0.0f)
                {
                    TakeDamage(0.0f);
                    HpImageUpdate();
                }

                m_hp -= 1.0f;
                HpImageUpdate();
                if (m_hp < 1.0f) { Die(); }

                m_OverlapBlock = coll.gameObject;
            }


            Destroy(coll.gameObject);
        }

        else if (coll.tag == "Item")
        {
            if (m_OverlapBlock != coll.gameObject)
            {
                m_hp += 1.0f;
                if (3.0f <= m_hp)
                    m_hp = 3.0f;
                HpImageUpdate();

                m_OverlapBlock = coll.gameObject;
            }
            Destroy(coll.gameObject);
        }

        else if (coll.tag == "Coin")
        {
            GameSc_Mgr.Inst.AddCoin();
            GameSc_Mgr.Inst.AddScore(10);
            Destroy(coll.gameObject);
        }
    }


    void HpImageUpdate()    // 체력 상태 변화
    {
        float a_CacHp = 0.0f;

        for (int ii = 0; ii < m_hpImage.Length; ii++)
        {
            a_CacHp = m_hp - (float)ii;
            if (a_CacHp < 0.0f) a_CacHp = 0.0f; // 마이너스가 안생기게 예외처리 구문

            if (1.0f < a_CacHp) a_CacHp = 1.0f;

            if (0.45f < a_CacHp && a_CacHp < 0.55f) a_CacHp = 0.445f;

            m_hpImage[ii].fillAmount = a_CacHp;
        }
    }

    void Jump()
    {
        if (jumpCnt < 1)
        {
            isJumping = true;
            jumpCnt++;
            Debug.Log("jumpCtn" + jumpCnt);
            this.animator.SetTrigger("dino_jump");
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            rb.AddForce(Vector2.up * this.jumpForce, ForceMode2D.Impulse);
        }
    }


    void MagnetUpdate()
    {
        float magnetForce = 2.0f;

        if (IsMagnet && Time.time - m_Magnet_OnTime < m_MagnetDur)
        {
            GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");

            foreach (GameObject coin in coins)
            {
                Vector3 directionToPlayer = (transform.position - coin.transform.position).normalized;
                coin.transform.Translate(directionToPlayer * Time.deltaTime * magnetForce, Space.World);
            }
        }
        else
        {
            IsMagnet = false;
        }
    }

    void LimitMove()
    {
        m_CacCurPos = transform.position;

        if (m_CacCurPos.x < CameraResolution.m_ScreenWMin.x + HalfSize.x)
            m_CacCurPos.x = CameraResolution.m_ScreenWMin.x + HalfSize.x;

        if (CameraResolution.m_ScreenWMax.x - HalfSize.x < m_CacCurPos.x)
            m_CacCurPos.x = CameraResolution.m_ScreenWMax.x - HalfSize.x;

        if (m_CacCurPos.y < CameraResolution.m_ScreenWMin.y + HalfSize.y)
            m_CacCurPos.y = CameraResolution.m_ScreenWMin.y + HalfSize.y;

        if (CameraResolution.m_ScreenWMax.y - HalfSize.y < m_CacCurPos.y)
            m_CacCurPos.y = CameraResolution.m_ScreenWMax.y - HalfSize.y;

        transform.position = m_CacCurPos;

    }//void LimitMove()

    void FlyUpdate()
    {
        float h = 0.0f;
        float v = 0.0f;
        float moveSpeed = 4.0f;
        Vector3 moveDir = Vector3.zero;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (IsFlying == true)
        {
            LimitMove();
            if (h != 0.0f || v != 0.0f)
            {
                moveDir = new Vector3(h, v, 0);
                if (1.0f < moveDir.magnitude)
                    moveDir.Normalize();

                transform.position += moveDir * moveSpeed * Time.deltaTime;
            }
        }
    }

    public void TakeDamage(float a_Value)
    {
        if (m_CurHp <= 0.0f)
            return;

        if (0.0f < m_SdOnTime)  //쉴드 스킬 발동 중 일 때
            return;

        m_CurHp -= a_Value;

        if (m_CurHp < 0.0f)
            m_CurHp = 0.0f;

        if (m_CurHp <= 0.0f)
            Die();
    }

    IEnumerator ReviveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Revive();
    }

    void Revive()
    {
        m_CurHp = m_hp;
    }

    void Update_Skill()
    {
        {//---- 쉴드 상태 업데이트
            if (0.0f < m_SdOnTime)
            {
                m_SdOnTime -= Time.deltaTime;
                if (ShieldObj != null && ShieldObj.activeSelf == false)
                    ShieldObj.SetActive(true);
                TakeDamage(0.0f);
            }
            else
            {
                if (ShieldObj != null && ShieldObj.activeSelf == true)
                    ShieldObj.SetActive(false);
            }
        }//---- 쉴드 상태 업데이트

        {//--- Magnet 상태 업데이트
            if (0.0f < m_Magnet_OnTime)
            {
                m_Magnet_OnTime -= Time.deltaTime;

                if (m_Magnet_OnTime < 0.0f)
                    m_Magnet_OnTime = 0.0f;

                IsMagnet = true;
            }
            else
            {
                IsMagnet = false;
            }
        }//--- Magnet 상태 업데이트

        {//---- Giant 상태 업데이트
            if (0.0f < m_Giant_OnTime)
            {
                m_Giant_OnTime -= Time.deltaTime;
                if (GiantObj != null && GiantObj.activeSelf == true)
                {
                    GiantObj.SetActive(true);
                    PlayerObj.SetActive(false);
                }
            }
            else
            {
                if (GiantObj != null && GiantObj.activeSelf == true)
                {
                    GiantObj.SetActive(false);
                    PlayerObj.SetActive(true);
                }
            }
        }//---- Giant 상태 업데이트

        {//--- Fly 상태 업데이트
            if (0.0f < m_Fly_OnTime)
            {
                m_Fly_OnTime -= Time.deltaTime;

                if (m_Fly_OnTime < 0.0f)
                    m_Fly_OnTime = 0.0f;

                IsFlying = true;
            }
            else
            {
                IsFlying = false;
            }
        }//--- Fly 상태 업데이트

        {//--- 속도 증가 상태 업데이트
            if (0.0f < m_Speedup_OnTime)
            {
                m_Speedup_OnTime -= Time.deltaTime;

                if (m_Speedup_OnTime < 0.0f)
                    m_Speedup_OnTime = 0.0f;

                IsSpeedup = true;
            }
            else
            {
                IsSpeedup = false;
            }
        }//--- 속도 증가 상태 업데이트

        {//--- 더블코인 상태 업데이트
            if (0.0f < m_DoubleCoin_OnTime)
            {
                m_DoubleCoin_OnTime -= Time.deltaTime;

                if (m_DoubleCoin_OnTime < 0.0f)
                    m_DoubleCoin_OnTime = 0.0f;

                IsDouble = true;
            }
            else
            {
                IsDouble = false;
            }
        }//--- 더블코인 상태 업데이트
    }
    public void UseSkill(SkillType a_SkType)
    {
        if (m_CurHp <= 0)
            return;

        if (a_SkType < 0 || SkillType.Skill_Count <= a_SkType)
            return;

        if (a_SkType == SkillType.Skill_0)   //체력회복 1칸 회복
        {
            if (m_hpImage.Length >= 1.0f)
            {
                m_CurHp += 1.0f;

                if (3.0f <= m_hp)
                {
                    m_hp = 3.0f;
                    m_CurHp = m_hp;
                }

                HpImageUpdate();
            }
        }

        else if (a_SkType == SkillType.Skill_1)  //5초간 시간 무적
        {
            if (0.0f < m_SdOnTime)
                return;

            m_SdOnTime = m_SdDuration;

            GameSc_Mgr.Inst.SkillCoolMethod(a_SkType, m_SdOnTime, m_SdDuration);

        }

        else if (a_SkType == SkillType.Skill_2)  //10초간 자석 
        {
            MagnetUpdate();

            if (0.0f < m_Magnet_OnTime)
                return;

            m_Magnet_OnTime = m_MagnetDur;

            // 자석 효과가 시작되었음을 알림
            IsMagnet = true;
            m_RefCoin = FindObjectOfType<Coin_Ctrl>();

            GameSc_Mgr.Inst.SkillCoolMethod(a_SkType, m_Magnet_OnTime, m_MagnetDur);

        }

        else if (a_SkType == SkillType.Skill_3)  //거대화(8초간 거대해지면 무적)
        {
            Giant_Ctrl a_Gaint = gameObject.GetComponent<Giant_Ctrl>();
            a_Gaint.GiantUpdate();

            if (0.0f < m_Giant_OnTime)
                return;

            m_Giant_OnTime = m_GiangDur;

            GameSc_Mgr.Inst.SkillCoolMethod(a_SkType, m_Giant_OnTime, m_GiangDur);
        }

        else if (a_SkType == SkillType.Skill_4)  //부활(체력3칸으로 부활)
        {
            if (0.0f < m_CurHp)
                return;

            if (m_CurHp <= 0)
                StartCoroutine(ReviveAfterDelay(1f));
            /*
             * 해당 스킬을 가지고 들어갔다라는 판정이 있어야한다.
             * 해당 스킬이 이번 게임에서 안쓰였다는 판정이 있어야한다.
             * 체력이 0이 되었다는 판정을 받아와야한다.
             * 사용할 것인지에 대한 여부를 물어볼 것인가.
             * 게임 일시정지되고 3초후에 체력이 0이 되기전 상황에서 이어지도록 설정.
             * 사용하지 않는다고 할 시에는 Die()를 Load해온다.
             */
        }

        else if (a_SkType == SkillType.Skill_5)  //파괴(10초간 앞의 장애물 파괴)
        {
            GameObject a_Clone = Instantiate(m_BombPrefab) as GameObject;
            a_Clone.transform.position =
                new Vector3(CameraResolution.m_ScreenWMin.x - 1.0f, 0.0f, 0.0f);
        }

        else if (a_SkType == SkillType.Skill_6)  //비행(8초간 비행,무적아님)
        {
            FlyUpdate();

            if (0.0f < m_Fly_OnTime)
                return;

            m_Fly_OnTime = m_FlyDur;

            GameSc_Mgr.Inst.SkillCoolMethod(a_SkType, m_Fly_OnTime, m_FlyDur);
        }

        else if (a_SkType == SkillType.Skill_7)  //속도 증가 (10초간 2배속)
        {
            if (0.0f < m_Speedup_OnTime)
                return;

            m_Speedup_OnTime = m_SpeedupDur;

            ObstructionCtrl.Inst.UpdateSpeed(2.0f);

            GameSc_Mgr.Inst.SkillCoolMethod(a_SkType, m_Fly_OnTime, m_FlyDur);
        }

        else if (a_SkType == SkillType.Skill_8)  //코인 획득시 들어오는 양 2배
        {
            if (0.0f < m_DoubleCoin_OnTime)
                return;

            m_DoubleCoin_OnTime = m_DoubleCoin_Dur;
            GameSc_Mgr.Inst.AddCoin(10);

            GameSc_Mgr.Inst.SkillCoolMethod(a_SkType, m_Fly_OnTime, m_FlyDur);
        }

    }

    void Die()
    {
        Time.timeScale = 0.0f; //일시정지
        GameSc_Mgr.Inst.GameOverFunc();
    }
}
