using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// <inheritdoc/> <br/>
/// <b>Use this class specifically for the player's character. </b>
/// </summary>
[CreateAssetMenu(menuName = "Battle Stats/User")]
public class UserStats : Stats
{
    /// <summary>
    /// Determines the rate of the heat generation for each basic attack.
    /// </summary>
    [Range(0, 1)] public float HeatRate;
    /// <summary>
    /// The basic abilities available to the character.
    /// </summary>
    public List<BasicAbility> BasicAbilities;

    /// <inheritdoc cref="Stats.Reset()"/>
    protected new void Reset()
    {
        base.Reset();
        HeatRate = 0.01f;
        BasicAbilities.Clear();
    }
}
