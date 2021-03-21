// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IFormatExit.cs -- интерфейс форматного выхода
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Интерфейс форматного выхода.
    /// </summary>
    public interface IFormatExit
    {
        /// <summary>
        /// Name of the format exit.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Execute the expression on given context.
        /// </summary>
        void Execute
            (
                PftContext context,
                PftNode? node,
                string? expression
            );
    }
}
