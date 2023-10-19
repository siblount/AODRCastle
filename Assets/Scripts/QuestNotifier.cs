using UnityEngine;

public class QuestNotifier : MonoBehaviour
{
    static QuestNotifier Notifier;
    public GameObject dot;

    private void Awake()
    {
        Notifier = this;
    }

    public static void Notify() => Notifier?.dot.SetActive(true);
    public static void Unnotify() => Notifier?.dot.SetActive(false);
}
