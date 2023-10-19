using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(menuName = "Create new Quest")]
public class Quest : ScriptableObject
{
    
    public delegate void StatusChangeHandler(Quest quest, QuestStatus from, QuestStatus to);
    /// <summary>
    /// This event is called when the status of a quest has changed.
    /// </summary>
    public static StatusChangeHandler StatusChanged;
    /// <summary>
    /// This event is called when the description of a quest has changed.
    /// </summary>
    public static Action<Quest> DescriptionChanged;
    /// <summary>
    /// The title of the quest.
    /// </summary>
    public string title = string.Empty;
    /// <summary>
    /// The current description of the quest.
    /// </summary>
    public string Description => descriptionAtState[State];
    /// <summary>
    /// The status of the Quest - is it active or has it been completed?
    /// These questions can be found here.
    /// </summary>
    public QuestStatus Status
    {
        get => status;
        set
        {
            if (value == status) return;
            var from = status;
            status = value;
            QuestNotifier.Notify();
            StatusChanged?.Invoke(this, from, value);
        }
    }
    /// <summary>
    /// The state of the quest. This is used to determine
    /// which description to show.
    /// </summary>
    public byte State = 0;
    [SerializeField] private QuestStatus status = QuestStatus.Invisible;
    [Header("Add a description per state.")]
    [SerializeField] private string[] descriptionAtState;
    /// <summary>
    /// Determines whether the quest is a main quest (or a side quest).
    /// </summary>
    public bool MainQuest;
    private void OnEnable()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlayingOrWillChangePlaymode) return;
#endif
        QuestDatabase.Quests[title] = this;
    }
    // gets called before OnEnable
    private void OnValidate()
    {
        status = QuestStatus.Invisible;
        State = 0;
        if (descriptionAtState.Length == 0)
            descriptionAtState = new string[1] { "Initial description" };
    }

    private void Reset()
    {
        descriptionAtState = new string[1] { "Initial description" };
        title = string.Empty;
        status = QuestStatus.Invisible;
    }

   
}

/// <summary>
/// Indicates the status of the quest.
/// </summary>
public enum QuestStatus
{
    /// <summary>
    /// Invisible - meaning the quest is not yet visible to the user.
    /// </summary>
    Invisible,
    /// <summary>
    /// In Progress - meaning the quest is currently active but not yet completed.
    /// </summary>
    InProgress,
    /// <summary>
    /// Success - The quest has been completed and was a success.
    /// </summary>
    Success,
    /// <summary>
    /// Failed - The quest has been completed but it was a fail.
    /// </summary>
    Failed
}
