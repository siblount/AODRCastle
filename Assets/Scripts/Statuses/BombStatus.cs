using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Statuses/Bomb")]
// Main logic is in BombAbility.cs
public class BombStatus : Status
{
    public override bool LoreAvailable { get; protected set; } = false;

    public override IEnumerator DoStatusLogicPost()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator DoStatusLogicPre()
    {
        throw new System.NotImplementedException();
    }
}


