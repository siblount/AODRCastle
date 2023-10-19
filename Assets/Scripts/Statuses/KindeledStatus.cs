using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Statuses/Kindeled")]
public class KindeledStatus : Status
{
    [Range(0, 10)] public float DamageMultiplier = 2;
    [Range(0, 10)] public byte PenetrationDecrease = 0;

    public override bool LoreAvailable { get; protected set; } = false;


    public override void ApplyStatus(FightData data)
    {
        Owner = data;
        Target.ActiveStatuses.Add(this);
        FightUI.Instance.UpdateStatuses();
        Fight.CurrentFight.AbilityAttack += OnAbilityAttack;
    }

    private void OnAbilityAttack(Ability ability)
    {
        // Do nothing if not a flame ability or if we got an attack from ourselves.
        if (ability is not FireballAbility && ability is not BasicAbility) return;
        if (ability is BasicAbility basicAbility && basicAbility.FlameAbility && Owner != ability.Owner) return;
        ability.WorkingAttackStats.Damage = (byte)Mathf.FloorToInt(ability.WorkingAttackStats.Damage * DamageMultiplier);
        ability.WorkingAttackStats.AttackPenetration -= PenetrationDecrease;
    }

    public override IEnumerator DoStatusLogicPost()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator DoStatusLogicPre()
    {
        throw new System.NotImplementedException();
    }

    public override void RemoveStatus()
    {
        Target.ActiveStatuses.Remove(this);
        LoreAvailable = false;
        FightUI.Instance.UpdateStatuses();
        Fight.CurrentFight.AbilityAttack -= OnAbilityAttack;
    }
}


