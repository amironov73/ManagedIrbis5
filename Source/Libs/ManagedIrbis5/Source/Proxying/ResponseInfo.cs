// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ResponseInfo.cs -- информация об ответе сервера в разобранном виде
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Proxying;

/// <summary>
/// Информация об ответе сервера.
/// </summary>
public sealed class ResponseInfo
{
    #region Properties

    /// <summary>
    /// Код команды.
    /// </summary>
    public string? Command { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Порядковый номер команды.
    /// </summary>
    public string? Index { get; set; }

    /// <summary>
    /// Зарезервировано.
    /// </summary>
    public string? Reserved1 { get; set; }

    /// <summary>
    /// Зарезервировано.
    /// </summary>
    public string? Reserved2 { get; set; }

    /// <summary>
    /// Зарезервировано.
    /// </summary>
    public string? Reserved3 { get; set; }

    /// <summary>
    /// Зарезервировано.
    /// </summary>
    public string? Reserved4 { get; set; }

    /// <summary>
    /// Зарезервировано.
    /// </summary>
    public string? Reserved5 { get; set; }

    /// <summary>
    /// Зарезервировано.
    /// </summary>
    public string? Reserved6 { get; set; }

    /// <summary>
    /// Зарезервировано.
    /// </summary>
    public string? Reserved7 { get; set; }

    /// <summary>
    /// Прочие данные, например, найденные записи.
    /// </summary>
    public byte[]? Data { get; set; }

    /// <summary>
    /// Прочие данные в текстовом виде.
    /// </summary>
    public string[]? Lines { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Код возврата (не для всех ответов имеет смысл).
    /// </summary>
    public string? ReturnCode
    {
        get
        {
            if ((Lines == null) || (Lines.Length == 0))
            {
                return null;
            }

            var result = Lines[0];
            result = OnlyNumbersAllowed (result);
            return result;
        }
    }

    // ====================================================================

    private static string? OnlyNumbersAllowed
        (
            string text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        var index = 0;
        if (text[0] == '-')
        {
            index++;
        }

        for (; index < text.Length; index++)
        {
            if (!char.IsDigit (text, index))
            {
                return null;
            }
        }

        return text;
    }

    /// <summary>
    /// Разбор ответа сервера по полям заголовка.
    /// </summary>
    public static ResponseInfo Parse
        (
            byte[] buffer,
            bool useUtf
        )
    {
        var parser = new HeaderParser (buffer);
        var result = new ResponseInfo
        {
            Command = parser.NextString(),
            ClientId = parser.NextString(),
            Index = parser.NextString(),
            Reserved1 = parser.NextString(),
            Reserved2 = parser.NextString(),
            Reserved3 = parser.NextString(),
            Reserved4 = parser.NextString(),
            Reserved5 = parser.NextString(),
            Reserved6 = parser.NextString(),
            Reserved7 = parser.NextString(),
            Data = parser.NextBytes()
        };

        result.Lines = parser.SplitLines
            (
                result.Data,
                useUtf
            );

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();

        builder.Append ($"Command: {Command}");
        builder.AppendLine();
        builder.Append ($"UserID: {ClientId}");
        builder.AppendLine();
        builder.Append ($"Index: {Index}");

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
