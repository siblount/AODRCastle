using System.Collections;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/Bomb Ability")]
public class BombAbility : Ability
{
    public override bool LoreAvailable { get; protected set; } = true;
    public bool TargettingTarget = true;
    public BombAbility()
    {
        Persistent = true;
    }
    protected byte turns = 3;
    BombStatus status;

    public void OnEnable()
    {
        hideFlags = HideFlags.None;
    }

    public override string BattleDescription
    {
        get => $"{Owner.Stats.Name} uses the {Name} ability!";
    }
    public override void ApplyAbility(FightData owner)
    {
        base.ApplyAbility(owner);
        
        status = (BombStatus) Instantiate(AssociatedStatus);
        status.ApplyStatus(Target);
        turns = 4;
        LoreAvailable = true;
    }

    public override void RemoveAbility()
    {
        base.RemoveAbility();
        status.RemoveStatus();
    }

    /// <summary>
    /// Main healing logic
    /// </summary>
    public override IEnumerator DoAbilityLogicPre()
    {
        turns--;
        yield break;
        
    }

    /// <summary>
    /// Bomb explodes!
    /// </summary>
    public override IEnumerator DoAbilityLogicPost()
    {
        if (turns > 0) yield break;
        var previousWorkingAttackStats = Instantiate(WorkingAttackStats);
        Fight.CurrentFight.InvokeAbilityAttack(this);
        var typeWriter = FightUI.Instance.DialogueChat.GetComponent<Typewriter>();
        var dmg = Fight.CurrentFight.CalculateAbilityDamage(WorkingAttackStats, Target, false);
        //Fight.CurrentFight.InvokeLateAbilityAttack(this);
        Target.Health -= dmg;
        if (Target is not UserFightData)
            Fight.CurrentFight.UserStats.HeatPercentage += WorkingAttackStats.HeatRate;

        FightUI.Instance.AddDialogueLine($"{Owner.Stats.Name}'s bomb EXPLODES, dealing {dmg} damage to {Target.Stats.Name}.");
        yield return typeWriter.TypeWriterEnumerator;
        FightUI.Instance.UpdateHealths();
        yield return Fight.CurrentFight.EndGameIfNoHealth();
        if (Fight.CurrentFight.Ended) yield break;
        // Switch to the previous working attack stats to fix GUARD for nullifying damage for both target & owner.
        WorkingAttackStats = previousWorkingAttackStats;
        TargettingTarget = false;
        Fight.CurrentFight.InvokeAbilityAttack(this);
        dmg = Fight.CurrentFight.CalculateAbilityDamage(WorkingAttackStats, Target, false);
        Owner.Health -= dmg;
        FightUI.Instance.AddDialogueLine($"{Owner.Stats.Name}'s bomb also deals {dmg} damage to themselves.");
        yield return typeWriter.TypeWriterEnumerator;
        yield return Fight.CurrentFight.EndGameIfNoHealth();
        if (Fight.CurrentFight.Ended) yield break;
        //yield return typeWriter.TypeWriterEnumerator;
        FightUI.Instance.UpdateHealths();
        RemoveAbility();
    }
}
