using UnityEngine;

public static class SkillFactory
{
    public static BaseSkill CreateSkill(GameObject card, MonoBehaviour monoBehaviour)
    {
        switch (card.GetComponent<������ʾ>().��������.����)
        {
            case "�������ʦ":
                return new �������ʦ(card, monoBehaviour);
            case "�ల����":
                return new �ల����(card, monoBehaviour);
            case "����":
                return new ����(card, monoBehaviour);
            case "��":
                return new ��(card, monoBehaviour);
            case "����֮��":
                return new ����֮��(card, monoBehaviour);
            case "���":
                return new ���(card, monoBehaviour);
            case "�йٱ�":
                return new �йٱ�(card, monoBehaviour);
            case "������":
                return new ������(card, monoBehaviour);
            case "�ŵ���":
                return new �ŵ���(card, monoBehaviour);
            case "Ыβ��":
                return new Ыβ��(card, monoBehaviour);
            case "����֮ŭ":
                return new ����֮ŭ(card, monoBehaviour);
            case "����":
                return new ����(card, monoBehaviour);
            case "������":
                return new ������(card, monoBehaviour);
            case "�⽻��":
                return new �⽻��(card, monoBehaviour);
            default:
                return null;
        }
    }
}
