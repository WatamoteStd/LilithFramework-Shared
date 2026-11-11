using System.Text;

namespace Shared.Lobby;

/// <summary>
/// One nickname rule for client and server: <= 24 UTF-8 bytes, valid UTF-8,
/// no control characters, surrounding whitespace trimmed. Truncation happens
/// on rune boundaries, never inside a multi-byte character.
/// </summary>
public static class NicknameRules
{
    public const int MaxBytes = 24;

    private static readonly UTF8Encoding StrictUtf8 = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    /// <summary>
    /// Writes the normalized nickname into <paramref name="destination" />
    /// (must be at least <see cref="MaxBytes" /> long).
    /// Returns false when the input is not valid UTF-8 or is empty after trimming.
    /// </summary>
    public static bool TryNormalize(ReadOnlySpan<byte> raw, Span<byte> destination, out int length)
    {
        length = 0;

        string decoded;
        try
        {
            decoded = StrictUtf8.GetString(raw);
        }
        catch (DecoderFallbackException)
        {
            return false;
        }

        decoded = decoded.Trim();
        if (decoded.Length == 0) return false;

        int written = 0;
        foreach (Rune rune in decoded.EnumerateRunes())
        {
            if (Rune.IsControl(rune)) continue;

            int size = rune.Utf8SequenceLength;
            if (written + size > MaxBytes) break;

            rune.EncodeToUtf8(destination[written..]);
            written += size;
        }

        length = written;
        return written > 0;
    }

    public static string NormalizeOrFallback(ReadOnlySpan<byte> raw, string fallback)
    {
        Span<byte> buffer = stackalloc byte[MaxBytes];
        return TryNormalize(raw, buffer, out int length)
            ? Encoding.UTF8.GetString(buffer[..length])
            : fallback;
    }
}
