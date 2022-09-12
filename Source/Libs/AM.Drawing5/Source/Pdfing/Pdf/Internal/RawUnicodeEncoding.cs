// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Internal;

/// <summary>
/// An encoder for Unicode strings.
/// (That means, a character represents a glyph index.)
/// </summary>
internal sealed class RawUnicodeEncoding
    : Encoding
{
    public override int GetByteCount (char[] chars, int index, int count)
    {
        // Each character represents exactly an ushort value, which is a glyph index.
        return 2 * count;
    }

    public override int GetBytes (char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        for (var count = charCount; count > 0; charIndex++, count--)
        {
            var ch = chars[charIndex];
            bytes[byteIndex++] = (byte)(ch >> 8);
            bytes[byteIndex++] = (byte)ch;
        }

        return charCount * 2;
    }

    public override int GetCharCount (byte[] bytes, int index, int count)
    {
        return count / 2;
    }

    public override int GetChars (byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        for (var count = byteCount; count > 0; byteIndex += 2, charIndex++, count--)
        {
            chars[charIndex] = (char)((int)bytes[byteIndex] << 8 + (int)bytes[byteIndex + 1]);
        }

        return byteCount;
    }

    public override int GetMaxByteCount (int charCount)
    {
        return charCount * 2;
    }

    public override int GetMaxCharCount (int byteCount)
    {
        return byteCount / 2;
    }
}
