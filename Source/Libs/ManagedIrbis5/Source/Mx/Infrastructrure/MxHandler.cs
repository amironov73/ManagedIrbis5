// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MxHandler.cs --
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
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Infrastructrure
{
    /// <summary>
    ///
    /// </summary>
    public abstract class MxHandler
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Prefix.
        /// </summary>
        public abstract string Prefix { get; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Initialize the handler.
        /// </summary>
        public virtual void Initialize
            (
                MxExecutive executive
            )
        {
        }

        /// <summary>
        /// Parse the command line.
        /// </summary>
        public abstract void Parse
            (
                MxExecutive executive,
                string? commandLine
            );

        /// <summary>
        /// Begin output.
        /// </summary>
        public virtual void BeginOutput
            (
                MxExecutive executive
            )
        {
        }

        /// <summary>
        /// Handle output.
        /// </summary>
        public abstract void HandleOutput
            (
                MxExecutive executive,
                string? output
            );

        /// <summary>
        /// End output.
        /// </summary>
        public virtual void EndOutput
            (
                MxExecutive executive
            )
        {
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public virtual void Dispose()
        {
            // Nothing to do here
        }

        #endregion

        #region Object members

        #endregion
    }
}
