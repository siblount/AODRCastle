using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class StatueCourtYardScriptedSceneStarter : MonoBehaviour
{
    public Canvas runthis;
   public bool isCurrentlyColliding;
   int runonce = 0;
    public BoxCollider2D cc;


    // Start is called before the first frame update
    void Start()
    {
        runonce = 0;
    }

    
    void OnCollisionEnter(Collision col)
    {
        isCurrentlyColliding = true;
    }

    void OnCollisionExit(Collision col)
    {
        isCurrentlyColliding = false;
    }

   void OnTriggerEnter2D(Collider2D col)
    {
        isCurrentlyColliding = true;
    }

    void Update()
    {
        if (isCurrentlyColliding && runonce ==0) //&& Input.GetKeyUp(KeyCode.Z))
        {
            runonce++;
            Instantiate(runthis);
        }
    }
}
