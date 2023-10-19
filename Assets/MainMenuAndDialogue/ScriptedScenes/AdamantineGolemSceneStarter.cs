using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamantineGolemSceneStarter : MonoBehaviour
{
    public Canvas runthis;
    public bool isCurrentlyColliding;

    public BoxCollider2D cc;

    public GiantEnemyScriptedScene gg = null;

   public Canvas CreatedObject = null;
    public PlayerMovement pm = null;


    public int exampleState = 0;


    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();

        if (pm != null)
        {
            pm.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        pm.enabled = false;
        isCurrentlyColliding = true;
        if (CreatedObject != null)
        {
            CreatedObject.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        isCurrentlyColliding = false; 
       CreatedObject.enabled = false;
    }



    void Update()
    {
        if(CreatedObject != null & gg ==null){
           
                       gg = GameObject.FindAnyObjectByType<GiantEnemyScriptedScene>();
        }

        if (isCurrentlyColliding && CreatedObject == null)
        {
            CreatedObject = Instantiate(runthis);
            gg = CreatedObject.GetComponentInChildren<GiantEnemyScriptedScene>();
            if(gg = null){
                gg= GameObject.FindObjectOfType<GiantEnemyScriptedScene>(); 
                if(gg = null){
                    gg = GameObject.FindAnyObjectByType<GiantEnemyScriptedScene>();
                }
            }
            
        }

        if (gg != null)
        {
            exampleState = gg.state;
            if (gg.state > 100)
            {
                pm.enabled = true;
                gg.state = 0;
            }
        }
        if (gg != null)
        {
            if (gg.state >= 7)
            {
                pm.enabled = true;
                CreatedObject.enabled = false;
            }
        }

    }




}
