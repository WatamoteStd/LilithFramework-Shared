
using System.Numerics;
using Shared.GameData.Entities.Classes;
using Shared.GameData.Units;
using Shared.GameData.Units.Property;

namespace Shared.GameData.Entities;

public class UnitEntity : EntityBase
{

    public ulong PlayerIdentificator {get; protected set;}
    protected Vector3 _moveTarget {get; set;}
    
    public UnitEntity(ulong playerIdentif, ushort entityId, Vector3 spawnPos, MatchTeam team, HeroId hero = HeroId.None, byte rotation = 0) 
        : base (entityId, spawnPos, team, hero, rotation)
    {
        
        PlayerIdentificator = playerIdentif;
        Hero = hero;

    }

    public virtual void SetMoveTarget(float x, float z)
    {
        
    }


}
