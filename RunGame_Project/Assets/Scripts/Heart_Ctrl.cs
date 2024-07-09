using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Ctrl : MonoBehaviour
{
    [HideInInspector] public PlayerCtrl m_RefPlayer = null;
    [HideInInspector] public ObstructionCtrl m_RefObstruction = null;

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
        Move(3.0f);

        if (transform.position.x < CameraResolution.m_ScreenWMin.x - 0.5f)
        {
            Destroy(gameObject);
        }
    }

    public void Move(float a_MoveSpeed)
    {
        m_CurPos = this.transform.position;
        m_DirVec = Vector3.left;

        m_CurPos += m_DirVec * a_MoveSpeed * Time.deltaTime;
        transform.position = m_CurPos;
    }
}
