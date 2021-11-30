// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* TextPerformanceCollector.cs -- реализация сборщика сведений о производительности поверх текстового файла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Performance
{
    /// <summary>
    /// Реализация сборщика сведений о производительности поверх потока.
    /// </summary>
    public sealed class TextPerformanceCollector
        : IPerformanceCollector
    {
        #region Properties

        /// <summary>
        /// Собственно поток.
        /// </summary>
        public TextWriter Writer => _writer;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TextPerformanceCollector
            (
                TextWriter writer
            )
        {
            Sure.NotNull (writer);

            _syncRoot = new object();
            _ownStream = false;
            _writer = writer;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TextPerformanceCollector
            (
                Stream stream
            )
        {
            Sure.NotNull (stream);

            _syncRoot = new object();
            _ownStream = false;
            stream.Seek (0, SeekOrigin.End);
            _writer = new StreamWriter (stream);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TextPerformanceCollector
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty (fileName);

            _syncRoot = new object();
            _ownStream = true;
            var stream = File.Open (fileName, FileMode.OpenOrCreate);
            stream.Seek (0, SeekOrigin.End);
            _writer = new StreamWriter (stream);
        }

        #endregion

        #region Private members

        private readonly object _syncRoot;
        private readonly bool _ownStream;
        private readonly TextWriter _writer;

        #endregion

        #region IPerformanceCollector members

        /// <inheritdoc cref="Collect"/>
        public void Collect
            (
                PerfRecord record
            )
        {
            Sure.NotNull (record);

            lock (_syncRoot)
            {
                _writer.WriteLine (record.ToString());
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (_ownStream)
            {
                lock (_syncRoot)
                {
                    _writer.Dispose();
                }
            }
        }

        #endregion
    }
}
