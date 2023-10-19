using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Statuses/Mending")]
public class MendingStatus : Status
{
    public override bool LoreAvailable { get; protected set; } = true;
    private byte maxHealAmount = 0;
    private byte healAmount
    {
        get => (byte)Mathf.Min(Owner.MaxHealth - Owner.Health, maxHealAmount);
    }
    protected byte lastHealAmount = 0;

    public bool heatIncrease = true; // level 2 
    public bool damageDebuff = true; // level 3
    public bool decreasesHeat = false; // level 4
    public override void ApplyStatus(FightData data)
    {
        base.ApplyStatus(data);
        maxHealAmount = (byte)Mathf.FloorToInt(Owner.MaxHealth * 0.2f);
        if (damageDebuff)
            Owner.DamageMultipler -= 0.4f;
        if (!heatIncrease && Owner == Fight.CurrentFight.UserStats)
            Fight.CurrentFight.User.HeatRate = 0;
        Fight.CurrentFight.AbilityAttack += CurrentFight_AbilityAttack;
    }

    public override void RemoveStatus()
    {
        base.RemoveStatus();
        Fight.CurrentFight.AbilityAttack -= CurrentFight_AbilityAttack;
    }

    private void CurrentFight_AbilityAttack(Ability obj)
    {
        if (!heatIncrease && Owner == Fight.CurrentFight.UserStats)
            obj.WorkingAttackStats.HeatRate = 0;
    }

    public override IEnumerator DoStatusLogicPost()
    {
        yield break;
    }

    public override IEnumerator DoStatusLogicPre()
    {
        lastHealAmount = healAmount;
        Owner.Health += lastHealAmount;
        var typeWriter = FightUI.Instance.DialogueChat.GetComponent<Typewriter>();
        FightUI.Instance.AddDialogueLine($"{Owner.Stats.Name}'s health was increased by {lastHealAmount}.");
        yield return typeWriter.TypeWriterEnumerator;
        FightUI.Instance.UpdateHealths();

        if (!decreasesHeat && Owner is not UserFightData) yield break;
        Fight.CurrentFight.UserStats.HeatPercentage -= Mathf.Min(Fight.CurrentFight.UserStats.HeatPercentage, 0.2f);
        //FightUI.Instance.AddDialogueLine($"{Fight.CurrentFight.User.Name}'s heat was reduced.");
        yield return typeWriter.TypeWriterEnumerator;
    }

}


