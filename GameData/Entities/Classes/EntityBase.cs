
using System.Numerics;
using Shared.GameData.Entities.Interfaces;
using Shared.GameData.Items;
using Shared.GameData.Units;
using Shared.GameData.Units.Property;

namespace Shared.GameData.Entities.Classes; 

public abstract class EntityBase : IDamagable
{
    public event Action<EntityBase>? OnDie;
    public event Action? OnRespawn;

    /// <summary>
    /// Fires after damage is applied: the victim, how much health it actually
    /// lost, and who caused it. Only used for the cosmetic damage number, so
    /// nothing listens to it on entities nobody watches.
    /// </summary>
    public event Action<EntityBase, float, DamageTypes, EntityBase>? OnDamagedTaked;

    public EntityStates CurrentState {get; protected set;} = EntityStates.Idle;


    public ushort EntityId {get; protected set;}
    public bool NeedtsBaseline {get; set;}
    public Vector3 Position {get; protected set;}
    protected Vector3 _spawnPosition;
    public byte Rotation {get; protected set;}
    public MatchTeam EntityTeam {get; protected set;}
    public float Radius {get; protected set;}
    protected UnitData _dllData {get; set;}
    public HeroId Hero {get; protected set;}
    public float Gold {get; protected set;}
    public int GoldReward {get; protected set;}

    protected float _health = 200;
    protected float _maxHealth = 200;
    public float Health
    {
        get => _health;
        set
        {
            _health = Math.Clamp(value, 0, _maxHealth);
        }
    }
    public float MaxHealth => _maxHealth;
    public float HealthRegen {get; protected set;}
    public float Mana {get; set;}
    public float MaxMana {get; protected set;}
    public float Speed {get; protected set;} = 5.0f;
    public float Damage {get; protected set;} = 23f;
    public float Armor {get; protected set;} = 1f;
    public float BasicAttackTime {get; protected set;} = 1.75f;
    public float AttackRange {get; protected set;} = 2.0f;
    public AttackTypes AttackType {get; protected set;} = AttackTypes.Melee;
    public float FieldOfView {get; protected set;} = 50f;


    protected float _respawnTime = 5.0f;
    protected float _curRespawnTime;

    public EntityBase(ushort entityId, Vector3 spawnPos, MatchTeam team, HeroId hero, byte rotation = 0)
    {
        
        EntityId = entityId;
        Position = spawnPos;
        Rotation = rotation;
        _spawnPosition = spawnPos;
        EntityTeam = team;
        Hero = hero;

        if(hero != HeroId.None && UnitsLibrary.TryGetHero(hero, out UnitData data))
        {
            _dllData = data;

            _maxHealth = data.BaseHealth;
            _health = _maxHealth;
            MaxMana = data.BaseMana;
            Mana = MaxMana;
            Radius = data.Radius;
            Damage = data.Damage;
            AttackRange = data.AttackRange;
            BasicAttackTime = data.BasicAttackTime;
            AttackType = data.AttackType;
            Speed = data.MoveSpeed;
            HealthRegen = data.HealthRegen;
            GoldReward = data.GoldReward;
            Armor = data.Armor;
        }

    }

    public virtual void Update(float deltaTime)
    {
        
        Regenerate(deltaTime);

    }
    
    public virtual void Die(EntityBase killer)
    {
        if(CurrentState == EntityStates.Dead) return;
        CurrentState = EntityStates.Dead;
        _curRespawnTime = _respawnTime;
        
        OnDie?.Invoke(killer);
    }

    public virtual void TakeDamage(float value, DamageTypes dmgType, EntityBase attacker)
    {

        if(CurrentState == EntityStates.Dead) return;

        // Overkill is not shown: a 92 hit on a 19 hp creep prints 19, so the
        // numbers on screen always add up to the health bar that disappeared.
        float applied = MathF.Min(value, _health);

        Health -= value;

        if(Health == 0) Die(attacker);

        if(applied > 0) OnDamagedTaked?.Invoke(this, applied, dmgType, attacker);

    }

    public bool IsInAttackRange(EntityBase target)
    {
        
        float dx = target.Position.X - Position.X;
        float dz = target.Position.Z - Position.Z;

        float distSquared = (dx * dx) + (dz * dz);
        
        float effectiveRange = AttackRange + Radius + target.Radius;
        float rangeSqueared = effectiveRange * effectiveRange;

        return distSquared <= rangeSqueared;

    }

    protected void Regenerate(float deltaTime)
    {
        
        Health += HealthRegen * deltaTime;

    }

    public virtual void Respawn()
    {
        
        CurrentState = EntityStates.Idle;
        Health = MaxHealth;
        Mana = MaxMana;
        Position = _spawnPosition; 

    }
    public virtual void ChangeGold(float value, ItemStatAction action)
    {
        switch(action)
        {
            case ItemStatAction.Increase:
                Gold += value;
            break;
            case ItemStatAction.Decrease:
                Gold -= value;
            break;  
        }

    }


}