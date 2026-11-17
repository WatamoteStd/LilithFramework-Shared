
using System.Buffers.Binary;

namespace Shared.Packets.PacketContent;

public struct S2C_GoldUpdatePacket
{   
    
    public float TotalGold;
    public float DeltaGold;

    public int Serialize(Span<byte> buffer)
    {
        int offset = 0;
        BinaryPrimitives.WriteSingleLittleEndian(buffer, TotalGold);
        offset += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], DeltaGold);
        offset += 4;
        return offset;
        
    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        int offset = 0;
        TotalGold = BinaryPrimitives.ReadSingleLittleEndian(buffer);
        offset += 4;
        DeltaGold = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset += 4;
    }
    
}