using UnityEngine;
using System;
using System.Collections;

#pragma warning disable 8632
public abstract class Ability : ScriptableObject
{
    public string NameRaw;
    public string Name
    {
        get
        {
            var i = NameRaw.IndexOf(':');
            return i == -1 ? NameRaw : NameRaw[(i+1)..];
        }
    }
    public string Description;
    [Range(1,4)]
    public byte Level = 1;
    // Determines if the ability is persistent or not.
    // No logical difference, just for documentation purposes.
    public bool Persistent = false;
    /// <summary>
    /// Determine if the ability is false or not.
    /// </summary>
    public bool AttackAbility = false;
    /// <summary>
    /// Tells whether the ability provide lore.
    /// </summary>
    public abstract bool LoreAvailable { get; protected set; }

    /// <summary>
    /// The description to use when the ability has been activated.
    /// </summary>
    public abstract string BattleDescription { get; }
    /// <summary>
    /// Holds the upgrade descriptions for each upgradable level.
    /// </summary>
    public string[] UpgradeDescriptionsPerLevel;
    /// <summary>
    /// Gets the associated status to use.
    /// </summary>
    public Status AssociatedStatus;
    /// <summary>
    /// Returns the stats of the attack, if any. <para/>
    /// Check if <see cref="AttackAbility"/> is <see langword="true"/> 
    /// or not before seeking this value. <para/>
    /// It will be <see langword="null"/> if <see cref="AttackAbility"/>
    /// is <see langword="false"/>.
    /// </summary>
    public AbilityAttackStats? AttackStats = null;
    /// <summary>
    /// A copy of <see cref="AttackStats"/>. <b>Do not use outside of 
    /// the ability.</b>
    /// </summary>
    [HideInInspector] public AbilityAttackStats? WorkingAttackStats = null;
    /// <summary>
    /// Describes who is the owner of the ability - in other words, who does this
    /// ability apply to? You can check if it's the user by checking if
    /// <code>Owner == Fight.CurrentFight.UserStats</code>
    /// It will return <see langword="true"/> if it is, otherwise <see langword="false"/>
    /// </summary>
    
    public FightData Owner;
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
    protected bool subscribed;

    /// <summary>
    /// Enables and sets up the ability for appropriate use. 
    /// </summary>
    public virtual void ApplyAbility(FightData Owner)
    {
        if (subscribed) return;
        subscribed = true;
        this.Owner = Owner;
        Owner.ActiveAbilities.Add(this);
        WorkingAttackStats = (AttackStats is null) ? null : Instantiate(AttackStats);
        if (Persistent) Owner.AvailableAbilityNames.Remove(NameRaw);
    }
    /// <summary>
    /// Disables and removes the ability.
    /// </summary>
    public virtual void RemoveAbility()
    {
        Owner.ActiveAbilities.Remove(this);
        LoreAvailable = false;
        if (Persistent) Owner.AvailableAbilityNames.Add(NameRaw);

    }
    /// <summary>
    /// The logic of the ability to be called BEFORE the attack is announced (and after 
    /// base attack logic if this ability is persistent).
    /// </summary>
    /// <returns>An IEnumerator that can be used for controlling the UI and flow of logic.</returns>
    public abstract IEnumerator DoAbilityLogicPre();

    /// <summary>
    /// The logic of the ability to be called AFTER the attack is announced.
    /// </summary>
    /// <returns>An IEnumerator that can be used for controlling the UI and flow of logic.</returns>
    public abstract IEnumerator DoAbilityLogicPost();

    private void OnEnable()
    {
        hideFlags = HideFlags.None;
    }

}


