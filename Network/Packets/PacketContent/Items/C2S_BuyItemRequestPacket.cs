
using System.Buffers.Binary;
using Shared.GameData.Items;

namespace Shared.Packets.PacketContent.Items;

public struct C2S_BuyItemRequestPacket
{
    
    public ItemId Item;

    public int Serialize(Span<byte> buffer)
    {
        
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)Item);
        return 2;

    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        Item = (ItemId)BinaryPrimitives.ReadUInt16LittleEndian(buffer);

    }

}