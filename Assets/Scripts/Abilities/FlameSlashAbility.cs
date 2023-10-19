using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Abilities/Flame Slash Ability")]
public class FlameSlashAbility : BasicAbility
{
    public override byte ApplyAbility(FightData Owner)
    {
        base.ApplyAbility(Owner);
        if (Level >= 2 && Owner is UserFightData) WorkingAttackStats.HeatRate += 0.05f;
        if (Level >= 3) WorkingAttackStats.Damage *= 2;
        if (Level >= 4)
        {
            WorkingAttackStats.Damage *= 4;
            WorkingAttackStats.AttackPenetration += 2;
        }
        return Fight.CurrentFight.CalculateAbilityDamage(WorkingAttackStats, Target, false);
    }
}
