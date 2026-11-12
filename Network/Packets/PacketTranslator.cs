
using System.Buffers.Binary;
using Shared.Packets.PacketContent;
using Shared.Packets.UDP;

namespace Shared.Packets;

/// <summary>
/// Packet type + payload. One Serialize/Read overload per packet struct:
/// no interfaces, no generics, no boxing, the exact byte layout is right here.
/// </summary>
public static class PacketTranslator
{
    
    public static int Serialize(PacketTypes type, Span<byte> buffer, ref C2S_HandshakeRequestPacket pck)
    {
        
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)type);
        int payloadSize = pck.Serialize(buffer[2..]);

        return payloadSize + 2;

    }
    public static void Read(ReadOnlySpan<byte> buffer, ref C2S_HandshakeRequestPacket pck)
    {
        pck.Deserialize(buffer);
    }

    public static int Serialize(PacketTypes type, Span<byte> buffer, ref S2C_HandshakeResponsePacket pck)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)type);
        int payloadSize = pck.Serialize(buffer[2..]);

        return payloadSize + 2;
    }
    public static void Read(ReadOnlySpan<byte> buffer, ref S2C_HandshakeResponsePacket pck)
    {
        pck.Deserialize(buffer);
    }

    public static int Serialize(PacketTypes type, Span<byte> buffer, ref S2C_WorldSnapshotPacket pck)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)type);
        int payloadSize = pck.Serialize(buffer[2..]);

        return payloadSize + 2;
    }
    public static void Read(ReadOnlySpan<byte> buffer, ref S2C_WorldSnapshotPacket pck)
    {
        pck.Deserialize(buffer);
    }

    public static int Serialize(PacketTypes type, Span<byte> buffer, ref C2S_MoveIntentPacket pck)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)type);
        int payloadSize = pck.Serialize(buffer[2..]);

        return payloadSize + 2;
    }
    public static void Read(ReadOnlySpan<byte> buffer, ref C2S_MoveIntentPacket pck)
    {
        pck.Deserialize(buffer);
    }

    public static int Serialize(PacketTypes type, Span<byte> buffer, ref C2S_AttackIntentPacket pck)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)type);
        int payloadSize = pck.Serialize(buffer[2..]);

        return payloadSize + 2;
    }
    public static void Read(ReadOnlySpan<byte> buffer, ref C2S_AttackIntentPacket pck)
    {
        pck.Deserialize(buffer);
    }

    public static int Serialize(PacketTypes type, Span<byte> buffer, ref C2S_LobbyJoinPacket pck)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)type);
        int payloadSize = pck.Serialize(buffer[2..]);

        return payloadSize + 2;
    }
    public static void Read(ReadOnlySpan<byte> buffer, ref C2S_LobbyJoinPacket pck)
    {
        pck.Deserialize(buffer);
    }

    public static int Serialize(PacketTypes type, Span<byte> buffer, ref C2S_LobbyReadyPacket pck)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)type);
        int payloadSize = pck.Serialize(buffer[2..]);

        return payloadSize + 2;
    }
    public static void Read(ReadOnlySpan<byte> buffer, ref C2S_LobbyReadyPacket pck)
    {
        pck.Deserialize(buffer);
    }

    public static int Serialize(PacketTypes type, Span<byte> buffer, ref S2C_LobbyWelcomePacket pck)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)type);
        int payloadSize = pck.Serialize(buffer[2..]);

        return payloadSize + 2;
    }
    public static void Read(ReadOnlySpan<byte> buffer, ref S2C_LobbyWelcomePacket pck)
    {
        pck.Deserialize(buffer);
    }

    public static int Serialize(PacketTypes type, Span<byte> buffer, ref S2C_LobbyStatePacket pck)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)type);
        int payloadSize = pck.Serialize(buffer[2..]);

        return payloadSize + 2;
    }
    public static void Read(ReadOnlySpan<byte> buffer, ref S2C_LobbyStatePacket pck)
    {
        pck.Deserialize(buffer);
    }

    public static int Serialize(PacketTypes type, Span<byte> buffer, ref C2S_ChatMessagePacket pck)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)type);
        int payloadSize = pck.Serialize(buffer[2..]);

        return payloadSize + 2;
    }
    public static void Read(ReadOnlySpan<byte> buffer, ref C2S_ChatMessagePacket pck)
    {
        pck.Deserialize(buffer);
    }

    public static int Serialize(PacketTypes type, Span<byte> buffer, ref S2C_ChatMessagePacket pck)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, (ushort)type);
        int payloadSize = pck.Serialize(buffer[2..]);

        return payloadSize + 2;
    }
    public static void Read(ReadOnlySpan<byte> buffer, ref S2C_ChatMessagePacket pck)
    {
        pck.Deserialize(buffer);
    }

}
