// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FileObject.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    sealed class FileObject
        : IDisposable
    {
        #region Properties

        [ExcludeFromCodeCoverage]
        public bool AppendMode { get; private set; }

        [ExcludeFromCodeCoverage]
        public string FileName { get; private set; }

        [ExcludeFromCodeCoverage]
        public bool WriteMode { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileObject
            (
                string fileName,
                bool writeMode,
                bool appendMode
            )
        {
            FileName = fileName;
            WriteMode = writeMode;
            AppendMode = appendMode;

            FileStream stream;

            if (writeMode)
            {
                stream = new FileStream
                    (
                        fileName,
                        appendMode
                        ? FileMode.Append
                        : FileMode.Create
                    );

                _writer = new StreamWriter
                    (
                        stream,
                        IrbisEncoding.Utf8
                    );
            }
            else
            {
                stream = new FileStream
                    (
                        fileName,
                        FileMode.Open
                    );

                _reader = new StreamReader
                    (
                        stream,
                        IrbisEncoding.Utf8
                    );
            }
        }

        #endregion

        #region Private members

        private readonly StreamReader? _reader;

        private readonly StreamWriter? _writer;

        #endregion

        #region Public methods

        public string ReadAll()
        {
            var result = _reader?.ReadToEnd();

            return result ?? string.Empty;
        }

        public string? ReadLine()
        {
            var result = _reader?.ReadLine();

            return result;
        }

        public void Write ( string text ) => _writer?.Write(text);

        public void WriteLine ( string text ) => _writer?.WriteLine(text);

        #endregion

        #region IDisposable

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (!ReferenceEquals(_reader, null))
            {
                _reader.Dispose();
            }

            if (!ReferenceEquals(_writer, null))
            {
                _writer.Dispose();
            }
        }

        #endregion
    }
}
