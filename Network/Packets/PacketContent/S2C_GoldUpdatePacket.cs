
using System.Buffers.Binary;

namespace Shared.Packets.PacketContent;

public struct S2C_GoldUpdatePacket
{   
    
    public int TotalGold;
    public int DeltaGold;

    public int Serialize(Span<byte> buffer)
    {
        int offset = 0;
        BinaryPrimitives.WriteInt32LittleEndian(buffer, TotalGold);
        offset += 4;
        BinaryPrimitives.WriteInt32LittleEndian(buffer[offset..], DeltaGold);
        offset += 4;
        return offset;
        
    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        int offset = 0;
        TotalGold = BinaryPrimitives.ReadInt32LittleEndian(buffer);
        offset += 4;
        DeltaGold = BinaryPrimitives.ReadInt32LittleEndian(buffer[offset..]);
        offset += 4;
    }
    
}