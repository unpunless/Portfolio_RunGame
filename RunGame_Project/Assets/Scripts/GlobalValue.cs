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
    public string m_Name = "";                  //ĳ���� �̸�
    public SkillType m_SkType = SkillType.Skill_0;  //ĳ���� Ÿ��
    public Vector2 m_IconSize = Vector2.one;        //�������� ���� ������, ���� ������
    public int m_Price = 30;   //������ �⺻ ���� 
    public int m_UpPrice = 30; //���׷��̵� ����, Ÿ�Կ� ����
    public int m_Level = 0;    //������ Lock, ����0 �̸� ���� ���� �ȵ� (���Ű� �Ϸ�Ǹ� ���� 1����)
    public int m_CurSkillCount = 0;   //����� �� �ִ� ��ų ī��Ʈ
    //public int m_MaxUsable = 1;     //����� �� �ִ� �ִ� ��ų ī��Ʈ�� Level�� ����.
    public string m_SkillExp = "";    //��ų ȿ�� ����
    public Sprite m_IconImg = null;   //ĳ���� �����ۿ� ���� �̹���

    public void SetType(SkillType a_SkType)
    {
        m_SkType = a_SkType;

        //TODO �� ��ų�� ���ݰ� ȿ�� ����

        if (a_SkType == SkillType.Skill_0)
        {
            m_Name = "ȸ��";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 30;
            m_UpPrice = 30;

            m_SkillExp = "ü�� 1ĭ ȸ��";
            m_IconImg = Resources.Load("Image/SkillImg/Heal", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_1)   //����
        {
            m_Name = "����";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 100;
            m_UpPrice = 100;

            m_SkillExp = "5�ʰ� ����";
            m_IconImg = Resources.Load("Image/SkillImg/Invincibility", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_2)   //�ڼ�
        {
            m_Name = "�ڼ�";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 20;
            m_UpPrice = 20;

            m_SkillExp = "10�ʰ� ������ ������δ�";
            m_IconImg = Resources.Load("Image/SkillImg/Magnet", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_3)   //�Ŵ�ȭ
        {
            m_Name = "�Ŵ�ȭ";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 200;
            m_UpPrice = 200;

            m_SkillExp = "10�ʰ� �Ŵ�������.";
            m_IconImg = Resources.Load("Image/SkillImg/Gigantic", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_4)   //��Ȱ
        {
            m_Name = "��Ȱ";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 500;
            m_UpPrice = 500;

            m_SkillExp = "ü��3ĭ���� ��Ȱ�Ѵ�.";
            m_IconImg = Resources.Load("Image/SkillImg/Revive", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_5)   //�ı�
        {
            m_Name = "�ı�";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 100;
            m_UpPrice = 100;

            m_SkillExp = "10�ʰ� ���� ��ֹ��� �ı��Ѵ�.";
            m_IconImg = Resources.Load("Image/SkillImg/Destroyer", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_6)   //����
        {
            m_Name = "����";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 150;
            m_UpPrice = 150;

            m_SkillExp = "10�ʰ� �����Ѵ�.";
            m_IconImg = Resources.Load("Image/SkillImg/Flying", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_7)   //�ӵ� ����
        {
            m_Name = "�ӵ� ����";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 120;
            m_UpPrice = 120;

            m_SkillExp = "10�ʰ� ��������.";
            m_IconImg = Resources.Load("Image/SkillImg/Speed_Up", typeof(Sprite)) as Sprite;
        }
        else if (a_SkType == SkillType.Skill_8)   //���� 2��
        {
            m_Name = "���� ����";
            m_IconSize.x = 0.766f;
            m_IconSize.y = 1.0f;

            m_Price = 80;
            m_UpPrice = 80;

            m_SkillExp = "10�ʰ� ���� ȹ��� 2��� ���´�.";
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

        #region ������ �ε��ϱ�...
        string a_KeyBuff = "";
        for (int ii = 0; ii < (int)SkillType.Skill_Count; ii++)
        {
            if (m_SkDataList.Count <= ii)
                continue;

            a_KeyBuff = string.Format("Skill_Item_{0}", ii);
            m_SkDataList[ii].m_Level = PlayerPrefs.GetInt(a_KeyBuff, 0);

            //m_SkDataList[ii].m_Level = 3;   //<--�׽�Ʈ�� ���� �������� ����
        }
        #endregion ������ �ε��ϱ�...

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
