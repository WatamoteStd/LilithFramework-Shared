using System.Buffers.Binary;
using Shared.GameData.Units;
using Shared.Lobby;

namespace Shared.Packets.PacketContent;

/// <summary>
/// C2S_LobbyJoin: nickname + picked hero, sent whenever either changes.
/// Nickname is raw UTF-8 bytes (<= 24), validated by the server.
/// </summary>
public struct C2S_LobbyJoinPacket
{
    public const int MaxSize = 1 + NicknameRules.MaxBytes + 2;

    public byte NicknameLength;
    public byte[] Nickname;
    public HeroId Hero;

    public int Serialize(Span<byte> buffer)
    {
        byte length = (byte)Math.Min((int)NicknameLength, NicknameRules.MaxBytes);
        ReadOnlySpan<byte> nickname = Nickname is null ? ReadOnlySpan<byte>.Empty : Nickname.AsSpan(0, length);

        buffer[0] = (byte)nickname.Length;
        nickname.CopyTo(buffer[1..]);
        BinaryPrimitives.WriteUInt16LittleEndian(buffer[(1 + nickname.Length)..], (ushort)Hero);

        return 3 + nickname.Length;
    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        int length = buffer[0];
        int available = Math.Max(0, buffer.Length - 3);

        if (length > NicknameRules.MaxBytes) length = NicknameRules.MaxBytes;
        if (length > available) length = available;

        NicknameLength = (byte)length;
        Nickname = buffer.Slice(1, length).ToArray();
        Hero = (HeroId)BinaryPrimitives.ReadUInt16LittleEndian(buffer[(1 + length)..]);
    }
}
