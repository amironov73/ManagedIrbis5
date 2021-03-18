﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class InfoCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public InfoCommand()
            : base("info")
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

            if (!executive.Provider.Connected)
            {
                executive.WriteLine("Not connected");
                return false;
            }

            ConnectedClient connected = executive.Provider as ConnectedClient;
            if (!ReferenceEquals(connected, null))
            {
                IIrbisConnection connection = connected.Connection;
                ServerStat serverStat = connection.GetServerStat();
                //executive.WriteLine(serverStat.ToString());

                Tablefier tablefier = new Tablefier();
                string[] properties =
                {
                    "IPAddress", "Name", "ID", "Workstation", "Registered",
                    "Acknowledged", "LastCommand", "CommandNumber"
                };
                string output = tablefier.Print(serverStat.RunningClients, properties)
                    .TrimEnd();
                executive.WriteLine(output);
            }

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
