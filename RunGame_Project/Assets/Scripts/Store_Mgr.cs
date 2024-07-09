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
            m_UserCoinTxt.text = "�������� : (" + GlobalValue.g_UserCoin + ")";

        //----- ������ ��� �߰�
        GameObject a_ItemObj = null;
        SkProductNode a_SkItemNode = null;
        for (int ii = 0; ii < GlobalValue.m_SkDataList.Count; ii++)
        {
            a_ItemObj = (GameObject)Instantiate(m_Item_NodeObj);
            a_SkItemNode = a_ItemObj.GetComponent<SkProductNode>();
            a_SkItemNode.InitData(GlobalValue.m_SkDataList[ii].m_SkType);
            a_ItemObj.transform.SetParent(m_Item_ScContent.transform, false);
        }
        //----- ������ ��� �߰�
    }

    // Update is called once per frame
    void Update()
    {
        if (0.0f < ShowMsTimer)
        {
            ShowMsTimer -= Time.deltaTime;
            if (ShowMsTimer <= 0.0f)
            {
                MessageOnOff("", false); //�޽��� ����
            }
        }

        RefreshSkItemList();

    }

    public void SelectSkill(SkillType selectedType)
    {
        GlobalValue.SelectedSkill(selectedType);

        if (selectedType < SkillType.Skill_0 || SkillType.Skill_Count <= selectedType)
            return;

        // �̹� ���õ� ��ų ��� ������ 3�� �̻��̸� �߰����� ����
        if (selectedSkillNodes.Count >= 3)
            return;

        // ���õ� ��ų�� SkSmallNode�� �߰�
        GameObject smallNodeObj = Instantiate(m_Small_NodeObj);
        SkSmallNode smallNode = smallNodeObj.GetComponent<SkSmallNode>();
        smallNode.InitState(selectedType);
        smallNodeObj.transform.SetParent(m_Small_ScContent.transform, false);

        // ���õ� ��ų ��带 ����Ʈ�� �߰�
        selectedSkillNodes.Add(smallNode);

    }

    public void BuySkillitem(SkillType a_SkType)
    { //����Ʈ�信 �ִ� ĳ���� ���ݹ�ư�� ���� ���Խõ��� �� ���

        if (a_SkType < SkillType.Skill_0 || SkillType.Skill_Count <= a_SkType)
            return;

        Skill_Info a_SkInfo = GlobalValue.m_SkDataList[(int)a_SkType];
        int a_Cost = 0;
        if (a_SkInfo.m_Level <= 0) //���� ������ ���
        {
            a_Cost = a_SkInfo.m_Price;
            if (GlobalValue.g_UserCoin < a_SkInfo.m_Price)
            {
                MessageOnOff("����(����) ������ �����մϴ�.");
                return;
            }
        }
        else //(���׷��̵� ����) ����
        {
            if (3 <= a_SkInfo.m_Level)
            {
                MessageOnOff("�ְ� �����Դϴ�.");
                return;
            }

            a_Cost = a_SkInfo.m_UpPrice +
                         (a_SkInfo.m_UpPrice * (a_SkInfo.m_Level - 1));
            if (GlobalValue.g_UserCoin < a_Cost)
            {
                MessageOnOff("�������� �ʿ��� ����(����) ������ �����մϴ�.");
                return;
            }
        }//else //(���׷��̵� ����) ����

        GlobalValue.g_UserCoin -= a_Cost; //��尪 ����
        GlobalValue.m_SkDataList[(int)a_SkType].m_Level++;    //���� ����


        m_UserCoinTxt.text = "�������� : (" + GlobalValue.g_UserCoin + ")";

        //----���ÿ� ����
        PlayerPrefs.SetInt("UserCoin", GlobalValue.g_UserCoin);
        string a_KeyBuff = string.Format("Skill_Item_{0}", (int)a_SkType);
        PlayerPrefs.SetInt(a_KeyBuff,
                            GlobalValue.m_SkDataList[(int)a_SkType].m_Level);
        //----���ÿ� ����
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

            if (0 < GlobalValue.m_SkDataList[ii].m_Level) //���Ի���
            {
                m_SkNodeList[ii].SetState(GlobalValue.m_SkDataList[ii].m_UpPrice,
                                      GlobalValue.m_SkDataList[ii].m_Level);

                for (int xx = 0; xx < a_SkSmallList.Length; xx++)
                {
                    if (a_SkSmallList[xx].m_SkType == m_SkNodeList[ii].m_SkType)   //m_Item_ScContent�� 9���� a_SkSmallList�� 3���ϱ� Type���� ã�ƾ� �մϴ�.
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
                    if (a_SkSmallList[xx].m_SkType == m_SkNodeList[ii].m_SkType)  //m_Item_ScContent�� 9���� a_SkSmallList�� 3���ϱ� Type���� ã�ƾ� �մϴ�.
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
