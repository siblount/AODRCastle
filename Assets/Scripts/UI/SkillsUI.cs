using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillsUI : MonoBehaviour
{
    public TMP_Text AvailableSkillPoints;
    [Header("Explanation Panel")]
    public TMP_Text SkillName;
    public TMP_Text Description;
    public GameObject UpgradeHeader;
    public TMP_Text SkillLevelsTxt;
    public TMP_Text ActionableTxt;
    [Header("Panels")]
    public GameObject[] StatPanels;
    public GameObject[] SkillPanels;
    [Header("Stat Descriptions"), TextArea]
    public string HPDesc;
    [TextArea]
    public string APDesc, DEFDesc, DMGDesc;
    [Header("Colors")]
    public Color SelectedItemColor;
    public Color DefaultSkillItemColor;
    public Color UpgradeNormalColor;
    public Color UpgradeErrorColor;

    struct StatPanelUIComponents
    {
        public GameObject BackgroundPanel, 
            ForegroundPanel;
        public TMP_Text StatText;
    }

    struct SkillPanelUIComponents
    {
        public GameObject Panel;
        public TMP_Text Text;
    }

    private StatPanelUIComponents[] StatsUIContainer;
    private SkillPanelUIComponents[] SkillsUIContainer;

    private bool StatsSelected = false;
    private byte StatsIndex, SkillsIndex;
    private bool switchingContext;

    private void Awake()
    {
        StatsUIContainer = new StatPanelUIComponents[StatPanels.Length];
        SkillsUIContainer = new SkillPanelUIComponents[SkillPanels.Length];
        for (int i = 0; i < StatsUIContainer.Length; i++)
        {
            StatsUIContainer[i] = new()
            {
                BackgroundPanel = StatPanels[i].transform.Find("Background Panel").gameObject,
                ForegroundPanel = StatPanels[i].transform.Find("Foreground Panel").gameObject,
                StatText = StatPanels[i].transform.Find("Skill Points Text").GetComponent<TMP_Text>(),
            };
        }
        for (int i = 0; i < SkillsUIContainer.Length; i++)
        {
            SkillsUIContainer[i] = new()
            {
                Panel = SkillPanels[i],
                Text = SkillPanels[i].transform.GetComponentInChildren<TMP_Text>(),
            };
        }
    }


    private void OnEnable() => InitView();
    private void OnDisable() => UnhighlightCurrent();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) HandleAction();
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            HandleScroll(true);
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            HandleScroll(false, true, true);
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            HandleScroll();
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            HandleScroll(horizontal: true);
    }

    void HandleScroll(bool up = false, bool left = false, bool horizontal = false)
    {
        if (StatsSelected)
        {
            sbyte dir = (sbyte)(up ? -1 : 1);
            if (IndexIsInRange(StatPanels, StatsIndex + dir))
                StatsIndex = (byte)(StatsIndex + dir);
            else if (dir < 0) return;
            else
            {
                switchingContext = true;
                StatsSelected = false;
                HighlightAndUnhighlight(dir);
            }
            HighlightAndUnhighlight(dir);
        } else
        {
            sbyte vdir = (sbyte)(up ? -3 : 3);
            sbyte hdir = (sbyte)(left ? -1 : 1);
            if (!horizontal && IndexIsInRange(SkillPanels, SkillsIndex + vdir))
            {
                SkillsIndex = (byte)(SkillsIndex + vdir);
                HighlightAndUnhighlight(vdir);
            }
            else if (horizontal && IndexIsInRange(SkillPanels, SkillsIndex + hdir))
            {
                SkillsIndex = (byte)(SkillsIndex + hdir);
                HighlightAndUnhighlight(hdir);
            }
            else if (!horizontal && vdir < 0)
            {
                switchingContext = StatsSelected = true;
                HighlightAndUnhighlight(-1);
            }
            else return;
        }
        UpdateView();
        
    }

    void HandleAction()
    {
        // Quick shortcut: if no skill points, then we do nothing.
        if (Player.SkillPoints == 0) return;
        if (StatsSelected)
        {
            var stat = GetStatValue(StatsIndex);
            if (stat == 10) return;
            SetStatValue(StatsIndex, ++stat);
        } else
        {
            var skillName = GetSkillName(SkillsIndex);
            var ability = GetAbilityFromPlayer(skillName);
            if (ability is null) Player.PlayerData.Abilities.Add(
                Instantiate(AbilityDatabase.StringToAbility[skillName])
            );
            else if (ability.Level != ability.UpgradeDescriptionsPerLevel.Length + 1) ability.Level++;
            else return;
        }
        Player.SkillPoints--;
        UpdateView();
    }

    void UpdateView()
    {
        AvailableSkillPoints.text = "Available Skill Points: " + Player.SkillPoints;
        UpdateExplanationPanel();
        if (StatsSelected) UpdateStatPanel(StatsIndex);
        switchingContext = false;
    }

    void HighlightCurrent()
    {
        if (StatsSelected) StatPanels[StatsIndex].GetComponent<Image>().color = SelectedItemColor;
        else SkillPanels[SkillsIndex].GetComponent<Image>().color = SelectedItemColor;
    }

    void HighlightAndUnhighlight(sbyte dir)
    {
        HighlightCurrent();
        if (StatsSelected)
        {
            if (switchingContext)
                SkillPanels[SkillsIndex].GetComponent<Image>().color = DefaultSkillItemColor;
            else StatPanels[StatsIndex - dir].GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
        else
        {
            if (switchingContext)
                StatPanels[StatsIndex].GetComponent<Image>().color = new Color(0, 0, 0, 0);
            else
                SkillPanels[SkillsIndex - dir].GetComponent<Image>().color = DefaultSkillItemColor;
        }
    }
    void UnhighlightCurrent()
    {
        if (StatsSelected) StatPanels[StatsIndex].GetComponent<Image>().color = new Color(0, 0, 0, 0);
        else SkillPanels[SkillsIndex].GetComponent<Image>().color = DefaultSkillItemColor;
    }

    void InitView()
    {
        StatsIndex = SkillsIndex = 0;
        StatsSelected = true;
        for (byte i = 0; i < StatsUIContainer.Length; i++)
        {
            UpdateStatPanel(i);
        }
        UpdateView();
        HighlightCurrent();
    }

    void UpdateStatPanel(byte i)
    {
        var statValue = GetStatValue(i);
        var percentage = statValue / 10f;
        var component = StatsUIContainer[i];
        component.StatText.text = $"{statValue}/10";
        // and now for the math magic ugh...
        // Use the background as a reference.
        var backRect = component.BackgroundPanel.GetComponent<RectTransform>();
        var frontRect = component.ForegroundPanel.GetComponent<RectTransform>();
        // Start from this point.
        float startSizeDelta = backRect.sizeDelta.x - backRect.rect.width;
        float endSizeDelta = backRect.sizeDelta.x;
        float fillSizeDelta = (endSizeDelta - startSizeDelta) * percentage;
        frontRect.sizeDelta = new Vector2(startSizeDelta + fillSizeDelta, frontRect.sizeDelta.y);
    }

    void UpdateExplanationPanel()
    {
        if (StatsSelected)
        {
            var statName = SkillName.text = GetStatName(StatsIndex).ToString();
            // Description logic.
            if (statName == "HP") Description.text = HPDesc;
            else if (statName == "DEF") Description.text = DEFDesc;
            else if (statName == "AP") Description.text = APDesc;
            else Description.text = DMGDesc;
            //
            if (GetStatValue(StatsIndex) == 10)
            {
                ActionableTxt.text = "Fully Upgraded";
                ActionableTxt.color = UpgradeNormalColor;
            } else if (Player.SkillPoints != 0)
            {
                ActionableTxt.text = "Press E to Upgrade. Costs 1 SP.";
                ActionableTxt.color = UpgradeNormalColor;
            } else
            {
                ActionableTxt.text = "Upgrade unavailable.\nNot enough SP (need 1 SP).";
                ActionableTxt.color = UpgradeErrorColor;
            }
            SkillLevelsTxt.text = $"Increases your {statName} by <b>1</b> point.";
        } else
        {
            var skillName = SkillName.text = GetSkillName(SkillsIndex);
            var ability = AbilityDatabase.StringToAbility[skillName];
            Description.text = ability.Description;
            if (ability.UpgradeDescriptionsPerLevel.Length != 0)
            {
                SkillLevelsTxt.text = "";
                var playerLevel = GetAbilityFromPlayer(skillName)?.Level ?? 0;
                for (int i = 0; i < ability.UpgradeDescriptionsPerLevel.Length; i++)
                {
                    if (playerLevel > i + 1)
                        SkillLevelsTxt.text += $" - <b><color=green>{ability.UpgradeDescriptionsPerLevel[i]}</color></b>\n";
                    else SkillLevelsTxt.text += $" - {ability.UpgradeDescriptionsPerLevel[i]}\n";
                }
            }
            else SkillLevelsTxt.text = string.Empty;
            var playerAbility = GetAbilityFromPlayer(ability.NameRaw);
            if (playerAbility is null) ActionableTxt.text = "Press E to obtain ability. Costs 1 SP.";
            else if (playerAbility.Level != ability.UpgradeDescriptionsPerLevel.Length + 1)
                ActionableTxt.text = "Press E to Upgrade. Costs 1 SP.";
            else ActionableTxt.text = "Fully Upgraded";
            ActionableTxt.color = UpgradeNormalColor;
            if (Player.SkillPoints == 0)
            {
                ActionableTxt.text = "Upgrade unavailable.\nNot enough SP (need 1 SP).";
                ActionableTxt.color = UpgradeErrorColor;
            }
        }
    }
    Ability? GetAbilityFromPlayer(string nameRaw)
    {
        if (AbilityDatabase.StringToAbility[nameRaw] is BasicAbility) 
            return Player.PlayerData.BasicAbilities.Find((a) => a.NameRaw == nameRaw);
        return Player.PlayerData.Abilities.Find((a) => a.NameRaw == nameRaw);
    }

    string GetSkillName(int i) => SkillsUIContainer[i].Text.text;

    ref byte GetStatValue(int i)
    {
        var statName = GetStatName(i).ToString();
        if (statName == "HP") return ref Player.PlayerData.HP;
        else if (statName == "AP") return ref Player.PlayerData.AP;
        else if (statName == "DEF") return ref Player.PlayerData.DEF;
        else return ref Player.PlayerData.DMG;
    }

    void SetStatValue(int i, byte val)
    {
        var statName = GetStatName(i).ToString();
        if (statName == "HP") Player.PlayerData.HP = val;
        else if (statName == "AP") Player.PlayerData.AP = val;
        else if (statName == "DEF") Player.PlayerData.DEF = val;
        else Player.PlayerData.DMG = val;
    }

    private ReadOnlySpan<char> GetStatName(int i)
    {
        var dashIndex = StatPanels[i].name.LastIndexOf("-");
        ReadOnlySpan<char> name = StatPanels[i].name;
        var field = name.Slice(dashIndex + 1, StatPanels[i].name.Length - dashIndex - 1).TrimStart();
        return field;
    }

    bool IndexIsInRange(Array array, int i) => i < array.Length && i >= 0;
}
