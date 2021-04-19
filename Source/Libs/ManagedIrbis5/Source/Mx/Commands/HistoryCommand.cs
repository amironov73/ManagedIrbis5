// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* HistoryCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class HistoryCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public HistoryCommand()
            : base("history")
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

            string? command = null;
            if (arguments.Length != 0)
            {
                command = arguments[0].Text;
            }

            var history = executive.History.ToArray();

            if (string.IsNullOrEmpty(command))
            {
                for (var i = 0; i < history.Length; i++)
                {
                    executive.WriteMessage($"{i + 1}: {history[i]}");
                }
            }
            else
            {
                if (int.TryParse(command, out var index))
                {
                    var argument = history.GetOccurrence
                        (
                            history.Length - index
                        );
                    if (string.IsNullOrEmpty(argument))
                    {
                        executive.WriteLine("No such entry");
                    }
                    else
                    {
                        var searchCommand = executive.Commands
                            .OfType<SearchCommand>().FirstOrDefault();
                        if (!ReferenceEquals(searchCommand, null))
                        {
                            executive.WriteLine(argument);
                            MxArgument[] newArguments =
                            {
                                new MxArgument { Text = argument }
                            };
                            searchCommand.Execute(executive, newArguments);
                            executive.History.Pop();
                        }
                    }
                }
            }

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
