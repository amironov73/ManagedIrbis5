// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InfoCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
    public sealed class InfoCommand
        : MxCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public InfoCommand()
            : base("info")
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

            var connected = executive.Provider as ConnectedClient;
            if (!ReferenceEquals(connected, null))
            {
                IIrbisConnection connection = connected.Connection;
                var serverStat = connection.GetServerStat();
                //executive.WriteLine(serverStat.ToString());

                var clients = serverStat.RunningClients;
                if (clients is not null)
                {
                    var tablefier = new Tablefier();
                    string[] properties =
                    {
                        "IPAddress", "Name", "ID", "Workstation", "Registered",
                        "Acknowledged", "LastCommand", "CommandNumber"
                    };
                    var output = tablefier.Print(clients, properties)
                        .TrimEnd();
                    executive.WriteLine(output);
                }
            }

            OnAfterExecute();

            return true;
        }

        #endregion

    } // class InfoCommand

} // namespace ManagedIrbis.Mx.Commands
