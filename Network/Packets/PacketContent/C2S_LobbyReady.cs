namespace Shared.Packets.PacketContent;

/// <summary>
/// C2S_LobbyReady: 1 byte, toggles the ready flag in the lobby roster.
/// </summary>
public struct C2S_LobbyReadyPacket
{
    public byte Ready;

    public int Serialize(Span<byte> buffer)
    {
        buffer[0] = Ready;
        return 1;
    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Ready = buffer[0];
    }
}
