using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OverworldSceneChanger : MonoBehaviour
{
    public Quest[] FailQuestsIfNotCompletedList;
    public GiveQuestExample example;

    private void Start()
    {
        example = FindFirstObjectByType<GiveQuestExample>();
        FailQuestsIfNotCompletedList = new Quest[example.questsToGive.Length];
        example.questsToGive.CopyTo(FailQuestsIfNotCompletedList, 0);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "player_0")
        {
            foreach (var quest in FailQuestsIfNotCompletedList)
            {
                if (quest.Status == QuestStatus.InProgress)
                    quest.Status = QuestStatus.Failed;
            }
            SceneManager.LoadScene("CastleLevel");
        }
    }
}
