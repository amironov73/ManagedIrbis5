// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* HelpCommand.cs --
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
    public sealed class HelpCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public HelpCommand()
            : base("help")
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

            string? name = null;
            if (arguments.Length != 0)
            {
                name = arguments[0].Text;
            }

            if (string.IsNullOrEmpty(name))
            {
                foreach (var command in executive.Commands)
                {
                    executive.WriteMessage($"{command.Name} {command.GetShortHelp()}");
                }
            }
            else
            {
                var command = executive.GetCommand(name);
                if (ReferenceEquals(command, null))
                {
                    executive.WriteError($"Unknown command '{name}'");
                }
                else
                {
                    executive.WriteMessage
                        (
                            command.GetLongHelp()
                        );
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
