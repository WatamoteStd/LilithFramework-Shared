
using System.Collections.Frozen;
using System.Text.Json;

namespace Shared.GameData.Units;

public static class UnitsLibrary
{
    
    private static readonly FrozenDictionary<UnitTypes, UnitData> _unitsLibrary = FrozenDictionary<UnitTypes, UnitData>.Empty;

    static UnitsLibrary()
    {
        
        _unitsLibrary = ParseHeroesFromJson().ToFrozenDictionary();

    }

    private static Dictionary<UnitTypes, UnitData> ParseHeroesFromJson()
    {
        
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while(dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Shared")))
        {
            dir = dir.Parent;
        }
        if (dir == null) throw new DirectoryNotFoundException("Shared folder not found!");

        var heroesDir = Path.Combine(dir.FullName, "Shared", "editor", "heroes");

        var dictionary = new Dictionary<UnitTypes, UnitData>();
        var heroesArray = Directory.GetFiles(heroesDir, "*.json");

        if(heroesArray.Length == 0) throw new Exception("Folder is empty.");

        for(int i = 0; i < heroesArray.Length; i++)
        {
            
            string filePath = heroesArray[i];

            var fileName = Path.GetFileNameWithoutExtension(filePath);
            UnitTypes unitType = Enum.Parse<UnitTypes>(fileName);

            var json = File.ReadAllText(filePath);
            UnitData data = JsonSerializer.Deserialize<UnitData>(json);
            
            dictionary.Add(unitType, data);

        }
        
        return dictionary;

    }

    public static bool TryGetUnit(UnitTypes unitId, out UnitData data) => _unitsLibrary.TryGetValue(unitId, out data);

}