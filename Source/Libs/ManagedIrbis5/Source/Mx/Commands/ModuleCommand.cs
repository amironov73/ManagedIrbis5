// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ModuleCommand.cs --
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

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ModuleCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ModuleCommand()
            : base("module")
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

            // TODO implement

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
