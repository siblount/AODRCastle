using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class RovoEncounter1 : MonoBehaviour
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
            wordstext.text = "As you approach the exit of the catacombs, a man stands before you. His robe is slightly tattered, and you can see cuts and bruising on his face. He moves confidently towards you.";
        }

        if (state == 1)
        {
            wordstext.text = "'My names Rovo. I'm a spellsword user just like you. We're of the same element too. Sadly, you're not going to be making it out of here alive.' ";
        }

        if (state == 2)
        {
            wordstext.text = "'This necromancer, he’s got some good ideas. A lot of 'em. I  beelined it to the grand hall and we had a good talk. He made some good points.'";
        }
        
        if (state == 3)
        {
            wordstext.text = "'All we have to do is keep this castle and the dukedom will implode on its own. We just need to flush out invaders - like you..'";
        }
        if(state ==4){
            image.sprite = spriteList[1];
            wordstext.text = "Your weapons clash in the darkness of the catacombs, your strikes lighting up the room.";
        }if(state ==5){
            wordstext.text = "There is no turning back.";
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
