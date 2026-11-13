
using System.Numerics;
using Shared.GameData.Entities.Interfaces;
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
    protected Vector3 _moveTarget {get; set;}

    public EntityBase(ushort entityId, Vector3 spawnPos, MatchTeam team, byte rotation = 0)
    {
        
        EntityId = entityId;
        Position = spawnPos;
        Rotation = rotation;
        _spawnPosition = spawnPos;
        EntityTeam = team;

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

    public abstract void TakeDamage(float value, DamageTypes dmgType);

    public abstract void SetMoveTarget(float x, float z);

}