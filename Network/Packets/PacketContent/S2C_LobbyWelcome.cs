using System.Buffers.Binary;

namespace Shared.Packets.PacketContent;

/// <summary>
/// S2C_LobbyWelcome: the server accepted the connection and tells the client
/// its lobby id. Lobby state entries are matched against this id.
/// </summary>
public struct S2C_LobbyWelcomePacket
{
    public uint PlayerId;

    public int Serialize(Span<byte> buffer)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, PlayerId);
        return 4;
    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        PlayerId = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
    }
}
