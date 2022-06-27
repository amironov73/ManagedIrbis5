// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* RequestInfo.cs -- клиентский запрос в разобранном виде
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Proxying;

/// <summary>
/// Клиентский запрос в разобранном виде.
/// </summary>
public sealed class RequestInfo
{
    #region Properties

    /// <summary>
    /// Код команды.
    /// </summary>
    public string? Command1 { get; set; }

    /// <summary>
    /// Тип АРМ.
    /// </summary>
    public string? Workstation { get; set; }

    /// <summary>
    /// Код команды (повтор).
    /// </summary>
    public string? Command2 { get; set; }

    /// <summary>
    /// Идентификатор клиента.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Порядковый номер запроса.
    /// </summary>
    public string? Index { get; set; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Логин.
    /// </summary>
    public string? Username { get; set; }

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
    /// Прочие данные, например, поисковый запрос.
    /// </summary>
    public byte[]? Data { get; set; }

    /// <summary>
    /// Прочие данные в текстовом виде.
    /// </summary>
    public string[]? Lines { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор клиентского запроса по полям заголовка.
    /// </summary>
    public static RequestInfo Parse
        (
            byte[] buffer
        )
    {
        HeaderParser parser = new HeaderParser (buffer);

        // Пропускаем заголовок, содержащий общую длину пакета
        parser.NextString();

        RequestInfo result = new RequestInfo
        {
            Command1 = parser.NextString(),
            Workstation = parser.NextString(),
            Command2 = parser.NextString(),
            ClientId = parser.NextString(),
            Index = parser.NextString(),
            Password = parser.NextString(),
            Username = parser.NextString(),
            Reserved1 = parser.NextString(),
            Reserved2 = parser.NextString(),
            Reserved3 = parser.NextString(),
            Data = parser.NextBytes()
        };

        result.Lines = parser.SplitLines
            (
                result.Data,
                !new[] { "8", "A", "B" }
                    .Contains (result.Command1)
            );

        return result;
    }

    /// <summary>
    /// Разбор клиентского запроса по полям заголовка.
    /// </summary>
    public static RequestInfo Parse
        (
            byte[] buffer,
            bool useUtf
        )
    {
        var parser = new HeaderParser (buffer);

        // Пропускаем заголовок, содержащий общую длину пакета
        parser.NextString();

        var result = new RequestInfo
        {
            Command1 = parser.NextString(),
            Workstation = parser.NextString(),
            Command2 = parser.NextString(),
            ClientId = parser.NextString(),
            Index = parser.NextString(),
            Password = parser.NextString(),
            Username = parser.NextString(),
            Reserved1 = parser.NextString(),
            Reserved2 = parser.NextString(),
            Reserved3 = parser.NextString(),
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

        builder.Append ($"Command1: {Command1}");
        builder.AppendLine();
        builder.Append ($"Workstation: {Workstation}");
        builder.AppendLine();
        builder.Append ($"Command2: {Command2}");
        builder.AppendLine();
        builder.Append ($"ClientID: {ClientId}");
        builder.AppendLine();
        builder.Append ($"Index: {Index}");
        builder.AppendLine();
        builder.Append ($"Password: {Password}");
        builder.AppendLine();
        builder.Append ($"Username: {Username}");

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
