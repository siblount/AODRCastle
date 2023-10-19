using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Trap1Script : MonoBehaviour
{
    // public button next;
    public TMP_Text wordstext;
    //public string wordstext = wordstext1.text;
    public int state = 0;
    public Image image;
    public AudioSource soundeffects;
    public AudioClip sound;

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
        if (soundeffects != null)
            GameSettings.SFXSources.Add(soundeffects);
    }

    private void OnDestroy()
    {
        GameSettings.SFXSources.Remove(soundeffects);
    }
    void Update()
    {
        if (state == 0)
        {
            image.sprite = spriteList[0];
            wordstext.text = "You enter the room, and scan around. All of a sudden, you hear a massive SCHWINK and scraping metal.";
        }

        if (state == 1)
        {
            wordstext.color = Color.red;
            if(played == false){
            soundeffects.PlayOneShot(sound, .5f);
            played =  true;
            }
            wordstext.text = "Before you can react you feel a hot flash of pain around your thigh and you fall to the ground. You barely managed to avoid the hidden spike trap under the floor ";
        }

        if (state == 2)
        {
            wordstext.text = "You barely avoided losing your leg. You only have a minor slash. You can still fight.";
        }

        if(state ==3){
            pm.enabled = true;
            thiscanvas.SetActive(false);
        }
        
    }


    public void YouPressedButton()
    {
        state++;
    }
}