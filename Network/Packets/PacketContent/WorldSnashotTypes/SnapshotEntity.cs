
using System.Runtime.CompilerServices;
using Shared.GameData.Units.Property;

namespace Shared.Packets.PacketContent.WorldSnapshotTypes;


public struct SnapshotEntity
{
    
    public ushort Id;
    public float X;
    public float Z;
    public byte Facing;
    public float Health;
    public float Mana;
    public EntityFlags Flags;
    public EntityStates State;

    public const int Size = 21;

}