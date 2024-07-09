using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInfoMgr : MonoBehaviour
{
    public Button LobbyBtn;
    public Text m_UserCoinTxt = null;

    // Start is called before the first frame update
    void Start()
    {
        GlobalValue.LoadGameData();

        if (LobbyBtn != null)
            LobbyBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("LobbySc");
            });

        if (m_UserCoinTxt != null)
            m_UserCoinTxt.text = "보유코인 : (" + GlobalValue.g_UserCoin + ")";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
