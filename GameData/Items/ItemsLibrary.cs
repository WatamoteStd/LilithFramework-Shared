
using System.Collections.Frozen;

namespace Shared.GameData.Items;

public static class ItemsLibrary
{
    
    private static readonly FrozenDictionary<ItemId, ItemData> _idToData = FrozenDictionary<ItemId, ItemData>.Empty;

    static ItemsLibrary()
    {
        
        ItemData woodenCrown = new ItemData
        {
            Id = ItemId.WoodenCrown, Cost = 60f, Modifiers = new StatModifier[]
            {
                new StatModifier
                {
                    Value = 20f, Action = ItemStatAction.Increase, Stat = StatType.Health
                },
                new StatModifier
                {
                    Value = 1.0f, Action = ItemStatAction.Increase, Stat = StatType.HealthRegen
                }
            }
        };

        var dict = new Dictionary<ItemId, ItemData>();
        dict[ItemId.WoodenCrown] = woodenCrown;

        _idToData = dict.ToFrozenDictionary();

    }

    public static bool TryGetItem(ItemId item, out ItemData data) => _idToData.TryGetValue(item, out data);

}