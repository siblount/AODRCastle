using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


// Update is called once per frame
public class QlipothStainedWindow : MonoBehaviour
{

    // public button next;
    public TMP_Text wordstext;
    //public string wordstext = wordstext1.text;
    public int state = 0;
    public Image image;
    public AudioSource soundeffects;

    public Sprite[] spriteList;
    // Start is called before the first frame update

    void Update()
    {
        if (state == 0)
        {
            image.sprite = spriteList[0];
            soundeffects.Play();
            wordstext.text = "You get close to the window and look directly at the portrait of Qlippoth. You remember learning about the pantheon of gods. Every single one of them are real, and maintain a careful balance of interfering with mortals and ignoring them.";
        }

        if (state == 1)
        { 
            wordstext.text = "Qlippoth is the god of life and death, the silent reaper who watches and protects human life, from birth to death. He is a very paternalistic god, lacking the fits of rage that characterize the others. ";
        }

        if (state == 2)
        {
            wordstext.text = "Legends say that he hates necromancers because he saw how one boy with a pure heart grew to be obsessed with living forever, and in doing so severed ties with his humanity.";
        }

        if (state == 3)
        {
            wordstext.text = "It's interesting how such a nice god has a follower like Uburukai. Maybe he�s got some sort of soft spot. ";
        }

    }


    public void YouPressedButton()
    {
        state++;
    }
}
