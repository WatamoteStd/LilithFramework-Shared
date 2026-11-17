
namespace Shared.GameData.Items;

public struct ItemData
{
    
    public required ItemId Id {get; init;}
    public required StatModifier[] Modifiers {get; init;}
    public required float Cost {get; init;}

}