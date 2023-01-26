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

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Настройки токенизации.
/// </summary>
public sealed class TokenizerSettings
{
    #region Properties

    /// <summary>
    /// Первый символ идентификатора.
    /// </summary>
    public char[] FirstIdentifierLetter { get; set; }

    /// <summary>
    /// Последующие символы идентификатора.
    /// </summary>
    public char[] NextIdentifierLetter { get; set; }

    /// <summary>
    /// Распознаваемые термы.
    /// </summary>
    public string[] KnownTerms { get; set; }

    /// <summary>
    /// Зарезервированные (ключевые) слова.
    /// </summary>
    public string[] ReservedWords { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TokenizerSettings()
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
            .ToCharArray();

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
            .ToCharArray();

        KnownTerms = new[]
        {
            "!", ";", ":", ",", "(", ")", "+", "-", "*", "/", "[", "]",
            "{", "}", "|", "%", "~", "=", "++", "--", "+=", "-=", "*=",
            "/=", "==", "<", ">", "<<", ">>", "<=", ">=", "||", "&&",
            ".", ",", "in", "is", "same", "<=>", "<:>", "@", "?", "??",
            "&", "===", "!==", "~~", "~~~", "<+>"
        };

        ReservedWords = new []
        {
            "abstract", "and", "as", "async", "await", "base", "bool", "break",
            "byte", "case", "catch", "char", "checked", "class", "const",
            "continue", "decimal", "default", "delegate", "do", "double",
            "else", "enum", "event", "explicit", "extern", "false", "finally",
            "fixed", "float", "for", "foreach", "func", "goto", "if", "implicit",
            "in", "int", "interface", "internal", "is", "lambda", "local", "lock",
            "long", "namespace", "new", "null", "object", "operator", "or", "out",
            "override", "params", "private", "protected", "public", "readonly",
            "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc",
            "static", "string", "struct", "switch", "this", "throw", "true",
            "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort",
            "using", "virtual", "void", "volatile", "while", "with"
        };
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Выдача настроек по умолчанию.
    /// </summary>
    public static TokenizerSettings CreateDefault()
    {
        return new TokenizerSettings();
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

        throw new NotImplementedException();
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

        throw new NotImplementedException();
    }

    #endregion
}
