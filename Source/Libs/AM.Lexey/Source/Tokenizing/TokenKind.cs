// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TokenKind.cs -- известные виды токенов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Известные виды токенов. Вы не обязаны использовать
/// именно эти виды токенов, всегда можно изобрести
/// собственный тип и использовать его.
/// </summary>
[PublicAPI]
public static class TokenKind
{
    #region Constants

    /// <summary>
    /// Комментарий.
    /// </summary>
    public const string Comment = "comment";

    /// <summary>
    /// Пробелы.
    /// </summary>
    public const string Whitespace = "whitespace";

    /// <summary>
    /// Перевод строки.
    /// </summary>
    public const string NewLine = "newline";

    /// <summary>
    /// Директива.
    /// </summary>
    public const string Directive = "directive";

    /// <summary>
    /// Терм -- последовательность символов,
    /// воспинимаемая как некое целое.
    /// </summary>
    public const string Term = "term";

    /// <summary>
    /// Пунктуация, например, "{" или "++".
    /// </summary>
    public const string Punctuation = "punctuation";

    /// <summary>
    /// Зарезервированное слово, например, "if".
    /// </summary>
    public const string ReservedWord = "reserved";

    /// <summary>
    /// Идентификатор.
    /// </summary>
    public const string Identifier = "identifier";

    /// <summary>
    /// Один символ в одиночных кавычках.
    /// </summary>
    public const string Char = "char";

    /// <summary>
    /// Произвольное количество символов в двойных кавычках
    /// (строка).
    /// </summary>
    public const string String = "string";

    /// <summary>
    /// Строка без экранирования.
    /// </summary>
    public const string RawString = "raw";

    /// <summary>
    /// Форматная строка вида `$"{z} = {x} + {y}"`.
    /// </summary>
    public const string Format = "format";

    /// <summary>
    /// Внешний по отношению к Barsik код.
    /// </summary>
    public const string External = "external";

    /// <summary>
    /// Альтернативная строка <c>`hello world`</c>.
    /// </summary>
    public const string AltString = "alt";

    /// <summary>
    /// Целое 32-битное число со знаком без префикса и суффикса.
    /// </summary>
    public const string Int32 = "int32";

    /// <summary>
    /// Целое 64-битное число без знака без префикса, суффикс 'L'.
    /// </summary>
    public const string Int64 = "int64";

    /// <summary>
    /// Целое 32-битное число без знака без префикса, суффикс 'U'.
    /// </summary>
    public const string UInt32 = "uint32";

    /// <summary>
    /// Целое 64-битное число без знака без префикса, суффикс 'UL'.
    /// </summary>
    public const string UInt64 = "uint64";

    /// <summary>
    /// Целое 32-битное число без знака c префиксом '0x', без суффикса.
    /// </summary>
    public const string Hex32 = "hex32";

    /// <summary>
    /// Целое 64-битное число без знака c префиксом '0x', суффикс 'L'.
    /// </summary>
    public const string Hex64 = "hex64";

    /// <summary>
    /// Длинное целое число в десятеричной системе без префикса, суффикс 'B'.
    /// </summary>
    public const string BigInteger = "big-integer";

    /// <summary>
    /// Число с плавающей точкой с одинарной точностью.
    /// </summary>
    public const string Single = "single";

    /// <summary>
    /// Число с плавающей точкой с двойной точностью.
    /// </summary>
    public const string Double = "double";

    /// <summary>
    /// Число с фиксированной точкой (денежное).
    /// </summary>
    public const string Decimal = "decimal";

    #endregion
}
