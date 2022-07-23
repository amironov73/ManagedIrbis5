// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FieldSpecification -- спецификация поля/подполя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Text;

using AM;
using AM.IO;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Serialization;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure;

/// <summary>
/// Спецификация поля/подполя.
/// </summary>
public sealed class FieldSpecification
    : ICloneable
{
    #region Properties

    /// <summary>
    /// Command code (must be lowercase).
    /// </summary>
    public char Command { get; set; }

    /// <summary>
    /// Embedded field tag.
    /// </summary>
    public string? Embedded { get; set; }

    /// <summary>
    /// Красная строка.
    /// </summary>
    public int FirstLine { get; set; }

    /// <summary>
    /// Общий абзацный отступ.
    /// </summary>
    public int ParagraphIndent { get; set; }

    /// <summary>
    /// Смещение.
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// Длина.
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// Field repeat.
    /// </summary>
    public IndexSpecification FieldRepeat { get; set; }

    /// <summary>
    /// Subfield.
    /// </summary>
    public char SubField { get; set; }

    /// <summary>
    /// Subfield repeat.
    /// </summary>
    public IndexSpecification SubFieldRepeat { get; set; }

    /// <summary>
    /// Tag.
    /// </summary>
    public int Tag { get; set; }

    /// <summary>
    /// Tag specification.
    /// </summary>
    public string? TagSpecification { get; set; }

    /// <summary>
    /// Subfield specification.
    /// </summary>
    public string? SubFieldSpecification { get; set; }

    /// <summary>
    /// Unparsed field specification.
    /// </summary>
    public string? RawText { get; set; }

    /// <summary>
    ///
    /// </summary>
    public static bool ParseSubFieldSpecification { get; set; } = true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public FieldSpecification()
    {
    } // constructor

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FieldSpecification
        (
            string text
        )
    {
        if (!Parse (text))
        {
            throw new IrbisException();
        }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public FieldSpecification
        (
            int tag,
            char code
        )
    {
        Tag = tag;
        SubField = code;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public FieldSpecification
        (
            int tag
        )
    {
        Tag = tag;
    }

    #endregion

    #region Private members

    private static readonly char[] _openChars = { '[' };
    private static readonly char[] _closeChars = { ']' };
    private static readonly char[] _stopChars = { ']' };

    private IndexSpecification _ParseIndex
        (
            TextNavigator navigator,
            string text
        )
    {
        text = text.Trim();
        if (string.IsNullOrEmpty (text))
        {
            Magna.Logger.LogError
                (
                    nameof (SubFieldSpecification) + "::" + nameof (_ParseIndex)
                    + ": text={Text}",
                    text.ToVisibleString()
                );

            throw new PftSyntaxException (navigator);
        }

        var result = new IndexSpecification
        {
            Expression = text
        };

        if (text == "*")
        {
            result.Kind = IndexKind.LastRepeat;
        }
        else if (text == "+")
        {
            result.Kind = IndexKind.NewRepeat;
        }
        else if (text == "-")
        {
            result.Kind = IndexKind.AllRepeats;
        }
        else if (text == ".")
        {
            result.Kind = IndexKind.CurrentRepeat;
        }
        else
        {
            if (Utility.TryParseInt32 (text, out var index))
            {
                result.Kind = IndexKind.Literal;
                result.Literal = index;
            }
            else
            {
                result.Kind = IndexKind.Expression;
                result.Expression = text;
            }
        }

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Compare two specifications.
    /// </summary>
    public static bool Compare
        (
            FieldSpecification left,
            FieldSpecification right
        )
    {
        Sure.NotNull (left);
        Sure.NotNull (right);

        var result = left.Command == right.Command
                     && PftSerializationUtility.CompareStrings
                         (
                             left.Embedded, right.Embedded
                         )
                     && left.FirstLine == right.FirstLine
                     && left.ParagraphIndent == right.ParagraphIndent
                     && left.Offset == right.Offset
                     && left.Length == right.Length
                     && IndexSpecification.Compare
                         (
                             left.FieldRepeat,
                             right.FieldRepeat
                         )
                     && left.SubField == right.SubField
                     && IndexSpecification.Compare
                         (
                             left.SubFieldRepeat,
                             right.SubFieldRepeat
                         )
                     && left.Tag == right.Tag
                     && PftSerializationUtility.CompareStrings
                         (
                             left.TagSpecification,
                             right.TagSpecification
                         )
                     && PftSerializationUtility.CompareStrings
                         (
                             left.SubFieldSpecification,
                             right.SubFieldSpecification
                         );

        return result;
    }

    /// <summary>
    /// Deserialize the specification.
    /// </summary>
    public void Deserialize
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Command = reader.ReadChar();
        Embedded = reader.ReadNullableString();
        FirstLine = reader.ReadPackedInt32();
        ParagraphIndent = reader.ReadPackedInt32();
        Offset = reader.ReadPackedInt32();
        Length = reader.ReadPackedInt32();
        FieldRepeat.Deserialize (reader);
        SubField = reader.ReadChar();
        SubFieldRepeat.Deserialize (reader);
        Tag = reader.ReadPackedInt32();
        TagSpecification = reader.ReadNullableString();
        SubFieldSpecification = reader.ReadNullableString();
        RawText = reader.ReadNullableString();
        ParseSubFieldSpecification = reader.ReadBoolean();
    }

    /// <summary>
    /// Parse the specification from text.
    /// </summary>
    public bool Parse
        (
            string text
        )
    {
        Sure.NotNull (text);

        var navigator = new TextNavigator (text);

        return Parse (navigator);
    }

    /// <summary>
    /// Parse the specification from navigator.
    /// </summary>
    public bool Parse
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        var start = navigator.Position;
        var saved = navigator.SavePosition();
        var c = navigator.ReadChar();
        var builder = new StringBuilder();

        switch (c)
        {
            case 'd':
            case 'D':
                Command = 'd';
                break;

            case 'g':
            case 'G':
                Command = 'g';
                break;

            case 'n':
            case 'N':
                Command = 'n';
                break;

            case 'v':
            case 'V':
                Command = 'v';
                break;

            default:
                navigator.RestorePosition (saved);
                return false;
        }

        c = navigator.ReadCharNoCrLf();

        if (c == '[')
        {
            var text = navigator.ReadUntil
                (
                    _openChars,
                    _closeChars,
                    _stopChars
                ).ToString();
            if (ReferenceEquals (text, null))
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (Parse)
                        + ": unclosed ["
                    );

                throw new PftSyntaxException (navigator);
            }

            text = text.Trim();
            if (string.IsNullOrEmpty (text))
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (Parse)
                        + "empty []"
                    );

                throw new PftSyntaxException (navigator);
            }

            TagSpecification = text;

            navigator.ReadCharNoCrLf();
        }
        else
        {
            if (!c.IsArabicDigit())
            {
                return false;
            }

            builder.Append (c);

            while (true)
            {
                c = navigator.PeekCharNoCrLf();
                if (!c.IsArabicDigit())
                {
                    break;
                }

                navigator.ReadCharNoCrLf();
                builder.Append (c);
            }

            Tag = builder.ToString().SafeToInt32();
        }

        navigator.SkipWhitespace();
        c = navigator.PeekCharNoCrLf();

        // now c is peeked char

        if (c == '@')
        {
            builder.Length = 0;
            navigator.ReadCharNoCrLf();

            navigator.SkipWhitespace();

            while (true)
            {
                c = navigator.PeekCharNoCrLf();
                if (!c.IsArabicDigit())
                {
                    break;
                }

                navigator.ReadCharNoCrLf();
                builder.Append (c);
            }

            if (builder.Length == 0)
            {
                throw new PftSyntaxException (navigator);
            }

            Embedded = builder.ToString();
        } // c == '@'

        navigator.SkipWhitespace();
        c = navigator.PeekCharNoCrLf();

        if (c == '[')
        {
            // parse the field repeat

            navigator.ReadCharNoCrLf();
            navigator.SkipWhitespace();

            var text = navigator.ReadUntil
                (
                    _openChars,
                    _closeChars,
                    _stopChars
                ).ToString();
            if (ReferenceEquals (text, null))
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (Parse)
                        + ": unclosed ["
                    );

                throw new PftSyntaxException (navigator);
            }

            FieldRepeat = _ParseIndex
                (
                    navigator,
                    text
                );

            navigator.ReadCharNoCrLf();
        }

        navigator.SkipWhitespace();
        c = navigator.PeekCharNoCrLf();

        if (c == '^')
        {
            navigator.ReadCharNoCrLf();
            if (navigator.IsEOF)
            {
                throw new PftSyntaxException (navigator);
            }

            c = navigator.ReadCharNoCrLf();

            if (c == '[' & ParseSubFieldSpecification)
            {
                var text = navigator.ReadUntil
                    (
                        _openChars,
                        _closeChars,
                        _stopChars
                    ).ToString();
                if (string.IsNullOrEmpty (text))
                {
                    var c2 = navigator.PeekCharNoCrLf();
                    if (c2 == ']')
                    {
                        SubFieldSpecification = null;
                        navigator.ReadCharNoCrLf();
                    }
                    else
                    {
                        SubField = c;
                    }
                }
                else
                {
                    SubFieldSpecification = text;

                    navigator.ReadCharNoCrLf();
                }
            }
            else
            {
                if (!SubFieldCode.IsValidCode (c))
                {
                    Magna.Logger.LogError
                        (
                            nameof (FieldSpecification) + "::" + nameof (Parse)
                            + "unexpected code={Code}",
                            c
                        );

                    throw new PftSyntaxException (navigator);
                }

                SubField = SubFieldCode.Normalize (c);
            }

            navigator.SkipWhitespace();
            c = navigator.PeekCharNoCrLf();

            // parse subfield repeat

            if (c == '[')
            {
                navigator.ReadCharNoCrLf();
                navigator.SkipWhitespace();

                var text = navigator.ReadUntil
                    (
                        _openChars,
                        _closeChars,
                        _stopChars
                    ).ToString();
                if (ReferenceEquals (text, null))
                {
                    Magna.Logger.LogError
                        (
                            nameof (FieldSpecification) + "::" + nameof (Parse)
                            + ": unclosed ["
                        );

                    throw new PftSyntaxException (navigator);
                }

                SubFieldRepeat = _ParseIndex
                    (
                        navigator,
                        text
                    );

                navigator.ReadCharNoCrLf();
            }
        } // c == '^'

        if (Command != 'v'
            && Command != 'g')
        {
            goto DONE;
        }

        navigator.SkipWhitespace();
        c = navigator.PeekCharNoCrLf();

        if (c == '*')
        {
            navigator.ReadCharNoCrLf();
            navigator.SkipWhitespace();
            builder.Length = 0;

            while (true)
            {
                c = navigator.PeekCharNoCrLf();
                if (!c.IsArabicDigit())
                {
                    break;
                }

                navigator.ReadCharNoCrLf();
                builder.Append (c);
            }

            if (builder.Length == 0)
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (Parse)
                        + ": empty offset"
                    );

                throw new PftSyntaxException (navigator);
            }

            Offset = int.Parse
                (
                    builder.ToString(),
                    CultureInfo.InvariantCulture
                );
        } // c == '*'

        navigator.SkipWhitespace();
        c = navigator.PeekCharNoCrLf();

        if (c == '.')
        {
            navigator.ReadCharNoCrLf();
            navigator.SkipWhitespace();
            builder.Length = 0;

            while (true)
            {
                c = navigator.PeekCharNoCrLf();
                if (!c.IsArabicDigit())
                {
                    break;
                }

                navigator.ReadCharNoCrLf();
                builder.Append (c);
            }

            if (builder.Length == 0)
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (Parse)
                        + ": empty length"
                    );

                throw new PftSyntaxException (navigator);
            }

            Length = int.Parse
                (
                    builder.ToString(),
                    CultureInfo.InvariantCulture
                );

            if (navigator.PeekChar() == '*')
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (Parse)
                        + ": offset after length"
                    );

                throw new PftSyntaxException (navigator);
            }
        } // c == '.'

        navigator.SkipWhitespace();
        c = navigator.PeekCharNoCrLf();

        if (c == '(')
        {
            navigator.ReadCharNoCrLf();
            navigator.SkipWhitespace();
            builder.Length = 0;

            while (true)
            {
                c = navigator.PeekCharNoCrLf();
                if (c == ')')
                {
                    navigator.ReadCharNoCrLf();
                    break;
                }

                if (!c.IsArabicDigit())
                {
                    Magna.Logger.LogError
                        (
                            nameof (FieldSpecification) + "::" + nameof (Parse)
                            + ": unexpected character={Code}",
                            c
                        );

                    throw new PftSyntaxException (navigator);
                }

                navigator.ReadCharNoCrLf();
                builder.Append (c);
            }

            if (builder.Length == 0)
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (Parse)
                        + ": empty paragraph indent"
                    );

                throw new PftSyntaxException (navigator);
            }

            ParagraphIndent = int.Parse
                (
                    builder.ToString(),
                    CultureInfo.InvariantCulture
                );

            // TODO FirstLine
        } // c == '('

        DONE:
        var length = navigator.Position - start;
        RawText = navigator.Substring (start, length).ToString();

        return true;
    }

    /// <summary>
    /// Parse short specification from text.
    /// </summary>
    public bool ParseShort
        (
            string text
        )
    {
        Sure.NotNull (text);

        var navigator = new TextNavigator (text);

        return ParseShort (navigator);
    }

    /// <summary>
    /// Parse short specification from navigator.
    /// </summary>
    public bool ParseShort
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        var start = navigator.Position;
        var saved = navigator.SavePosition();
        var c = navigator.ReadChar();
        var builder = new StringBuilder();

        switch (c)
        {
            case 'g':
            case 'G':
                Command = 'g';
                break;

            case 'v':
            case 'V':
                Command = 'v';
                break;

            default:
                navigator.RestorePosition (saved);
                return false;
        }

        c = navigator.ReadChar();
        if (!c.IsArabicDigit())
        {
            return false;
        }

        builder.Append (c);

        while (true)
        {
            c = navigator.PeekChar();
            if (!c.IsArabicDigit())
            {
                break;
            }

            navigator.ReadChar();
            builder.Append (c);
        }

        Tag = builder.ToString().SafeToInt32();

        navigator.SkipWhitespace();
        c = navigator.PeekChar();

        if (c == '^')
        {
            navigator.ReadChar();
            if (navigator.IsEOF)
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (ParseShort)
                        + ": unexpected end of stream"
                    );

                throw new PftSyntaxException (navigator);
            }

            c = navigator.ReadChar();
            if (!SubFieldCode.IsValidCode (c))
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (ParseShort)
                        + ": unexpected code={Code}",
                        c
                    );

                throw new PftSyntaxException (navigator);
            }

            SubField = SubFieldCode.Normalize (c);

            /* c = navigator.PeekChar(); */
        } // c == '^'

        var length = navigator.Position - start;
        RawText = navigator.Substring (start, length).ToString();

        return true;
    }

    /// <summary>
    /// Parse specification for Unifor.
    /// </summary>
    public bool ParseUnifor
        (
            string text
        )
    {
        Sure.NotNull (text);

        var navigator = new TextNavigator (text);

        return ParseUnifor (navigator);
    }

    /// <summary>
    /// Parse short specification for Unifor from navigator.
    /// </summary>
    public bool ParseUnifor
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        var start = navigator.Position;
        var saved = navigator.SavePosition();
        var c = navigator.ReadChar();
        var builder = new StringBuilder();

        switch (c)
        {
            case 'v':
            case 'V':
                Command = 'v';
                break;

            default:
                navigator.RestorePosition (saved);
                return false;
        }

        c = navigator.ReadChar();
        if (!c.IsArabicDigit())
        {
            return false;
        }

        builder.Append (c);

        while (true)
        {
            c = navigator.PeekChar();
            if (!c.IsArabicDigit())
            {
                break;
            }

            navigator.ReadChar();
            builder.Append (c);
        }

        Tag = builder.ToString().SafeToInt32();

        // now c is peeked char

        if (c == '^')
        {
            navigator.ReadChar();
            if (navigator.IsEOF)
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (ParseUnifor)
                        + ": unexpected end of stream"
                    );

                throw new PftSyntaxException (navigator);
            }

            c = navigator.ReadChar();
            if (!SubFieldCode.IsValidCode (c))
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (ParseUnifor)
                        + "unexpected code={Code}",
                        c
                    );

                throw new PftSyntaxException (navigator);
            }

            SubField = SubFieldCode.Normalize (c);

            c = navigator.PeekChar();
        } // c == '^'

        if (c == '*')
        {
            navigator.ReadCharNoCrLf();
            navigator.SkipWhitespace();
            builder.Length = 0;

            while (true)
            {
                c = navigator.PeekCharNoCrLf();
                if (!c.IsArabicDigit())
                {
                    break;
                }

                navigator.ReadCharNoCrLf();
                builder.Append (c);
            }

            if (builder.Length == 0)
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (ParseUnifor)
                        + ": empty offset"
                    );

                throw new PftSyntaxException (navigator);
            }

            Offset = int.Parse
                (
                    builder.ToString(),
                    CultureInfo.InvariantCulture
                );

            c = navigator.PeekChar();
        } // c == '*'

        if (c == '.')
        {
            navigator.ReadCharNoCrLf();
            navigator.SkipWhitespace();
            builder.Length = 0;

            while (true)
            {
                c = navigator.PeekCharNoCrLf();
                if (!c.IsArabicDigit())
                {
                    break;
                }

                navigator.ReadCharNoCrLf();
                builder.Append (c);
            }

            if (builder.Length == 0)
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (ParseUnifor)
                        + ": empty length"
                    );

                throw new PftSyntaxException (navigator);
            }

            Length = int.Parse
                (
                    builder.ToString(),
                    CultureInfo.InvariantCulture
                );

            if (navigator.PeekChar() == '*')
            {
                Magna.Logger.LogError
                    (
                        nameof (FieldSpecification) + "::" + nameof (ParseUnifor)
                        + ": offset after length"
                    );

                throw new PftSyntaxException (navigator);
            }
        } // c == '.'

        if (c == '#')
        {
            navigator.ReadChar();

            if (navigator.PeekChar() == '*')
            {
                navigator.ReadChar();
                FieldRepeat = new IndexSpecification
                {
                    Kind = IndexKind.LastRepeat,
                    Expression = "*"
                };
            }
            else
            {
                var minus = navigator.PeekChar() == '-';
                if (minus)
                {
                    navigator.ReadChar();
                }

                var indexText = navigator.ReadInteger().ToString();
                if (string.IsNullOrEmpty (indexText))
                {
                    Magna.Logger.LogError
                        (
                            nameof (FieldSpecification) + "::" + nameof (ParseUnifor)
                            + ": empty index"
                        );

                    throw new PftSyntaxException (navigator);
                }

                var indexValue = int.Parse (indexText);
                if (minus)
                {
                    indexValue = -indexValue;
                }

                FieldRepeat = new IndexSpecification
                {
                    Kind = IndexKind.Literal,
                    Expression = indexText,
                    Literal = indexValue
                };
            }
        }

        var length = navigator.Position - start;
        RawText = navigator.Substring (start, length).ToString();

        return true;
    }

    /// <summary>
    /// Serialize the specification.
    /// </summary>
    public void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.Write (Command);
        writer.WriteNullable (Embedded);
        writer.WritePackedInt32 (FirstLine);
        writer.WritePackedInt32 (ParagraphIndent);
        writer.WritePackedInt32 (Offset);
        writer.WritePackedInt32 (Length);
        FieldRepeat.Serialize (writer);
        writer.Write (SubField);
        SubFieldRepeat.Serialize (writer);
        writer.WritePackedInt32 (Tag);
        writer.WriteNullable (TagSpecification);
        writer.WriteNullable (SubFieldSpecification);
        writer.WriteNullable (RawText);
        writer.Write (ParseSubFieldSpecification);
    }

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public object Clone()
    {
        var result = (FieldSpecification) MemberwiseClone();

        result.FieldRepeat = (IndexSpecification) FieldRepeat.Clone();
        result.SubFieldRepeat = (IndexSpecification) SubFieldRepeat.Clone();

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();

        builder.Append (Command);
        builder.Append (Tag);
        if (!string.IsNullOrEmpty (Embedded))
        {
            builder.Append ('@');
            builder.Append (Embedded);
        }

        if (SubField != '\0')
        {
            builder.Append ('^');
            builder.Append (SubField);
        }

        if (Offset != 0)
        {
            builder.Append ('*');
            builder.Append (Offset);
        }

        if (Length != 0)
        {
            builder.Append ('.');
            builder.Append (Length);
        }

        if (ParagraphIndent != 0)
        {
            builder.Append ('(');
            builder.Append (ParagraphIndent);
            builder.Append (')');
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
