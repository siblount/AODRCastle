using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// This ensures the script attached to the gameobject must have a boxcollider
// and will add one for you if not already attached and will prevent it from being removed.
[RequireComponent(typeof(BoxCollider2D))]
public class ExampleBattleScript : MonoBehaviour
{
    // This is the stats of the enemy (since this script is attached to an enemy).
    [SerializeField]

    private Stats EnemyStats;
    // For dimenstration purposes, just set it up in the unity editor.
    [SerializeField]
    private Sprite background;
    // For dimenstration purposes, just set it up in the unity editor.
    [SerializeField]
    private UserStats Stats;
    // For dimnestraton purposes, it is set up in the unity editor.
    // In production, we can have a static GameSettings object that determines
    // this value.
    [SerializeField]
    private FightSettings FightSettings;
    [SerializeField]
    [Range(0, 69)] private byte WinSkillPointsReward = 3;

    private GameObject player;


    /// <summary>
    /// This function is called when the player runs into the character with this script 
    /// attached (the enemy).
    /// </summary>
    /// <param name="collision">The collison data.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Starting fight...RAWR!!!");


            // Start the fight.
#if false
            var fight = Fight.StartFight(background: background, user: Stats, enemy: EnemyStats, fightSettings: FightSettings);
#else 
            var fight = Fight.StartFight(background: background, user: Player.PlayerData, enemy: EnemyStats, fightSettings: GameSettings.Difficulty);
#endif


            // Make sure you add logic on the player to stop accepting input.
            // Disable the player and all of it's attached scripts.
            player = collision.collider.gameObject;
            player.SetActive(false);
            // Call the function when the fight has ended.
            fight.FightEnded += Fight_FightEnded;

            // Disable the object we are attached to; which hides the object from the view,
            // disables the collider to prevent OnCollisionEnter2D calls, which ultimately
            // prevents starting another fight.
            gameObject.SetActive(false);
        }
    }

    private void Fight_FightEnded()
    {

        if (Fight.CurrentFight.PlayerWonFight)
        {
            player.SetActive(true);
            Player.SkillPoints += WinSkillPointsReward;
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
