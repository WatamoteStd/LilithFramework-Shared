
using Shared.GameData.Entities.Classes;
using Shared.GameData.Units.Property;

namespace Shared.GameData.Entities.Interfaces;

public interface IDamagable
{
    
    void TakeDamage(float value, DamageTypes dmgType, EntityBase attacker);

}