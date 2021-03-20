﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FileAppendHandler.cs --
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
    public sealed class FileAppendHandler
        : FileWriteHandler
    {
        #region Properties

        /// <inheritdoc cref= "FileWriteHandler.Prefix" />
        public override string Prefix
        {
            get { return "|>>"; }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private StreamWriter? _writer;

        #endregion

        #region Public methods

        #endregion

        #region MxHandler members

        /// <inheritdoc cref="FileWriteHandler.BeginOutput" />
        public override void BeginOutput
            (
                MxExecutive executive
            )
        {
            Encoding encoding = Encoding ?? Encoding.UTF8;

            if (!ReferenceEquals(_writer, null))
            {
                _writer.Dispose();
                _writer = null;
            }

            if (!string.IsNullOrEmpty(FileName))
            {
                _writer = TextWriterUtility.Append(FileName, encoding);
            }
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
