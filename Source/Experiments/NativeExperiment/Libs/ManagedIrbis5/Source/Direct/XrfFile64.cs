// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable TemplateIsNotCompileTimeConstantProblem
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
using AM.Logging;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Direct;

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
public sealed class XrfFile64
    : IDisposable,
    ISupportLogging,
    IServiceProvider
{
    #region Properties

    /// <summary>
    /// Имя файла.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Режим доступа.
    /// </summary>
    public DirectAccessMode Mode { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public XrfFile64
        (
            string fileName,
            DirectAccessMode mode = DirectAccessMode.Exclusive,
            IServiceProvider? serviceProvider = null
        )
    {
        Sure.NotNullNorEmpty (fileName, nameof (fileName));

        _serviceProvider = serviceProvider ?? Magna.Host.Services;
        _logger = (ILogger?)_serviceProvider.GetService (typeof (ILogger<MstFile64>));
        _logger?.LogTrace ($"{nameof (MstFile64)}::Constructor ({fileName}, {mode})");

        FileName = Unix.FindFileOrThrow (fileName);
        Mode = mode;

        Magna.Logger.LogTrace (nameof (XrfFile64) + "::Constructor: {FileName}", FileName);

        _stream = DirectUtility.OpenFile (fileName, mode);
    }

    #endregion

    #region Private members

    private ILogger? _logger;
    private readonly IServiceProvider _serviceProvider;
    private Stream _stream;

    private long _GetOffset
        (
            int mfn
        )
    {
        // ibatrak умножение в Int32 с преобразованием результата в Int64,
        // при больших mfn вызывает переполнение и результат
        // становится отрицательным
        var result = (long) XrfRecord64.RecordSize * (mfn - 1);

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Установка флага блокировки записи.
    /// </summary>
    public void LockRecord
        (
            int mfn,
            bool flag
        )
    {
        Sure.Positive (mfn);

        var record = ReadRecord (mfn);
        if (flag != record.IsLocked)
        {
            record.IsLocked = flag;
            WriteRecord (mfn, record);
        }
    }

    /// <summary>
    /// Чтение записи.
    /// </summary>
    public XrfRecord64 ReadRecord
        (
            int mfn
        )
    {
        Sure.Positive (mfn);

        var result = new XrfRecord64();
        lock (this)
        {
            var offset = _GetOffset (mfn);
            if (offset >= _stream.Length)
            {
                throw new ArgumentOutOfRangeException (nameof (mfn));
            }

            _stream.Seek (offset, SeekOrigin.Begin);
            _stream.Flush();
            var span = MemoryMarshal.CreateSpan (ref result, 1);
            var bytes = MemoryMarshal.Cast<XrfRecord64, byte> (span);
            if (_stream.Read (bytes) < XrfRecord64.RecordSize)
            {
                throw new IOException();
            }

            DirectUtility.FixNetwork64 (ref result.Offset);
            result.Status = (RecordStatus) BinaryPrimitives.ReverseEndianness ((int) result.Status);
        }

        return result;
    }

    /// <summary>
    /// Переоткрытие файла в указанном режиме.
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
                _stream = null!; // на случай вылетания следующей строки
                _stream = DirectUtility.OpenFile (FileName, mode);
            }
        }
    }

    /// <summary>
    /// Сохранение записи.
    /// </summary>
    public void WriteRecord
        (
            int mfn,
            XrfRecord64 record
        )
    {
        var offset = _GetOffset (mfn);
        lock (this)
        {
            _stream.Seek (offset, SeekOrigin.Begin);
            var span = MemoryMarshal.CreateSpan (ref record, 1);
            var bytes = MemoryMarshal.Cast<XrfRecord64, byte> (span);
            DirectUtility.FixNetwork64 (ref record.Offset);
            record.Status = (RecordStatus)BinaryPrimitives.ReverseEndianness ((int) record.Status);
            _stream.Write (bytes);
            _stream.Flush();
        }
    }

    #endregion

    #region ISupportLogging members

    /// <inheritdoc cref="ISupportLogging.Logger"/>

    // TODO implement
    public ILogger? Logger => _logger;

    /// <inheritdoc cref="ISupportLogging.SetLogger"/>
    public void SetLogger (ILogger? logger)
        => _logger = logger;

    #endregion

    #region IServiceProvider members

    /// <inheritdoc cref="IServiceProvider.GetService"/>
    public object? GetService (Type serviceType)
        => _serviceProvider.GetService (serviceType);

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        Magna.Logger.LogTrace (nameof (XrfFile64) + "::" + nameof (Dispose) + ": {FileName}", FileName);

        lock (this)
        {
            _stream.Dispose();
        }
    }

    #endregion
}
