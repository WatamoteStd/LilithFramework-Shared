
using System.Buffers.Binary;

namespace Shared.Packets.PacketContent;

/// <summary>
/// C2S_AttackIntent: "attack this entity". 2 bytes.
/// Range, cooldown and damage are all checked by the server.
/// </summary>
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
