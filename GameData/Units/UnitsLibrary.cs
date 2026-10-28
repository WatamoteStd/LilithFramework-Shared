
using System.Collections.Frozen;
using System.Text.Json;

namespace Shared.GameData.Units;

public static class UnitsLibrary
{

    private static readonly FrozenDictionary<ushort, UnitData> _unitsFrozen = FrozenDictionary<ushort, UnitData>.Empty;
    public static ushort Count {get; private set;}

    static UnitsLibrary()
    {

        _unitsFrozen = ParseHeroesFromJson().ToFrozenDictionary();

    }

    private static Dictionary<ushort, UnitData> ParseHeroesFromJson()
    {

        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while(dir != null && !Directory.Exists(Path.Combine(dir.FullName, "gamedata")))
        {
            dir = dir.Parent;
        }
        if (dir == null) throw new DirectoryNotFoundException("gamedata folder not found. Check the path.");
        var heroesDir = Path.Combine(dir.FullName, "gamedata", "heroes");

        var heroesArray = Directory.GetFiles(heroesDir, "*.json");
        if(heroesArray.Length == 0) throw new Exception("Folder is empty.");

        var originDictionary = new Dictionary<ushort, UnitData>();

        for (int i = 0; i < heroesArray.Length; i++)
        {

            string filePath = heroesArray[i];

            var json = File.ReadAllText(filePath);
            UnitData data = JsonSerializer.Deserialize<UnitData>(json);

            originDictionary[data.FrameworkId] = data;
            Count++;

        }

        return originDictionary;

    }

    public static bool TryGetUnit(ushort unitId, out UnitData data) => _unitsFrozen.TryGetValue(unitId, out data);

}
