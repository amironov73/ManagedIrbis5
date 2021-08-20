// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ISupportLogging.cs -- интерфейс поддержки логирования
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Интерфейс поддержки логирования.
    /// </summary>
    public interface ISupportLogging
    {
        /// <summary>
        /// Логгер для текущего экземпляра.
        /// </summary>
        ILogger? Logger { get; }

    } // interface ISupportLogging

} // namespace ManagedIrbis
