using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class RovoEncounter2Script : MonoBehaviour
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
            wordstext.text = "Rovo stands before you again, this time he looks a little different.";
        }

        if (state == 1)
        {
            wordstext.text = "'You got lucky last fight! Thankfully, I have access to some new power. Feel my wrath!'";
        }

        if (state == 2)
        {
            image.sprite = spriteList[1];
            wordstext.text = "Once again, the two of you clash. Fire starts to spread to around the grass.";
        }
        
        if (state == 3)
        {
            wordstext.text = "'I have no idea why you guys are so adamant about stopping us when there's a traitor in your ranks'";
        }
        if(state ==4){
            wordstext.text = "'Someone planted the bomb - and that was when I knew it was a bad idea to work with amateurs'.";
        }if(state ==5){
            wordstext.text = "Your swords clash and release a strong explosion, sending the two of you back. It's time to fight!";
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
