// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FileWriteHandler.cs --
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

using ManagedIrbis.Mx.Infrastructrure;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Handlers
{
    /// <summary>
    ///
    /// </summary>
    public class FileWriteHandler
        : MxHandler
    {
        #region Properties

        /// <summary>
        /// File name.
        /// </summary>
        public string? FileName { get; set; }

        /// <inheritdoc cref= "MxHandler.Prefix" />
        public override string Prefix
        {
            get { return "|>"; }
        }

        /// <summary>
        /// Encoding.
        /// </summary>
        public Encoding? Encoding { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private StreamWriter? _writer;

        #endregion

        #region Public methods

        #endregion

        #region MxHandler members

        /// <inheritdoc cref="MxHandler.Initialize" />
        public override void Initialize
            (
                MxExecutive executive
            )
        {
            base.Initialize(executive);
        }

        /// <inheritdoc cref="MxHandler.Parse" />
        public override void Parse
            (
                MxExecutive executive,
                string? commandLine
            )
        {
            // TODO Implement properly

            FileName = commandLine;
        }

        /// <inheritdoc cref="MxHandler.BeginOutput" />
        public override void BeginOutput
            (
                MxExecutive executive
            )
        {
            base.BeginOutput(executive);

            Encoding encoding = Encoding ?? Encoding.UTF8;

            if (!ReferenceEquals(_writer, null))
            {
                _writer.Dispose();
                _writer = null;
            }

            if (!string.IsNullOrEmpty(FileName))
            {
                _writer = TextWriterUtility.Create(FileName, encoding);
            }
        }

        /// <inheritdoc cref="MxHandler.HandleOutput" />
        public override void HandleOutput
            (
                MxExecutive executive,
                string? output
            )
        {
            if (!ReferenceEquals(_writer, null)
                && !string.IsNullOrEmpty(output))
            {
                _writer.Write(output);
            }
        }

        /// <inheritdoc cref="MxHandler.EndOutput" />
        public override void EndOutput
            (
                MxExecutive executive
            )
        {
            if (!ReferenceEquals(_writer, null))
            {
                string output = executive.GetOutput();
                if (!string.IsNullOrEmpty(output))
                {
                    _writer.Write(output);
                }

                _writer.Dispose();
                _writer = null;
            }

            base.EndOutput(executive);
        }

        /// <inheritdoc cref="MxHandler.Dispose" />
        public override void Dispose()
        {
            if (!ReferenceEquals(_writer, null))
            {
                _writer.Dispose();
                _writer = null;
            }

            base.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
