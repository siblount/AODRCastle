using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearOnTouch : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject first; 

    public GameObject second;

    public GameObject third;

    public GameObject forth;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onTriggerEnter(){
first.SetActive(false);
second.SetActive(false);
third.SetActive(false);
forth.SetActive(false);

    }
}
