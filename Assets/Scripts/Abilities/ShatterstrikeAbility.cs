using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/Shatterstrike Ability")]
public class ShatterstrikeAbility : Ability
{
    public override bool LoreAvailable { get; protected set; } = true;
    public ShatterstrikeAbility() : base()
    {
        Persistent = true;
    }
    protected byte turns = 2;
    ShatteredStatus shattered;

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
        shattered = (ShatteredStatus) Instantiate(AssociatedStatus);
        shattered.ApplyStatus((Owner is UserFightData) ? 
            Fight.CurrentFight.EnemyStats : Fight.CurrentFight.UserStats);
        turns = 2;
    }

    public override void RemoveAbility()
    {
        base.RemoveAbility();
        shattered.RemoveStatus();
    }

    /// <summary>
    /// Decrements turns used.
    /// </summary>
    public override IEnumerator DoAbilityLogicPre()
    {
        turns--;
        if (turns != 1) yield break;
        Fight.CurrentFight.InvokeAbilityAttack(this);
        var typeWriter = FightUI.Instance.DialogueChat.GetComponent<Typewriter>();
        var target = (Owner is UserFightData) ? Fight.CurrentFight.EnemyStats : Fight.CurrentFight.UserStats;
        var dmg = Fight.CurrentFight.CalculateAbilityDamage(WorkingAttackStats, target, false);

        target.Health -= dmg;
        Fight.CurrentFight.UserStats.HeatPercentage += WorkingAttackStats.HeatRate;
        FightUI.Instance.AddDialogueLine($"{Owner.Stats.Name} shatter strikes dealing {dmg} damage to {target.Stats.Name}.");
        FightUI.Instance.UpdateHealths();
        yield return typeWriter.TypeWriterEnumerator;
        yield return Fight.CurrentFight.EndGameIfNoHealth();
        if (Fight.CurrentFight.Ended) yield break;
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
