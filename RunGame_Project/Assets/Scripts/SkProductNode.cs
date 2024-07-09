using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SkProductNode : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType = SkillType.Skill_Count; //ÃÊ±âÈ­

    public Text m_LevelText;
    public Image m_SkIconImg;
    public Text m_SkillText;
    public Text m_BuyText;
    public Button BuyBtn;
    public Button SelectBtn;

    // Start is called before the first frame update
    void Start()
    {
        if (BuyBtn != null)
        {
            BuyBtn.onClick.AddListener(() =>
            {
                Store_Mgr a_StoreMgr = null;
                GameObject a_StoreObj = GameObject.Find("StoreMgr");
                if (a_StoreObj != null)
                    a_StoreMgr = a_StoreObj.GetComponent<Store_Mgr>();
                if (a_StoreMgr != null)
                    a_StoreMgr.BuySkillitem(m_SkType);
            });
        }

        if (SelectBtn != null)
        {
            SelectBtn.onClick.AddListener(() =>
            {
                Store_Mgr a_StoreMgr = null;
                GameObject a_StoreObj = GameObject.Find("StoreMgr");
                if (a_StoreObj != null)
                    a_StoreMgr = a_StoreObj.GetComponent<Store_Mgr>();
                if (a_StoreMgr != null)
                    a_StoreMgr.SelectSkill(m_SkType);
            });
        }
    }

    public void InitData(SkillType a_SkType)
    {
        if (a_SkType < SkillType.Skill_0 || SkillType.Skill_Count <= a_SkType)
            return;

        m_SkType = a_SkType;
        m_SkIconImg.sprite = GlobalValue.m_SkDataList[(int)a_SkType].m_IconImg;

        m_SkIconImg.GetComponent<RectTransform>().sizeDelta =
            new Vector2(GlobalValue.m_SkDataList[(int)a_SkType].m_IconSize.x * 135.0f,
                     GlobalValue.m_SkDataList[(int)a_SkType].m_IconSize.y * 135.0f);

        m_SkillText.text = GlobalValue.m_SkDataList[(int)a_SkType].m_SkillExp;
    }

    public void SetState(int a_Price, int a_Lv = 0)
    {
        m_LevelText.text = a_Lv.ToString() + "/3";

        if (a_Lv <= 0)
            m_BuyText.text = a_Price.ToString() + " °ñµå";
        else
        {
            int a_CacPrice = a_Price + (a_Price * (a_Lv - 1));
            m_BuyText.text = "Up " + a_CacPrice.ToString() + " °ñµå";
        }
    }
}
