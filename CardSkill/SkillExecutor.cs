using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;


public class SkillExecutor : MonoBehaviour
{
    // 静态队列，用于全局存储技能
    public static LinkedList<KeyValuePair<BaseSkill, Action>> skillQueue = new LinkedList<KeyValuePair<BaseSkill, Action>>();

    // 全局UID，用于阻塞技能执行
    public static string currentRunningSkillUid = null;


    // 添加技能到队列
    public static void EnqueueSkill(BaseSkill skill, Action action)
    {
        // 正常的入队方法，将元素添加到尾部
        skillQueue.AddLast(new KeyValuePair<BaseSkill, Action>(skill, action));
    }

    public static void EnqueueSkillAtFront(BaseSkill skill, Action action)
    {
        // 从头部插入元素
        skillQueue.AddFirst(new KeyValuePair<BaseSkill, Action>(skill, action));
    }

    private void Start()
    {
        // 在启动时开始检查队列
        StartCoroutine(ProcessQueue());
    }

    // 使用协程来持续检查并执行技能队列
    private System.Collections.IEnumerator ProcessQueue()
    {
        while (true)
        {
            //Debug.Log("技能队列长度：" + skillQueue.Count);
            //Debug.Log("当前UID：" + currentRunningSkillUid);
            // 如果没有技能在执行且队列中有技能，执行下一个技能
            if (currentRunningSkillUid == null && skillQueue.Count > 0)
            {
                var skillPair = skillQueue.First.Value;
                skillQueue.RemoveFirst(); // 从队列中移除已执行的技能
                var skill = skillPair.Key;
                var action = skillPair.Value;

                // 设置当前UID，以表示正在执行技能
                currentRunningSkillUid = null;
                action.Invoke(); // 执行技能动作
            }

            // 检查UID状态，UID为空时表示技能完成，继续执行下一个
            if (currentRunningSkillUid == null && skillQueue.Count > 0)
            {
                yield return null; // 等待下一帧，继续检查队列
            }
            else
            {
                if (currentRunningSkillUid == null && skillQueue.Count == 0 && ValueHolder.申请释放技能队列.Count > 0 && ValueHolder.is_myturn == 1)
                {
                    while (ValueHolder.申请释放技能队列.Count > 0)
                    {
                        Dictionary<string, int> skillInfo = ValueHolder.申请释放技能队列.Dequeue();
                        currentRunningSkillUid = skillInfo.First().Key;
                        int skill_type = skillInfo.First().Value;
                        mainfunction.Send技能释放同意(currentRunningSkillUid,skill_type);
                    }
                }else if (currentRunningSkillUid == null && skillQueue.Count == 0  && ValueHolder.is_myturn == 0 && ValueHolder.敌方回合运行我方技能 == 1)
                {
                    mainfunction.Send对方继续();
                    ValueHolder.敌方回合运行我方技能 = 0;
                }
                yield return new WaitForSeconds(0.2f); // 阻塞0.2秒，避免过度循环
            }
        }
    }

    // 提供一个方法来清除当前UID，这样可以控制技能执行的继续
    public static void CompleteCurrentSkill()
    {
        currentRunningSkillUid = null; // 重置UID，表示当前技能已完成
    }
}
