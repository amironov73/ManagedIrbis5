// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* ServerVersion.cs -- информация о версии сервера
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis
{
    /// <summary>
    /// Информация о версии сервера ИРБИС64.
    /// </summary>
    public sealed class ServerVersion
    {
        #region Properties

        /// <summary>
        /// Организация, на которую зарегистрирован сервер.
        /// </summary>
        public string? Organization { get; set; }

        /// <summary>
        /// Собственно версия сервера в формате 64.2008.1.
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// Максимальное количество подключений.
        /// </summary>
        public int MaxClients { get; set; }

        /// <summary>
        /// Текущее количество подключений.
        /// </summary>
        public int ConnectedClients { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static ServerVersion Parse
            (
                Response response
            )
        {
            var lines = response.ReadRemainingAnsiLines();
            var result = new ServerVersion();

            if (lines.Length >= 4)
            {
                result.Organization = lines[0];
                result.Version = lines[1];
                result.ConnectedClients = lines[2].SafeToInt32();
                result.MaxClients = lines[3].SafeToInt32();
            }
            else
            {
                result.Version = lines[0];
                result.ConnectedClients = lines[1].SafeToInt32();
                result.MaxClients = lines[2].SafeToInt32();
            }

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => string.Format
            (
                "Version: {0}, MaxClients: {1}, "
                + "ConnectedClients: {2}, Organization: {3}",
                Version.ToVisibleString(),
                MaxClients,
                ConnectedClients,
                Organization.ToVisibleString()
            );

        #endregion
    }
}
