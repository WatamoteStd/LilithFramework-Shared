
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
    public float MaxMana {get; private set;}
    public float Speed {get; private set;} = 5.0f;
    public float Damage {get; private set;}
    public float BasicAttackTime {get; private set;}
    public float AttackRange {get; private set;}
    
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
        }

    }

    public override void TakeDamage(float value, DamageTypes dmgType)
    {
        
        Health -= value;
        Console.WriteLine($"[UnitId:{EntityId} | PlayerIdentificator:{PlayerIdentificator}] Take {value} damage! Health: {_health} / {_maxHealth}");

    }

    public override void SetMoveTarget(float x, float z)
    {
        
    }


}
