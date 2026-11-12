using System.Buffers.Binary;
using Shared.GameData.Units;
using Shared.GameData.Units.Property;
using Shared.Lobby;

namespace Shared.Packets.PacketContent;


public struct LobbyPlayerState
{
    public const int MaxSize = 4 + 1 + NicknameRules.MaxBytes + 2 + 1 + 1;

    public uint PlayerId;
    public byte NicknameLength;
    public byte[] Nickname;
    public HeroId Hero;
    public MatchTeam Team;
    public byte Ready;

    public string GetNickname() => Nickname is null
        ? string.Empty
        : System.Text.Encoding.UTF8.GetString(Nickname, 0, NicknameLength);

    public void SetNickname(ReadOnlySpan<byte> utf8)
    {
        int length = Math.Min(utf8.Length, NicknameRules.MaxBytes);

        Nickname ??= new byte[NicknameRules.MaxBytes];
        utf8[..length].CopyTo(Nickname);
        NicknameLength = (byte)length;
    }
}

/// <summary>
/// S2C_LobbyState: full roster, 1 + N * (9..33) bytes. Sent on every change;
/// pre-match only, so a lost packet is fixed by the next one.
/// </summary>
public struct S2C_LobbyStatePacket
{
    public const int MaxPlayers = 10;
    public const int MaxSize = 1 + MaxPlayers * LobbyPlayerState.MaxSize;

    public byte Count;
    public LobbyPlayerState[] Players;

    public static S2C_LobbyStatePacket Create()
    {
        S2C_LobbyStatePacket packet = default;
        packet.Players = new LobbyPlayerState[MaxPlayers];

        for (int i = 0; i < MaxPlayers; i++)
        {
            packet.Players[i].Nickname = new byte[NicknameRules.MaxBytes];
        }

        return packet;
    }

    public int Serialize(Span<byte> buffer)
    {
        buffer[0] = Count;

        int offset = 1;
        for (int i = 0; i < Count; i++)
        {
            ref LobbyPlayerState p = ref Players[i];
            int length = Math.Min((int)p.NicknameLength, NicknameRules.MaxBytes);

            BinaryPrimitives.WriteUInt32LittleEndian(buffer[offset..], p.PlayerId);
            buffer[offset + 4] = (byte)length;

            if (p.Nickname is not null && length > 0)
            {
                p.Nickname.AsSpan(0, length).CopyTo(buffer[(offset + 5)..]);
            }

            int heroOffset = offset + 5 + length;
            BinaryPrimitives.WriteUInt16LittleEndian(buffer[heroOffset..], (ushort)p.Hero);
            buffer[heroOffset + 2] = (byte)p.Team;
            buffer[heroOffset + 3] = p.Ready;

            offset = heroOffset + 4;
        }

        return offset;
    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Players ??= new LobbyPlayerState[MaxPlayers];

        int count = Math.Min((int)buffer[0], MaxPlayers);
        Count = (byte)count;

        int offset = 1;
        for (int i = 0; i < count; i++)
        {
            ref LobbyPlayerState p = ref Players[i];

            p.PlayerId = BinaryPrimitives.ReadUInt32LittleEndian(buffer[offset..]);

            int length = Math.Min((int)buffer[offset + 4], NicknameRules.MaxBytes);
            length = Math.Min(length, Math.Max(0, buffer.Length - offset - 9));

            p.Nickname ??= new byte[NicknameRules.MaxBytes];
            buffer.Slice(offset + 5, length).CopyTo(p.Nickname);
            p.NicknameLength = (byte)length;

            int heroOffset = offset + 5 + length;
            p.Hero = (HeroId)BinaryPrimitives.ReadUInt16LittleEndian(buffer[heroOffset..]);
            p.Team = (MatchTeam)buffer[heroOffset + 2];
            p.Ready = buffer[heroOffset + 3];

            offset = heroOffset + 4;
        }
    }
}
