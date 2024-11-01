using System;
using System.Collections.Generic;
using UnityEngine;


public class SkillExecutor : MonoBehaviour
{
    // 静态队列，用于全局存储技能
    private static Queue<KeyValuePair<BaseSkill, Action>> skillQueue = new Queue<KeyValuePair<BaseSkill, Action>>();

    // 全局UID，用于阻塞技能执行
    public static string currentRunningSkillUid = null;


    // 添加技能到队列
    public static void EnqueueSkill(BaseSkill skill, Action action)
    {
        skillQueue.Enqueue(new KeyValuePair<BaseSkill, Action>(skill, action));
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
            // 如果没有技能在执行且队列中有技能，执行下一个技能
            if (currentRunningSkillUid == null && skillQueue.Count > 0)
            {
                var skillPair = skillQueue.Dequeue();
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
                        currentRunningSkillUid = ValueHolder.申请释放技能队列.Dequeue();
                        mainfunction.Send技能释放同意(currentRunningSkillUid);
                    }
                }else if (currentRunningSkillUid == null && skillQueue.Count == 0  && ValueHolder.is_myturn == 0)
                {
                    mainfunction.Send对方继续();
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
