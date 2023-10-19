using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomExit : MonoBehaviour
{
    public Transform exitBedroom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player_0")
        {
            other.transform.position = new Vector2(exitBedroom.position.x, exitBedroom.position.y);
        }
    }
}
