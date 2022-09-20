// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* ServerSetup.cs -- настройки серверного движка
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Настройки серверного движка.
    /// </summary>
    public sealed class ServerSetup
    {
        #region Properties

        /// <summary>
        /// Server INI-file.
        /// </summary>
        public ServerIniFile IniFile { get; private set; }

        /// <summary>
        /// Override for root path.
        /// </summary>
        public string? RootPathOverride { get; set; }

        /// <summary>
        /// TCP port number override.
        /// </summary>
        public int PortNumberOverride { get; set; }

        /// <summary>
        /// Override for workdir path.
        /// </summary>
        public string? WorkdirOverride { get; set; }

        /// <summary>
        /// Use TCP/IP v4 (enabled by default).
        /// </summary>
        public bool UseTcpIpV4 { get; set; }

        /// <summary>
        /// Use TCP/IP v6.
        /// </summary>
        public bool UseTcpIpV6 { get; set; }

        /// <summary>
        /// Port number for HTTP listener.
        /// </summary>
        public int HttpPort { get; set; }

        /// <summary>
        /// Name of the Windows pipe server.
        /// </summary>
        public string? PipeName { get; set; }

        /// <summary>
        /// Instance count for Windows pipe server.
        /// </summary>
        public int PipeInstanceCount { get; set; }

        /// <summary>
        /// Вызвать отладчик в конструкторе.
        /// </summary>
        public bool Break { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public ServerSetup
            (
                ServerIniFile iniFile
            )
        {
            IniFile = iniFile;
            UseTcpIpV4 = true;

        } // constructor

        #endregion

    } // class ServerSetup

} // namespace ManagedIrbis.Serve
