using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StatueCourtyard : MonoBehaviour
{
    // public button next;
    public TMP_Text wordstext;
    //public string wordstext = wordstext1.text;
    public int state = 0;
    public Image image;
    public Sprite[] spriteList;
    // Start is called before the first frame update


    public GameObject thiscanvas;
    public PlayerMovement pm =null;
    // Update is called once per frame

    private void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();

        if(pm != null)
        {
            pm.enabled= false;
        }

//WARNING CAN CAUSE ISSUES
//EXPLICITY TAKING SPRITE DATA FROM FILES
//SO IF WE MOVE THEM AROUND IT CAN CAUSE ISSUES
      //  spriteList[0] = "";
    }
    void Update()
    {
        if (state == 0)
        {
            image.sprite = spriteList[0];
            wordstext.text = "You approach the statue and instantly you feel a twinge of restlessness come over you. You can't seem to stand still.";
        }

        if (state == 1)
        {
            wordstext.text = " You recognize the statue as being one of Mercuita, the goddess of action, fires and recklessness. She is the embodiment of everything people love and hate about fire spellsword users. ";
        }

        if (state == 2)
        {
            wordstext.text = "VERY motivating, but that could be a good or a bad thing. You remember a famous anecdote. A man gets impatient and starts working on a broken clock in his house. If he breaks it more, he curses her, he fixes it he thanks her.";
        }

        if (state == 2)
        {
            wordstext.text = "With how chaotic this age is, many of her supporters have surged to positions of new power. She is mostly disliked, however. Most people don't like to see others acting reckless and crazy - especially those in power.";
        }
        if (state == 3)
        {
            wordstext.text = "It's why spellsword users are so feared and disliked... and why Rovo thought he could do better.";
        }

        if (state == 4)
        {
            wordstext.text = "You feel a fire ignite in your heart. It's time to go and get some work done.";

            
        }if(state == 5){
            if (pm != null)
            {
                pm.enabled = true;
            }
        thiscanvas.SetActive(false);    

        }

    }


    public void YouPressedButton()
    {
        state++;
    }
}