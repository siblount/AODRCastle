using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/Kindling Ability")]
public class KindlingAbility : Ability
{
    public override bool LoreAvailable { get; protected set; } = true;
    public KindlingAbility() : base()
    {
        Persistent = true;
    }
    protected byte turns = 3;
    protected AbilityAttackStats workingAttackStats;
    KindeledStatus kindling;

#if UNITY_EDITOR
    private void OnValidate()
    {
        subscribed = false;
    }
#endif

    public override string BattleDescription
    {
        get => $"{Owner.Stats.Name} activates the {Name} ability!";
    }
    public override void ApplyAbility(FightData Owner)
    {

        base.ApplyAbility(Owner);
        kindling = (KindeledStatus) Instantiate(AssociatedStatus);
        if (Level >= 2) kindling.PenetrationDecrease += 2;
        kindling.DamageMultiplier = Level == 3 ? 3 : 1.5f;
        kindling.ApplyStatus(Owner);
        // 4 turns because on the first time applied, it will use a turn.
        turns = (byte)(Level == 3 ? 2 : 4);
    }

    public override void RemoveAbility()
    {
        base.RemoveAbility();
        kindling.RemoveStatus();
    }

    /// <summary>
    /// Decrements turns used.
    /// </summary>
    public override IEnumerator DoAbilityLogicPre()
    {
        turns--;
        yield break;
    }

    /// <summary>
    /// Removes the ability if turns has ran out.
    /// </summary>
    public override IEnumerator DoAbilityLogicPost()
    {
        if (turns <= 0) RemoveAbility();
        yield break;
    }
}
