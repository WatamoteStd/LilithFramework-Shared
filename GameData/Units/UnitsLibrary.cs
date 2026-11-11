
using System.Collections.Frozen;
using System.Text.Json;

namespace Shared.GameData.Units;

/// <summary>
/// Hero stats loaded from gamedata/heroes/*.json once per process.
/// The key is the generated <see cref="HeroId" />, which is also the network id.
/// File name must match the enum member: Lolka.json -> HeroId.Lolka.
/// </summary>
public static class UnitsLibrary
{

    private static readonly FrozenDictionary<HeroId, UnitData> _heroesFrozen = ParseHeroesFromJson().ToFrozenDictionary();

    public static ushort Count => (ushort)_heroesFrozen.Count;

    private static Dictionary<HeroId, UnitData> ParseHeroesFromJson()
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

        var originDictionary = new Dictionary<HeroId, UnitData>(heroesArray.Length);

        for (int i = 0; i < heroesArray.Length; i++)
        {

            string filePath = heroesArray[i];
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            if(!Enum.TryParse(fileName, ignoreCase: false, out HeroId hero) || hero == HeroId.None)
            {
                throw new InvalidOperationException(
                    $"gamedata/heroes/{fileName}.json has no HeroId entry. Run: dotnet run --project Tools/HeroesGen");
            }

            var json = File.ReadAllText(filePath);
            UnitData data = JsonSerializer.Deserialize<UnitData>(json);

            originDictionary.Add(hero, data);

        }

        return originDictionary;

    }

    public static bool TryGetHero(HeroId hero, out UnitData data) => _heroesFrozen.TryGetValue(hero, out data);

    public static UnitData GetHero(HeroId hero)
    {
        if(_heroesFrozen.TryGetValue(hero, out UnitData data)) return data;
        throw new KeyNotFoundException($"No gamedata for hero '{hero}'. Run: dotnet run --project Tools/HeroesGen");
    }

}
