using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RovoEncounter3 : MonoBehaviour
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
            wordstext.text = "Rovo sees you again, this time they are completley calm.";
        }

        if (state == 1)
        {
            wordstext.text = "'I have reached my final form. This is as strong as I'll ever be. I see that you've managed to survive so far.'";
        }

        if (state == 2)
        {
            image.sprite = spriteList[1];
            wordstext.text = "'This will be our final fight. If I win, the entire operation is finished. If I lose, you'll be too drained to fight the necromancer at the end.'";
        }
        
        if (state == 3)
        {
            wordstext.text = "'Even though I have you in checkmate, it's time to fight. Engarde!'";
        }
        if(state ==4){
            pm.enabled = true;
            thiscanvas.SetActive(false);

        }


    

    }


    public void YouPressedButton()
    {
        state++;
    }
}
