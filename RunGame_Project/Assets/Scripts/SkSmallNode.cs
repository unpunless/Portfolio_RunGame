using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkSmallNode : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType = SkillType.Skill_Count;
    [HideInInspector] public int m_CurSkCount = 0;

    GameObject Ref_ProductNode = null;

    public Text m_LevelText;
    public Image m_SkIconImg;
    public Text m_SkillText;
    public Button m_ExceptBtn;

    // Start is called before the first frame update
    void Start()
    {
        Ref_ProductNode = GameObject.Find("SkProductNode");
    }

    public void InitState(SkillType a_SkType)
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

    public void SkState(int a_Lv = 0)
    {
        m_LevelText.text = a_Lv.ToString() + "/3";
    }
}
