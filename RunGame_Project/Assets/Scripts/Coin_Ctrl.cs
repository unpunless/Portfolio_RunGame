using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum CoinType
{
    Coin1 = 0,
    Coin2 = 1,
}
public class Coin_Ctrl : MonoBehaviour
{
    [HideInInspector] public PlayerCtrl m_RefPlayer = null;
    [HideInInspector] public ObstructionCtrl m_RefObstruction = null;

    public CoinType coinType = CoinType.Coin1;

    Vector3 m_CurPos;           //위치 계산용 변수
    Vector3 m_SpawnPos;         //스폰 위치
    Vector3 m_DirVec;           //이동 방향 계산용 변수

    // Start is called before the first frame update
    void Start()
    {
        m_RefPlayer = GameObject.FindObjectOfType<PlayerCtrl>();

        if (m_RefObstruction != null)
        {
            m_SpawnPos = m_RefObstruction.transform.right;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckOutOfBounds();
        Coin1();
        Coin2();

        if (transform.position.x < CameraResolution.m_ScreenWMin.x - 0.5f)
        {
            Destroy(gameObject);
        }

        if (m_RefPlayer != null && m_RefPlayer.IsMagnet)
        {
            Vector3 directionToPlayer = (m_RefPlayer.transform.position - transform.position).normalized;
            MoveToPlayer(directionToPlayer, 10.0f);
        }
    }

    void Coin1()
    {
        if (coinType == CoinType.Coin1)
        {
            Move(3.0f);
        }
    }
    void Coin2()
    {
        if (coinType == CoinType.Coin2)
        {
            Move(6.0f);
        }
    }

    public void Move(float a_MoveSpeed)
    {
        m_CurPos = transform.position;
        m_DirVec = Vector3.left;

        m_CurPos += m_DirVec * a_MoveSpeed * Time.deltaTime;
        transform.position = m_CurPos;
    }

    void MoveToPlayer(Vector3 direction, float moveSpeed)
    {
        m_CurPos = transform.position;
        m_CurPos += direction * moveSpeed * Time.deltaTime;
        transform.position = m_CurPos;
    }

    public void CheckOutOfBounds()
    {
        if (transform.position.x < CameraResolution.m_ScreenWMin.x - 2.0f)
            Destroy(gameObject); // 화면을 벗어나면 삭제
    }
}