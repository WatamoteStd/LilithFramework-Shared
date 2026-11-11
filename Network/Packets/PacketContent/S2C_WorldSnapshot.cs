
using System.Buffers.Binary;
using Shared.GameData.Units;
using Shared.GameData.Units.Property;

namespace Shared.Packets.PacketContent;

[Flags]
public enum EntityFlags : byte
{
    None = 0,
    Dead = 1
}

/// <summary>
/// One unit in the world snapshot. 43 bytes on the wire, little-endian.
/// Static data (hero, max hp, speed...) is sent every tick on purpose:
/// the packet is self-contained, so a lost snapshot can never desync the client.
/// </summary>
public struct EntitySnapshot
{
    public const int Size = 43;

    public ushort Id;
    public HeroId Hero;
    public MatchTeam Team;
    public EntityFlags Flags;
    public float X;
    public float Z;
    public byte Facing;
    public ushort Health;
    public ushort MaxHealth;
    public ushort Mana;
    public ushort MaxMana;
    public float Speed;
    public float Radius;
    public float Damage;
    public float AttackTime;
    public float AttackRange;
}

/// <summary>
/// S2C_WorldSnapshot: full state of every entity, 5 + 43 * N bytes.
/// MaxEntities is limited by the MTU, not by taste:
/// 5 + 43 * 32 = 1381 bytes, REI_PAYLOAD_MAXIMUM_SIZE is 1495.
/// Use <see cref="Create" />: a default struct has a null Entities array.
/// </summary>
public struct S2C_WorldSnapshotPacket
{
    public const int MaxEntities = 32;
    public const int MaxSize = 5 + EntitySnapshot.Size * MaxEntities;

    public uint Tick;
    public byte Count;
    public EntitySnapshot[] Entities;

    public static S2C_WorldSnapshotPacket Create()
    {
        S2C_WorldSnapshotPacket packet = default;
        packet.Entities = new EntitySnapshot[MaxEntities];
        return packet;
    }

    public int Serialize(Span<byte> buffer)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, Tick);
        buffer[4] = Count;

        int offset = 5;
        for(int i = 0; i < Count; i++)
        {
            ref EntitySnapshot e = ref Entities[i];

            BinaryPrimitives.WriteUInt16LittleEndian(buffer[offset..], e.Id);
            BinaryPrimitives.WriteUInt16LittleEndian(buffer[(offset + 2)..], (ushort)e.Hero);
            buffer[offset + 4] = (byte)e.Team;
            buffer[offset + 5] = (byte)e.Flags;
            BinaryPrimitives.WriteSingleLittleEndian(buffer[(offset + 6)..], e.X);
            BinaryPrimitives.WriteSingleLittleEndian(buffer[(offset + 10)..], e.Z);
            buffer[offset + 14] = e.Facing;
            BinaryPrimitives.WriteUInt16LittleEndian(buffer[(offset + 15)..], e.Health);
            BinaryPrimitives.WriteUInt16LittleEndian(buffer[(offset + 17)..], e.MaxHealth);
            BinaryPrimitives.WriteUInt16LittleEndian(buffer[(offset + 19)..], e.Mana);
            BinaryPrimitives.WriteUInt16LittleEndian(buffer[(offset + 21)..], e.MaxMana);
            BinaryPrimitives.WriteSingleLittleEndian(buffer[(offset + 23)..], e.Speed);
            BinaryPrimitives.WriteSingleLittleEndian(buffer[(offset + 27)..], e.Radius);
            BinaryPrimitives.WriteSingleLittleEndian(buffer[(offset + 31)..], e.Damage);
            BinaryPrimitives.WriteSingleLittleEndian(buffer[(offset + 35)..], e.AttackTime);
            BinaryPrimitives.WriteSingleLittleEndian(buffer[(offset + 39)..], e.AttackRange);

            offset += EntitySnapshot.Size;
        }

        return offset;
    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Tick = BinaryPrimitives.ReadUInt32LittleEndian(buffer);

        int count = buffer[4];
        if(count > MaxEntities) count = MaxEntities;
        Count = (byte)count;

        int offset = 5;
        for(int i = 0; i < count; i++)
        {
            ref EntitySnapshot e = ref Entities[i];

            e.Id = BinaryPrimitives.ReadUInt16LittleEndian(buffer[offset..]);
            e.Hero = (HeroId)BinaryPrimitives.ReadUInt16LittleEndian(buffer[(offset + 2)..]);
            e.Team = (MatchTeam)buffer[offset + 4];
            e.Flags = (EntityFlags)buffer[offset + 5];
            e.X = BinaryPrimitives.ReadSingleLittleEndian(buffer[(offset + 6)..]);
            e.Z = BinaryPrimitives.ReadSingleLittleEndian(buffer[(offset + 10)..]);
            e.Facing = buffer[offset + 14];
            e.Health = BinaryPrimitives.ReadUInt16LittleEndian(buffer[(offset + 15)..]);
            e.MaxHealth = BinaryPrimitives.ReadUInt16LittleEndian(buffer[(offset + 17)..]);
            e.Mana = BinaryPrimitives.ReadUInt16LittleEndian(buffer[(offset + 19)..]);
            e.MaxMana = BinaryPrimitives.ReadUInt16LittleEndian(buffer[(offset + 21)..]);
            e.Speed = BinaryPrimitives.ReadSingleLittleEndian(buffer[(offset + 23)..]);
            e.Radius = BinaryPrimitives.ReadSingleLittleEndian(buffer[(offset + 27)..]);
            e.Damage = BinaryPrimitives.ReadSingleLittleEndian(buffer[(offset + 31)..]);
            e.AttackTime = BinaryPrimitives.ReadSingleLittleEndian(buffer[(offset + 35)..]);
            e.AttackRange = BinaryPrimitives.ReadSingleLittleEndian(buffer[(offset + 39)..]);

            offset += EntitySnapshot.Size;
        }
    }
}
