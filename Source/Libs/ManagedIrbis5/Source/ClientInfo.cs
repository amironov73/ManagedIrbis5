// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* ClientInfo.cs -- информация о клиенте, подключенном к серверу ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Информация о клиенте, подключенном к серверу ИРБИС
    /// (не обязательно о текущем).
    /// </summary>
    [DebuggerDisplay("{IPAddress} {Name} {Workstation}")]
    public sealed class ClientInfo
    {
        #region Properties

        /// <summary>
        /// Номер
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        /// Адрес клиента
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string? IPAddress { get; set; }

        /// <summary>
        /// Порт клиента
        /// </summary>
        public string? Port { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Идентификатор клиентской программы
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string? ID { get; set; }

        /// <summary>
        /// Клиентский АРМ
        /// </summary>
        public string? Workstation { get; set; }

        /// <summary>
        /// Время подключения к серверу
        /// </summary>
        public string? Registered { get; set; }

        /// <summary>
        /// Последнее подтверждение
        /// </summary>
        public string? Acknowledged { get; set; }

        /// <summary>
        /// Последняя команда
        /// </summary>
        public string? LastCommand { get; set; }

        /// <summary>
        /// Номер последней команды
        /// </summary>
        public string? CommandNumber { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() =>
            $"IP: {IPAddress}, ID: {ID}, {Workstation}";

        #endregion

    } // class ClientInfo

} // namespace ManagedIrbis
