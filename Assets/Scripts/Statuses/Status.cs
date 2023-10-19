using UnityEngine;
using System.Collections;

public abstract class Status : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite StatusImage;
    /// <summary>
    /// The stats applied to.
    /// </summary>
    protected FightData Owner;
    /// <summary>
    /// Tells whether the ability provide lore.
    /// </summary>
    public abstract bool LoreAvailable { get; protected set; }
    /// <summary>
    /// Determines whether a status should remain regardless of 
    /// </summary>
    public bool Permanent = false;
    protected FightData Target
    {
        get
        {
            if (target is null)
                return target = (Owner is UserFightData) ? 
                    Fight.CurrentFight.EnemyStats : Fight.CurrentFight.UserStats;
            return target;
        }
        set => target = value;
    }
    private FightData target;

    /// <summary>
    /// Applies the status effect to <paramref name="who"/>.
    /// </summary>
    /// <param name="who">The status effect to apply to.</param>
    public virtual void ApplyStatus(FightData who)
    {
        Owner = who;
        who.ActiveStatuses.Add(this);
        FightUI.Instance?.UpdateStatuses();
    }
    /// <summary>
    /// Removes the status effect.
    /// </summary>
    public virtual void RemoveStatus()
    {
        if (Permanent) return;
        Owner.ActiveStatuses.Remove(this);
        LoreAvailable = false;
        FightUI.Instance.UpdateStatuses();
    }

    /// <summary>
    /// The logic of the status to be called BEFORE the attack is announced (and after 
    /// base attack logic if this ability is persistent).
    /// </summary>
    /// <returns>An IEnumerator that can be used for controlling the UI and flow of logic.</returns>
    public abstract IEnumerator DoStatusLogicPre();

    /// <summary>
    /// The logic of the status to be called AFTER the attack is announced.
    /// </summary>
    /// <returns>An IEnumerator that can be used for controlling the UI and flow of logic.</returns>
    public abstract IEnumerator DoStatusLogicPost();
    private void OnEnable()
    {
        hideFlags = HideFlags.None;
    }
}


