
using System.Buffers.Binary;
using Shared.GameData.Units;
using Shared.GameData.Units.Property;

namespace Shared.Packets.PacketContent;

public struct S2C_HandshakeResponsePacket
{
    
    public enum ResponseType : byte { Ok = 0, Droped = 1};
    public ResponseType Response;
    public ushort EntityId;
    public MatchTeam Team;
    public uint ServerTick;
    public HeroId Hero;

    public int Serialize(Span<byte> buffer)
    {
        
        buffer[0] = (byte)Response;
        BinaryPrimitives.WriteUInt16LittleEndian(buffer[1..], EntityId);
        buffer[3] = (byte)Team;
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[4..], ServerTick);
        BinaryPrimitives.WriteUInt16LittleEndian(buffer[8..], (ushort)Hero);
        return 10;

    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Response = (ResponseType)buffer[0];
        EntityId = BinaryPrimitives.ReadUInt16LittleEndian(buffer[1..]);
        Team = (MatchTeam)buffer[3];
        ServerTick = BinaryPrimitives.ReadUInt32LittleEndian(buffer[4..]);
        Hero = (HeroId)BinaryPrimitives.ReadUInt16LittleEndian(buffer[8..]);
    }

}
