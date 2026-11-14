
using System.Numerics;
using Shared.GameData.Entities.Classes;
using Shared.GameData.Units;
using Shared.GameData.Units.Property;

namespace Shared.GameData.Entities;

public class UnitEntity : EntityBase
{

    public ulong PlayerIdentificator {get; private set;}
    public HeroId Hero {get; private set;}
    private float _health = 200;
    private float _maxHealth = 200;
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
    
    public UnitEntity(ulong playerIdentif, ushort entityId, Vector3 spawnPos, MatchTeam team, HeroId hero = HeroId.None, byte rotation = 0) 
        : base (entityId, spawnPos, team, rotation)
    {
        
        PlayerIdentificator = playerIdentif;
        Hero = hero;

        // Stats come from gamedata. HeroId.None keeps the legacy 200/200 for code
        // that spawns a unit before a hero was picked (console tests, old call sites).
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

    public override void TakeDamage(float value, DamageTypes dmgType, ushort attackerId)
    {
        
        Health -= value;
        Console.WriteLine($"[UnitId:{EntityId} | PlayerIdentificator:{PlayerIdentificator}] Take {value} damage! Health: {_health} / {_maxHealth}");

    }

    public override void SetMoveTarget(float x, float z)
    {
        
    }


}
