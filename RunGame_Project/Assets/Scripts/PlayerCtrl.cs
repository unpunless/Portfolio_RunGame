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

    //--- ���ΰ��� ���� ������ ���� �� ������ ���� ���� ����
    Vector3 HalfSize = Vector3.zero;
    Vector3 m_CacCurPos = Vector3.zero;
    //--- ���ΰ��� ���� ������ ���� �� ������ ���� ���� ����

    //--- ���ΰ� ü�� ����
    float m_hp = 3.0f;
    [HideInInspector] public float m_CurHp = 3.0f;
    public Image[] m_hpImage;
    //--- ���ΰ� ü�� ����

    //--- ����
    float m_SdOnTime = 0.0f;
    float m_SdDuration = 5.0f;
    public GameObject ShieldObj = null;
    //--- ����

    //--- �ڼ�
    [HideInInspector] public bool IsMagnet = false;
    [HideInInspector] public Coin_Ctrl m_RefCoin = null;
    float m_Magnet_OnTime = 0.0f;
    float m_MagnetDur = 10.0f;
    //--- �ڼ�

    //-- �Ŵ�ȭ
    float m_Giant_OnTime = 0.0f;
    float m_GiangDur = 8.0f;
    public GameObject PlayerObj = null;
    public GameObject GiantObj = null;
    //-- �Ŵ�ȭ

    //--- ��Ȱ
    [HideInInspector] public bool IsRevive = false;
    public GameObject Player = null;
    //--- ��Ȱ

    //--- �ı�
    public GameObject m_BombPrefab = null;
    //--- �ı�

    //--- ����
    [HideInInspector] public bool IsFlying = false;
    float m_Fly_OnTime = 0.0f;
    float m_FlyDur = 8.0f;
    //--- ����

    //--- ���ǵ��
    [HideInInspector] public bool IsSpeedup = false;
    float m_Speedup_OnTime = 0.0f;
    float m_SpeedupDur = 10.0f;
    //--- ���ǵ��

    //--- ���� ȹ�� 2��
    [HideInInspector] public bool IsDouble = false;
    float m_DoubleCoin_OnTime = 0.0f;
    float m_DoubleCoin_Dur = 10.0f;
    //--- ���� ȹ�� 2��

    GameObject m_OverlapBlock = null;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f; //�����ӵ���...

        this.rb = GetComponent<Rigidbody2D>();

        //--- ĳ������ ���� �ݻ�����, ���� �ݻ����� ���ϱ�
        //���忡 �׷��� ��������Ʈ ������ ������
        SpriteRenderer sprRend = gameObject.GetComponentInChildren<SpriteRenderer>();
        HalfSize.x = sprRend.bounds.size.x / 2.0f - 0.23f;  //������ Ŀ�� ���� ����
        HalfSize.y = sprRend.bounds.size.y / 2.0f - 0.05f;
        HalfSize.z = 1.0f;
        //���忡 �׷��� ��������Ʈ ������ ������

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


    void HpImageUpdate()    // ü�� ���� ��ȭ
    {
        float a_CacHp = 0.0f;

        for (int ii = 0; ii < m_hpImage.Length; ii++)
        {
            a_CacHp = m_hp - (float)ii;
            if (a_CacHp < 0.0f) a_CacHp = 0.0f; // ���̳ʽ��� �Ȼ���� ����ó�� ����

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

        if (0.0f < m_SdOnTime)  //���� ��ų �ߵ� �� �� ��
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
        {//---- ���� ���� ������Ʈ
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
        }//---- ���� ���� ������Ʈ

        {//--- Magnet ���� ������Ʈ
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
        }//--- Magnet ���� ������Ʈ

        {//---- Giant ���� ������Ʈ
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
        }//---- Giant ���� ������Ʈ

        {//--- Fly ���� ������Ʈ
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
        }//--- Fly ���� ������Ʈ

        {//--- �ӵ� ���� ���� ������Ʈ
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
        }//--- �ӵ� ���� ���� ������Ʈ

        {//--- �������� ���� ������Ʈ
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
        }//--- �������� ���� ������Ʈ
    }
    public void UseSkill(SkillType a_SkType)
    {
        if (m_CurHp <= 0)
            return;

        if (a_SkType < 0 || SkillType.Skill_Count <= a_SkType)
            return;

        if (a_SkType == SkillType.Skill_0)   //ü��ȸ�� 1ĭ ȸ��
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

        else if (a_SkType == SkillType.Skill_1)  //5�ʰ� �ð� ����
        {
            if (0.0f < m_SdOnTime)
                return;

            m_SdOnTime = m_SdDuration;

            GameSc_Mgr.Inst.SkillCoolMethod(a_SkType, m_SdOnTime, m_SdDuration);

        }

        else if (a_SkType == SkillType.Skill_2)  //10�ʰ� �ڼ� 
        {
            MagnetUpdate();

            if (0.0f < m_Magnet_OnTime)
                return;

            m_Magnet_OnTime = m_MagnetDur;

            // �ڼ� ȿ���� ���۵Ǿ����� �˸�
            IsMagnet = true;
            m_RefCoin = FindObjectOfType<Coin_Ctrl>();

            GameSc_Mgr.Inst.SkillCoolMethod(a_SkType, m_Magnet_OnTime, m_MagnetDur);

        }

        else if (a_SkType == SkillType.Skill_3)  //�Ŵ�ȭ(8�ʰ� �Ŵ������� ����)
        {
            Giant_Ctrl a_Gaint = gameObject.GetComponent<Giant_Ctrl>();
            a_Gaint.GiantUpdate();

            if (0.0f < m_Giant_OnTime)
                return;

            m_Giant_OnTime = m_GiangDur;

            GameSc_Mgr.Inst.SkillCoolMethod(a_SkType, m_Giant_OnTime, m_GiangDur);
        }

        else if (a_SkType == SkillType.Skill_4)  //��Ȱ(ü��3ĭ���� ��Ȱ)
        {
            if (0.0f < m_CurHp)
                return;

            if (m_CurHp <= 0)
                StartCoroutine(ReviveAfterDelay(1f));
            /*
             * �ش� ��ų�� ������ ���ٶ�� ������ �־���Ѵ�.
             * �ش� ��ų�� �̹� ���ӿ��� �Ⱦ����ٴ� ������ �־���Ѵ�.
             * ü���� 0�� �Ǿ��ٴ� ������ �޾ƿ;��Ѵ�.
             * ����� �������� ���� ���θ� ��� ���ΰ�.
             * ���� �Ͻ������ǰ� 3���Ŀ� ü���� 0�� �Ǳ��� ��Ȳ���� �̾������� ����.
             * ������� �ʴ´ٰ� �� �ÿ��� Die()�� Load�ؿ´�.
             */
        }

        else if (a_SkType == SkillType.Skill_5)  //�ı�(10�ʰ� ���� ��ֹ� �ı�)
        {
            GameObject a_Clone = Instantiate(m_BombPrefab) as GameObject;
            a_Clone.transform.position =
                new Vector3(CameraResolution.m_ScreenWMin.x - 1.0f, 0.0f, 0.0f);
        }

        else if (a_SkType == SkillType.Skill_6)  //����(8�ʰ� ����,�����ƴ�)
        {
            FlyUpdate();

            if (0.0f < m_Fly_OnTime)
                return;

            m_Fly_OnTime = m_FlyDur;

            GameSc_Mgr.Inst.SkillCoolMethod(a_SkType, m_Fly_OnTime, m_FlyDur);
        }

        else if (a_SkType == SkillType.Skill_7)  //�ӵ� ���� (10�ʰ� 2���)
        {
            if (0.0f < m_Speedup_OnTime)
                return;

            m_Speedup_OnTime = m_SpeedupDur;

            ObstructionCtrl.Inst.UpdateSpeed(2.0f);

            GameSc_Mgr.Inst.SkillCoolMethod(a_SkType, m_Fly_OnTime, m_FlyDur);
        }

        else if (a_SkType == SkillType.Skill_8)  //���� ȹ��� ������ �� 2��
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
        Time.timeScale = 0.0f; //�Ͻ�����
        GameSc_Mgr.Inst.GameOverFunc();
    }
}
