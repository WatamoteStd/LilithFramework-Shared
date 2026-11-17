
namespace Shared.GameData.Items;

public struct StatModifier
{
    public required StatType Stat {get; init;}
    public required ItemStatAction Action {get; init;}
    public required float Value {get; init;}
}