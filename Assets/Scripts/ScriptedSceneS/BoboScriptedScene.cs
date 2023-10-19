using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoboScriptedScene : MonoBehaviour
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

    public PlayerMovement pm = null;

    public GameObject bobo = null;
    public Quest PaladinQuest;

    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        if (pm != null)
        {
            pm.enabled = false;
        }
        bobo = GameObject.Find("Bobo");
    }

    public void YouPressedButton()
    {
        state++;
        if (state == 0)
        {
            image.sprite = spriteList[0];
            wordstext.text = "The paladin you heard Uburukai talk about is right in front of you.";
        }

        if (state == 1)
        {
            wordstext.text = "He finally looks at you, and has a smile on his face.";
        }

        if (state == 2)
        {
            wordstext.text = "'Oh hey! We thought you were injured! Glad to see you're okay!'";
        }

        if (state == 3)
        {
            wordstext.text = "'I'm going to have to leave in a bit before the final battle, there's some stuff I've got to do. See ya!'";
        }
        if (state == 4)
        {
            if (PaladinQuest.Status != QuestStatus.Success) state = 100;
            else wordstext.text = "The paladin begins to channel a weak gravity spell. You recognize this and cast a counterspell easily. You're not going to let him get away.";
        }

        if (state == 5)
        {
            image.sprite = spriteList[1];
            wordstext.text = "The paladin grits his teeth at you denying him the escape. 'Hey, guess it's as good a time as ever to come clean'";
        }

        if (state == 6)
        {
            wordstext.text = "'I was the one who planted the bomb on the ship that disabled the shield. Sorry, I was paid to do so by the necromancer. I'm going to collect my payment now, and then help in the final battle.'";
        }

        if (state == 7)
        {
            wordstext.text = "'I was the one who planted the bomb on the ship that disabled the shield. Sorry, I was paid to do so by the necromancer. I'm going to collect my payment now, and then help in the final battle.'";
        }
        if (state == 8)
        {
            wordstext.text = "'Anyway, that was all the magic I had left to escape.'";
        }

        if (state == 9)
        {
            wordstext.text = "You put the cuffs on the paladin as he clicks his tongue. 'Well, this is annoying'. He won't be going anywhere.";
        }
        if (state == 10)
        {
            pm.enabled = true;
            thiscanvas.SetActive(false);
        }

        if (state == 100)
        {
            wordstext.color = Color.red;
            wordstext.text = "Before you can react, the paladin is surrounded by runes, forcing you away from him. He dissapears out of the room. No matter what you try, there's no way to track him.";
        }

        if (state == 101)
        {
            pm.enabled = true;
            thiscanvas.SetActive(false);
            bobo.SetActive(false);

        }
    }
}