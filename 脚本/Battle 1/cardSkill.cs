using UnityEngine;
public class cardSkill
{
    public BaseSkill skill;
    public GameObject card;
    public cardSkill(GameObject card,BaseSkill skill)
    {
        this.skill = skill;
        this.card = card;
    }
}
