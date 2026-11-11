
using System.Numerics;
using Shared.GameData.Entities.Interfaces;
using Shared.GameData.Units.Property;

namespace Shared.GameData.Entities.Classes; 

public abstract class EntityBase : IDamagable
{
    public event Action<EntityBase>? OnDie;

    public ushort EntityId {get; private set;}
    public Vector3 Position {get; protected set;}
    protected Vector3 _spawnPosition;
    public byte Rotation {get; protected set;}
    public bool IsDead {get; protected set;} = false;
    public MatchTeam EntityTeam {get; private set;}

    public EntityBase(ushort entityId, Vector3 spawnPos, MatchTeam team, byte rotation = 0)
    {
        
        EntityId = entityId;
        Position = spawnPos;
        Rotation = rotation;
        _spawnPosition = spawnPos;
        EntityTeam = team;

    }
    
    public virtual void Die()
    {
        if(IsDead) return;
        IsDead = true;
        OnDie?.Invoke(this);
    }

    public abstract void TakeDamage(float value, DamageTypes dmgType);

}