// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* StatCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Text;

using ManagedIrbis.Client;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class StatCommand
        : MxCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public StatCommand()
            : base("stat")
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

            if (!executive.Provider.IsConnected)
            {
                executive.WriteLine("Not connected");
                return false;
            }

            string? lastSearch = null;
            if (executive.History.Count != 0)
            {
                lastSearch = executive.History.Peek();
            }

            throw new NotImplementedException();

            /*

            var client = executive.Provider as ConnectedClient;
            if (ReferenceEquals(client, null))
            {
                return true;
            }

            IIrbisConnection connection = client.Connection;

            string? text = null;
            if (arguments.Length != 0)
            {
                text = arguments[0].Text;
            }
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            var parts = text.Split(CommonSeparators.Comma, StringSplitOptions.None);
            var item = new StatDefinition.Item
            {
                Field = parts.GetOccurrence(0),
                Length = parts.GetOccurrence(1).SafeToInt32(10),
                Count = parts.GetOccurrence(2).SafeToInt32(1000),
                Sort = (StatDefinition.SortMethod) parts.GetOccurrence(3).SafeToInt32()
            };
            var definition = new StatDefinition
            {
                SearchQuery = lastSearch,
                Items = { item },
                DatabaseName = connection.Database
            };
            string output = connection.GetDatabaseStat(definition);
            if (!string.IsNullOrEmpty(output))
            {
                executive.WriteLine(output);
            }

            OnAfterExecute();

            return true;

            */
        }

        #endregion

    } // class StatCommand

} // namespace ManagedIrbis.Mx.Commands
