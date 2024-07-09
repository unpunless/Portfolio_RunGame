using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkPlayerNode : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType = SkillType.Skill_Count;
    [HideInInspector] public int m_CurSkCount = 0;

    public Image m_RootBtnImg;  //Root�� ��ư���� ��������Ƿ� ��ư ���̹��� ���ٿ� ����
    public Text m_ShortcutText; //����Ű �ؽ�Ʈ
    public Text m_LvText;       //���� ��ų�� ������ ǥ���� �� �ؽ�Ʈ
    public Text m_SkCountText;  //��ų ī��Ʈ �ؽ�Ʈ
    public Image m_SkIconImg;   //ĳ���� ������ �̹���

    // Start is called before the first frame update
    void Start()
    {
        //�� ��ư�� ������ ��ų�� ����ϰ� ����
        Button a_BtnCom = this.GetComponent<Button>();
        if (a_BtnCom != null)
            a_BtnCom.onClick.AddListener(() =>
            {
                if (GlobalValue.m_SkDataList[(int)m_SkType].m_CurSkillCount <= 0)
                    return; //�����ϰ� �ִ� ��ų �������� ����� �� ����

                PlayerCtrl a_Hero = GameObject.FindObjectOfType<PlayerCtrl>();
                if (a_Hero != null)
                    a_Hero.UseSkill(m_SkType);

                Refresh_UI(m_SkType);
            });

    }//void Start()

    public void InitState(Skill_Info a_SkInfo)
    {
        m_SkType = a_SkInfo.m_SkType;

        // ����� �޽��� �߰�
        Debug.Log("Loading image for SkType: " + m_SkType);

        m_SkIconImg.sprite = GlobalValue.m_Bag.Find(sk => sk.m_SkType == m_SkType)?.m_IconImg;

        if (m_SkIconImg.sprite != null)
        {
            Debug.Log("Sprite loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load sprite for SkType: " + m_SkType);
        }

        m_SkIconImg.GetComponent<RectTransform>().sizeDelta =
                            new Vector2(a_SkInfo.m_IconSize.x * 103.0f, 103.0f);

        //��������Ʈ ������ �̹����� ������ �°� ����
        m_CurSkCount = a_SkInfo.m_Level;
        m_LvText.text = "Lv " + a_SkInfo.m_Level.ToString();
        m_SkCountText.text = m_CurSkCount.ToString() +
                                    " / " + a_SkInfo.m_Level.ToString();
    }

    public void Refresh_UI(SkillType a_SkType)
    {
        if (m_SkType != a_SkType)
            return;

        m_CurSkCount = GlobalValue.m_SkDataList[(int)m_SkType].m_CurSkillCount;
        if (m_SkCountText != null)
            m_SkCountText.text = m_CurSkCount.ToString() +
                " / " + GlobalValue.m_SkDataList[(int)m_SkType].m_Level.ToString();

        if (m_CurSkCount <= 0)
        {
            if (m_SkIconImg != null)
                m_SkIconImg.color = new Color32(255, 255, 255, 80);

            Destroy(this.gameObject);
        }
    } //public void Refresh_UI(SkillType a_SkType)
}
