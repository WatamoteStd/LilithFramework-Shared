
using System.Runtime.CompilerServices;
using Shared.GameData.Units;
using Shared.GameData.Units.Property;

namespace Shared.Packets.PacketContent.WorldSnapshotTypes;

public struct SnapshotEntityBaseline
{
    
    public ushort Id;
    public HeroId Hero;
    public MatchTeam Team;
    public float MaxHealth;
    public float MaxMana;
    public float Speed;
    public float Radius;
    public float Damage;
    public float AttackTime;
    public float AttackRange;

    public const int Size = 33;

}