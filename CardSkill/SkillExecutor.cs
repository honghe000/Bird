using System;
using System.Collections.Generic;
using UnityEngine;


public class SkillExecutor : MonoBehaviour
{
    // ��̬���У�����ȫ�ִ洢����
    private static Queue<KeyValuePair<BaseSkill, Action>> skillQueue = new Queue<KeyValuePair<BaseSkill, Action>>();

    // ȫ��UID��������������ִ��
    public static string currentRunningSkillUid = null;

    public static int ������־ = 0;


    // ��Ӽ��ܵ�����
    public static void EnqueueSkill(BaseSkill skill, Action action)
    {
        skillQueue.Enqueue(new KeyValuePair<BaseSkill, Action>(skill, action));
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
                var skillPair = skillQueue.Dequeue();
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
                        currentRunningSkillUid = ValueHolder.�����ͷż��ܶ���.Dequeue();
                        mainfunction.Send�����ͷ�ͬ��(currentRunningSkillUid);
                    }
                    ������־ = 1;
                }else if (currentRunningSkillUid == null && skillQueue.Count == 0  && ValueHolder.is_myturn == 0 && ������־ == 1)
                {
                    mainfunction.Send�Է�����();
                    ������־ = 0;
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
