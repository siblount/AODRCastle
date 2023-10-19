using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Basic Attack Ability")]
public class BasicAttackAbility : BasicAbility
{
    [Header("Only change below value: ")]
    [Range(0, 1)] public float HeatRateIncrease = 0.03f;
    public override byte ApplyAbility(FightData Owner)
    {
        this.Owner = Owner;
        Owner.ActiveAbilities.Add(this);
        WorkingAttackStats = new AbilityAttackStats
        {
            AttackPenetration = Owner.Stats.AP,
            Damage = Owner.Stats.DMG,
            HeatRate = HeatRateIncrease
        };
        Fight.CurrentFight.InvokeAbilityAttack(this);
        Fight.CurrentFight.UserStats.HeatPercentage += WorkingAttackStats.HeatRate;
        RemoveAbility();
        return Fight.CurrentFight.CalculateAbilityDamage(WorkingAttackStats, Target, false);
    }
}
