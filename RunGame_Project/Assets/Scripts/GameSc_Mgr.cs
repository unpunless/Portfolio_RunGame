using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSc_Mgr : MonoBehaviour
{
    public Button JumpBtn;

    [Header("------- Skill -------")]
    public Transform m_Player_ScContent;
    public GameObject m_PlayerPrefab;

    [Header("------- Skill Timer -------")]
    public GameObject m_SkCoolNode = null;
    public Transform m_SkillCoolRoot = null;

    [Header("------- Score -------")]
    public Text m_CurScoreText = null; //현재점수 표시 UI
    public Text m_BestScoreText = null; //최고점수 표시 UI
    public static int Record_Text;        //현재 기록
    public static int BestRecord_Text;    //최고 기록
    public Text m_CoinText = null;     //보유골드 표시 UI

    int m_CurScore = 0; //이번 스테이지에서 얻은 게임 점수
    int m_CurCoin = 0; //이번 스테이지에서 얻은 골드값

    [Header("------- PlayTime -------")]
    public Text m_PlayTime_Text;
    float m_PlayTime;

    Vector3 m_StartPos;

    [Header("------- Item -------")]
    GameObject m_HeartItem = null;
    GameObject m_CoinItem = null;
    GameObject m_Coin3Item = null;

    PlayerCtrl m_RefPlayer = null;  //플레이어 객체 변수

    [Header("------- GameOver -------")]
    public GameObject GameOverPanel;
    public Text Result_Text;
    public Button StoreBtn;
    public Button InfoBtn;
    public Button ReplayBtn;

    [HideInInspector] public SkillType m_SkType = SkillType.Skill_Count; //초기화

    public static GameSc_Mgr Inst = null;   //싱글턴

    void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GlobalValue.LoadGameData();
        RefreshSkillList();
        m_RefPlayer = GameObject.FindObjectOfType<PlayerCtrl>();

        m_CoinItem = Resources.Load("CoinPrefab") as GameObject;
        m_Coin3Item = Resources.Load("Coin3Prefab") as GameObject;
        m_HeartItem = Resources.Load("HeartPrefab") as GameObject;

        //Load();

        if (StoreBtn != null)
            StoreBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("StoreSc");
            });

        if (InfoBtn != null)
            InfoBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Player_InfoSc");
            });

        if (ReplayBtn != null)
            ReplayBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("GameSc");
            });

        //---UI 갱신
        m_BestScoreText.text = "최고점수(" + GlobalValue.g_BestScore + ")";
        m_CoinText.text = "보유골드(" + GlobalValue.g_UserCoin + ")";
        //---UI 갱신
    }

    float m_ScoreTimer = 0;

    // Update is called once per frame
    void Update()
    {
        PlayTime();

        if (Input.GetKeyDown(KeyCode.Alpha1) ||
           Input.GetKeyDown(KeyCode.Keypad1))
        {
            UseSkill_Key(GlobalValue.m_Bag[0].m_SkType);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) ||
                Input.GetKeyDown(KeyCode.Keypad2))
        {
            UseSkill_Key(GlobalValue.m_Bag[1].m_SkType);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) ||
                Input.GetKeyDown(KeyCode.Keypad3))
        {
            UseSkill_Key(GlobalValue.m_Bag[2].m_SkType);
        }

        //--- 치트키 (저장 정보 초기화)
        if (Input.GetKeyDown(KeyCode.K) == true)
        {
            m_PlayTime = 0;
            m_CurScore = 0;
            m_CurCoin = 0;
            PlayerPrefs.DeleteAll();
            GlobalValue.LoadGameData();
        }
        //--- 치트키 (저장 정보 초기화)

        if (m_RefPlayer != null && m_RefPlayer.m_CurHp <= 0.0f)
            GameOverFunc();

        m_ScoreTimer += Time.deltaTime;
        if ( 3.0f <= m_ScoreTimer )
        {
            AddScore(10);
            m_ScoreTimer = 0.0f;
        }
    }

    void UseSkill_Key(SkillType a_SkType)
    {
        if (GlobalValue.m_SkDataList[(int)a_SkType].m_CurSkillCount <= 0)
            return;

        if(m_RefPlayer == null)
            m_RefPlayer = GameObject.FindObjectOfType<PlayerCtrl>();

        if (m_RefPlayer != null)
            m_RefPlayer.UseSkill(a_SkType);

        if (m_Player_ScContent == null)
            return;

        SkPlayerNode[] a_PlayerList = m_Player_ScContent.GetComponentsInChildren<SkPlayerNode>();
    }

    public void SkillCoolMethod(SkillType a_SkType, float a_Time, float a_Duration)
    {
        GameObject Obj = Instantiate(m_SkCoolNode) as GameObject;
        Obj.transform.SetParent(m_SkillCoolRoot, false);
        SkillCool_Ctrl a_SCtrl = Obj.GetComponent<SkillCool_Ctrl>();
        if (a_SCtrl != null)
            a_SCtrl.InitState(a_SkType, a_Time, a_Duration);
    }

    public void SpawnCoin(Vector3 a_Pos, int a_Value = 10)
    {
        if (m_CoinItem == null)
            return;

        GameObject a_CoinObj = (GameObject)Instantiate(m_CoinItem);
        a_CoinObj.transform.position = a_Pos;
        Coin_Ctrl a_Coin_Ctrl = a_CoinObj.GetComponent<Coin_Ctrl>();
        if (a_Coin_Ctrl != null)
            a_Coin_Ctrl.m_RefPlayer = m_RefPlayer;

    }

    public void SpawnHeart(Vector3 a_Pos)
    {
        if (m_HeartItem == null)
            return;

        GameObject a_HeartObj = (GameObject)Instantiate(m_HeartItem);
        a_HeartObj.transform.position = a_Pos;
        Heart_Ctrl a_Heart_Ctrl = a_HeartObj.GetComponent<Heart_Ctrl>();
        if (a_Heart_Ctrl != null) { a_Heart_Ctrl.m_RefPlayer = m_RefPlayer; }
        Destroy(a_HeartObj, 1.0f);
    }

    public void GameOverFunc()
    {
        if (GameOverPanel != null && GameOverPanel.activeSelf == false)
            GameOverPanel.SetActive(true);

        if (Result_Text != null)
        {
            Result_Text.text = "플레이 타임\n" + GlobalValue.g_PlayTime + "\n\n" +
                              "획득 점수\n" + m_CurScore + "\n\n" +
                              "획득 골드\n" + m_CurCoin;
        }
    }

    void PlayTime()
    {
        m_PlayTime += Time.deltaTime;
        GlobalValue.g_PlayTime = (int)m_PlayTime;

        m_PlayTime_Text.text = "플레이 타임 : " + Mathf.RoundToInt(m_PlayTime);
        PlayerPrefs.SetFloat("PlayTime", Mathf.RoundToInt(GlobalValue.g_PlayTime));
        
        //Debug.Log("Time" + Mathf.RoundToInt(m_PlayTime));
    }

    public void AddCoin(int a_Value = 5)
    {
        m_CurCoin += a_Value;   //이번 스테이지에서 얻은 골드값
        GlobalValue.g_UserCoin += a_Value;  //로컬에 저장되어 있는 유저의 보유 골드값

        int a_MaxValue = int.MaxValue - 5;
        if (a_MaxValue < GlobalValue.g_UserCoin)
            GlobalValue.g_UserCoin = a_MaxValue;

        m_CoinText.text = "보유골드(" + GlobalValue.g_UserCoin + ")";
        PlayerPrefs.SetInt("UserCoin", GlobalValue.g_UserCoin);

        Debug.Log("코인 : " + a_Value);
        Debug.Log("글로벌 코인 : " + GlobalValue.g_UserCoin);
        Debug.Log("현재코인 : " + m_CurCoin);
    }

    public void AddScore(int a_Value = 10)
    {
        m_CurScore += a_Value;
        if (m_CurScore < 0)
            m_CurScore = 0;

        int a_MaxValue = int.MaxValue - 10;
        if (a_MaxValue < m_CurScore)
            m_CurScore = a_MaxValue;

        m_CurScoreText.text = "현재점수(" + m_CurScore + ")";
        if (GlobalValue.g_BestScore < m_CurScore)
        {
            GlobalValue.g_BestScore = m_CurScore;
            m_BestScoreText.text = "최고점수(" + GlobalValue.g_BestScore + ")";
            PlayerPrefs.SetInt("BestScore", GlobalValue.g_BestScore);
        }        
    }

    void RefreshSkillList() //보유 Skill Item 목록을 UI에 복원하는 함수
    {
        for (int ii = 0; ii < GlobalValue.m_Bag.Count; ii++)
        {
            GlobalValue.m_Bag[ii].m_CurSkillCount =
                            GlobalValue.m_Bag[ii].m_Level;

            if (GlobalValue.m_Bag[ii].m_Level <= 0)
                continue;

            GameObject a_SkillClone = Instantiate(m_PlayerPrefab);
            a_SkillClone.GetComponent<SkPlayerNode>().InitState(GlobalValue.m_Bag[ii]);
            a_SkillClone.transform.SetParent(m_Player_ScContent, false);

        }//for(int ii = 0; ii < GlobalValue.m_SkDataList.Count; ii++)

        Store_Mgr storeManager = GameObject.FindObjectOfType<Store_Mgr>();

        if (storeManager != null)
        {
            List<SkSmallNode> selectedSkillNodes = storeManager.selectedSkillNodes;

            for (int i = 0; i < selectedSkillNodes.Count; i++)
            {
                SkillType selectedType = selectedSkillNodes[i].m_SkType;
                SkPlayerNode skPlayerNode = Instantiate(m_PlayerPrefab).GetComponent<SkPlayerNode>();
                skPlayerNode.InitState(GlobalValue.m_SkDataList[(int)selectedType]);
                skPlayerNode.transform.SetParent(m_Player_ScContent, false);
            }
        }

    }//void RefreshSkillList()
}
