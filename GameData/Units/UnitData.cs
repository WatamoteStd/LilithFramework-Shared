
using Shared.GameData.Units.Property;

namespace Shared.GameData.Units;

public struct UnitData
{
    
    public required float Radius {get; init;}
    public required float Height {get; init;}
    public required float BaseHealth {get; init;}
    public required float BaseMana {get; init;}
    public required float HealthRegen {get; init;}
    public required float ManaRegen {get; init;}
    public required float Armor {get; init;}
    public required float MoveSpeed {get; init;}
    public required float Damage {get; init;}
    public required float BasicAttackTime {get; init;}
    public required float AttackRange {get; init;}
    public required AttackTypes AttackType {get; init;}


}