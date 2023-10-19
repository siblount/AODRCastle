using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable 8632
public class Fight : MonoBehaviour
{
    public static Fight CurrentFight;
    public Sprite Background;
    public UserStats User;
    public Stats Enemy;

    private GameObject FightUICanvasPrefab;

    /// <summary>
    /// This event will be raised AFTER an attack function ends.
    /// </summary>
    public event Action<FightData> EndAttack;
    /// <summary>
    /// This event will be raised BEFORE the attack is announced but AFTER the base calculations have been made.
    /// </summary>
    public event Action<FightData> PreAttack;
    /// <summary>
    /// This event will be raised AFTER the attack calculations are made and attack announcement has been made.
    /// </summary>
    public event Action<FightData> PostAttack;
    /// <summary>
    /// This event will be raised BEFORE ability attack calculations.
    /// </summary>
    public event Action<Ability> AbilityAttack;
    /// <summary>
    /// This event will be raised when the battle has ended.
    /// </summary>
    public event Action FightEnded;
    [SerializeReference] public FightSettings FightSettings;

    public FightData EnemyStats;
    public UserFightData UserStats;

    public bool Ended;
    public bool PlayerWonFight
    {
        get
        {
            if (!Ended) throw new InvalidOperationException("Do not check if the player has won if the game has not ended. " +
                "Use `Fight.CurrentFight.Ended` to see if the fight has ended or not.");
            return EnemyStats.Health == 0;
        }
    }
    public AbilityDatabase AbilityDatabase;
    public StatusDatabase StatusDatabase;

    // Used for tracking which attack call is first call or not.
    public byte recursiveCall = 0;
    private void Awake()
    {
        CurrentFight = this;
        #if UNITY_EDITOR
        if (!SceneManager.GetActiveScene().name.StartsWith("Battle")) return;
        User = Instantiate(User);
        Enemy = Instantiate(Enemy);
        UserStats = new UserFightData(User, this);
        EnemyStats = new FightData(Enemy, this);
        FightUICanvasPrefab = GameObject.Find("Canvas");

#endif
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (CurrentFight != this) return;
        // TODO: Load async.
        //AbilityDatabase = (AbilityDatabase) Resources.Load("Ability Database");
        StatusDatabase = (StatusDatabase)Resources.Load("Status Database");
        
#if UNITY_EDITOR
        if (!SceneManager.GetActiveScene().name.StartsWith("Battle")) return;
        FightUICanvasPrefab.transform.SetSiblingIndex(0);
        Instantiate(AbilityDatabase.StringToAbility["Heat"]).ApplyAbility(UserStats);
        foreach (var status in Enemy.InitialStatuses)
            Instantiate(status).ApplyStatus(EnemyStats);
        FightUI.Instance.InitializeFight();

#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !Ended)
        {
            // Force win.
            EnemyStats.Health = 0;
            EndGameEarly();
        } else if (Input.GetKeyDown(KeyCode.P) && !Ended)
        {
            // Force lose.
            UserStats.Health = 0;
            EndGameEarly();
        }
    }

    private void EndGameEarly()
    {
        StopAllCoroutines();
        if (!FightUI.Instance.ChatPanel.activeInHierarchy)
            FightUI.Instance.ShowDialogue("Game over forced!");
        StartCoroutine(EndGameIfNoHealth());
    }

    private void OnDestroy()
    {
        CurrentFight = null;
    }

    /// <summary>
    /// Prepares the fight and switches to the scene. DO NOT USE INSIDE OF 
    /// A BATTLE SCENE, THE FIGHT WILL AUTOMATICALLY START!
    /// </summary>
    /// <param name="background">The background for the UI of the fight to use.</param>
    /// <param name="user">The player's data</param>
    /// <param name="enemy">The enemy's data.</param>
    /// <param name="fightSettings">The settings for the fight to use.</param>
    public static Fight StartFight(Sprite background, UserStats user, Stats enemy, 
        FightSettings fightSettings)
    {
        var fight = new GameObject("Fight", typeof(Fight)).GetComponent<Fight>();
        fight.FightSettings = GameSettings.Instance == null ? Instantiate(fightSettings) :
            Instantiate(GameSettings.Difficulty);

        fight.Background = background;
        fight.User = Instantiate(user);


        fight.UserStats = new UserFightData(fight.User, fight);
        fight.Enemy = Instantiate(enemy);
        fight.EnemyStats = new FightData(fight.Enemy, fight);
        fight.FightUICanvasPrefab = Instantiate((GameObject)Resources.Load("BattlePrefab"));
        fight.FightUICanvasPrefab.transform.SetSiblingIndex(0);

        // Make sure current wearable is applied.
        if (fight.User.CurrentWearable != null) Instantiate(fight.User.CurrentWearable).ApplyWearable(fight.UserStats);
        if (fight.Enemy.CurrentWearable != null) Instantiate(fight.Enemy.CurrentWearable).ApplyWearable(fight.EnemyStats);

        Instantiate(AbilityDatabase.StringToAbility["Heat"]).ApplyAbility(fight.UserStats);

        foreach (var status in fight.Enemy.InitialStatuses)
            Instantiate(status).ApplyStatus(fight.EnemyStats);

        FightUI.Instance.InitializeFight();
        return fight;

    }

