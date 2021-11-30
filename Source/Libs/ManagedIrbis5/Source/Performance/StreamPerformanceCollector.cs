// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* StreamPerformanceCollector.cs -- реализация сборщика сведений о производительности поверх потока
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
    public sealed class StreamPerformanceCollector
        : IPerformanceCollector
    {
        #region Properties

        /// <summary>
        /// Собственно поток.
        /// </summary>
        public BinaryWriter Writer => _writer;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StreamPerformanceCollector
            (
                BinaryWriter writer
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
        public StreamPerformanceCollector
            (
                Stream stream
            )
        {
            _syncRoot = new object();
            _ownStream = false;
            _writer = new BinaryWriter (stream);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StreamPerformanceCollector
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty (fileName);

            _syncRoot = new object();
            _ownStream = true;
            var stream = File.Open (fileName, FileMode.OpenOrCreate);
            stream.Seek (0, SeekOrigin.End);
            _writer = new BinaryWriter (stream);
        }

        #endregion

        #region Private members

        private readonly object _syncRoot;
        private readonly bool _ownStream;
        private readonly BinaryWriter _writer;

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
                record.SaveToStream (_writer);
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
