// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* Token.cs -- токен
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace AM.Text.Tokenizer;

/// <summary>
/// Текстовый токен.
/// </summary>
[XmlRoot("token")]
[DebuggerDisplay("Kind={Kind} Value='{Value}'")]
public class Token
    : IHandmadeSerializable
{
    #region Properties

    /// <summary>
    /// Тип токена.
    /// </summary>
    [XmlAttribute("kind")]
    [JsonPropertyName("kind")]
    public TokenKind Kind { get; private set; }

    /// <summary>
    /// Номер колонки.
    /// </summary>
    [XmlAttribute("column")]
    [JsonPropertyName("column")]
    public int Column { get; private set; }

    /// <summary>
    /// Номер строки.
    /// </summary>
    [XmlAttribute("line")]
    [JsonPropertyName("line")]
    public int Line { get; private set; }

    /// <summary>
    /// Значение.
    /// </summary>
    [XmlText]
    [JsonPropertyName("value")]
    public string? Value { get; internal set; }

    /// <summary>
    /// Признак конца текста?
    /// </summary>
    public bool IsEOF => Kind == TokenKind.EOF;

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public Token()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Token
        (
            TokenKind kind,
            string? value,
            int line,
            int column
        )
    {
        Kind = kind;
        Value = value;
        Line = line;
        Column = column;
    }

    #endregion

    #region Private members

    #endregion

    #region Public methods

    /// <summary>
    /// Convert array of words to array of tokens.
    /// </summary>
    public static Token[] Convert
        (
            string[] words
        )
    {
        var result = new Token[words.Length];
        for (var i = 0; i < words.Length; i++)
        {
            result[i] = new Token
                (
                    TokenKind.Unknown,
                    words[i],
                    0,
                    0
                );
        }

        return result;
    }

    /// <summary>
    /// Create token from <see cref="TextNavigator"/>.
    /// </summary>
    public static Token FromNavigator
        (
            TextNavigator navigator,
            string value
        )
    {
        Sure.NotNull (navigator);

        var result = new Token
            (
                TokenKind.Unknown,
                value,
                navigator.Line,
                navigator.Column
            );

        return result;
    }

    /// <summary>
    /// Create token from <see cref="TextNavigator"/>.
    /// </summary>
    public static Token FromNavigator
        (
            TextNavigator navigator,
            TokenKind kind,
            string value
        )
    {
        Sure.NotNull (navigator);

        var result = new Token
            (
                kind,
                value,
                navigator.Line,
                navigator.Column
            );

        return result;
    }

    /// <summary>
    /// Convert token to string.
    /// </summary>
    public static implicit operator string?
        (
            Token? token
        )
    {
        return token?.Value;
    }

    /// <summary>
    /// Convert text to token.
    /// </summary>
    public static implicit operator Token
        (
            string? text
        )
    {
        return new Token(TokenKind.Unknown, text, 0, 0);
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Kind = (TokenKind) reader.ReadPackedInt32();
        Column = reader.ReadPackedInt32();
        Line = reader.ReadPackedInt32();
        Value = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WritePackedInt32((int) Kind)
            .WritePackedInt32(Column)
            .WritePackedInt32(Line)
            .WriteNullable(Value);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return $"Kind: {Kind}, Column: {Column}, Line: {Line}, Value: {Value.ToVisibleString()}";
    }

    #endregion
}