    /// <summary>
    /// Handles all of the logic for attacking a character along with displaying
    /// UI events. This function is called from FightUI script.
    /// </summary>
    /// <param name="attacker">The character that is attacking.</param>
    public void Attack(BasicAbility ability) {
        StartCoroutine(DoAttackLogic(ability));
    }

    /// <summary>
    /// Handles all of the battle logic for the user selecting an ability as a turn,
    /// along with UI events. This function will be called from FightUI script.
    /// </summary>
    /// <param name="ability">The ability the user is activating.</param>

    public void UseAbility(Ability ability)
    {
        // TODO: Undefined condition if a persisent ability is being used.
        //ability.ApplyAbility(who);
        StartCoroutine(DoAbilityLogic(ability));
    }

    System.Collections.IEnumerator InvokePreAttackAbility(Ability[] abilities, Ability? ability = null)
    {
        if (ability is not null)
        {
            PreAttack?.Invoke(ability.Owner);
            if (ability.LoreAvailable)
                yield return ability.DoAbilityLogicPre();
        }

        foreach (var a in abilities)
        {
            if (a == ability) continue;
            if (a is null) break;
            if (a.LoreAvailable) yield return a.DoAbilityLogicPre();
            yield return EndGameIfNoHealth();
            
        }
    }

    System.Collections.IEnumerator InvokePostAttackAbility(Ability[] abilities, Ability? ability = null)
    {
        if (ability is not null)
            PostAttack?.Invoke(ability.Owner);
        foreach (var a in abilities)
        {
            if (a is null) break;
            if (a.LoreAvailable) yield return a.DoAbilityLogicPost();
            yield return EndGameIfNoHealth();
            
        }
    }

    System.Collections.IEnumerator InvokePreAttackStatus(Status[] statuses)
    {
        foreach (var s in statuses)
        {
            if (s is null) break;
            if (s.LoreAvailable) yield return s.DoStatusLogicPre();
            yield return EndGameIfNoHealth();
            
        }
    }

    System.Collections.IEnumerator InvokePostAttackStatus(Status[] statuses)
    {
        foreach (var s in statuses)
        {
            if (s is null) break;
            if (s.LoreAvailable) yield return s.DoStatusLogicPost();
            yield return EndGameIfNoHealth();
            
        }
    }
    System.Collections.IEnumerator DoAbilityLogic(Ability ability)
    {
        var call = recursiveCall++;
        ShowOrAddDialogue(ability.BattleDescription);

        var savedAbilities = new Ability[10];
        var savedStatuses = new Status[8];
        ability.Owner.ActiveAbilities.CopyTo(savedAbilities, 0);
        ability.ApplyAbility(ability.Owner);
        yield return InvokePreAttackAbility(savedAbilities, ability);
        ability.Owner.ActiveStatuses.CopyTo(savedStatuses, 0);
        yield return InvokePreAttackStatus(savedStatuses);
        var typeWriter = FightUI.Instance.DialogueChat.GetComponent<Typewriter>();

        yield return EndGameIfNoHealth();
        if (ability.Owner != EnemyStats)
            yield return DoEnemyLogic();
        // End attack.
        yield return InvokePostAttackAbility(savedAbilities, ability);
        yield return InvokePostAttackStatus(savedStatuses);
        yield return new WaitForSecondsRealtime(0.5f);
        EndAttack?.Invoke(ability.Owner);
        if (Ended || call != 0) yield break;
        yield return new WaitForSecondsRealtime(0.5f);
        FightUI.Instance.AddDialogueLine("What will you do?");
        yield return typeWriter.TypeWriterEnumerator;
        yield return new WaitForSecondsRealtime(1.5f);
        FightUI.Instance.HideDialogue();
        recursiveCall = 0;
    }



