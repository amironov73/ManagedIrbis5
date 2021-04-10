// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SortCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class SortCommand
        : MxCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SortCommand()
            : base("Sort")
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

            string? argument = null;
            if (arguments.Length != 0)
            {
                argument = arguments[0].Text;
            }

            if (string.IsNullOrEmpty(argument))
            {
                var sort = executive.OrderFormat;
                if (string.IsNullOrEmpty(sort))
                {
                    sort = "OFF";
                }
                executive.WriteMessage($"SORT is: {sort}");
            }
            else if (argument.SameString("off"))
            {
                executive.OrderFormat = null;
                executive.WriteMessage("SORT is OFF now");
            }
            else
            {
                executive.OrderFormat = argument;
            }

            OnAfterExecute();

            return true;
        }

        #endregion

    }
}
