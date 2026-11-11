using System.Buffers.Binary;

namespace Shared.Packets.PacketContent;

/// <summary>
/// C2S_HandshakeRequest: first packet of a connection. 9 bytes.
/// ClientId is generated once by the client (not inside Serialize!),
/// so a resent handshake carries the same identity.
/// </summary>
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
