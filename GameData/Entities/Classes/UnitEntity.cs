
using System.Numerics;
using Shared.GameData.Entities.Classes;
using Shared.GameData.Units;
using Shared.GameData.Units.Property;

namespace Shared.GameData.Entities;

public class UnitEntity : EntityBase
{

    protected Vector3 _moveTarget {get; set;}
    protected float _currentAttackReload = 0.0f;
    protected float _windUpTime = 0.25f;
    protected float _curWindUpTime;
    protected EntityBase? _attackTarget = null;
    
    public UnitEntity(ushort entityId, Vector3 spawnPos, MatchTeam team, HeroId hero = HeroId.None, byte rotation = 0) 
        : base (entityId, spawnPos, team, hero, rotation)
    {
        
        Hero = hero;

    }

    public virtual void SetMoveTarget(float x, float z)
    {
        
    }

}
