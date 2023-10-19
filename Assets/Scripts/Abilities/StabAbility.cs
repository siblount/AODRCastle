using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Stab Ability")]
public class StabAbility : BasicAbility
{
    public override byte ApplyAbility(FightData Owner)
    {
        base.ApplyAbility(Owner);
        if (Level >= 2 && Owner is UserFightData data) data.HeatPercentage -= 0.05f;
        if (Level >= 3) Owner.Health += (byte) (Owner.MaxHealth * 0.05f);
        return Fight.CurrentFight.CalculateAbilityDamage(WorkingAttackStats, Target, false);
    }
}
