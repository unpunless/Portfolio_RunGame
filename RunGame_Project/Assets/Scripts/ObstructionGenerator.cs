using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class ObstructionGenerator : MonoBehaviour
{
    public GameObject[] ObstructionPrefab;

    float m_SpDelta = 5.0f;
    float m_DiffSpawn = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        m_DiffSpawn = m_SpDelta;
    }

    // Update is called once per frame
    void Update()
    {
        m_SpDelta -= Time.deltaTime;

        GameObject Go = null;
        if (m_SpDelta < 0.0f)
        {
            int dice = Random.Range(1, 21);

            if (dice < 5) 
            {
                Go = Instantiate(ObstructionPrefab[0]) as GameObject;
            }
            else if (6 <= dice && dice < 10) 
            {
                Go = Instantiate(ObstructionPrefab[1]) as GameObject;
            }
            else if (11 <= dice && dice < 16)
            {
                Go = Instantiate(ObstructionPrefab[2]) as GameObject;
            }
            else
            {
                Go = Instantiate(ObstructionPrefab[3]) as GameObject;
            }

            if (Go != null)
            {
                Go.transform.position = new Vector3(CameraResolution.m_ScreenWMax.x + 1.0f, 0.0f, 0.0f);
            }

            m_SpDelta = m_DiffSpawn;
        }
    }
}
