// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ConnectCommand.cs -- connect to the server
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

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    /// Connect to the server.
    /// </summary>
    public sealed class ConnectCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectCommand()
            : base("Connect")
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Initialize the provider.
        /// </summary>
        public static ISyncIrbisProvider InitializeProvider
            (
                string argument
            )
        {
            var result = ProviderManager.GetAndConfigureProvider(argument);

            return result;
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

            if (arguments.Length != 0)
            {
                string argument = arguments[0].Text;
                executive.Provider.Dispose();
                if (string.IsNullOrEmpty(argument))
                {
                    executive.Provider = ProviderManager.GetPreconfiguredProvider();
                }
                else
                {
                    executive.Provider = InitializeProvider(argument);
                }
                executive.Context.SetProvider(executive.Provider);
                executive.WriteMessage(string.Format
                    (
                        "Connected, current database: {0}",
                        executive.Provider.Database
                    ));
            }

            OnAfterExecute();

            return true;
        }

        #endregion
    }
}
