using UnityEngine;
using System.Collections;
[CreateAssetMenu(menuName = "Abilities/Heal Ability")]
public class HealAbility : Ability
{
    public override bool LoreAvailable { get; protected set; } = true;
    private byte maxHealAmount = 0;
    private byte healAmount
    {
        get => (byte)Mathf.Min(Fight.CurrentFight.UserStats.MaxHealth - Fight.CurrentFight.UserStats.Health,
            maxHealAmount);
    }
    protected byte lastHealAmount = 0;

    protected byte turns = 3;
    protected MendingStatus Mending;
#if UNITY_EDITOR
    public HealAbility() {
        Persistent = true;
    }

    private void OnValidate()
    {
        subscribed = false;
    }
#endif

    public override string BattleDescription {
        get => $"{Owner.Stats.Name} activates the {Name} ability!";
    }
    public override void ApplyAbility(FightData owner)
    {
        base.ApplyAbility(owner);
        Mending = (MendingStatus) Instantiate(AssociatedStatus);
        Mending.ApplyStatus(Owner);
        Mending.damageDebuff = Level == 1;
        Mending.heatIncrease = Level > 1;
        Mending.decreasesHeat = Level >= 3;

        turns = 3;
        LoreAvailable = true;
    }
    public override void RemoveAbility() {
        base.RemoveAbility();
        Mending.RemoveStatus();
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
    /// Removes the ability if turns has ran out.
    /// </summary>
    public override IEnumerator DoAbilityLogicPost()
    {
        if (turns != 0) yield break;
        RemoveAbility();
    }

}


