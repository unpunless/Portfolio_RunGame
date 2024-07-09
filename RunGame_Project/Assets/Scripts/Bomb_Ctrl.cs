using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Ctrl : MonoBehaviour
{
    float m_MoveSpeed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime * m_MoveSpeed;

        if (CameraResolution.m_ScreenWMax.x + 0.5f < transform.position.x)
        {
            Destroy(gameObject);
        }
    }

    [System.Obsolete]
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Obstruction")
        {
            ObstructionCtrl a_Enemy = coll.gameObject.GetComponent<ObstructionCtrl>();
            if(a_Enemy != null)
                DestroyObject(a_Enemy);
        }
    }
}
