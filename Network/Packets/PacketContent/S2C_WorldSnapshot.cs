using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using Shared.GameData.Units;
using Shared.GameData.Units.Property;
using Shared.Packets.PacketContent.WorldSnapshotTypes;

namespace Shared.Packets.PacketContent;


public enum StateType : byte
{
    Baseline = 0,
    Delta = 1
}

[Flags]
public enum EntityFlags : byte
{    None = 0,
    Dead = 1 << 0,
    Stunned = 1 << 1,
    Slowed = 1 << 2
}

public struct S2C_WorldSnapshotPacket
{
    
    public const int MAX_ENTITIES = 32;
    public StateType Type;
    public uint Tick;
    public byte Count;

    public SnapshotEntityBaseline[] Baselines;
    public SnapshotEntity[] Deltas;

    public static S2C_WorldSnapshotPacket Create()
    {
        
        S2C_WorldSnapshotPacket packet = default;
        packet.Baselines = new SnapshotEntityBaseline[MAX_ENTITIES];
        packet.Deltas = new SnapshotEntity[MAX_ENTITIES];
        return packet;

    }

    public int Serialize(Span<byte> buffer)
    {
        
        int offset = 0;

        buffer[offset] = (byte)Type;
        offset++;

        if(Type == StateType.Delta)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(buffer[offset..], Tick);
            offset += 4;
        }

        buffer[offset] = Count;
        offset++;

        if(Type == StateType.Baseline)
        {
            
            for(int i = 0; i < Count; i++)
            {
                
                ref var b = ref Baselines[i];

                BinaryPrimitives.WriteUInt16LittleEndian(buffer[offset..], b.Id);
                offset += 2;
                BinaryPrimitives.WriteUInt16LittleEndian(buffer[offset..], (ushort)b.Hero);
                offset += 2;
                buffer[offset] = (byte)b.Team;
                offset++;

                BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], b.MaxHealth);
                offset += 4;
                BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], b.MaxMana);
                offset += 4;
                BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], b.Speed);
                offset += 4;
                BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], b.Radius);
                offset += 4;
                BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], b.Damage);
                offset += 4;
                BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], b.AttackTime);
                offset += 4;
                BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], b.AttackRange);
                offset += 4;

            }

        }
        else
        {
            
            for(int i = 0; i < Count; i++)
            {
                
                ref var d = ref Deltas[i];

                BinaryPrimitives.WriteUInt16LittleEndian(buffer[offset..], d.Id);
                offset += 2;
                BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], d.X);
                offset += 4;
                BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], d.Z);
                offset += 4;
                buffer[offset] = d.Facing;
                offset++;
                BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], d.Health);
                offset += 4;
                BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], d.Mana);
                offset += 4;
                buffer[offset] = (byte)d.Flags;
                offset++;
                buffer[offset] = (byte)d.State;
                offset++;


            }

        }

        return offset;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        int offset = 0;

        Type = (StateType)buffer[offset];
        offset++;

        if(Type == StateType.Delta)
        {
            Tick = BinaryPrimitives.ReadUInt32LittleEndian(buffer[offset..]);
            offset += 4;
        }

        Count = buffer[offset];
        if (Count > MAX_ENTITIES) Count = MAX_ENTITIES;
        offset++;

        if(Type == StateType.Baseline)
        {
            
            for(int i = 0; i < Count; i++)
            {
                
                ref var b = ref Baselines[i];

                b.Id = BinaryPrimitives.ReadUInt16LittleEndian(buffer[offset..]);
                offset += 2;

                b.Hero = (HeroId)BinaryPrimitives.ReadUInt16LittleEndian(buffer[offset..]);
                offset += 2;

                b.Team = (MatchTeam)buffer[offset];
                offset++;

                b.MaxHealth = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
                offset += 4;
                b.MaxMana = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
                offset += 4;
                b.Speed = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
                offset += 4;
                b.Radius = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
                offset += 4;
                b.Damage = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
                offset += 4;
                b.AttackTime = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
                offset += 4;
                b.AttackRange = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
                offset += 4;

            }
            

        }
        else
        {
            
            for(int i = 0; i < Count; i++)
            {
                
                ref var d = ref Deltas[i];

                d.Id = BinaryPrimitives.ReadUInt16LittleEndian(buffer[offset..]);
                offset += 2;

                d.X = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
                offset += 4;
                d.Z = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
                offset += 4;

                d.Facing = buffer[offset];
                offset++;

                d.Health = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
                offset += 4;
                d.Mana = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
                offset += 4;

                d.Flags = (EntityFlags)buffer[offset];
                offset++;
                d.State = (EntityStates)buffer[offset];
                offset++;



            }

        }

    }

    
}
