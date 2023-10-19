using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class UpdateQuestExample : MonoBehaviour
{
    public Quest quest;
    public QuestStatus ChangeQuestStatusTo = QuestStatus.Success;
    [Range(0, 69)] public byte SkillPointsReward = 2;

    void OnTriggerEnter2D(){
        quest.Status = ChangeQuestStatusTo;
        if (quest.Status == QuestStatus.Success)
            Player.SkillPoints += SkillPointsReward;
        Destroy(this);
    }
}
