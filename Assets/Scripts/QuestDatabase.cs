using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class QuestDatabase
{
    /// <summary>
    /// Gets the available quests seen so far.
    /// </summary>
    public static NotifyDictionary<string, Quest> Quests => quests;
    private static readonly NotifyDictionary<string, Quest> quests = new();

    static QuestDatabase()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        quests.DictionaryChanged += Quests_DictionaryChanged;
    }

    private static void Quests_DictionaryChanged()
    {
        //Debug.Log("Logging quests change " +  string.Join(", ", quests.Values));
    }

    private static void SceneManager_sceneLoaded(Scene scene, LoadSceneMode _)
    {
        if (scene.name.StartsWith("Main")) Quests.Clear();
    }
}
