﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ListUsersCommands.cs --
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

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ListUsersCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListUsersCommand()
            : base("listUsers")
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

            throw new NotImplementedException();

            /*

            string pattern = null;
            if (arguments.Length != 0)
            {
                pattern = arguments[0].Text;
            }

            ConnectedClient connected = executive.Provider as ConnectedClient;
            if (!ReferenceEquals(connected, null))
            {
                IIrbisConnection connection = connected.Connection;
                UserInfo[] users = connection.ListUsers().OrderBy(u => u.Name).ToArray();
                List<UserInfo> list = new List<UserInfo>();
                foreach (UserInfo user in users)
                {
                    if (!string.IsNullOrEmpty(pattern)
                       && !string.IsNullOrEmpty(user.Name))
                    {
                        if (!Regex.IsMatch(user.Name, pattern, RegexOptions.IgnoreCase))
                        {
                            continue;
                        }
                    }

                    list.Add(user);
                    //executive.WriteLine(user.ToString());
                }

                Tablefier tablefier = new Tablefier();
                string[] properties =
                {
                    "Name", "Password", "Cataloger", "Circulation",
                    "Administrator"
                };
                string output = tablefier.Print(list, properties).TrimEnd();
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
