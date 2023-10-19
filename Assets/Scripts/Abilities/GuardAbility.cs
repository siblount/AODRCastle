using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "Abilities/Guard Ability")]
public class GuardAbility : Ability
{
    public override bool LoreAvailable { get; protected set; } = true;
    private byte ArmorIncrease;
    private byte previousMinimumDamage = 0;
    private float previousDamageMultiplier = 0;
#if UNITY_EDITOR
    public GuardAbility() {
        Persistent = true;
    }

    private void OnValidate()
    {
        subscribed = false;
    }
#endif

    public override string BattleDescription {
        get => $"{Owner.Stats.Name} GUARDS themselves for the next attack!";
    }
    public override void ApplyAbility(FightData owner)
    {
        base.ApplyAbility(owner);
        LoreAvailable = false;
        Owner.Stats.DEF += 4;
        Fight.CurrentFight.EndAttack += CurrentFight_EarlyAttack;
        if (Level >= 2 && Owner is UserFightData) ((UserFightData)Owner).HeatPercentage += 0.1f;
        if (Level >= 3 && Owner is UserFightData) ((UserFightData)Owner).HeatPercentage -= 0.2f;
        if (Level >= 4)
        {
            previousMinimumDamage = Fight.CurrentFight.FightSettings.MinimumDamage;
            Fight.CurrentFight.FightSettings.MinimumDamage = 0;
            previousDamageMultiplier = Target.DamageMultipler;
            Target.DamageMultipler = 0;
            Fight.CurrentFight.AbilityAttack += CurrentFight_AbilityAttack;
        }
    }

    public override void RemoveAbility()
    {
        base.RemoveAbility();
        Fight.CurrentFight.EndAttack -= CurrentFight_EarlyAttack;
        Owner.Stats.DEF -= 4;
        if (Level < 3) return;
        Fight.CurrentFight.FightSettings.MinimumDamage = previousMinimumDamage;
        Target.DamageMultipler = previousDamageMultiplier;
        Fight.CurrentFight.AbilityAttack -= CurrentFight_AbilityAttack;
    }

    /// <summary>
    /// Main guard logic
    /// </summary>
    public override IEnumerator DoAbilityLogicPre()
    {
        yield break;
    }

    // This ensures that abilities such as the Bomb will still be guarded.
    private void CurrentFight_EarlyAttack(FightData _) => RemoveAbility();
    private void CurrentFight_AbilityAttack(Ability obj)
    {
        if (obj.Owner == Owner) return;
        // DO NOT SET STATS TO ZERO IF FIREBALL OR BOMB IS ATTACKING THEMSELVES.
        else if (obj is FireballAbility fireballAbility && !fireballAbility.TargettingTarget)
            return;
        else if (obj is BombAbility bombAbility && !bombAbility.TargettingTarget)
            return;
        if (obj.WorkingAttackStats)
            obj.WorkingAttackStats.Damage = 0;
    }

    public override IEnumerator DoAbilityLogicPost()
    {
        yield break;
    }

}


