using System.Buffers.Binary;

namespace Shared.Packets.PacketContent;


public struct C2S_HandshakeRequestPacket
{
    // 2: handshake response is deferred until the lobby starts the match.
    public const byte CurrentProtocolVersion = 2;

    public byte Version;
    public ulong ClientId;

    public int Serialize(Span<byte> buffer)
    {
        buffer[0] = Version;
        BinaryPrimitives.WriteUInt64LittleEndian(buffer[1..], ClientId);
        return 9;
    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Version = buffer[0];
        ClientId = BinaryPrimitives.ReadUInt64LittleEndian(buffer[1..]);
    }

}
