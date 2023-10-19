using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GiantEnemyScriptedScene : MonoBehaviour
{
    // public button next;
    public TMP_Text wordstext;
    //public string wordstext = wordstext1.text;
    public int state = 0;
    public Image image;
    public AudioSource soundeffects;

    public Sprite[] spriteList;
    // Start is called before the first frame update


    public QuestDatabase qq;
    public Quest UQuest;
    public bool metUchukowazi = false;

    public GameObject ExitButton;
    bool played = false;
    public AudioClip sound;


    void Start() {
        if (UQuest.Status == QuestStatus.Success) {
            metUchukowazi = true;
        }
        ExitButton.SetActive(false);
        if (soundeffects != null)
            GameSettings.SFXSources.Add(soundeffects);
    }

    void OnDestroy() {
        GameSettings.SFXSources.Remove(soundeffects);
    }
    

    void Update()
    {
        if (state == 0)
        {
            image.sprite = spriteList[0];
            if(played == false){
            soundeffects.PlayOneShot(sound, .5f);
            played =  true;
            }
            wordstext.text = "You walk across the courtyard and see a massive, hulking golem guarding a treasure chest. In between its rock armor, the core energy it uses grows unstable and overbearing. It has noticed you, but it won't move from its spot.";
        }

        if (state == 1)
        {
            ExitButton.SetActive(true);
            wordstext.text = " It's strong. You know for a fact that you can't defeat this alone. In fact, most of your martial companions wouldn't be able to help you. Someone versed in the school of death would help tremendously here. ";
        }

        if (state == 2 & metUchukowazi == false)
        {
            wordstext.text = " Throwing caution to the wind, you rush towards the golem. You pepper it with fireballs, however they don't do any damage. In fact, none of your magic seems to be working. ";
            played = false;
        }

        if (state == 3 & metUchukowazi == false)
        {
            ExitButton.SetActive(false);
            wordstext.color = Color.red;
            if(played == false){
            soundeffects.PlayOneShot(sound, .5f);
            played =  true;
            }
            wordstext.text = " The ground around you starts to crumble and quake. A hill grows behind you. You have no way to escape! You have to fight. ";
        }
        if (state == 4 & metUchukowazi == false)
        {
            wordstext.text = " You hold your ground and try to attack with your blade - all of your strikes bounce off the golems armor! Very suddenly, it picks you up. ";
        }
        if (state == 5 & metUchukowazi == false)
        {
            wordstext.color = Color.red;
            wordstext.text = " The world goes black before you can scream.";
        }
        if(state == 6 & metUchukowazi == false){
            SceneManager.LoadScene("MainMenu");
        }


        if (state == 2 & metUchukowazi == true)
        { 
            wordstext.text = " Uchukowazi looks at you and nods his head. 'This one's special. It's impervious to all of your attacks. I can weaken it, you have to finish it'. You nod your head and Uchukowazi raises his hands";
        }
        if (state == 3 & metUchukowazi == true)
        { 
            wordstext.text = "'But that box.... it HAS to be Fiora. I'd bet money on it. She'll just be more help - but you'll have to finish it! Let's go!'";
        }
        
        if (state == 4 & metUchukowazi == true)
        {
            image.sprite = spriteList[1];
            wordstext.text = " At once the plant life around the golem starts to wither and die. The energies that shone so brightly inside the golem are now rapidly failing.";       
        }


        if (state == 5 & metUchukowazi == true)
        { 
            //TODO ADD FIRORA SPRITE
            wordstext.text = " A dozen daggers at once all explode. The golem, which once stood so tall, now starts to falter";
        }

        if (state == 6 & metUchukowazi == true)
        {
            image.sprite = spriteList[1];
            wordstext.text = " Now! Attack!";
        }


    }


    public void YouPressedButton()
    {
        state++;
    }

    public void leaveButton()
    {

        //Destroy(transform.root.gameObject);
        state = 9000;
        // // state = 9000;
       wordstext.text = " Time to leave. You can't face this - at least not now, and especially not alone ";
         if(state > 9000){
             Destroy(transform.root.gameObject);
             }
    }
}