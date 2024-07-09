using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMgr : MonoBehaviour
{
    public Button StartBtn;
    public Button Player_InfoBtn;
    public Button StoreBtn;

    // Start is called before the first frame update
    void Start()
    {
        if(StartBtn != null)
            StartBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("GameSc");
            });
        if (Player_InfoBtn != null)
            Player_InfoBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Player_InfoSc");
            });
        if (StoreBtn != null)
            StoreBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("StoreSc");
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
