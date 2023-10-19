using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class QuestKeyboardScrollView : MonoBehaviour
{
    public TMP_Text[] ActiveQuests;
    public TMP_Text[] CompletedQuests;
    internal NotifyList<string> ActiveQuestsSource { get => activeQuestsSource; }
    private readonly NotifyList<string> activeQuestsSource = new(0);
    internal NotifyList<string> CompletedQuestsSource { get => completedQuestsSource; }
    private readonly NotifyList<string> completedQuestsSource = new(0);

    [SerializeField] private TMP_Text ActiveQuestsHeaderTxt;
    [SerializeField] private TMP_Text CompletedQuestsHeaderTxt;

    /// <summary>
    /// The items that have been set up in the Unity Editor.
    /// These items will NEVER be added or removed for scroll optimization.
    /// </summary>
    public TMP_Text[] Items;
    /// <summary>
    /// Gets the text of the selected item minus the "<--".
    /// </summary>
    public string SelectedText => Items[items_index].text[0..^4];
    /// <summary>
    /// Returns the raw text of the selected item, including the "<--".
    /// </summary>
    public string SelectedTextRaw => Items[items_index].text;
    /// <summary>
    /// Returns the selected item object.
    /// </summary>
    public TMP_Text SelectedItem => Items[items_index];
    Scrollbar vScrollbar;
    VerticalLayoutGroup verticalLayoutGroup;
    /// <summary>
    /// Represents the active quests and completed quests from it's source arrays.
    /// </summary>
    private int QuestCount => ActiveQuestsSource.Count + CompletedQuestsSource.Count;
    /// <summary>
    /// Total count represents the number of active & completed quests + 2 (headers).
    /// </summary>
    public int TotalCount => QuestCount + 2;
    protected byte safeSourceItemsPtr
    {
        get => scrolled_index;
        set
        {
            if (value < 0) scrolled_index = 0;
            if (value > 0 && scrolled_index + Items.Length >= TotalCount)
            {
                scrolled_index = value > scrolled_index ?
                    Convert.ToByte(TotalCount - Items.Length)
                    : value;

            }
            else scrolled_index = value;
        }
    }
    /// <summary>
    /// The true index of the source item.
    /// </summary>
    internal byte CurrentIndex { get => (byte)(items_index + scrolled_index); }


    /// <summary>
    /// The index to use for the <see cref="Items"/> list.
    /// </summary>
    protected byte items_index = 0;
    /// <summary>
    /// Represents the 'off-screen' index. For example, if there are 4 objects on screen and we scroll down
    /// 'off-screen' will become 1. Do it again, it will become 2 (assuming there is capacity).
    /// </summary>
    protected byte scrolled_index = 0;
    protected byte visibleCount = 0;
    private bool initialized = false;
    

    public void Awake()
    {
        if (!initialized) Init();
        InitView();
    }
    private void OnEnable()
    {
        if (!initialized) Init();
        InitView();
    }
    private void OnDisable()
    {
        ActiveQuestsSource.ListChanged -= RefreshContents;
        CompletedQuestsSource.ListChanged -= RefreshContents;
    }

    private void LateUpdate()
    {
        //if (verticalLayoutGroup.enabled) 
        //    verticalLayoutGroup.enabled = false;
    }
    /// <summary>
    /// This function is called when 
    /// </summary>
    private void RefreshContents()
    {
        items_index = scrolled_index = 0;
        UpdateContents();
        Select(true);
    }

    private void UpdateContents()
    {
        OrganizeItems();
        UpdateScrollbar();
        HighlightSelectedItem();
        visibleCount = (byte)Math.Min(Items.Length, TotalCount);
    }

    public void Select(bool up)
    {
        // If there are no active quests or completed quest, we are done.
        if (ActiveQuestsSource.Count == 0 && CompletedQuestsSource.Count == 0) return;
        var dir = up ? 1 : -1;
        var nextI = dir + items_index;
        // If the next item is a header then skip it or it is out of range...
        if (isHeader(Items[nextI]))
        {
            var r = findNextNonHeader(nextI, dir);
            if (r == -1) return;
            nextI = r;
        }
        // This logic will handle if it is out of bounds or not.
        if (nextI < 0 && scrolled_index != items_index)
            safeSourceItemsPtr--;
        else if (scrolled_index != items_index && nextI >= Items.Length)
            safeSourceItemsPtr++;
        else items_index = Convert.ToByte(Math.Clamp(nextI, 0, visibleCount - 1));
        UpdateContents();

    }

    public void Select()
    {
        // If there are no active quests or completed quest, we are done.
        if (ActiveQuestsSource.Count == 0 && CompletedQuestsSource.Count == 0) return;
        const int nextI = 0;
        // If the next item is a header then skip it or it is out of range...
        if (isHeader(Items[nextI]))
        {
            var r = findNextNonHeader(nextI, 1);
            if (r == -1) return;
        }
        UpdateContents();

    }

    private int findNextNonHeader(int start, int dir)
    {
        if (visibleCount == 2) return -1;
        bool f(TMP_Text item) => !isHeader(item) && item.isActiveAndEnabled;
        int e;
        if (dir > 0)
        {
            e = Array.FindIndex(Items, start, f);
            if (e == -1) e = Array.FindIndex(Items, 0, start, f);
        } else
        {
            e = Array.FindLastIndex(Items, start, f);
            //e = Array.FindLastIndex(Items, Items.Length - 1, start, f);
            if (e == -1) e = Array.FindIndex(Items, start, f);
        }
        return e;
    }
    /// <summary>
    /// Returns whether the TMP_Text is a header or not.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private bool isHeader(TMP_Text t) => t == ActiveQuestsHeaderTxt || t == CompletedQuestsHeaderTxt;

    private protected void Init()
    {
        initialized = true;
        verticalLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>(true);
        verticalLayoutGroup.enabled = false;
        Items = transform.GetComponentsInChildren<TMP_Text>(true);
        vScrollbar = GetComponentInChildren<Scrollbar>();
        TMP_Text header;
        foreach (var item in Items)
        {
            if (ActiveQuestsHeaderTxt != null && CompletedQuestsHeaderTxt != null) break;
            if (item.name.StartsWith("Active"))
                header = ActiveQuestsHeaderTxt = item;
            else if (item.name.StartsWith("Completed"))
                header = CompletedQuestsHeaderTxt = item;
            else continue;
            header.gameObject.SetActive(false);
        }
        // Make the headers the first elements of the items.
        Swap(Items, 0, Array.IndexOf(Items, ActiveQuestsHeaderTxt));
        Swap(Items, Items.Length - 1, Array.IndexOf(Items, CompletedQuestsHeaderTxt));

    }

    private void Swap(object[] array, int a, int b)
    {
        var t = array[a];
        array[a] = array[b];
        array[b] = t;
    }


    protected void InitView()
    {
        OnDisable();

        if (ActiveQuestsSource is not null)
            ActiveQuestsSource.ListChanged += RefreshContents;
        if (CompletedQuestsSource is not null)
            CompletedQuestsSource.ListChanged += RefreshContents;
        RefreshContents();

    }

    private void OrganizeItems()
    {
        var cqi = Array.IndexOf(Items, CompletedQuestsHeaderTxt);
        Swap(Items, Items.Length - 1, cqi); // Ensure CompletedQuestsHeader is last element.
        cqi = Items.Length - 1;
        int reduction = 2;
        int index = 0;
        // If the active quests header is not view, add the extra "normal" item.
        // Otherwise, set it active.
        bool headerCheck = scrolled_index != 0;
        if (headerCheck) reduction--;
        Items[index++].gameObject.SetActive(!headerCheck);
        // If the completed quests header is not view, add the extra "normal" item.
        if (Array.IndexOf(Items, CompletedQuestsHeaderTxt) > Items.Length - 1) reduction--;
        for (int j = scrolled_index; j < activeQuestsSource.Count && index < Items.Length - reduction; index++, j++)
        {
            EnableItem(index);
            if (isHeader(Items[index])) continue;
            UpdateTextItem(activeQuestsSource, j, index);
        }
        // If there is room for the completed quest header, swap it to the next positoon.
        // and set is active. Otherwise, add the extra "normal" item.
        headerCheck = index < Items.Length - reduction;
        if (headerCheck)
        {
            Swap(Items, cqi, index);
            EnableItem(index++);
        }
        else reduction--;
        cqi = index - Math.Max(0, scrolled_index - Items.Length);
        // Select completed quests length to be ordered below completed quests header.
        for (int j = index - cqi - scrolled_index; j < completedQuestsSource.Count && index < Items.Length - reduction; index++, j++)
        {
            EnableItem(index);
            if (isHeader(Items[index])) continue;
            UpdateTextItem(completedQuestsSource, j, index);
        }
        // Everything else we set as inactive.
        for (; index < Items.Length; index++) Items[index].gameObject.SetActive(false);
        // Trigger the vertical layout group to calculate the positioning and sizing of text.
        verticalLayoutGroup.enabled = true;
        //// Then disable it because there is no need for it anymore.
        //verticalLayoutGroup.enabled = false;
    }

    private void UpdateTextItem(NotifyList<string> source, int j, int index)
    {
        Items[index].color = Color.white;
        Items[index].text = source[j];
        Items[index].fontStyle = FontStyles.Normal;
    }

    private void EnableItem(int index)
    {
        Items[index].gameObject.SetActive(true);
        Items[index].transform.SetSiblingIndex(index);
    }
    private void UpdateScrollbar()
    {
        // Set the position of the scrollbar handle out of 1.
        vScrollbar.value = 1 - (scrolled_index / (float)(TotalCount - Items.Length));
        vScrollbar.size = (float)Items.Length / TotalCount;
        vScrollbar.gameObject.SetActive(vScrollbar.size != 1); // ALT: value
    }

    private void HighlightSelectedItem()
    {
        if (isHeader(Items[items_index])) return;
        TMP_Text toTxt = Items[items_index];
        toTxt.color = Color.blue;
        toTxt.fontStyle = FontStyles.Bold;
        toTxt.text += " <--";
    }
}
