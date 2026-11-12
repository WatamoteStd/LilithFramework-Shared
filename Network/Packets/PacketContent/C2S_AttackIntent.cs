
using System.Buffers.Binary;

namespace Shared.Packets.PacketContent;
public struct C2S_AttackIntentPacket
{
    public ushort TargetEntityId;

    public int Serialize(Span<byte> buffer)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, TargetEntityId);
        return 2;
    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        TargetEntityId = BinaryPrimitives.ReadUInt16LittleEndian(buffer);
    }
}
