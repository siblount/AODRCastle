using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Trap2Script : MonoBehaviour
{
    public TMP_Text wordstext;
    //public string wordstext = wordstext1.text;
    public int state = 0;
    public Image image;
    public AudioSource soundeffects;

    public Sprite[] spriteList;
    bool played = false;

    public GameObject thiscanvas;
    // Start is called before the first frame update
   
    // Update is called once per frame
    
    public PlayerMovement pm =null;

void Start(){
    pm = FindObjectOfType<PlayerMovement>();
        if(pm != null)
        {
            pm.enabled= false;
        }
}
    void Update()
    {
        if (state == 0)
        {
            image.sprite = spriteList[0];
            wordstext.text = "You enter the room and scan around, No enemies. All of a sudden, you hear an expected drudge of scraping metal on the ground. You react and jump out of the way in time.";
        }

        if (state == 1)
        {
 
            //soundeffects.Play();
            wordstext.text = "The spike comes out and hits nothing. A lot of these traps can only be activated once before they have to be reset, which is great for you exploring.";
        }

        if(state == 2){
            pm.enabled = true;
            thiscanvas.SetActive(false);
        }
        
    }


    public void YouPressedButton()
    {
        state++;
    }
}