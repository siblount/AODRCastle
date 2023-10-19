using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Heat Ability")]
public class HeatAbility : Ability
{
    public override bool LoreAvailable { get; protected set; } = false;
    public HeatAbility()
    {
        Persistent = true;
    }
    private UserFightData lastChangedUserStats;
    private FightData lastChangedEnemyStats;
    private RageStatus rageStatus;
    [Header("Heat Effect Activation Percentages: ")]

    private float Level1 = 0.2f;
    private float Level2 = 0.4f;
    private float Level3 = 0.6f;
    private float Level4 = 0.8f;

    private void OnEnable() => hideFlags = HideFlags.None;
    private void UpdateHeatEffects(float oldValue, float newValue)
    {
        UndoStatChanges();
        UndoStatuses();
        lastChangedUserStats ??= new UserFightData(Instantiate((UserStats) Owner.Stats), Fight.CurrentFight); 
        lastChangedEnemyStats ??= new FightData(Instantiate(Target.Stats), Fight.CurrentFight);
        ZeroOut();
        if (newValue >= Level1 && newValue < Level3)
            rageStatus.ApplyStatus(Owner);
        if (newValue >= Level2)
            Target.Stats.DMG += lastChangedEnemyStats.Stats.DMG = 2;
        else if (newValue >= Level3) Owner.DamageMultipler += lastChangedUserStats.DamageMultipler = 0.2f;
        else if (newValue >= Level4) Target.DamageMultipler += lastChangedEnemyStats.DamageMultipler = Mathf.Abs(0.8f - newValue);

    }

    private void ZeroOut()
    {
        lastChangedEnemyStats.Stats.DMG = 0;
        lastChangedEnemyStats.DamageMultipler = 0;
        lastChangedUserStats.DamageMultipler = 0;
    }

    private void UndoStatChanges()
    {
        if (lastChangedUserStats is null) return;
        Target.Stats.DMG -= lastChangedEnemyStats.Stats.DMG;
        Target.DamageMultipler -= lastChangedEnemyStats.DamageMultipler;
        Owner.DamageMultipler -= lastChangedUserStats.DamageMultipler;
    }

    private void UndoStatuses()
    {
        if (Owner.ActiveStatuses.Contains(rageStatus))
            rageStatus.RemoveStatus();
    }

    public override string BattleDescription => string.Empty;
    public override void ApplyAbility(FightData _)
    {
        base.ApplyAbility(Fight.CurrentFight.UserStats);
        rageStatus = (RageStatus) Instantiate(AssociatedStatus);
        Fight.CurrentFight.UserStats.HeatPercentageChanged += UpdateHeatEffects;
    }

    public override void RemoveAbility()
    {
        base.RemoveAbility();
        Fight.CurrentFight.UserStats.HeatPercentageChanged -= UpdateHeatEffects;
        UndoStatChanges();
    }

    public override IEnumerator DoAbilityLogicPre() => throw new System.NotImplementedException();
    public override IEnumerator DoAbilityLogicPost() => throw new System.NotImplementedException();
}
