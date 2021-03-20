// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ClsCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives


#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ClsCommand
        : MxCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ClsCommand()
            : base("cls")
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

            executive.MxConsole.Clear();

            OnAfterExecute();

            return true;
        }

        /// <inheritdoc cref="MxCommand.GetShortHelp" />
        public override string GetShortHelp()
        {
            return "Clear the console";
        }

        #endregion
    }
}
