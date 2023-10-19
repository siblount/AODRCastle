using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine;

public class QuestsLogUI : MonoBehaviour
{
    public TMP_Text StatusText;
    public TMP_Text DescriptionText;
    public TMP_Text QuestTitle;
    public QuestKeyboardScrollView QuestKeyboardScrollView;

    public Color InProgressColor = Color.yellow;
    public Color CompleteColor = Color.green;
    public Color FailedColor = Color.red;

    private void Start()
    {
        Quest.StatusChanged += OnStatusChange;
    }

    private void OnEnable()
    {
        ToggleNotification(false);
        // If the keyboard scroll view is not enabled...
        // call the function.
        if (QuestKeyboardScrollView.Items.Length == 0)
            QuestKeyboardScrollView.Awake();
        QuestKeyboardScrollView.ActiveQuestsSource.Clear();
        QuestKeyboardScrollView.CompletedQuestsSource.Clear();
        foreach (var quest in QuestDatabase.Quests.Values)
        {
            if (quest.Status == QuestStatus.InProgress)
                QuestKeyboardScrollView.ActiveQuestsSource.Add(quest.title);
            else if ((byte)quest.Status > (byte)QuestStatus.InProgress)
                QuestKeyboardScrollView.CompletedQuestsSource.Add(quest.title);
        }
        ToggleNotification(true);
        QuestKeyboardScrollView.ActiveQuestsSource.EmitListChangedEvent();
        UpdateQuestHeader();
    }

    //private void OnDisable()
    //{
    //    QuestKeyboardScrollView.gameObject.SetActive(false);
    //}

    private void OnDestroy()
    {
        Quest.StatusChanged -= OnStatusChange;
    }

    private void OnStatusChange(Quest quest, QuestStatus from, QuestStatus to)
    {
        ToggleNotification(false);
        if (from == QuestStatus.InProgress)
        {
            QuestKeyboardScrollView.ActiveQuestsSource.Remove(quest.title);
            QuestKeyboardScrollView.CompletedQuestsSource.Add(quest.title);
        }
        else if (from == QuestStatus.Invisible) 
            QuestKeyboardScrollView.ActiveQuestsSource.Add(quest.title);
        ToggleNotification(true);
        // Trigger the scrollview to update.
        QuestKeyboardScrollView.ActiveQuestsSource.EmitListChangedEvent();
        UpdateQuestHeader();
    }
    void ToggleNotification(bool enable)
    {
        QuestKeyboardScrollView.ActiveQuestsSource.Notify =
            QuestKeyboardScrollView.CompletedQuestsSource.Notify = enable;
    }

    private void UpdateQuestHeader()
    {
        if (!isActiveAndEnabled) return;
        // if there are no quests show.
        if (QuestKeyboardScrollView.TotalCount == 2)
        {
            StatusText.text = DescriptionText.text = string.Empty;
            QuestTitle.text = "No available quests";
            return;
        } 
        else if (QuestKeyboardScrollView.SelectedItem.name.EndsWith("Header"))
            QuestKeyboardScrollView.Select();
        try
        {
            var quest = QuestDatabase.Quests[QuestKeyboardScrollView.SelectedText];
            QuestTitle.text = quest.title;
            DescriptionText.text = quest.MainQuest ? "<b>Main Quest: </b>" : "<b>Side Quest: </b>";
            DescriptionText.text += quest.Description;
            switch (quest.Status)
            {
                case QuestStatus.InProgress:
                    StatusText.text = "In Progress";
                    StatusText.color = InProgressColor;
                    break;
                case QuestStatus.Failed:
                    StatusText.text = "Failed";
                    StatusText.color = FailedColor;
                    break;
                case QuestStatus.Success:
                    StatusText.text = "Completed";
                    StatusText.color = CompleteColor;
                    break;
            }
        } catch (Exception ex)
        {
            Debug.LogWarning($"This dogfood code keeps throwing errors but I'm too lazy to apply a proper fix to it." +
                $"Here's that error message btw at QuestsLogUI.UpdateQusetHeader(): {ex}");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            QuestKeyboardScrollView.Select(true);
            UpdateQuestHeader();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            QuestKeyboardScrollView.Select(false);
            UpdateQuestHeader();
        }
    }
}
