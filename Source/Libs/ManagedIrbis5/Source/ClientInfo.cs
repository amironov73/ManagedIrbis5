// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* ClientInfo.cs -- информация о клиенте, подключенном к серверу ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Информация о клиенте, подключенном к серверу ИРБИС
/// (не обязательно о текущем).
/// </summary>
public sealed class ClientInfo
{
    #region Properties

    /// <summary>
    /// Порядковый номер.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Адрес клиента.
    /// </summary>
    public string? IPAddress { get; set; }

    /// <summary>
    /// Порт клиента.
    /// </summary>
    public string? Port { get; set; }

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Идентификатор клиента.
    /// </summary>
    public string? ID { get; set; }

    /// <summary>
    /// Вид клиентского АРМ.
    /// </summary>
    public string? Workstation { get; set; }

    /// <summary>
    /// Время подключения данного клиента к серверу.
    /// </summary>
    public string? Registered { get; set; }

    /// <summary>
    /// Время последнего подтверждения от данного клиента.
    /// </summary>
    public string? Acknowledged { get; set; }

    /// <summary>
    /// Последняя команда, принятая сервером от данного клиента.
    /// </summary>
    public string? LastCommand { get; set; }

    /// <summary>
    /// Номер последней команды, отданной данным клиентом.
    /// </summary>
    public string? CommandNumber { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"IP: {IPAddress}, ID: {ID}, {Workstation}";
    }

    #endregion
}
