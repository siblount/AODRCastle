using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleGiveWearable : MonoBehaviour
{
    public Wearable wearable;
    private bool chestOpened;
    public Color chestAlreadyOpenedColor;
    private SpriteRenderer render;
    private bool inTriggerRange;

    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        chestOpened = false;
    }

    private void Update()
    {
        if (!chestOpened && inTriggerRange && Input.GetKeyDown(KeyCode.E))
        {
            Player.PlayerData.Wearables.Add(wearable);
            chestOpened = true;
            render.color = chestAlreadyOpenedColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inTriggerRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inTriggerRange = false;
    }

}
