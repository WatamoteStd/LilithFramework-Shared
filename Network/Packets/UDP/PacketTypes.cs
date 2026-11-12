
namespace Shared.Packets.UDP;

public enum PacketTypes : ushort
{
    
    None = 0,
    C2S_HandshakeRequest = 1,
    S2C_HandshakeResponse = 2,
    S2C_WorldSnapshot = 3,
    C2S_MoveIntent = 4,
    C2S_AttackIntent = 5,
    C2S_LobbyJoin = 6,
    C2S_LobbyReady = 7,
    S2C_LobbyWelcome = 8,
    S2C_LobbyState = 9,
    C2S_ChatMessage = 10,
    S2C_ChatMessage = 11

}
