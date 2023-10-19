using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightUI : MonoBehaviour
{
    public static FightUI Instance;
    private struct EnemyUI {
        public TMP_Text HealthText;
        public GameObject HealthForeground;
        public Image[] Statuses;
    }

    private struct UserUI {
        public TMP_Text HealthText;
        public GameObject HealthForeground;
        public GameObject HeatForeground;
        public Image[] Statuses;
    }
    [SerializeField] public GameObject[] SelectionText;
    [SerializeField] public string[] SelectionExplainationText;
    [SerializeField] public GameObject ActionText;
    [SerializeField] public Color HighlightColor;
    [SerializeField] public GameObject FadePanel;
    [SerializeField] public GameObject ChatPanel;
    [SerializeField] public GameObject AbilityPanel;
    [SerializeField] public GameObject Enemy;
    [SerializeField] public GameObject User;
    [SerializeField] public TMP_Text UserStatsInfo;
    [SerializeField] public TMP_Text EnemyStatsInfo;

    [SerializeField] public Image PlayerAvatar;
    [SerializeField] public Image EnemyAvatar;

    private UserUI userUIElements;
    private EnemyUI enemyUIElements;

    private GameObject SelectedItem;
    private KeyboardScrollView keyboardView;
    public GameObject DialogueChat;
    private int next = 0;

    private bool acceptingInput = true;
    private bool abilitiesSelected = false;
    private bool attackSelected = false;
    private bool wardrobeSelected = false;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        InitializeUIElements();
    }

    // Update is called once per frame
    void Update()
    {
        if (!acceptingInput) return;
        DetermineUIScroll();
        DetermineUIAction();
    }

    private void DetermineUIScroll()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (abilitiesSelected || attackSelected || wardrobeSelected)
            {
                keyboardView.Select(true);
                UpdateActionText();
                if (!wardrobeSelected) return;
                HighlightEquippedWearable();
                return;
            }
            next = Mathf.Min(next + 1, SelectionText.Length - 1);
            SwitchMainItemText(SelectionText[next]);
            SelectedItem = SelectionText[next];
            UpdateActionText();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (abilitiesSelected || attackSelected || wardrobeSelected)
            {
                keyboardView.Select(false);
                UpdateActionText();
                if (!wardrobeSelected) return;
                // For wardrobe, also show the currently equipped item.
                HighlightEquippedWearable();
                return;
            }
            next = Mathf.Max(0, next - 1);
            SwitchMainItemText(SelectionText[next]);
            SelectedItem = SelectionText[next];
            UpdateActionText();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            abilitiesSelected = attackSelected = wardrobeSelected = false;
            HideAbilities();
        }
    }

    private void HighlightEquippedWearable()
    {
        foreach (var item in keyboardView.Items)
        {
            if (item == keyboardView.SelectedItem) continue;
            if (item.text == Fight.CurrentFight.UserStats.Stats.CurrentWearable?.Name)
                item.color = Color.green;

        }
    }

    private void DetermineUIAction()
    {
        if (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return))
            return;
        // If we selected Attack...
        if (!abilitiesSelected && !attackSelected && !wardrobeSelected)
        {
            if (SelectedItem == SelectionText[0]) // Basic attack
            {
                var userStats = (UserStats)Fight.CurrentFight.UserStats.Stats;
                var a = new List<string>(userStats.BasicAbilities.Count);
                userStats.BasicAbilities.ForEach((ab) => a.Add(ab.Name));

                keyboardView.SourceItems = a;
                attackSelected = true;
                ShowAbilities();
            }
            else if (SelectedItem == SelectionText[1]) // Abilities 
            {
                abilitiesSelected = true;
                // Show available abilities.
                var a = new string[Fight.CurrentFight.UserStats.AvailableAbilityNames.Count];
                Fight.CurrentFight.UserStats.AvailableAbilityNames.CopyTo(a);
                keyboardView.SourceItems = a;
                ShowAbilities();
            }
            else if (SelectedItem == SelectionText[2]) // Wardrobe 
            {
                if (Fight.CurrentFight.User.Wearables.Count == 0) return;
                wardrobeSelected = true;
                var a = new List<string>(Fight.CurrentFight.User.Wearables.Count);
                Fight.CurrentFight.User.Wearables.ForEach((wb) => a.Add(wb.Name));
                keyboardView.SourceItems = a;
                ShowAbilities();
                HighlightEquippedWearable();
            }
            //ShowDialogue(string.Join('\n', new string[] { "The quick brown fox jumped over the lazy dog.",
            //"The fox realized it was not a dog, but a wolfdog.",
            //"The fox ran away in fear!"}));
        } else if (attackSelected)
        {
            var selectionText = keyboardView.SelectedText;
            var ability = Instantiate(Fight.CurrentFight.UserStats.NameToAvailableAbility[selectionText]);
            ability.Owner = Fight.CurrentFight.UserStats;
            Fight.CurrentFight.Attack((BasicAbility)ability);
            attackSelected = false;
            HideAbilities();
        } else if (wardrobeSelected)
        {
            var selectedItemTxt = keyboardView.SelectedText;
            var selectedWearable = WearableDatabase.StringToWearables[selectedItemTxt];
            if (selectedItemTxt == Fight.CurrentFight.UserStats.Stats.CurrentWearable?.Name)
            {
                Fight.CurrentFight.UserStats.Stats.CurrentWearable.RemoveWearable();
                return;
            }
            Instantiate(selectedWearable).ApplyWearable(Fight.CurrentFight.UserStats);
            wardrobeSelected = false;
            HideAbilities();
        }
        else // ability selected
        {
            var selectionText = keyboardView.SelectedText;
            var ability = Instantiate(Fight.CurrentFight.UserStats.NameToAvailableAbility[selectionText]);
            ability.Owner = Fight.CurrentFight.UserStats;
            Fight.CurrentFight.UseAbility(ability);
            abilitiesSelected = false;
            HideAbilities();
        }
    }

    private void ShowAbilities()
    {
        AbilityPanel.SetActive(true);
        UpdateActionText();
    }

    private void HideAbilities()
    {
        AbilityPanel.SetActive(false);
        UpdateActionText();
    }

    public void SwitchMainItemText(GameObject curr) {
        if (curr == SelectedItem) return;
        var text = SelectedItem.GetComponent<TextMeshProUGUI>();

        if (curr is not null) {
            text.color = Color.white;
            text.text = text.text[0..^3];
            text.fontStyle = FontStyles.Normal;
            SelectedItem = curr;
        }

        text = SelectedItem.GetComponent<TextMeshProUGUI>();
        text.color = Color.blue;
        text.text += " <-";
        text.fontStyle = FontStyles.Bold;

    }

    public void UpdateActionText()
    {
        var t = ActionText.GetComponent<TMP_Text>();
        if (abilitiesSelected || attackSelected)
        {
            t.text = AbilityDatabase.StringToAbility[keyboardView.SelectedText].Description;
            return;
        }
        else if (wardrobeSelected)
        {
            var wearable = WearableDatabase.StringToWearables[keyboardView.SelectedText];
            t.text = wearable.Description + "; " + wearable.Effect;
            return;
        }
        else if (!acceptingInput) t.text = string.Empty;
        t.text = SelectionExplainationText[next];
    }

    // No references to Fight stats should be called in here!
    public void InitializeUIElements() {
        Transform t = User.transform;
        var hpPanel = t.Find("HP Panel");
        var heatPanel = t.Find("Heat Panel");

        Func<GameObject, Image[]> findAllStatuses = (g) =>
        {
            List<Image> s = new(4);
            g.GetComponentsInChildren(true, s);
            s.RemoveAll((i) => !i.CompareTag("Image"));
            return s.ToArray();
        };

        userUIElements = new UserUI
        {
            HealthForeground = hpPanel.Find("HP Panel Health Foreground").gameObject,
            HealthText = hpPanel.Find("HP Value Text").GetComponent<TMP_Text>(),
            HeatForeground = heatPanel.Find("Heat Foreground").gameObject,
            Statuses = findAllStatuses(User)
        };
        t = Enemy.transform;
        hpPanel = t.Find("HP Panel");
        heatPanel = t.Find("Heat Panel");

        enemyUIElements = new EnemyUI
        {
            HealthForeground = hpPanel.Find("HP Panel Health Foreground").gameObject,
            HealthText = hpPanel.Find("HP Value Text").GetComponent<TMP_Text>(),
            Statuses = findAllStatuses(Enemy)
        };

        SelectedItem = SelectionText[0];
        UpdateActionText();
        SwitchMainItemText(null);
        DialogueChat = ChatPanel.transform.GetChild(0).gameObject;

        keyboardView = AbilityPanel.GetComponentInChildren<KeyboardScrollView>();
    }

    /// <summary>
    /// Updates the UI to match enemy and user's health.
    /// </summary>
    public void UpdateHealths()
    {
        // Update health texts for enemy and user.
        userUIElements.HealthText.text = string.Format("{0}/{1}", Fight.CurrentFight.UserStats.Health,
            Fight.CurrentFight.UserStats.MaxHealth);
        enemyUIElements.HealthText.text = string.Format("{0}/{1}", Fight.CurrentFight.EnemyStats.Health,
            Fight.CurrentFight.EnemyStats.MaxHealth);

        float diff = (float)Fight.CurrentFight.UserStats.Health / Fight.CurrentFight.UserStats.MaxHealth;

        // Animate health bar decreasing/incrementing for enemy and user.
        StartCoroutine(UIUtilities.TweenUIBarsX(userUIElements.HealthForeground.GetComponent<RectTransform>(), diff));
        diff = (float)Fight.CurrentFight.EnemyStats.Health / Fight.CurrentFight.EnemyStats.MaxHealth;
        StartCoroutine(UIUtilities.TweenUIBarsX(enemyUIElements.HealthForeground.GetComponent<RectTransform>(), diff));
    }

    /// <summary>
    /// Updates the UI to match enemy and user's statuses.
    /// </summary>
    public void UpdateStatuses()
    {
        byte i = 0;
        for (; i < Fight.CurrentFight.UserStats.ActiveStatuses.Count; i++)
        {
            userUIElements.Statuses[i].gameObject.SetActive(true);
            userUIElements.Statuses[i].enabled = true;
            userUIElements.Statuses[i].sprite = Fight.CurrentFight.UserStats.ActiveStatuses[i].StatusImage;
        }
        for (; i < userUIElements.Statuses.Length; i++) userUIElements.Statuses[i].gameObject.SetActive(false);
        for (i = 0; i < Fight.CurrentFight.EnemyStats.ActiveStatuses.Count; i++)
        {
            enemyUIElements.Statuses[i].gameObject.SetActive(true);
            enemyUIElements.Statuses[i].enabled = true;
            enemyUIElements.Statuses[i].sprite = Fight.CurrentFight.EnemyStats.ActiveStatuses[i].StatusImage;
        }
        for (; i < enemyUIElements.Statuses.Length; i++) enemyUIElements.Statuses[i].gameObject.SetActive(false);
    }

    /// <summary>
    /// Updates the UI to match user's heat level.
    /// </summary>
    // Animate heat bar decreasing/incrementing for user.
    public void UpdateHeat() => StartCoroutine(
        UIUtilities.TweenUIBarsX(
            userUIElements.HeatForeground.GetComponent<RectTransform>(), 
            Fight.CurrentFight.UserStats.HeatPercentage)
        );


    public void InitializeFight() {
        // Update health texts for enemy and user.
        userUIElements.HealthText.text = string.Format("{0}/{1}", Fight.CurrentFight.UserStats.Health,
            Fight.CurrentFight.UserStats.MaxHealth);
        enemyUIElements.HealthText.text = string.Format("{0}/{1}", Fight.CurrentFight.EnemyStats.Health,
            Fight.CurrentFight.EnemyStats.MaxHealth);

        // Update heat
        var rh = userUIElements.HeatForeground.GetComponent<RectTransform>();
        rh.sizeDelta = new Vector2(-rh.rect.width, 0); // zero out heat

        // Statuses
        InitializeStatuses(userUIElements.Statuses, Fight.CurrentFight.UserStats.ActiveStatuses);
        InitializeStatuses(enemyUIElements.Statuses, Fight.CurrentFight.EnemyStats.ActiveStatuses);

        
        // Avatar
        PlayerAvatar.sprite = Fight.CurrentFight.User.Avatar;
        EnemyAvatar.sprite = Fight.CurrentFight.Enemy.Avatar;

        ShowStats();
    }

    private void InitializeStatuses(Image[] Images, List<Status> StatusInfos)
    {
        byte i = 0;
        for (; i < StatusInfos.Count; i++)
        {
            Images[i].sprite = StatusInfos[i].StatusImage;
            Images[i].enabled = true;
        }
        // Hide everything else.
        for (; i < Images.Length; i++) {
            Images[i].enabled = false;
        }
    }

    private void ShowStats()
    {
        var uStats = Fight.CurrentFight.UserStats.Stats;
        StringBuilder builder = new StringBuilder(50);
        const string format = "{0}: {1}\n" +
                            "{2}: {3}\n" +
                            "{4}: {5}\n" +
                            "{6}: {7}";
        builder.AppendFormat(format, nameof(uStats.HP), uStats.HP, 
            nameof(uStats.DMG), uStats.DMG,
            nameof(uStats.DEF), uStats.DEF,
            nameof(uStats.AP), uStats.AP);
        UserStatsInfo.text = builder.ToString();
        
        // Now clear and do enemy stats.
        builder.Clear();
        uStats = Fight.CurrentFight.EnemyStats.Stats;
        builder.AppendFormat(format, nameof(uStats.HP), uStats.HP,
            nameof(uStats.DMG), uStats.DMG,
            nameof(uStats.DEF), uStats.DEF,
            nameof(uStats.AP), uStats.AP);
        EnemyStatsInfo.text = builder.ToString();
    }

    public void ShowDialogue(string lines)
    {
        ChatPanel.SetActive(true);
        var rect = ChatPanel.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, -rect.rect.height + rect.sizeDelta.y);
        var t = DialogueChat.GetComponent<Typewriter>();
        t.DoBeforeTyping = UIUtilities.TweenUIBarsY(rect, 1);
        t.enabled = true;
        t.StartTyping(lines);
        acceptingInput = false;
    }

    public void HideDialogue() => StartCoroutine(HideDialogueLogic());

    public void AddDialogueLine(string line)
    {
        var t = DialogueChat.GetComponent<Typewriter>();
        if (!t.isActiveAndEnabled)
        {
            ShowDialogue(line);
            return;
        }
        t.AddLine(line);
    }

    public void InterruptDialogueAnimation()
    {
        var t = DialogueChat.GetComponent<Typewriter>();
        t.InterruptTyping();
        t.enabled = false;
        ChatPanel.SetActive(false);
        acceptingInput = true;
    }

    IEnumerator HideDialogueLogic()
    {
        var rect = ChatPanel.GetComponent<RectTransform>();
        yield return UIUtilities.TweenUIBarsY(rect, 0);
        //LayoutUtility.GetPreferredHeight(rect);
        acceptingInput = true;
        ChatPanel.SetActive(false);
    }
        

    public IEnumerator EndFight()
    {
        FadePanel.SetActive(true);
        yield return UIUtilities.TweenUIAlpha(FadePanel.GetComponent<Image>(), 1.3f);
    }
    
}
