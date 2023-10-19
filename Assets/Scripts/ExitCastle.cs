using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitCastle : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player_0")
        {
            SceneManager.LoadScene("OverWorldWScriptedScene");
        }
    }
}
