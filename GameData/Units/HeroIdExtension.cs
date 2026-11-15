
using Shared.GameData.Units;

namespace Shared.GameData.Entities;

public static class HeroesIdExtension
{
    public static bool IsTower(this HeroId heroId)
    {
        return (ushort)heroId >= (ushort)HeroId.TowerLight1
            && (ushort)heroId <= (ushort)HeroId.TowerDark3;
    }

    public static bool IsLightTower(this HeroId heroId) => 
        heroId is HeroId.TowerLight1 or HeroId.TowerLight2 or HeroId.TowerLight3;

    public static bool IsDarkTower(this HeroId heroId) =>
        heroId is HeroId.TowerDark1 or HeroId.TowerDark2 or HeroId.TowerDark3;

}