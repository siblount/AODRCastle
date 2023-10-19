using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class FinalFightScript : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text wordstext;
    //public string wordstext = wordstext1.text;
    public int state = 0;
    public Image image;

    public Sprite[] spriteList;
    bool played = false;

    public GameObject thiscanvas;
    // Start is called before the first frame update
   
    // Update is called once per frame
    
    public PlayerMovement pm =null;
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        if(pm != null)
        {
            pm.enabled= false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            image.sprite = spriteList[0];
            wordstext.text = "This is the moment you've been waiting for. To fight the necromancer at the end of the throne room.";
        }

        if (state == 1)
        {
            wordstext.text = "The looks at you with a scowl 'Mortal... You did well to get this far. This body I'm using is not good enough for me, I'm afraid. You have grown stronger past my expectations. '";
        }

        if (state == 2)
        {
            wordstext.text = "'That guy earlier, Rovo, he wanted to rule the world with me. I only have space for one firespellswordsman, so you're not going to get a chance to live.' ";
        }

        if (state == 3)
        {
            image.sprite = spriteList[1];
            wordstext.text = "At once, the necromancer explodes, sending shrapnel everywhere. You barely manage to protect your face from the blast. Your eyes twinge. ";
        }
        if (state == 4)
        {
            image.sprite = spriteList[2];
            wordstext.text = "His body is replaced, no longer zombified. Instead robofied to some form of robot crab. Wires come off his body as he stands before you. ";
            
        }

        if(state == 5){
            wordstext.text = "'This world belongs to Idauzar the Animated!' ";

        }


        if(state ==6){
            pm.enabled = true;
            thiscanvas.SetActive(false);
        }


    

    }


    public void YouPressedButton()
    {
        state++;
    }
}
