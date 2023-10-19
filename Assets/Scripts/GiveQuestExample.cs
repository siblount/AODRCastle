using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GiveQuestExample : MonoBehaviour
{
    public Quest[] questsToGive;
    public bool GiveOnStart = true;
    private void Start()
    {
        if (!GiveOnStart) return;
        foreach (var quest in questsToGive)
        {
            quest.Status = QuestStatus.InProgress;
        }
        // Do not trigger this event again by removing this script.
        Destroy(this);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        // TODO: Check if the collision was triggered by the player.
        // if (col.othercollider.comparetag("Player") or something like that.
        foreach (var quest in questsToGive)
        {
            quest.Status = QuestStatus.InProgress;
        }
        // Do not trigger this event again by removing this script.
        Destroy(this);
    }

}
