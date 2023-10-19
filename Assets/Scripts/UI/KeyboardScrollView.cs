using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class KeyboardScrollView : MonoBehaviour
{
    public TMP_Text[] Items;
    public string SelectedText=> Items[i].text[0..^4];
    public string SelectedTextRaw => Items[i].text;
    public TMP_Text SelectedItem => Items[i];
    byte i = 0;
    public Scrollbar vScrollbar;
    VerticalLayoutGroup vlg;
    internal IList<string> SourceItems;
    internal byte CurrentIndex { get => (byte)(i + j); }
    byte safeSourceItemsPtr
    {
        get => j;
        set
        {
            if (value < 0) j = 0;
            if (value > 0 && j + Items.Length >= SourceItems.Count)
            {
                j = value > j ? 
                    Convert.ToByte(SourceItems.Count - Items.Length) 
                    : value;

            }
            else j = value;
        }
    }
    byte j = 0;
    byte visibleCount = 0;

    private void Awake()
    {
        Init();
        InitView();
    }

    private void OnEnable() => InitView();

    private protected void Init()
    {
        Items = transform.GetComponentsInChildren<TMP_Text>(true);
        vlg = GetComponent<VerticalLayoutGroup>();
    }


    internal void InitView()
    {
        vlg.enabled = false;
        i = j = 0;
        var z = 0;
        UpdateTextsRaw();
        UpdateScrollbar();
        HighlightSelectedItem();
        visibleCount = (byte)Math.Min(Items.Length, SourceItems.Count);
        for (; z < visibleCount; z++)
            Items[z].gameObject.SetActive(true);
        for (; z < Items.Length; z++)
            Items[z].gameObject.SetActive(false);
    }

    private void UpdateTextsRaw()
    {
        for (var i = 0; i < Math.Min(Items.Length, SourceItems.Count); i++)
        {
            Items[i].text = SourceItems[j + i];
            Items[i].color = Color.white;
        }
    }

    private void UpdateScrollbar()
    {
        // Set the position of the scrollbar handle out of 1.
        vScrollbar.value = 1 - (j / (float)(SourceItems.Count - Items.Length));
        vScrollbar.size = (float) Items.Length / SourceItems.Count;
    }

    internal void Select(bool up)
    {
        ClearLastHighlight();
        var dir = up ? 1 : -1;
        var nextI = dir + i;
        if (nextI < 0 && j != i)
        {
            safeSourceItemsPtr--;
            UpdateScrollbar();
        }
        else if (j != i && nextI >= Items.Length)
        {
            safeSourceItemsPtr++;
            UpdateScrollbar();
        }
        else i = Convert.ToByte(Math.Clamp(i + dir, 0, visibleCount - 1));
        UpdateTextsRaw();
        HighlightSelectedItem();
    }

    private void ClearLastHighlight() => Items[i].fontStyle = FontStyles.Normal;


    private void HighlightSelectedItem()
    {
        TMP_Text toTxt = Items[i];
        toTxt.color = Color.blue;
        toTxt.fontStyle = FontStyles.Bold;
        toTxt.text += " <--";
    }
}
