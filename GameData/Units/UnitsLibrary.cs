
using System.Collections.Frozen;
using System.Text.Json;

namespace Shared.GameData.Units;

public static class UnitsLibrary
{
    
    private static readonly UnitData[] _units;
    public static ushort Count {get; private set;}

    static UnitsLibrary()
    {
        
        _units = ParseHeroesFromJson();

    }

    private static UnitData[] ParseHeroesFromJson()
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

        var originArray = new UnitData[heroesArray.Length];

        for(int i = 0; i < heroesArray.Length; i++)
        {
            
            string filePath = heroesArray[i];

            var json = File.ReadAllText(filePath);
            UnitData data = JsonSerializer.Deserialize<UnitData>(json);
            
            originArray[data.FrameworkId] = data;
            Count++;

        }
        
        return originArray;

    }

    public static bool TryGetUnit(ushort unitId, out UnitData data)
    {
        data = default;
        if(unitId > Count || unitId > ushort.MaxValue) return false;

        data = _units[unitId];
        return true;

    }

}