    System.Collections.IEnumerator DoAttackLogic(BasicAbility attacker)
    {
        var call = recursiveCall++;
        // Pre attack - before the attack has been calculated and announced.
        var dest = new Ability[10];
        var savedStatuses = new Status[8];
        var fightData = attacker.Owner;
        byte damage;
        if (attacker is BasicAttackAbility b1) damage = b1.ApplyAbility(attacker.Owner);
        else damage = attacker.ApplyAbility(attacker.Owner);

        fightData.ActiveAbilities.CopyTo(dest, 0, 10);
        yield return InvokePreAttackAbility(dest);
        attacker.Owner.ActiveStatuses.CopyTo(savedStatuses, 0);
        yield return InvokePreAttackStatus(savedStatuses);
        

        var target = fightData == UserStats ? EnemyStats : UserStats;
        target.Health -= damage;
        var typeWriter = FightUI.Instance.DialogueChat.GetComponent<Typewriter>();

        ShowOrAddDialogue($"{attacker.Owner.Stats.Name} deals {damage} damage.");

        yield return typeWriter.TypeWriterEnumerator;
        FightUI.Instance.UpdateHealths();

        yield return new WaitForSecondsRealtime(0.5f);
        yield return EndGameIfNoHealth();
        if (attacker.Owner != EnemyStats)
            yield return DoEnemyLogic();
        // Post attack - after the attack has been announced.
        yield return InvokePostAttackAbility(dest);
        yield return InvokePostAttackStatus(savedStatuses);
        FightUI.Instance.UpdateHeat();

        // End attack.
        EndAttack?.Invoke(attacker.Owner);
        if (Ended || call != 0) yield break;
        yield return new WaitForSecondsRealtime(0.5f);
        FightUI.Instance.AddDialogueLine("What will you do?");
        yield return typeWriter.TypeWriterEnumerator;
        yield return new WaitForSecondsRealtime(1.5f);
        FightUI.Instance.HideDialogue();
        recursiveCall = 0;

    }
    
    private void ShowOrAddDialogue(string msg)
    {
        if (FightUI.Instance.ChatPanel.activeInHierarchy)
            FightUI.Instance.AddDialogueLine(msg);
        else
            FightUI.Instance.ShowDialogue(msg);
    }

    /// <summary>
    /// Enholds the decision logic of the enemy and attacks the user.
    /// </summary>
    public System.Collections.IEnumerator DoEnemyLogic()
    {
        if (EnemyStats.AvailableAbilityNames.Count == 0)
        {
            Debug.LogWarning("Enemy had 0 available abilities, returning...");
            yield break;
        }
        var a = new string[EnemyStats.AvailableAbilityNames.Count];
        EnemyStats.AvailableAbilityNames.CopyTo(a);
        var r = UnityEngine.Random.Range(0, EnemyStats.AvailableAbilityNames.Count);
        var ability = AbilityDatabase.StringToAbility[a[r]];
        if (ability is BasicAbility ability1)
        {
            ability1 = Instantiate(ability1);
            ability1.Owner = EnemyStats;
            yield return DoAttackLogic(ability1);
            yield break;
        }
        ability = Instantiate(ability);
        ability.Owner = EnemyStats;
        yield return DoAbilityLogic(ability);
    }

    public System.Collections.IEnumerator EndGameIfNoHealth()
    {
        var typeWriter = FightUI.Instance.DialogueChat.GetComponent<Typewriter>();

        CheckHealths();
        if (Ended)
        {
            FightUI.Instance.AddDialogueLine(UserStats.Health == 0 ? "You died!" :
                $"You won the battle! {Enemy.Name} has been defeated!");
            yield return typeWriter.TypeWriterEnumerator;
            yield return new WaitForSecondsRealtime(1.5f);
            FightUI.Instance.HideDialogue();
            yield return FightUI.Instance.EndFight();
            StopAllCoroutines();
            FightEnded?.Invoke();
            Destroy(FightUICanvasPrefab);
            Destroy(gameObject);
        }
    }

    private void CheckHealths() => Ended = UserStats.Health == 0 || EnemyStats.Health == 0;
    /// <summary>
    /// Calculates the damage an ability will do to the <paramref name="target"/>.
    /// </summary>
    /// <param name="abilityAttackStats">The ability's attack stats.</param>
    /// <param name="target">The target of the ability.</param>
    /// <param name="self">Whether this is a self-attack.</param>
    /// <returns>The damage the ability will cause to the target.</returns>
    internal byte CalculateAbilityDamage(AbilityAttackStats abilityAttackStats, FightData target, bool self)
    {
        // Targetting enemy.
        if (target != UserStats)
            return (byte)Mathf.Min(FightSettings.MaximumDamage, Mathf.RoundToInt(
                    Mathf.Min(target.Health,
                        Mathf.Max(FightSettings.MinimumDamage, abilityAttackStats.Damage * UserStats.DamageMultipler * 
                            FightSettings.GlobalDamageMultiplier * ((float)(abilityAttackStats.AttackPenetration + FightSettings.AdditionalAP)/ target.Stats.DEF)
                        )
                    )
                ));
        // Targetting user.
        return (byte)Mathf.Min(FightSettings.MaximumDamage, Mathf.RoundToInt(
                    Mathf.Min(target.Health,
                        Mathf.Max(FightSettings.MinimumDamage, abilityAttackStats.Damage * (self ? UserStats.DamageMultipler : EnemyStats.DamageMultipler) * 
                            FightSettings.GlobalDamageMultiplier * ((float)abilityAttackStats.AttackPenetration / target.Stats.DEF)
                        )
                    )
                ));
    }
    /// <summary>
    /// Invoke the <see cref="AbilityAttack"/> event.
    /// </summary>
    /// <param name="a">The ability that is attacking.</param>
    public void InvokeAbilityAttack(Ability a) => AbilityAttack?.Invoke(a);
}
