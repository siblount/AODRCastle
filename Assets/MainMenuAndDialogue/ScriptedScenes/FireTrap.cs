using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FireTrap : MonoBehaviour
{
    // public button next;
    public TMP_Text wordstext;
    //public string wordstext = wordstext1.text;
    public int state = 0;
    public Image image;
    public AudioSource soundeffects;

    public Sprite[] spriteList;
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
            wordstext.text = "As you walk along, you trip over an invisible line on the ground. From both sides, you are instantly hit by flamethrowers. The magic spews and spews - and at once stops.";
        }

        if (state == 1)
        {
 
            
            wordstext.text = "You are completely unaffected - not even your clothes are impacted. You shrug and continue onwards. ";
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