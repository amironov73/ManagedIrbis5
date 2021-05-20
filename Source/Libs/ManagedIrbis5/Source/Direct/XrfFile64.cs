// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* XrfFile64.cs -- XRF-файл
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.InteropServices;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    //
    // Extract from official documentation:
    // http://sntnarciss.ru/irbis/spravka/wtcp006002000.htm
    //
    // Каждая ссылка состоит из 3-х полей:
    // Число бит Параметр
    // 32        XRF_LOW – младшее слово в 8 байтовом смещении на запись;
    // 32        XRF_HIGH– старшее слово в 8 байтовом смещении на запись;
    // 32        XRF_FLAGS – Индикатор записи в виде битовых флагов
    //           следующего содержания:
    //             BIT_LOG_DEL(1)  - логически удаленная запись;
    //             BIT_PHYS_DEL(2) - физически удаленная запись;
    //             BIT_ABSENT(4)  - несуществующая запись;
    //             BIT_NOTACT_REC(8)- неактуализированная запись;
    //             BIT_LOCK_REC(64)- заблокированная запись.
    //

    /// <summary>
    /// XRF-файл.
    /// Файл перекрестных ссылок XRF представляет собой
    /// таблицу ссылок на записи файла документов.
    /// Первая ссылка соответствует записи файла документов
    /// с номером 1, вторая – 2  и тд.
    /// </summary>
    public class XrfFile64
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Access mode.
        /// </summary>
        public DirectAccessMode Mode { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public XrfFile64
            (
                string fileName,
                DirectAccessMode mode
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            FileName = Unix.FindDirectoryOrThrow(fileName);
            Mode = mode;

            _stream = DirectUtility.OpenFile(fileName, mode);
        }

        #endregion

        #region Private members

        private Stream _stream;

        private long _GetOffset
            (
                int mfn
            )
        {
            // ibatrak умножение в Int32 с преобразованием результата в Int64,
            // при больших mfn вызывает переполнение и результат
            // становится отрицательным
            var result = (long)XrfRecord64.RecordSize * (mfn - 1);

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Lock the record.
        /// </summary>
        public void LockRecord
            (
                int mfn,
                bool flag
            )
        {
            Sure.Positive(mfn, nameof(mfn));

            var record = ReadRecord(mfn);
            if (flag != record.Locked)
            {
                record.Locked = flag;
                WriteRecord(mfn, record);
            }

        } // method LockRecord

        /// <summary>
        /// Read the record.
        /// </summary>
        public XrfRecord64 ReadRecord
            (
                int mfn
            )
        {
            Sure.Positive(mfn, nameof(mfn));

            var result = new XrfRecord64();
            lock (this)
            {
                var offset = _GetOffset(mfn);
                if (offset >= _stream.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(mfn));
                }

                _stream.Seek(offset, SeekOrigin.Begin);

                _stream.Flush();
                var span = MemoryMarshal.CreateSpan(ref result, 1);
                var bytes = MemoryMarshal.Cast<XrfRecord64, byte>(span);
                _stream.Read(bytes);
                DirectUtility.FixNetwork64(ref result.Offset);
                result.Status = (RecordStatus)BinaryPrimitives.ReverseEndianness((int)result.Status);
            }

            return result;
        }

        /// <summary>
        /// Reopen the file.
        /// </summary>
        public void ReopenFile
            (
                DirectAccessMode mode
            )
        {
            if (Mode != mode)
            {
                lock (this)
                {
                    Mode = mode;

                    _stream.Dispose();
                    _stream = DirectUtility.OpenFile(FileName, mode);
                }
            }
        }

        /// <summary>
        /// Write the record.
        /// </summary>
        public void WriteRecord
            (
                int mfn,
                XrfRecord64 record
            )
        {
            var offset = _GetOffset(mfn);
            lock (this)
            {
                _stream.Seek(offset, SeekOrigin.Begin);
                var span = MemoryMarshal.CreateSpan(ref record, 1);
                var bytes = MemoryMarshal.Cast<XrfRecord64, byte>(span);
                DirectUtility.FixNetwork64(ref record.Offset);
                record.Status = (RecordStatus)BinaryPrimitives.ReverseEndianness((int)record.Status);
                _stream.Write(bytes);
                _stream.Flush();
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            lock (this)
            {
                _stream.Dispose();
            }
        } // method Dispose

        #endregion

    } // class XrfFile64

} // namespace ManagedIrbis.Direct
