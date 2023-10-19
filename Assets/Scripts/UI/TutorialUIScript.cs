using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIScript : MonoBehaviour
{
    public GameObject thisUI;
    public PlayerMovement pm;
    // Start is called before the first frame update
    void Start()
    {
        pm.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickButton(){
        pm.enabled = true;
        thisUI.SetActive(false);

    }
}
