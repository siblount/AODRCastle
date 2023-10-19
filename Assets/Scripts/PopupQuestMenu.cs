using System.Collections;
using UnityEngine;
public class PopupQuestMenu : MonoBehaviour
{
    [Range(0.01f, 1f)]public float AnimateTime = 0.3f;
    [System.Serializable]
    public struct UIContainer
    {
        public GameObject Menu;
        public Vector3 InitPosition;
        public LTDescr activeDescr;
    }
    public UIContainer QuestContainer = new();
    public UIContainer SkillsContainer = new();
    public UIContainer SettingsContainer = new();

    private GameObject activeMenu;
    public static PopupQuestMenu Instance;

    public void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !QuestContainer.Menu.activeInHierarchy)
        {
            ToggleMenu(SkillsContainer);
            SkillsNotifier.Unnotify();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && !SkillsContainer.Menu.activeInHierarchy)
        {
            ToggleMenu(QuestContainer);
            QuestNotifier.Unnotify();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
            ToggleSettings();
    }

    private void Start()
    {
        QuestContainer.InitPosition = QuestContainer.Menu.GetComponent<RectTransform>().position;
        SkillsContainer.InitPosition = SkillsContainer.Menu.GetComponent<RectTransform>().position;
        SettingsContainer.InitPosition = SettingsContainer.Menu.GetComponent<RectTransform>().position;
    }
    
    public void ToggleSettings()
    {
        var lastActive = activeMenu;
        ToggleMenu(SettingsContainer);
        if (lastActive != null) 
            lastActive.SetActive(true);
    }

    private void ToggleMenu(UIContainer container)
    {
        if (container.activeDescr is not null)
        {
            LeanTween.cancel(container.activeDescr.uniqueId);
            container.activeDescr = null;
        }
        container.Menu.SetActive(!container.Menu.activeSelf);
        GameSettings.Paused = container.Menu.activeSelf;

        // If the menu is deactivated, do not continue 
        // with animation logic.
        if (container.Menu.activeSelf && container.Menu != SettingsContainer.Menu)
            activeMenu = container.Menu;
        else if (!container.Menu.activeSelf)
        {
            activeMenu = null;
            return;
        }
        var rect = container.Menu.GetComponent<RectTransform>();
        // Get the width of the screen
        float screenWidth = Screen.width;

        // Calculate the target position for the panel to move off the screen to the left
        var rectangle = rect.rect;
        var outOfScreenPosition = new Vector2(screenWidth - rectangle.width - rectangle.xMax, rect.position.y);
        var targetPosition = new Vector2(container.InitPosition.x, outOfScreenPosition.y);
        rect.position = new Vector3(outOfScreenPosition.x, outOfScreenPosition.y, rect.position.z);

        // Use LeanTween to move the panel smoothly
        container.activeDescr = LeanTween.moveX(container.Menu, targetPosition.x, AnimateTime)
                                         .setIgnoreTimeScale(true)
                                         .setOnComplete(() => container.activeDescr = null);
    }

}
