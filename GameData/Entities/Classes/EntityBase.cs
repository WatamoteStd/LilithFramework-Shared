
using System.Numerics;
using Shared.GameData.Entities.Interfaces;
using Shared.GameData.Units;
using Shared.GameData.Units.Property;

namespace Shared.GameData.Entities.Classes; 

public abstract class EntityBase : IDamagable
{
    public event Action<EntityBase>? OnDie;

    public EntityStates CurrentState {get; protected set;} = EntityStates.Idle;

    public ushort EntityId {get; protected set;}
    public Vector3 Position {get; protected set;}
    protected Vector3 _spawnPosition;
    public byte Rotation {get; protected set;}
    public MatchTeam EntityTeam {get; protected set;}
    public float Radius {get; protected set;}
    protected UnitData _dllData {get; set;}
    public HeroId Hero {get; protected set;}

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
    public float Mana {get; set;}
    public float MaxMana {get; protected set;}
    public float Speed {get; protected set;} = 5.0f;
    public float Damage {get; protected set;} = 23;
    public float BasicAttackTime {get; protected set;} = 1.75f;
    public float AttackRange {get; protected set;} = 2.0f;
    public AttackTypes AttackType {get; protected set;} = AttackTypes.Melee;

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
            _maxHealth = data.BaseHealth;
            _health = _maxHealth;
            MaxMana = data.BaseMana;
            Mana = MaxMana;
            Radius = data.Radius;
            Damage = data.Damage;
            AttackRange = data.AttackRange;
            BasicAttackTime = data.BasicAttackTime;
            AttackType = data.AttackType;
        }

    }

    public virtual void Update(float deltaTime)
    {
        
    }
    
    public virtual void Die()
    {
        if(CurrentState == EntityStates.Dead) return;
        CurrentState = EntityStates.Dead;
        OnDie?.Invoke(this);
    }

    public virtual void TakeDamage(float value, DamageTypes dmgType, ushort attackerId)
    {

        if(CurrentState == EntityStates.Dead) return;
        
        Health -= value;
        Console.WriteLine($"[UnitId:{EntityId}] Take {value} damage! Health: {_health} / {_maxHealth}");

        if(Health == 0)
        {
            CurrentState = EntityStates.Dead;
        }

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


}