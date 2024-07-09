using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Skill_0 = 0, //healing = 0,
    Skill_1,    //invincibility,
    Skill_2,    //magnet,
    Skill_3,    //gigantic,
    Skill_4,    //Revive,
    Skill_5,    //Destroyer,
    Skill_6,    //flying,
    Skill_7,    //peed_up,
    Skill_8,    //double_coin,
    Skill_Count
}

public class Skill_Info
{
    public string m_Name = "";                  //캐릭터 이름
    public SkillType m_SkType = SkillType.Skill_0;  //캐릭터 타입
    public Vector2 m_IconSize = Vector2.one;        //아이콘의 가로 사이즈, 세로 사이즈
    public int m_Price = 30;   //아이템 기본 가격 
    public int m_UpPrice = 30; //업그레이드 가격, 타입에 따라서
    public int m_Level = 0;    //그전엔 Lock, 레벨0 이면 아직 구매 안됨 (구매가 완료되면 레벨 1부터)
    public int m_CurSkillCount = 0;   //사용할 수 있는 스킬 카운트
    //public int m_MaxUsable = 1;     //사용할 수 있는 최대 스킬 카운트는 Level과 같다.
    public string m_SkillExp = "";    //스킬 효과 설명
    public Sprite m_IconImg = null;   //캐릭터 아이템에 사용될 이미지

    public void SetType(SkillType a_SkType)
    {
        m_SkType = a_SkType;

        //TODO 각 스킬별 가격과 효과 세팅

        if (a_SkType == SkillType.Skill_0)
        {
            m_Name = "회복";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 30;
            m_UpPrice = 30;

            m_SkillExp = "체력 1칸 회복";
            m_IconImg = Resources.Load("Image/SkillImg/Heal", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_1)   //무적
        {
            m_Name = "무적";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 100;
            m_UpPrice = 100;

            m_SkillExp = "5초간 무적";
            m_IconImg = Resources.Load("Image/SkillImg/Invincibility", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_2)   //자석
        {
            m_Name = "자석";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 20;
            m_UpPrice = 20;

            m_SkillExp = "10초간 코인을 끌어들인다";
            m_IconImg = Resources.Load("Image/SkillImg/Magnet", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_3)   //거대화
        {
            m_Name = "거대화";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 200;
            m_UpPrice = 200;

            m_SkillExp = "10초간 거대해진다.";
            m_IconImg = Resources.Load("Image/SkillImg/Gigantic", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_4)   //부활
        {
            m_Name = "부활";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 500;
            m_UpPrice = 500;

            m_SkillExp = "체력3칸으로 부활한다.";
            m_IconImg = Resources.Load("Image/SkillImg/Revive", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_5)   //파괴
        {
            m_Name = "파괴";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 100;
            m_UpPrice = 100;

            m_SkillExp = "10초간 앞의 장애물을 파괴한다.";
            m_IconImg = Resources.Load("Image/SkillImg/Destroyer", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_6)   //비행
        {
            m_Name = "비행";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 150;
            m_UpPrice = 150;

            m_SkillExp = "10초간 비행한다.";
            m_IconImg = Resources.Load("Image/SkillImg/Flying", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_7)   //속도 증가
        {
            m_Name = "속도 증가";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 120;
            m_UpPrice = 120;

            m_SkillExp = "10초간 빨라진다.";
            m_IconImg = Resources.Load("Image/SkillImg/Speed_Up", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_8)   //코인 2배
        {
            m_Name = "더블 코인";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 80;
            m_UpPrice = 80;

            m_SkillExp = "10초간 코인 획득시 2배로 들어온다.";
            m_IconImg = Resources.Load("Image/SkillImg/Double_Coin", typeof(Sprite)) as Sprite;
        }
    }
}


public class GlobalValue
{
    public static List<Skill_Info> m_SkDataList = new List<Skill_Info>();

    public static List<Skill_Info> m_Bag = new List<Skill_Info>();

    public static List<SkillType> m_SelectedSkills = new List<SkillType>();

    public static int g_PlayTime = 0;
    public static int g_BestScore = 0;
    public static int g_UserCoin = 0;

    public static void LoadGameData()
    {
        if (m_SkDataList.Count <= 0)
        {
            Skill_Info a_SkItemNd;
            for (int ii = 0; ii < (int)SkillType.Skill_Count; ii++)
            {
                a_SkItemNd = new Skill_Info();
                a_SkItemNd.SetType((SkillType)ii);
                m_SkDataList.Add(a_SkItemNd);
            }
        }

        g_PlayTime = PlayerPrefs.GetInt("PlayTime", 0);
        g_BestScore = PlayerPrefs.GetInt("BestScore", 0);
        g_UserCoin = PlayerPrefs.GetInt("UserCoin", 100000000);

        #region 아이템 로딩하기...
        string a_KeyBuff = "";
        for (int ii = 0; ii < (int)SkillType.Skill_Count; ii++)
        {
            if (m_SkDataList.Count <= ii)
                continue;

            a_KeyBuff = string.Format("Skill_Item_{0}", ii);
            m_SkDataList[ii].m_Level = PlayerPrefs.GetInt(a_KeyBuff, 0);

            //m_SkDataList[ii].m_Level = 3;   //<--테스트를 위해 만랩으로 시작
        }
        #endregion 아이템 로딩하기...

        string a_selBuff = "";
        for (int ii = 0; ii < 3; ii++)
        {
            if (m_Bag.Count <= ii)
                continue;

            a_selBuff = string.Format("Skill_Item_{0}", ii);
            m_Bag[ii].m_Level = PlayerPrefs.GetInt(a_selBuff, 0);
        }
    }

    public static void SelectedSkill(SkillType selectedType)
    {
        Skill_Info selectedSkill = m_SkDataList.Find(sk => sk.m_SkType == selectedType);
        if (selectedSkill != null && !m_Bag.Contains(selectedSkill))
        {
            m_Bag.Add(selectedSkill);
        }
    }

}
