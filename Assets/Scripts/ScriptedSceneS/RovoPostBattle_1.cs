using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RovoPostBattle_1 : MonoBehaviour
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
            wordstext.text = "Rovo falls to the ground, flabbergasted. 'What! This can't be! There's no way.... I'd lose.!'";
        }

        if (state == 1)
        {
            wordstext.text = "Rovo runs away from you. You're too winded to chase. You barely won that battle.";
        }

        if (state == 2)
        {
            image.sprite = spriteList[1];
            wordstext.text = "Directly ahead of you is the exit into the courtyard, and you know that it leads directly into the necromancers tower. Before escaping, you decide to head back to the priest.";
        }
        
        if (state == 3)
        {
            wordstext.text = "You tell Uburukai everything that transpired so far, and he is not amused. 'Betrayed by one of our own? Lets hope that no one else decides to switch sides. I don't think he sabotaged us on the boat... seems unlikely'";
        }
        if (state == 4)
        {
            wordstext.text = "'Whatever the case, I'll be joining you in the courtyard. Whoever is in the catacombs can stay there. You did find the bard and the barbarian, right? '";
        }
        if(state ==5){
            pm.enabled = true;
            thiscanvas.SetActive(false);
        }
    }


    public void YouPressedButton()
    {
        state++;
    }
}
