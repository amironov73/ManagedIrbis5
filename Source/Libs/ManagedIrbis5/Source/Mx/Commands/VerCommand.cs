// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* VerCommand.cs -- определение версии сервера
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    /// Определение версии сервера.
    /// </summary>
    public sealed class VerCommand
        : MxCommand
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public VerCommand()
            : base("ver")
        {
        }

        #endregion

        #region MxCommand members

        /// <inheritdoc cref="MxCommand.Execute" />
        public override bool Execute
            (
                MxExecutive executive,
                MxArgument[] arguments
            )
        {
            OnBeforeExecute();

            if (!executive.Provider.Connected)
            {
                executive.WriteLine("Not connected");
                return false;
            }

            var version = executive.Provider.GetServerVersion();
            var text = version?.ToString() ?? "Unknown version";
            executive.WriteLine(text);

            OnAfterExecute();

            return true;
        }

        #endregion

    } // class VerCommand

} // namespace ManagedIrbis.Mx.Commands
