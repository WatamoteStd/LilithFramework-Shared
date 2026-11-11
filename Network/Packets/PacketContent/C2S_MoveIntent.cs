
using System.Buffers.Binary;

namespace Shared.Packets.PacketContent;

/// <summary>
/// C2S_MoveIntent: where the player wants the unit to walk. 8 bytes.
/// This is an intent, not a position: the server decides how and how fast.
/// </summary>
public struct C2S_MoveIntentPacket
{
    public float X;
    public float Z;

    public int Serialize(Span<byte> buffer)
    {
        BinaryPrimitives.WriteSingleLittleEndian(buffer, X);
        BinaryPrimitives.WriteSingleLittleEndian(buffer[4..], Z);
        return 8;
    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        X = BinaryPrimitives.ReadSingleLittleEndian(buffer);
        Z = BinaryPrimitives.ReadSingleLittleEndian(buffer[4..]);
    }
}
