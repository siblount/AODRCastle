using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Abilities/Basic Ability")]
public class BasicAbility : Ability
{
    public override bool LoreAvailable { get; protected set; } = false;
    public bool FlameAbility = false;

    public override string BattleDescription => throw new System.NotImplementedException();

    public new virtual byte ApplyAbility(FightData Owner)
    {
        base.ApplyAbility(Owner);
        
        Fight.CurrentFight.InvokeAbilityAttack(this);
        WorkingAttackStats.Damage += (byte)(Owner.Stats.DMG / 2f); 
        WorkingAttackStats.AttackPenetration += (byte)(Owner.Stats.AP / 2f);
        Fight.CurrentFight.UserStats.HeatPercentage += WorkingAttackStats.HeatRate;
        RemoveAbility();
        return Fight.CurrentFight.CalculateAbilityDamage(WorkingAttackStats, Target, false);
    }

    public override IEnumerator DoAbilityLogicPost() => throw new System.NotImplementedException();

    public override IEnumerator DoAbilityLogicPre() => throw new System.NotImplementedException();
}
