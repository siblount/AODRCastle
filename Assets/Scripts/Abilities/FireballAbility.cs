using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/Fireball Ability")]
public class FireballAbility : Ability
{
    public override bool LoreAvailable { get; protected set; } = true;
    public bool TargettingTarget = true;
    public FireballAbility()
    {
        Persistent = false;
    }
    public override string BattleDescription
    {
        get => $"{Owner.Stats.Name} uses the {Name} ability!";
    }
    public override void ApplyAbility(FightData Owner)
    {
        base.ApplyAbility(Owner);
        LoreAvailable = true;
        WorkingAttackStats = CreateInstance<AbilityAttackStats>();
        WorkingAttackStats.Damage = (byte)(Level == 3 ? 30 : 15);
        WorkingAttackStats.HeatRate = AttackStats.HeatRate;
        if (Level == 1) WorkingAttackStats.AttackPenetration = 2;
        else if (Level == 2) WorkingAttackStats.AttackPenetration = 4;
        else WorkingAttackStats.AttackPenetration = 1;
    }

    /// <summary>
    /// Main fireball logic
    /// </summary>
    public override IEnumerator DoAbilityLogicPre()
    {
        var previousWorkingAttackStats = Instantiate(WorkingAttackStats);
        Fight.CurrentFight.InvokeAbilityAttack(this);
        var typeWriter = FightUI.Instance.DialogueChat.GetComponent<Typewriter>();
        var dmg = Fight.CurrentFight.CalculateAbilityDamage(WorkingAttackStats, Target, false);
        Target.Health -= dmg;
        Fight.CurrentFight.UserStats.HeatPercentage += WorkingAttackStats.HeatRate;

        FightUI.Instance.AddDialogueLine($"{Owner.Stats.Name}'s fireball deals {dmg} damage to {Target.Stats.Name}.");
        yield return typeWriter.TypeWriterEnumerator;
        FightUI.Instance.UpdateHealths();
        yield return Fight.CurrentFight.EndGameIfNoHealth();
        if (Fight.CurrentFight.Ended) yield break;
        // Switch to the previous working attack stats to fix GUARD for nullifying damage for both target & owner.
        WorkingAttackStats = previousWorkingAttackStats;
        TargettingTarget = false;
        Fight.CurrentFight.InvokeAbilityAttack(this);
        dmg = Fight.CurrentFight.CalculateAbilityDamage(WorkingAttackStats, Owner, true);
        Owner.Health -= dmg;
        FightUI.Instance.AddDialogueLine($"{Owner.Stats.Name}'s fireball also deals {dmg} damage to themselves.");
        yield return typeWriter.TypeWriterEnumerator;
        FightUI.Instance.UpdateHealths();
        yield return Fight.CurrentFight.EndGameIfNoHealth();
        if (Fight.CurrentFight.Ended) yield break;

        //yield return typeWriter.TypeWriterEnumerator;
        RemoveAbility();
    }

    /// <summary>
    /// Removes the ability if turns has ran out.
    /// </summary>
    public override IEnumerator DoAbilityLogicPost()
    {
        yield break;
    }
}
