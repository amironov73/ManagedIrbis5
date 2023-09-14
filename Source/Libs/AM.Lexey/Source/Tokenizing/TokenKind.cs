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
/// собственный вид и использовать его.
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
    /// воспринимаемая как некое целое.
    /// Например, "++" или "+=".
    /// Может состоять из единственного символа, например, "{".
    /// </summary>
    public const string Term = "term";

    /// <summary>
    /// Нераспознанный остаток текста.
    /// </summary>
    public const string Remainder = "remainder";

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
    public const string Interpolation = "interpolation";

    /// <summary>
    /// Внешний по отношению к Barsik код.
    /// </summary>
    public const string External = "external";

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
    /// Длинное целое число в десятеричной системе без префикса,
    /// суффикс 'N'.
    /// </summary>
    public const string BigInteger = "big-integer";

    /// <summary>
    /// Число с плавающей точкой с одинарной точностью.
    /// Суффикс 'F'.
    /// </summary>
    public const string Single = "single";

    /// <summary>
    /// Число с плавающей точкой с двойной точностью.
    /// Без суффикса.
    /// </summary>
    public const string Double = "double";

    /// <summary>
    /// Число с фиксированной точкой (денежное).
    /// Суффикс 'M'.
    /// </summary>
    public const string Decimal = "decimal";

    #endregion
}
