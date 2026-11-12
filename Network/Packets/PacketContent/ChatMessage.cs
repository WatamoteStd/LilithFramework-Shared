
using System.Buffers.Binary;
using System.Text;

namespace Shared.Packets.PacketContent;

/// <summary>
/// One chat line as the server keeps it: stamp is the simulation tick, not the
/// wall clock. SenderId is the peer id, so serviced creeps never write here.
/// </summary>
public readonly record struct ChatMessageRecord(uint Tick, uint SenderId, string Message);

/// <summary>
/// C2S_ChatMessage: 1 + N bytes, N up to 500. The server only checks the
/// length; there is no other validation by design.
/// </summary>
public struct C2S_ChatMessagePacket
{
    public const int MaxBytes = 500;

    public byte Length;
    public byte[] Text;

    public string GetText() => Text is null
        ? string.Empty
        : Encoding.UTF8.GetString(Text, 0, Length);

    public void SetText(string text)
    {
        byte[] raw = Encoding.UTF8.GetBytes(text ?? string.Empty);
        int length = Math.Min(raw.Length, MaxBytes);

        Text ??= new byte[MaxBytes];
        raw.AsSpan(0, length).CopyTo(Text);
        Length = (byte)length;
    }

    public int Serialize(Span<byte> buffer)
    {
        int length = Math.Min((int)Length, MaxBytes);

        buffer[0] = (byte)length;
        if (length > 0 && Text is not null) Text.AsSpan(0, length).CopyTo(buffer[1..]);

        return 1 + length;
    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        int length = buffer.Length > 0 ? buffer[0] : 0;
        length = Math.Min(length, MaxBytes);
        length = Math.Min(length, Math.Max(0, buffer.Length - 1));

        Text ??= new byte[MaxBytes];
        buffer.Slice(1, length).CopyTo(Text);
        Length = (byte)length;
    }
}

/// <summary>
/// S2C_ChatMessage: the stored record on the wire, 9 + N bytes.
/// </summary>
public struct S2C_ChatMessagePacket
{
    public const int MaxSize = 4 + 4 + 1 + C2S_ChatMessagePacket.MaxBytes;

    public uint Tick;
    public uint SenderId;
    public byte Length;
    public byte[] Text;

    public static S2C_ChatMessagePacket FromRecord(in ChatMessageRecord record)
    {
        byte[] raw = Encoding.UTF8.GetBytes(record.Message ?? string.Empty);
        int length = Math.Min(raw.Length, C2S_ChatMessagePacket.MaxBytes);

        S2C_ChatMessagePacket packet = default;
        packet.Text = new byte[C2S_ChatMessagePacket.MaxBytes];
        packet.Tick = record.Tick;
        packet.SenderId = record.SenderId;
        packet.Length = (byte)length;
        raw.AsSpan(0, length).CopyTo(packet.Text);

        return packet;
    }

    public ChatMessageRecord ToRecord()
    {
        string message = Text is null
            ? string.Empty
            : Encoding.UTF8.GetString(Text, 0, Math.Min((int)Length, C2S_ChatMessagePacket.MaxBytes));

        return new ChatMessageRecord(Tick, SenderId, message);
    }

    public int Serialize(Span<byte> buffer)
    {
        int length = Math.Min((int)Length, C2S_ChatMessagePacket.MaxBytes);

        BinaryPrimitives.WriteUInt32LittleEndian(buffer, Tick);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer[4..], SenderId);
        buffer[8] = (byte)length;

        if (length > 0 && Text is not null) Text.AsSpan(0, length).CopyTo(buffer[9..]);

        return 9 + length;
    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        Tick = BinaryPrimitives.ReadUInt32LittleEndian(buffer);
        SenderId = BinaryPrimitives.ReadUInt32LittleEndian(buffer[4..]);

        int length = buffer[8];
        length = Math.Min(length, C2S_ChatMessagePacket.MaxBytes);
        length = Math.Min(length, Math.Max(0, buffer.Length - 9));

        Text ??= new byte[C2S_ChatMessagePacket.MaxBytes];
        buffer.Slice(9, length).CopyTo(Text);
        Length = (byte)length;
    }
}
