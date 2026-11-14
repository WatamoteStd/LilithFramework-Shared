
using System.Buffers.Binary;

namespace Shared.Packets.PacketContent;

public struct S2C_SpawnProjectilePacket
{
    
    public ushort CasterId;
    public ushort TargetId;
    public float Speed;
    public float Damage;
    public ushort ProjectileId;

    public int Serialize(Span<byte> buffer)
    {
        
        int offset = 0;

        BinaryPrimitives.WriteUInt16LittleEndian(buffer[offset..], CasterId);
        offset += 2;
        BinaryPrimitives.WriteUInt16LittleEndian(buffer[offset..], TargetId);
        offset += 2;

        BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], Speed);
        offset += 4;
        BinaryPrimitives.WriteSingleLittleEndian(buffer[offset..], Damage);
        offset += 4;
        
        BinaryPrimitives.WriteUInt16LittleEndian(buffer[offset..], ProjectileId);
        offset += 2;

        return offset;

    }

    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        int offset = 0;

        CasterId = BinaryPrimitives.ReadUInt16LittleEndian(buffer[offset..]);
        offset += 2;
        TargetId = BinaryPrimitives.ReadUInt16LittleEndian(buffer[offset..]);
        offset += 2;

        Speed = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset += 4;
        Damage = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset += 4;

        ProjectileId = BinaryPrimitives.ReadUInt16LittleEndian(buffer[offset..]);

    }

}