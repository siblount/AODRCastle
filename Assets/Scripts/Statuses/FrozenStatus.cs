using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Statuses/Frozen")]
// Main logic is in BombAbility.cs
public class FrozenStatus : Status
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


