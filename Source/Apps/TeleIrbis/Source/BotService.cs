// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* BotService.cs -- сервис, запускающий прослушивание телеграм.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using Topshelf;
using Topshelf.Logging;

#endregion

#nullable enable

namespace TeleIrbis
{
    public sealed class BotService
        : ServiceControl
    {
        #region Private members

        private LogWriter? _log;

        #endregion

        #region ServiceControl members

        /// <summary>
        /// Запуск сервиса.
        /// </summary>
        public bool Start
            (
                HostControl hostControl
            )
        {
            _log = HostLogger.Get<BotService>();
            _log.Info(nameof(BotService) + "::" + nameof(Start));

            return true;
        }

        /// <summary>
        /// Остановка сервиса.
        /// </summary>
        public bool Stop
            (
                HostControl hostControl
            )
        {
            _log?.Info(nameof(BotService) + "::" + nameof(Stop));

            return true;
        }

        #endregion
    }
}
