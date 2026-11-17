
using System.Buffers.Binary;
using Shared.GameData.Units.Property;

namespace Shared.Packets.PacketContent;

/// <summary>
/// One cosmetic damage number. Sent only to the two players involved: the owner
/// of the attacker and the owner of the victim. The client finds the node by
/// VictimId and prints the amount above it, so 6 bytes are enough.
/// </summary>
public struct S2C_DamageNumberPacket
{

    public ushort VictimId;
    public float Amount;
    public DamageTypes DmgType;

    public const int Size = 7;

    public int Serialize(Span<byte> buffer)
    {

        int offset = 0;

        BinaryPrimitives.WriteUInt16LittleEndian(buffer[offset..], VictimId);
        offset += 2;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], Amount);
        offset += 4;
        buffer[offset] = (byte)DmgType;
        offset++;

        return offset;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {

        int offset = 0;

        VictimId = BinaryPrimitives.ReadUInt16LittleEndian(buffer[offset..]);
        offset += 2;
        Amount = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset += 4;
        DmgType = (DamageTypes)buffer[offset];

    }

}
