using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Giant_Ctrl : MonoBehaviour
{
    PlayerCtrl m_RefPlayer = null;

    float m_Subdino_Time = 0.0f;    //8ÃÊ

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_RefPlayer == null)
            return;
    
        GiantUpdate();
    }

    public void SubPlayerSpawn ( PlayerCtrl a_Player, float a_Subdino_Time)
    {
        m_RefPlayer = a_Player;
        m_Subdino_Time = a_Subdino_Time;
    }

    public void GiantUpdate()
    {
        if(m_RefPlayer != null)
        {
            float m_SdOnTime = 0.0f;
            float m_SdDuration = 5.0f;

            if (m_Subdino_Time > 0.0f)
            {
                m_Subdino_Time -= Time.deltaTime;

                if (0.0f < m_SdOnTime)
                    return;

                m_SdOnTime = m_SdDuration;
            }
        }
    }

}
