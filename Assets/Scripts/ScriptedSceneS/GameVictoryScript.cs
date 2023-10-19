using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class GameVictoryScript : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text wordstext;
    //public string wordstext = wordstext1.text;
    public int state = 0;
    public Image image;

    public Quest Bard, Barb, DeathSpellsman, Paladin, Marshall, Thief;

    public Sprite[] spriteList;

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
            wordstext.text = "You walk to the throne room and deactivate the defenses of the necromancer. You're hurt and tired, but finally you are victorious.";
        }

        if (state == 1)
        {
            wordstext.text = " Stormclouds dissapate overhead, mutated plants wither and zombies fall. Somehow, the mage tower that struck your ship also explodes. You're not sure what to blame that on.";
        }

        if (state == 2)
        {
            wordstext.text = "It's clear as the day above, that you have won. You barely relax for 15 minutes before a boat comes to pick you and everyone else up.";
        }
        
        if (state == 3)
        {
            int sum = 0;
            foreach (var quest in QuestDatabase.Quests.Values)
            {
                if (quest.Status == QuestStatus.Success) sum++;
            }
            if(sum<4){
                wordstext.text = "Sadness hangs over the exit party. More than half of you haven't came back from the island. This doesn't bode well at all. Everything feels... bad.";
            }else{
                wordstext.text = "Your rescuers are just as happy as your team is, as they finally get to live free of a world-ending threat. The town's best journalists ask what it was like to reach the throne and fight the necromancer in combat. You tell them it was easy.";
            }
        }

        if(state == 4){
            wordstext.text = "You left before the dawn, fought all morning and afternoon, and feast all evening. The music is good and the food is nice. Your reward is even nicer. A hefty sack of gold coins you have trouble carrying";
        }
        if(state == 5){
            wordstext.text = "You look around the room at the people you fought so hard with";
        }
        if(state == 6){
            wordstext.text = "Uburukai looks genuinely happy, not just to be alive, but also that he was on a quest to destroy one of the taboo undead creatures that his god hates. He's trying to get a statue in front of the local Qlipoth cathedral.";
        }
        if(state == 7){
            
            if(Bard.Status == QuestStatus.Success){
                wordstext.text = "The bard happilly sings to the crowd. Apparently, he was a part of the prison battalion, and is now free. You keep the fact that he didn't do anything to yourself";
            }else{
                wordstext.text = "You wonder about the bard. You don't see him around at all. Word is, since he didn't fight, he picked up another charge of desertion. One that carries years of hard labor. You hope he's able to come out alright";
            }
        }
        if(state == 8){
            if(Barb.Status == QuestStatus.Success){
                wordstext.text = "The barbarian has been arm wrestling most of the nobles & townsfolk in attendance, and absolutley beating them. No one stands a chance. You hear that she's considering joining the dukedom and getting a massive piece of land instead of getting the money.";
            }else{
                wordstext.text = "The barbarian wanted to come, however they are recovering pretty extensively in the local hospital. They're not hurt or anything, just unaturally tired.";
            }
        }
        if(state == 9){
            wordstext.text = "Rovo pretty openly and unambiguously tried to kill you, and sabotage the entire operation. They were unable to find him - he's still at large and the most wanted man within thousands of square miles. Your exploits bring more glory to your school than his brings infamy. You have a feeling you'll see him again. ";
    }
    if(state == 10){
            if(Thief.Status == QuestStatus.Success){
                wordstext.text = " The rogue slipped out of the party quickly after reciving payment, but she came back just as fast. A good conversationalist, she makes everyone laugh with her cardboard box stories.";
            }else{
                wordstext.text = "The rogue survived, and is at the party. She's still so scared that she feels like she has to stay in the box for a lot longer, it makes her feel safe somehow. She's been smuggling a lot of food down there.";
            }
        }
        if(state == 11){
            if(DeathSpellsman.Status == QuestStatus.Success){
                wordstext.text = "The death spellswordsman enjoys the atmosphere and relaxes. You would never be able to tell such a friendly person practices such reviled magic. For today though, no one really cares. Everyone's a friend.";
            }else{
                wordstext.text = "The death spellswordsman is at the party, but clearly forcing a smile. He must be in so much pain right now - apparently his magic overflowed a lot and caused lots of phyiscal damage to his body.. It's gonna take some time to get over it.";
            }
        }
        if(state == 12){
            if(Marshall.Status == QuestStatus.Success){
                wordstext.text = "The marshall struck you as a deathly serious person who doesn't know how to kick back. Your assessment was right, but after a while he started going in the festivities and relaxing. Despite the failures, winning feels good.";
            }else{
                wordstext.text = "The marshall is a grumpy guy, and he stays away from everyone else. You won, but he's still vigilant.";
            }
        }
        if(state == 13){
            if(Paladin.Status == QuestStatus.Success){
                wordstext.text = "The paladin got a huge payday, but they weren't able to enjoy it at all. Turns out his patron saint was the god of greed. Someone should've done a background check. They're in prison right now.";
            }else{
                wordstext.text = "The paladin... is at the party? You're dumbfounded at his audacity and approach him. How did he even get here without the boat and without seeing anyone on your team? He sees you and throws down a smoke bomb, vanishing.";
            }
        }
        if(state == 14){
            wordstext.text = "Despite the fighting today, the party is jubilant and you are the center of everything. They call you 'the hero of deathrock castle' and offer to let you live in wealth and glory as a bodyguard. It does seem appealing to you. This is a good place to settle down, but you don't feel that in your future. ";
        }   

        if(state == 15){
            wordstext.text = ".....";
        }

        if(state == 16){
            wordstext.text = "You awake today, at the same time that you've left. It's time to go. Thankfully, you were able to go to the local jewlers place and exchange your money for a collection of much easier to handle gems. This'll serve you nicely.";
        }
        if(state == 17){
            wordstext.text = "As you slip out, you take one last look behind you at a life you could've had. Your mind wanders, but your heart is made up. Step, after step, you go out in search of a new adventure.";
        }
        if(state == 18){
                wordstext.text = "Even if it's in one of the 10 indie cat games they showed during the playstation games conference..";
        }

        




    }


    public void YouPressedButton()
    {
        state++;
    }
}
