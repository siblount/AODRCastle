using System.Collections;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/Freeze Ability")]

// NOTE: Only the user can have this ability.
// NO ENEMIES SHOULD HAVE THIS ABILITY.
public class FreezeAbility : Ability
{
    public override bool LoreAvailable { get; protected set; } = true;
    public FreezeAbility()
    {
        Persistent = true;
    }
    FrozenStatus status;
    private byte ArmorAddition = 5;

    public override string BattleDescription
    {
        get => $"{Owner.Stats.Name} uses the {Name} ability!";
    }
    public override void ApplyAbility(FightData owner)
    {
        base.ApplyAbility(owner);

        status = (FrozenStatus)Instantiate(AssociatedStatus);
        status.ApplyStatus(Owner);
        LoreAvailable = true;
    }

    public override void RemoveAbility()
    {
        base.RemoveAbility();
        status.RemoveStatus();
        Fight.CurrentFight.EndAttack -= CurrentFight_EndAttack;
    }

    /// <summary>
    /// Main healing logic
    /// </summary>
    public override IEnumerator DoAbilityLogicPre()
    {
        var typeWriter = FightUI.Instance.DialogueChat.GetComponent<Typewriter>();

        //LoreAvailable = false; // prevent calling us multiple times.
        Fight.CurrentFight.UserStats.HeatPercentage = 0;
        FightUI.Instance.AddDialogueLine($"{Owner.Stats.Name}'s is FROZEN and cannot move!");
        yield return typeWriter.TypeWriterEnumerator;
        for (int i = 0; i < 2; i++)
        {
            if (Fight.CurrentFight.Ended) yield break;
            if (i == 0)
            {
                Fight.CurrentFight.UserStats.Stats.DEF += ArmorAddition;
                yield return Fight.CurrentFight.DoEnemyLogic();
                Fight.CurrentFight.UserStats.Stats.DEF -= ArmorAddition;
                continue;
            }
            yield return Fight.CurrentFight.DoEnemyLogic();
        }
        LoreAvailable = false;
        Fight.CurrentFight.EndAttack += CurrentFight_EndAttack;
        // Note that after this function, the 

    }

    private void CurrentFight_EndAttack(FightData obj)
    {
        if (obj != Owner) return;
        FightUI.Instance.AddDialogueLine($"{Owner.Stats.Name} has been THAWED.");
        RemoveAbility();
    }

    /// <summary>
    /// Bomb explodes!
    /// </summary>
    public override IEnumerator DoAbilityLogicPost() { yield break; }
}
