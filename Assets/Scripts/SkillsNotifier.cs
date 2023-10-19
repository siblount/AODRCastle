using UnityEngine;

public class SkillsNotifier : MonoBehaviour
{
    static SkillsNotifier Notifier;
    public GameObject dot;

    private void Awake()
    {
        Notifier = this;
    }

    public static void Notify()
    {
        if (Notifier != null)
        Notifier.dot.SetActive(true);
    }
    public static void Unnotify()
    {
        if (Notifier != null)
        Notifier.dot.SetActive(false);
    }
}
