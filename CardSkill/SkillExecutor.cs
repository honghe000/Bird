using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;


public class SkillExecutor : MonoBehaviour
{
    // ��̬���У�����ȫ�ִ洢����
    public static LinkedList<KeyValuePair<BaseSkill, Action>> skillQueue = new LinkedList<KeyValuePair<BaseSkill, Action>>();

    // ȫ��UID��������������ִ��
    public static string currentRunningSkillUid = null;


    // ��Ӽ��ܵ�����
    public static void EnqueueSkill(BaseSkill skill, Action action)
    {
        // ��������ӷ�������Ԫ����ӵ�β��
        skillQueue.AddLast(new KeyValuePair<BaseSkill, Action>(skill, action));
    }

    public static void EnqueueSkillAtFront(BaseSkill skill, Action action)
    {
        // ��ͷ������Ԫ��
        skillQueue.AddFirst(new KeyValuePair<BaseSkill, Action>(skill, action));
    }

    private void Start()
    {
        // ������ʱ��ʼ������
        StartCoroutine(ProcessQueue());
    }

    // ʹ��Э����������鲢ִ�м��ܶ���
    private System.Collections.IEnumerator ProcessQueue()
    {
        while (true)
        {
            //Debug.Log("���ܶ��г��ȣ�" + skillQueue.Count);
            //Debug.Log("��ǰUID��" + currentRunningSkillUid);
            // ���û�м�����ִ���Ҷ������м��ܣ�ִ����һ������
            if (currentRunningSkillUid == null && skillQueue.Count > 0)
            {
                var skillPair = skillQueue.First.Value;
                skillQueue.RemoveFirst(); // �Ӷ������Ƴ���ִ�еļ���
                var skill = skillPair.Key;
                var action = skillPair.Value;

                // ���õ�ǰUID���Ա�ʾ����ִ�м���
                currentRunningSkillUid = null;
                action.Invoke(); // ִ�м��ܶ���
            }

            // ���UID״̬��UIDΪ��ʱ��ʾ������ɣ�����ִ����һ��
            if (currentRunningSkillUid == null && skillQueue.Count > 0)
            {
                yield return null; // �ȴ���һ֡������������
            }
            else
            {
                if (currentRunningSkillUid == null && skillQueue.Count == 0 && ValueHolder.�����ͷż��ܶ���.Count > 0 && ValueHolder.is_myturn == 1)
                {
                    while (ValueHolder.�����ͷż��ܶ���.Count > 0)
                    {
                        Dictionary<string, int> skillInfo = ValueHolder.�����ͷż��ܶ���.Dequeue();
                        currentRunningSkillUid = skillInfo.First().Key;
                        int skill_type = skillInfo.First().Value;
                        mainfunction.Send�����ͷ�ͬ��(currentRunningSkillUid,skill_type);
                    }
                }else if (currentRunningSkillUid == null && skillQueue.Count == 0  && ValueHolder.is_myturn == 0 && ValueHolder.�з��غ������ҷ����� == 1)
                {
                    mainfunction.Send�Է�����();
                    ValueHolder.�з��غ������ҷ����� = 0;
                }
                yield return new WaitForSeconds(0.2f); // ����0.2�룬�������ѭ��
            }
        }
    }

    // �ṩһ�������������ǰUID���������Կ��Ƽ���ִ�еļ���
    public static void CompleteCurrentSkill()
    {
        currentRunningSkillUid = null; // ����UID����ʾ��ǰ���������
    }
}
