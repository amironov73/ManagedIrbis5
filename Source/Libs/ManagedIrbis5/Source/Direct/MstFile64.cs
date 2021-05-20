﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MstFile64.cs -- reading MST file
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;
using AM.IO;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    //
    // Extract from official documentation:
    // http://sntnarciss.ru/irbis/spravka/wtcp006001020.htm
    //
    // Первая запись в файле документов – управляющая запись,
    // которая формируется(в момент определения базы данных
    // или при ее инициализации) и поддерживается автоматически.
    // Ее содержание следующее:
    // Число бит Параметр
    // 32        CTLMFN – резерв;
    // 32        NXTMFN –номер записи файла документов,
    //           назначаемый для следующей записи,
    //           создаваемой в базе данных;
    // 32        NXT_LOW – младшее слово смещения на свободное место
    //           в файле; (всегда указывает на конец файла MST)
    // 32        NXT_HIGH – старшее слово смещения на свободное
    //           место в файле
    // 32        MFTYPE – резерв;
    // 32        RECCNT – резерв;
    // 32        MFCXX1 – резерв;
    // 32        MFCXX2 – резерв;
    // 32        MFCXX3 – индикатор блокировки базы данных
    //           (0 – нет, >0 – да).
    //

    /// <summary>
    /// Direct reads MST file.
    /// </summary>
    public sealed class MstFile64
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// How many data to preload?
        /// </summary>
        public static int PreloadLength = 10 * 1024;

        /// <summary>
        /// Control record.
        /// </summary>
        public MstControlRecord64 ControlRecord { get; internal set; }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Access mode.
        /// </summary>
        public DirectAccessMode Mode { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MstFile64
            (
                string fileName,
                DirectAccessMode mode
            )
        {
            FileName = Unix.FindDirectoryOrThrow(fileName);
            Mode = mode;

            _lockObject = new object();
            _stream = DirectUtility.OpenFile(fileName, mode);

            ControlRecord = MstControlRecord64.Read(_stream);
            _lockFlag = ControlRecord.Blocked != 0;
        }

        #endregion

        #region Private members

        private object _lockObject;

        private bool _lockFlag;

        private Stream _stream;

        private static void _AppendStream
            (
                Stream source,
                Stream target,
                int amount
            )
        {
            if (amount <= 0)
            {
                throw new IOException();
            }

            var savedPosition = target.Position;
            target.Position = target.Length;

            var buffer = new byte[amount];
            var readed = source.Read(buffer, 0, amount);
            if (readed <= 0)
            {
                throw new IOException();
            }

            target.Write(buffer, 0, readed);
            target.Position = savedPosition;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Read the record (with preload optimization).
        /// </summary>
        public MstRecord64 ReadRecord
            (
                long position
            )
        {
            MemoryStream memory;
            MstRecordLeader64 leader;

            lock (_lockObject)
            {
                _stream.Seek(position, SeekOrigin.Begin);

                memory = new MemoryStream(PreloadLength);
                _AppendStream(_stream, memory, PreloadLength);
                memory.Position = 0;

                leader = MstRecordLeader64.Read(memory);
                var amountToRead = (int)(leader.Length - memory.Length);
                if (amountToRead > 0)
                {
                    _AppendStream(_stream, memory, amountToRead);
                }
            }

            var encoding = IrbisEncoding.Utf8;
            var dictionary = new List<MstDictionaryEntry64>(leader.Nvf);

            var bytes = memory.ToArray();
            for (var i = 0; i < leader.Nvf; i++)
            {
                var entry = new MstDictionaryEntry64
                {
                    Tag = memory.ReadInt32Network(),
                    Position = memory.ReadInt32Network(),
                    Length = memory.ReadInt32Network()
                };
                var endOffset = leader.Base + entry.Position;
                entry.Text = encoding.GetString(bytes, endOffset, entry.Length);
                dictionary.Add(entry);
            }

            var result = new MstRecord64
            {
                Leader = leader,
                Dictionary = dictionary
            };

            return result;
        }

        /// <summary>
        /// Блокировка базы данных в целом.
        /// </summary>
        public void LockDatabase
            (
                bool flag
            )
        {
            lock (_lockObject)
            {
                var buffer = new byte[4];

                _stream.Position = MstControlRecord64.LockFlagPosition;

                //StreamUtility.Lock(_stream, 0, MstControlRecord64.RecordSize);

                if (flag)
                {
                    buffer[0] = 1;
                }

                _stream.Write(buffer, 0, buffer.Length);

                //StreamUtility.Unlock(_stream, 0, MstControlRecord64.RecordSize);

                _lockFlag = flag;
            }
        }

        /// <summary>
        /// Чтение флага блокировки базы данных в целом.
        /// </summary>
        public bool ReadDatabaseLockedFlag()
        {
            lock (_lockObject)
            {
                var buffer = new byte[4];

                _stream.Position = MstControlRecord64.LockFlagPosition;
                _stream.Read(buffer, 0, buffer.Length);

                var result = Convert.ToBoolean
                    (
                        BitConverter.ToInt32(buffer, 0)
                    );
                _lockFlag = result;

                return result;
            }
        }

        /// <summary>
        /// Read record leader only.
        /// </summary>
        public MstRecordLeader64 ReadLeader
            (
                long position
            )
        {
            lock (_lockObject)
            {
                _stream.Position = position;
                var result = MstRecordLeader64.Read(_stream);

                return result;
            }
        }

        /// <summary>
        /// Reopen file.
        /// </summary>
        public void ReopenFile
            (
                DirectAccessMode mode
            )
        {
            if (Mode != mode)
            {
                lock (_lockObject)
                {
                    Mode = mode;

                    _stream.Dispose();
                    _stream = DirectUtility.OpenFile(FileName, mode);
                }
            }
        }

        /// <summary>
        /// Update control record.
        /// </summary>
        public void UpdateControlRecord
            (
                bool reread
            )
        {
            lock (_lockObject)
            {
                _stream.Position = 0;
                if (reread)
                {
                    ControlRecord = MstControlRecord64.Read(_stream);
                    _stream.Position = 0;
                }

                var control = ControlRecord;
                control.NextPosition = _stream.Length;
                ControlRecord = control;
                control.Write(_stream);
                _lockFlag = ControlRecord.Blocked != 0;
                _stream.Flush();
            }
        }

        /// <summary>
        /// Update therecord leader.
        /// </summary>
        public void UpdateLeader
            (
                MstRecordLeader64 leader,
                long position
            )
        {
            lock (_lockObject)
            {
                _stream.Position = position;
                leader.Write(_stream);
                _stream.Flush();
            }
        }

        /// <summary>
        /// Write the record.
        /// </summary>
        public long WriteRecord
            (
                MstRecord64 record
            )
        {
            lock (_lockObject)
            {
                var position = _stream.Length;
                _stream.Position = position;

                record.Prepare();
                record.Write(_stream);
                _stream.Flush();

                return position;
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            lock (_lockObject)
            {
                _stream.Dispose();
            }
        }

        #endregion

    } // class MstFile64

} // namespace ManagedIrbis.Direct

