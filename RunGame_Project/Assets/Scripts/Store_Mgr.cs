using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Store_Mgr : MonoBehaviour
{
    public Button LobbyBtn;
    public Button GameBtn;
    public Text m_UserCoinTxt = null;

    public GameObject m_Item_ScContent;
    public GameObject m_Item_NodeObj;

    public GameObject m_Small_ScContent;
    public GameObject m_Small_NodeObj;

    [Header("Message")]
    public Text MessageText;
    float ShowMsTimer = 0.0f;

    SkProductNode[] m_SkNodeList;
   [HideInInspector] public List<SkSmallNode> selectedSkillNodes = new List<SkSmallNode>();

    // Start is called before the first frame update
    void Start()
    {
        GlobalValue.LoadGameData();

        if (LobbyBtn != null)
            LobbyBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("LobbySc");
            });

        if (GameBtn != null)
            GameBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("GameSc");
            });

        if (m_UserCoinTxt != null)
            m_UserCoinTxt.text = "보유코인 : (" + GlobalValue.g_UserCoin + ")";

        //----- 아이템 목록 추가
        GameObject a_ItemObj = null;
        SkProductNode a_SkItemNode = null;
        for (int ii = 0; ii < GlobalValue.m_SkDataList.Count; ii++)
        {
            a_ItemObj = (GameObject)Instantiate(m_Item_NodeObj);
            a_SkItemNode = a_ItemObj.GetComponent<SkProductNode>();
            a_SkItemNode.InitData(GlobalValue.m_SkDataList[ii].m_SkType);
            a_ItemObj.transform.SetParent(m_Item_ScContent.transform, false);
        }
        //----- 아이템 목록 추가
    }

    // Update is called once per frame
    void Update()
    {
        if (0.0f < ShowMsTimer)
        {
            ShowMsTimer -= Time.deltaTime;
            if (ShowMsTimer <= 0.0f)
            {
                MessageOnOff("", false); //메시지 끄기
            }
        }

        RefreshSkItemList();

    }

    public void SelectSkill(SkillType selectedType)
    {
        GlobalValue.SelectedSkill(selectedType);

        if (selectedType < SkillType.Skill_0 || SkillType.Skill_Count <= selectedType)
            return;

        // 이미 선택된 스킬 노드 개수가 3개 이상이면 추가하지 않음
        if (selectedSkillNodes.Count >= 3)
            return;

        // 선택된 스킬을 SkSmallNode에 추가
        GameObject smallNodeObj = Instantiate(m_Small_NodeObj);
        SkSmallNode smallNode = smallNodeObj.GetComponent<SkSmallNode>();
        smallNode.InitState(selectedType);
        smallNodeObj.transform.SetParent(m_Small_ScContent.transform, false);

        // 선택된 스킬 노드를 리스트에 추가
        selectedSkillNodes.Add(smallNode);

    }

    public void BuySkillitem(SkillType a_SkType)
    { //리스트뷰에 있는 캐릭터 가격버튼을 눌러 구입시도를 한 경우

        if (a_SkType < SkillType.Skill_0 || SkillType.Skill_Count <= a_SkType)
            return;

        Skill_Info a_SkInfo = GlobalValue.m_SkDataList[(int)a_SkType];
        int a_Cost = 0;
        if (a_SkInfo.m_Level <= 0) //최초 구입인 경우
        {
            a_Cost = a_SkInfo.m_Price;
            if (GlobalValue.g_UserCoin < a_SkInfo.m_Price)
            {
                MessageOnOff("보유(누적) 코인이 부족합니다.");
                return;
            }
        }
        else //(업그레이드 가능) 상태
        {
            if (3 <= a_SkInfo.m_Level)
            {
                MessageOnOff("최고 레벨입니다.");
                return;
            }

            a_Cost = a_SkInfo.m_UpPrice +
                         (a_SkInfo.m_UpPrice * (a_SkInfo.m_Level - 1));
            if (GlobalValue.g_UserCoin < a_Cost)
            {
                MessageOnOff("레벨업에 필요한 보유(누적) 코인이 부족합니다.");
                return;
            }
        }//else //(업그레이드 가능) 상태

        GlobalValue.g_UserCoin -= a_Cost; //골드값 차감
        GlobalValue.m_SkDataList[(int)a_SkType].m_Level++;    //레벨 증가


        m_UserCoinTxt.text = "보유코인 : (" + GlobalValue.g_UserCoin + ")";

        //----로컬에 저장
        PlayerPrefs.SetInt("UserCoin", GlobalValue.g_UserCoin);
        string a_KeyBuff = string.Format("Skill_Item_{0}", (int)a_SkType);
        PlayerPrefs.SetInt(a_KeyBuff,
                            GlobalValue.m_SkDataList[(int)a_SkType].m_Level);
        //----로컬에 저장
    }


    public void RefreshSkItemList()
    {
        if (m_Item_ScContent != null)
        {
            if (m_SkNodeList == null || m_SkNodeList.Length <= 0)
                m_SkNodeList = m_Item_ScContent.GetComponentsInChildren<SkProductNode>();
        }

        SkSmallNode[] a_SkSmallList = m_Small_ScContent.GetComponentsInChildren<SkSmallNode>();

        for (int ii = 0; ii < GlobalValue.m_SkDataList.Count; ii++)
        {
            if (m_SkNodeList[ii].m_SkType != GlobalValue.m_SkDataList[ii].m_SkType)
                continue;

            if (0 < GlobalValue.m_SkDataList[ii].m_Level) //구입상태
            {
                m_SkNodeList[ii].SetState(GlobalValue.m_SkDataList[ii].m_UpPrice,
                                      GlobalValue.m_SkDataList[ii].m_Level);

                for (int xx = 0; xx < a_SkSmallList.Length; xx++)
                {
                    if (a_SkSmallList[xx].m_SkType == m_SkNodeList[ii].m_SkType)   //m_Item_ScContent는 9개고 a_SkSmallList는 3개니까 Type으로 찾아야 합니다.
                    {
                        a_SkSmallList[xx].SkState(GlobalValue.m_SkDataList[ii].m_Level); //GlobalValue.m_Bag[ii].m_Level);
                        break;
                    }
                }
            }
            else
            {
                m_SkNodeList[ii].SetState(GlobalValue.m_SkDataList[ii].m_Price,
                                      GlobalValue.m_SkDataList[ii].m_Level);
                for (int xx = 0; xx < a_SkSmallList.Length; xx++)
                {
                    if (a_SkSmallList[xx].m_SkType == m_SkNodeList[ii].m_SkType)  //m_Item_ScContent는 9개고 a_SkSmallList는 3개니까 Type으로 찾아야 합니다.
                    {
                        a_SkSmallList[xx].SkState(GlobalValue.m_SkDataList[ii].m_Level); //GlobalValue.m_Bag[ii].m_Level);
                        break;
                    }
                }
            }

        }//for(int ii = 0; ii < GlobalValue.m_SkDataList.Count; ii++)
    }


    void MessageOnOff(string Mess = "", bool isOn = true)
    {
        if (isOn == true)
        {
            MessageText.text = Mess;
            MessageText.gameObject.SetActive(true);
            ShowMsTimer = 3.0f;
        }
        else
        {
            MessageText.text = "";
            MessageText.gameObject.SetActive(false);
        }
    }
}
