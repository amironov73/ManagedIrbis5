// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RefineCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class RefineCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RefineCommand()
            : base("refine")
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

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

            if (!executive.Provider.IsConnected)
            {
                executive.WriteLine("Not connected");
                return false;
            }

            if (arguments.Length != 0
                && executive.History.Count != 0)
            {
                var argument = arguments[0].Text;
                var previous = executive.History.Peek();
                argument = string.Format("({0}) * ({1})", previous, argument);

                if (!string.IsNullOrEmpty(argument))
                {
                    var searchCommand = executive.Commands
                        .OfType<SearchCommand>().FirstOrDefault();
                    if (!ReferenceEquals(searchCommand, null))
                    {
                        MxArgument[] newArguments =
                        {
                            new() { Text = argument }
                        };
                        searchCommand.Execute(executive, newArguments);
                    }
                }
            }

            OnAfterExecute();

            return true;
        }

        #endregion

    }
}
