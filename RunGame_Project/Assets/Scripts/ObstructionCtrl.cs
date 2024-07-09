using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstrucType
{
    OT_Cactus = 0,
    OT_CactusTwin,
    OT_Cloud,
    OT_UpsetCactus
}

public class ObstructionCtrl : MonoBehaviour
{
    public ObstrucType obstrucType = ObstrucType.OT_Cactus;
    float base_speed;

    Vector3 m_CurPos;           //��ġ ���� ����
    Vector3 m_SpawnPos;         //���� ��ġ
    Vector3 m_DirVec;           //�̵� ���� ���� ����

    PlayerCtrl m_RefPlayer = null;
    Bomb_Ctrl m_RefBomb = null;

    public static ObstructionCtrl Inst = null;

    void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_RefPlayer = GameObject.FindObjectOfType<PlayerCtrl>();
        m_RefBomb = GameObject.FindObjectOfType<Bomb_Ctrl>();

        m_SpawnPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Cactus();
        CactusTwin();
        Cloud();
        UpsetCactus();
        CheckOutOfBounds();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bomb")
        {
            Destroy(this.gameObject);
        }
    }

    void Cactus()
    {
        if (obstrucType == ObstrucType.OT_Cactus)
        {
            MoveLeft(3.0f);
            GameSc_Mgr.Inst.SpawnCoin(transform.position);
        }
    }

    void CactusTwin()
    {
        if (obstrucType == ObstrucType.OT_CactusTwin)
        {
            MoveLeft(5.0f);
            GameSc_Mgr.Inst.SpawnCoin(transform.position);
        }
    }

    void Cloud()
    {
        if (obstrucType == ObstrucType.OT_Cloud)
        {
            MoveLeft(3.5f);
            GameSc_Mgr.Inst.SpawnCoin(transform.position);
        }
    }

    void UpsetCactus()
    {
        if (obstrucType == ObstrucType.OT_UpsetCactus)
        {
            MoveLeft(3.0f);
            GameSc_Mgr.Inst.SpawnHeart(transform.position);
        }
    }
    void MoveLeft(float speed)
    {
        base_speed = speed;

        m_CurPos = transform.position;
        m_DirVec = Vector3.left; // �������� �̵��ϴ� ���� ����

        m_CurPos += m_DirVec * speed * Time.deltaTime; // �Էµ� �ӵ��� �̵�
        transform.position = m_CurPos; // ��ġ ������Ʈ
    }

    public void UpdateSpeed(float speedMultiplier)
    {
        float multiple_speed = base_speed * speedMultiplier;
    }

    public void CheckOutOfBounds()
    {
        if (transform.position.x < CameraResolution.m_ScreenWMin.x - 2.0f)
            Destroy(gameObject); // ȭ���� ����� ����
    }
}