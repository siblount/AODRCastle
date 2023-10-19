using UnityEngine;
using System.Text;
[CreateAssetMenu(menuName ="Wearable/Flame Repelling Paper Wearable")]
public class FlameRepellingPaperWearable : Wearable
{   
    public override void ApplyWearable(FightData Owner)
    {
        base.ApplyWearable(Owner);
        Fight.CurrentFight.PostAttack += CurrentFight_PostAttack;
    }

    private void CurrentFight_PostAttack(FightData obj)
    {
        if (obj != Owner) return;
        foreach (var stat in obj.ActiveStatuses)
        {
            if (stat is KindeledStatus)
            {
                FightUI.Instance.AddDialogueLine($"{Owner.Stats.Name} is no longer kindled due to mysterious forces.");
                stat.RemoveStatus();
                return;
            }
        }
    }

    public override void RemoveWearable()
    {
        base.RemoveWearable();
        Fight.CurrentFight.PostAttack -= CurrentFight_PostAttack;
    }
}
