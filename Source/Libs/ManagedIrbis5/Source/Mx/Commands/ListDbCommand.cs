// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ListDbCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Reflection;
using AM.Runtime;

using ManagedIrbis.Client;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ListDbCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListDbCommand()
            : base("listdb")
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

            throw new NotImplementedException();

            /*

            string? pattern = null;
            if (arguments.Length != 0)
            {
                pattern = arguments[0].Text;
            }

            var connected = executive.Provider as ConnectedClient;
            if (!ReferenceEquals(connected, null))
            {
                var connection = connected.Connection;
                var databases = connection.ListDatabases("dbnam1.mnu")
                    .OrderBy(db => db.Name).ToArray();
                var list = new List<DatabaseInfo>();
                foreach (DatabaseInfo db in databases)
                {
                    if (!string.IsNullOrEmpty(pattern)
                        && !string.IsNullOrEmpty(db.Name))
                    {
                        if (!Regex.IsMatch(db.Name, pattern, RegexOptions.IgnoreCase))
                        {
                            continue;
                        }
                    }

                    list.Add(db);
                }

                var tablefier = new Tablefier();
                var output = tablefier.Print(list, "Name", "Description")
                    .TrimEnd();
                executive.WriteLine(output);
            }

            OnAfterExecute();

            return true;

            */
        }

        #endregion

        #region Object members

        #endregion
    }
}
