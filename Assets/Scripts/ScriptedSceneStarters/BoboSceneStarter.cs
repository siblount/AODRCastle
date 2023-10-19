using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoboSceneStarter : MonoBehaviour
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

    void OnTriggerEnter2D(Collider2D col)
    {
        isCurrentlyColliding = true;
        
    }

    void OnTriggerExit2D(Collider2D col){
        isCurrentlyColliding = false;
        
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
