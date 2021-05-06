// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* ConsoleOutput.cs -- вывод с помощью системной консоли.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
using AM.ConsoleIO;

#endregion

#nullable enable

namespace AM.Text.Output
{
    /// <summary>
    /// Вывод с помощью системной консоли.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ConsoleOutput
        : AbstractOutput
    {
        #region AbstractOutput members

        /// <inheritdoc cref="AbstractOutput.HaveError" />
        public override bool HaveError { get; set; }

        /// <inheritdoc cref="AbstractOutput.Clear" />
        public override AbstractOutput Clear()
        {
            HaveError = false;
            ConsoleInput.Clear();

            return this;
        }

        /// <inheritdoc cref="AbstractOutput.Configure"/>
        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // TODO: implement properly
            return this;
        }

        /// <inheritdoc cref="AbstractOutput.Write(string)" />
        public override AbstractOutput Write
            (
                string text
            )
        {
            ConsoleInput.Write(text);

            return this;
        }

        /// <inheritdoc cref="AbstractOutput.WriteError(string)" />
        public override AbstractOutput WriteError
            (
                string text
            )
        {
            HaveError = true;

            // TODO implement properly: System.Console.Error.Write(text);
            ConsoleInput.Write(text);

            return this;
        }

        #endregion

    } // class ConsoleOutput

} // namespace AM.Text.Output
