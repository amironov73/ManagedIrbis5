// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DirCommand.cs --
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
using AM.Runtime;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class DirCommand
        : MxCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DirCommand()
            : base("dir")
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

            var fileName = "*.*";
            if (arguments.Length != 0)
            {
                fileName = arguments[0].Text;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "*.*";
            }

            throw new NotImplementedException();

            /*

            ConnectedClient connected = executive.Provider as ConnectedClient;
            if (!ReferenceEquals(connected, null))
            {
                var connection = connected.Connection;
                var specification = new FileSpecification
                    {
                        Path = IrbisPath.MasterFile,
                        Database = connection.Database,
                        FileName = fileName
                    };
                string[] list = connection.ListFiles(specification);
                foreach (string file in list)
                {
                    executive.WriteLine(file);
                }
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
