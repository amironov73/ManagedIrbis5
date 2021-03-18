﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

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
        [NotNull]
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
                [NotNull] MxExecutive executive
            )
        {
            Code.NotNull(executive, "executive");
        }

        /// <summary>
        /// Parse the command line.
        /// </summary>
        public abstract void Parse
            (
                [NotNull] MxExecutive executive,
                [CanBeNull] string commandLine
            );

        /// <summary>
        /// Begin output.
        /// </summary>
        public virtual void BeginOutput
            (
                [NotNull] MxExecutive executive
            )
        {
            Code.NotNull(executive, "executive");
        }

        /// <summary>
        /// Handle output.
        /// </summary>
        public abstract void HandleOutput
            (
                [NotNull] MxExecutive executive,
                [CanBeNull] string output
            );

        /// <summary>
        /// End output.
        /// </summary>
        public virtual void EndOutput
            (
                [NotNull] MxExecutive executive
            )
        {
            Code.NotNull(executive, "executive");
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
