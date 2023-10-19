using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroTextChange : MonoBehaviour
{
   // public button next;
    public TMP_Text wordstext;
    //public string wordstext = wordstext1.text;
    public int state = 0;
    public Image image;
    public AudioSource soundeffects;

    public Sprite[] spriteList;

    public string nextSceneName;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            image.sprite = spriteList[0]; 
            wordstext.text = "The land of Xuweth used to be a land of peace. Not anymore. Time, civil wars, invasions and political machinations weakened this land and placed it within crisis.";
        }

        if (state == 1)
        {
            wordstext.text = "You are a flame spellsword user. You combine the advanced and technical sword play of a warrior with the magical powers of the wizard to create a melee art that gives you a fearsome reputation.";
        }
        if (state == 2)
        {
            wordstext.text = "In these troubling times you have found and completed many work contracts around the land. You do your job well, however events outside of your control forced you to travel far from where you are known. You come to a mid-sized town where no one knows you or your exploits.";
        }

        if (state == 3)
        {
            wordstext.text = "Today is a different day, and as you approach the local dukes manor, you can feel the anxiety in the air. They hate spellswords way more than people do in the capital, and people stare you down in the street.";
        }

        if (state == 4)
        {
            image.sprite = spriteList[1];
            wordstext.text = "Precisely one week ago, the evil necromancer Idauzar was defeated in the brutal siege of Deathrock castle, smited by the pure priests of Qlipoth.";
        }

        if (state == 5)
        {
            wordstext.text = "His flesh was destroyed, however his spirit managed to live on and possess one of the warriors as a massive burst of corruption spread through the halls, forcing the militia to retreat.";
        }

        if (state == 6)
        {
            wordstext.text = "The castle was lost to the undead invader, and there are no hostages he has kept alive. Within hours the castle was filled with disgusting bugs, zombies and other unsavory creatures.";
        }

        if (state == 7)
        {
            wordstext.text = "A week later, the only response the duke could muster was a recruitment call for strong warriors to take back the area... and a large bounty on the necromancers head.";
        }

        if (state == 8)
        {
            //soundeffects.Play();
            image.sprite = spriteList[2];
            wordstext.text = "You enter the meeting hall, and you see multiple people sitting and looking morose. Most people here look like they're not made for intense combat, to put it lightly. Ill-fitting scavenged armor, rusty weapons and awkward movement for most people you see. ";
        }

        if (state == 9)
        {
            wordstext.text = "The air is tense. A life-changing amount of money is promised. For some of those here, it looks like this is going to be their first actual combat encounter. ";
        }

       if(state == 10)
        {
            SceneManager.LoadScene(nextSceneName);   
        }



    }


    public void YouPressedButton()
    {
        state++;
    }
}