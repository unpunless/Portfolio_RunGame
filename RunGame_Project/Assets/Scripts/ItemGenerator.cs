using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public GameObject[] Items_Prefab;

    float m_SpDelta = 3.0f;
    float m_DiffSpawn = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_DiffSpawn = m_SpDelta;
    }

    // Update is called once per frame
    void Update()
    {
        m_SpDelta -= Time.deltaTime;

        if (m_SpDelta < 0.0f)
        {
            GameObject Go = null;

            int dice = Random.Range(1, 11);
            if (dice < 4)
            {
                Go = Instantiate(Items_Prefab[0]) as GameObject;
            }
            else if (4 <= dice && dice < 9)
            {
                Go = Instantiate(Items_Prefab[1]) as GameObject;
            }
            else
            {
                Go = Instantiate(Items_Prefab[2]) as GameObject;
            }

            if (Go != null)
            {
                Go.transform.position = new Vector3(CameraResolution.m_ScreenWMax.x + 1.0f, 0.0f, 0.0f);
            }

            m_SpDelta = m_DiffSpawn;
        }
    }
}
