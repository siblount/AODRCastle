using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MeetingTextChange : MonoBehaviour
{
   // public button next;
    public TMP_Text wordstext;
    //public string wordstext = wordstext1.text;
    public int state = 0;
    public SpriteRenderer image;

    public AudioSource effectsource;
    public AudioClip clip;
    public string nextSceneName;

     public Sprite[] spriteList;
    void Start()
    {
        image.sprite = spriteList[0];
        wordstext.text = " After a brief period of waiting, an older man goes to the front. Donned in practical armor, his gait shows off his strength.";
    }

    public void YouPressedButton()
    {
        state++;
        

        
         if (state == 1)
         {
            image.sprite = spriteList[1];
             wordstext.text = " Behind him men with cuts and bruises all over their faces slowly trickle in. They keep their heads down. Most look like they can handle themselves. Your eyes are drawn to a gaunt smaller man  - the only one looking at everyone else. Only one of them isn't a fighter.";
         }

         if(state ==2){
            image.sprite = spriteList[0];
            wordstext.text = "The marshall clears his throat and begins explaining the actions of the dukedom for the past few weeks";
         }
         if(state ==3){
            wordstext.text = "'The necromancer has managed to install an extremely rare and deadly massive force death ray hours after the first battle for the castle. Thus this means all attempts to attack by air, land or sea are impossible'";
         }

         if(state ==4){
            wordstext.text = "'Only now, after a long period of not being able to do anything, the dukedom has managed to get access to a rare runeshield to protect us while we attack.'";
         }

         if(state ==5){
            image.sprite = spriteList[2];
            wordstext.text = "'The plan will go like this. We will begin by using speed magic on the boat to move extremely quickly through the water. Then, we will quickly move past the Tower - it can only get one or two shots during this time -shots that we should be protected from.'";
         }

         if(state ==6){
            wordstext.text = "'From there, we will go by the shore until we approach a broken piece of the landing area. All of the defenses here were destroyed from the last invasion, and we have intel that the inner bailey is still destroyed. '";
         }

         if(state == 7){
            wordstext.text = "'It's been a week - but you'd be suprised just how slow these zombies and skeletons work when constructing things. They haven't made any significant defenses.'";
         }

         if(state == 8){
            wordstext.text = "The marshall then goes into many more details about the plan, and before you know it he opens the floor for questions. Debates and arguments spill out, however the core tactic seems to have been set in stone.";
         }
         if(state == 9){
            wordstext.text = "With a swipe of his hands, all debate has been finished. Many people are not happy, but you don't really have a choice. You turn into an inn to sleep for the night. You're a bit anxious.";
         }

         if(state == 10){
            SceneManager.LoadScene(nextSceneName);
         }


        // else if (state == 2)
        // {

        //     wordstext.text = " The Militia leader clears his throat and then booms in a loud voice. 'The necromancer possesses a superweapon. To put it plainly, They have access to a tool we can't fight against.' The room goes silent and people look around in shock.";
        // }

        // else if (state == 3)
        // {
        //     wordstext.text = " A gnome stands up at the top of a table. Unlike what her frame would suggest, her tone is intense. 'You're late! This is all because you guys refused to attack earlier! It's been a week that we've had to live like this!'. Whispers and cheers fill the hall.";
        // }

        // else if (state == 3)
        // {
        //     wordstext.text = " 'We are in a *fragile* position. This ray has denied us the ability to siege on land, water or air. Every single one of us feels horrible about the situation - but no longer! ";
        // }


        // else if (state == 4)
        // {
        //     /// wordstext.text = " A paladin raises his hand 'Triple the pay of my troupe. We will divert the MFDR for everyone and take the most dangerous job ourselves.' ";
        //     wordstext.text = " ' A few hours ago, we have finally came in possession of a maegfolc runeshield, an artifact that can help us siege.' ";
        // }

        // else if (state == 5)
        // {
        //     wordstext.text = " The priest behind you laughs 'You're kidding me, how would that do anything? A runeshield won't protect you from a shank in the ribs'";
        // }
        // else if (state == 9)
        // {
        //     wordstext.text = " 'Right now all of the land entrances to the castle are locked down by waves of creatures. A ground attack is impossible. Our air capabilities are also lacking.";
        // }

        // else if (state == 12)
        // {
        //     wordstext.text = " 'Instead we will use the chaos to our advantage: we will travel by sea'. The room is once again alight in discussion.'";
        // }

        // else if (state == 13)
        // {
        //     wordstext.text = " In a non condescending tone, the priest manages to speak 'The necromatic energies around the tower are still excessive and intense. The seas are around 20 miles per hour and converge straight towards the shores. '";
        // }

        // else if (state == 14)
        // {
        //     wordstext.text = " 'All of us combined would be able to row the boat. The angle from the sea to the alchemists laboratory is bad for their archers. And to deal with the weapon, we have the runeshield, do we not?'";
        // }
        // else if (state == 15)
        // {
        //     wordstext.text = " The marshall looks at the crowd. 'The meeting is over. See you tomorrow'.";
        // }

        // else if (state == 16)
        // {
        //     wordstext.text = "You spend the night in a nice inn, waiting with bated breath.'";
        // } else
        // {
        //     SceneManager.LoadScene(nextSceneName);
        // }
    }
}