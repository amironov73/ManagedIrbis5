// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* TokenizerSettings.cs -- настройки токенизации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

using AM.Json;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Tokenizing;

/// <summary>
/// Настройки токенизации.
/// </summary>
[PublicAPI]
public sealed class TokenizerSettings
{
    #region Properties

    /// <summary>
    /// Первый символ идентификатора.
    /// </summary>
    [JsonPropertyName ("first-letter")]
    public char[] FirstIdentifierLetter { get; set; }

    /// <summary>
    /// Последующие символы идентификатора.
    /// </summary>
    [JsonPropertyName ("next-letter")]
    public char[] NextIdentifierLetter { get; set; }

    /// <summary>
    /// Распознаваемые термы.
    /// </summary>
    [JsonPropertyName ("known-terms")]
    public string[] KnownTerms { get; set; }

    /// <summary>
    /// Зарезервированные (ключевые) слова.
    /// </summary>
    [JsonPropertyName ("reserved-words")]
    public string[] ReservedWords { get; set; }

    /// <summary>
    /// Токенайзеры.
    /// </summary>
    [JsonPropertyName ("tokenizers")]
    public List<Tokenizer>? Tokenizers { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TokenizerSettings()
    {
        FirstIdentifierLetter = Array.Empty<char>();
        NextIdentifierLetter = Array.Empty<char>();
        KnownTerms = Array.Empty<string>();
        ReservedWords = Array.Empty<string>();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Выдача настроек по умолчанию.
    /// </summary>
    public static TokenizerSettings CreateDefault()
    {
        return new TokenizerSettings
        {
            FirstIdentifierLetter =
                (
                    "abcdefghijklmnopqrstuvwxyz" // строчная латиница
                    + "ABCDEFGHIJKLMNOPQRSTUVWXYZ" // заглавная латиница
                    + "абвгдеёжзийклмнопрстуфхцчшщъыьэюя" // строчная кириллица
                    + "АБСГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" // заглавная кириллица
                    + "αβϐγδεϵζηθϑικϰλμνξοπϖρϱσςτυφϕχψω" // строчные греческие
                    + "ΑΒΓΔΕΖΗΘϴΙΚΛΜΝΞΟΠΡΣΤΥϒΦΧΨΩ" // заглавные греческие
                    + "_$"
                )
                .ToCharArray(),

            NextIdentifierLetter =
                (
                    "abcdefghijklmnopqrstuvwxyz" // строчная латиница
                    + "ABCDEFGHIJKLMNOPQRSTUVWXYZ" // заглавная латиница
                    + "0123456789" // цифры
                    + "абвгдеёжзийклмнопрстуфхцчшщъыьэюя" // строчная кирилица
                    + "АБСГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" // заглавная кириллица
                    + "αβϐγδεϵζηθϑικϰλμνξοπϖρϱσςτυφϕχψω" // строчные греческие
                    + "ΑΒΓΔΕΖΗΘϴΙΚΛΜΝΞΟΠΡΣΤΥϒΦΧΨΩ" // заглавные греческие
                    + "_$"
                )
                .ToCharArray(),

            KnownTerms = new[]
            {
                "!", ";", ":", ",", "(", ")", "+", "-", "*", "/", "[", "]",
                "{", "}", "|", "%", "~", "=", "++", "--", "+=", "-=", "*=",
                "/=", "==", "<", ">", "<<", ">>", "<=", ">=", "||", "&&",
                ".", ",", "in", "is", "<=>", "@", "?", "??", "&",
                "!=", "===", "!==", "~~",
            },

            ReservedWords = new []
            {
                "abstract", "and", "as", "async", "await", "base", "bool", "break",
                "by", "byte", "case", "catch", "char", "checked", "class", "const",
                "continue", "decimal", "default", "delegate", "descending", "do",
                "double", "else", "enum", "equals", "event", "explicit", "extern",
                "false", "finally", "fixed", "float", "for", "foreach", "from", "func",
                "goto", "group", "if", "implicit", "in", "int", "interface", "internal",
                "is", "join", "lambda", "let", "local", "lock", "long", "namespace",
                "new", "null", "object", "on", "operator", "or", "orderby", "out",
                "override", "params", "private", "protected", "public", "readonly", "ref",
                "return", "sbyte", "sealed", "select", "short", "sizeof", "stackalloc",
                "static", "string", "struct", "switch", "this", "throw", "true", "try",
                "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using",
                "virtual", "void", "volatile", "where", "while", "with"
            }
        };
    }

    /// <summary>
    /// Чтение настроек из JSON-файла.
    /// </summary>
    public static TokenizerSettings Load
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        return JsonUtility.ReadObjectFromFile<TokenizerSettings> (fileName);
    }

    /// <summary>
    /// Сохранение настроек в JSON-файл.
    /// </summary>
    public void Save
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var text = JsonUtility.SerializeIndented (this);
        File.WriteAllText (fileName, text);
    }

    #endregion
}
