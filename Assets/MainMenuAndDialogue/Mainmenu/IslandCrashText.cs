using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class IslandCrashText : MonoBehaviour
{
   // public button next;
    public TMP_Text wordstext;
    //public string wordstext = wordstext1.text;
    public int state = 0;
    public Image image;
    // Start is called before the first frame update
    public Sprite[] spriteList;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            image.sprite = spriteList[0];
            wordstext.text = "One hour before sunup, you hear a knock on your door.";
        }

        if (state == 1)
        {
            wordstext.text = "It's time to go.";
        }
        if (state == 2)
        {
            wordstext.text = " You come to the ship, alongside everyone else, and take your position. The shield is already installed.";
        }

        if (state == 3)
        {   
            image.sprite = spriteList[4];
            wordstext.text = "You see it glowing a bright blue hue, although something seems off about it. Oh well, there's gotta be someone who looked over it and made sure it was working alright.";
        }

        if (state == 4)
        {
            image.sprite = spriteList[0];
            wordstext.text = "The air is cold. The sky is dark. Your group is uneasy. ";
        }

        if (state == 5)
        {
            wordstext.text = "The anchor is lifted. 30 minutes until the most extreme fighting in your life.";
        }

        if (state == 6)
        {
            wordstext.text = "Your crewmates deal with their anxities in different ways.'";
        }

        if (state == 7)
        {
            wordstext.text = "Some drink coffee. Some drink other things. Some are praying";
        }

        if (state == 8)
        {
            /// wordstext.text = " A paladin raises his hand 'Triple the pay of my troupe. We will divert the MFDR for everyone and take the most dangerous job ourselves.' ";
            wordstext.text = "'By the power vested in me, even if we lose, I wish to make the cycles of life and death be set right again.' You hear spoken from an old, disgruntled voice.";
        }

        if (state == 9)
        {
            wordstext.text = "All thats left to do is wait., and the mood is heavy.";
        }
        if (state == 10)
        {
            wordstext.text = "";
        }

        if (state == 11)
        {
            image.sprite = spriteList[1];
            wordstext.text = "Right as daylight approaches, you see the castle in the distance.";
        }

        if (state == 12)
        {
            wordstext.text = "The sky feels hopeful, but you can't help but feel like your stomach is being knotted with the anxiety you feel.  ";
        }

        if (state == 13)
        {
            wordstext.text = " Your boat draws closer. Everyone is tensed. All of a sudden -";
        }

        if (state == 14)
        {
            image.sprite  = spriteList[2];
            wordstext.text = "The skies begin to blacken, almost instantly. The waves start to crash and rain starts to fall";
        }

        if (state == 15)
        {
            wordstext.text = " 'Basic weather manipuation magic. Nothing to be scared about. They'ved noticed us though.'";
        }

        if (state == 16)
        {
            wordstext.text = " The skies start shooting lighting. The waves start to grow.";
        }
        if (state == 17)
        {
            wordstext.text = " At once, panic sets in. The Marshall takes control of the situation. 'All hands on deck!.'";
        }

        if (state == 18)
        {
            wordstext.text = "All of your crewmates grab their paddles and begin helping the ship row.";
        }

        if (state == 19)
        {
            wordstext.text = "Rowing, rowing, the entire ship comes to a pandemonium of screaming, rowing and water as the storm rages overhead ";
        }

        if (state == 18)
        {
            wordstext.text = "Sweat forms on your brow as you rowing, slowly, over time, you notice something and your blood runs cold";
        }

        if (state == 19)
        {
            wordstext.text = "All this rowing, and you haven't moved an inch.'";
        }

        if (state == 20)
        {
            wordstext.text = "'We're sitting ducks!'";
        }
        if (state == 21)
        {
            wordstext.text = "'COMPOSE YOURSELF! We still have the shield!'";
        }

        if (state == 22)
        {
            image.sprite = spriteList[3];
            wordstext.text = "In the distance, the mage tower begins charging up";
        }

        if (state == 23)
        {
            wordstext.text = "'Theres no way they can power this storm and the tower at the same time. Brace for impact!' ";
        }

        if (state == 24)
        {
            wordstext.text = "All at once, your crew drops their oars and hits the deck. You hear a whirring noise.... but it's coming from nearby";
        }

        if (state == 25)
        {
            image.sprite = spriteList[4];
            wordstext.text = "Your eyes rotate to the source of the noise, the shield you and your crew are betting their lives on.";
        }
        if (state == 26)
        {
            image.sprite = spriteList[5];
            wordstext.text = " All of a sudden.... You hear a massive crack. The shield has failed.";
        }

        if (state == 27)
        {
            wordstext.text = "You barely have time to process the destruction of the shield before you are thrown overboard.";
        }

        if (state == 28)
        {
            wordstext.text = "Screams are all you know before you hit the water and instantly become unconscious";
        }

        if (state == 29)
        {
            wordstext.text = "...........";
        }

        if (state == 30)
        {
            image.sprite = spriteList[6];
            wordstext.text = "When you come to, you are on a deserted beach.";
        }

        if (state == 31)
        {
            wordstext.text = "Your entire body aches, it's gonna take some time before you feel normal and back to your usual strength.";
        }

        if(state == 32){
            wordstext.text = "It's time to fight";
        }

        if(state == 33){
            wordstext.text = "Gathering your senses, you decide your best course of action is to swim into the nearby catacombs. It seems like the most hidden place right now.";
        }
        if(state > 33){
            SceneManager.LoadScene("Assets/Scenes/CatacombsWScriptedScenesAndSTuff.unity");
        }
        ////OVERWORLD CHANGE HERE! OVERWORLD SHOULD APPEAR! OVERWORLD SHOULD APPEAR  AND BE COOL. 



    }


    public void YouPressedButton()  
    {
        state++;
    }
}