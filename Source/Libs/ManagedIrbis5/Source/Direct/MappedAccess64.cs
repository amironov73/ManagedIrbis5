// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* MappedAccess64.cs -- прямой доступ к базе данных с помощью проецирования файлов в оперативную память
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Direct;

/// <summary>
/// Прямой доступ к базе данных ИРБИС64
/// с помощью проецирования файлов в оперативную память
/// <see cref="MemoryMappedFile"/>.
/// </summary>
public sealed class MappedAccess64
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Мастер-файл.
    /// </summary>
    public MappedMstFile64 Mst { get; private set; }

    /// <summary>
    /// Файл перекрестных ссылок.
    /// </summary>
    public MappedXrfFile64 Xrf { get; private set; }

    /// <summary>
    /// Инвертированный (индексный) файл.
    /// </summary>
    public MappedInvertedFile64 InvertedFile { get; private set; }

    /// <summary>
    /// Имя базы данных.
    /// </summary>
    public string Database { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MappedAccess64
        (
            string masterFile
        )
    {
        Sure.FileExists (masterFile);

        Database = Path.GetFileNameWithoutExtension (masterFile);
        Mst = new MappedMstFile64 (Path.ChangeExtension (masterFile, ".mst"));
        Xrf = new MappedXrfFile64 (Path.ChangeExtension (masterFile, ".xrf"));
        InvertedFile = new MappedInvertedFile64 (Path.ChangeExtension (masterFile, ".ifp"));
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение максимального MFN для текущей базы данных.
    /// Обратите внимание: максимального, а не следующего MFN!
    /// </summary>
    public int GetMaxMfn()
    {
        return Mst.ControlRecord.NextMfn - 1;
    }

    /// <summary>
    /// Чтение записи.
    /// </summary>
    public MstRecord64? ReadRawRecord
        (
            int mfn
        )
    {
        Sure.Positive (mfn);

        var xrfRecord = Xrf.ReadRecord (mfn);
        if (xrfRecord.Offset == 0)
        {
            return null;
        }

        var result = Mst.ReadRecord (xrfRecord.Offset);

        return result;
    }

    /// <summary>
    /// Чтение записи с указанным MFN.
    /// </summary>
    /// <returns><c>null</c>, если прочитать запись не удалось.
    /// </returns>
    public Record? ReadRecord
        (
            int mfn
        )
    {
        Sure.Positive (mfn);

        var xrfRecord = Xrf.ReadRecord (mfn);
        if (xrfRecord.Offset == 0
            || (xrfRecord.Status & RecordStatus.PhysicallyDeleted) != 0)
        {
            return null;
        }

        var mstRecord = Mst.ReadRecord (xrfRecord.Offset);
        var result = mstRecord.DecodeRecord();
        result.Database = Database;

        return result;
    }

    /// <summary>
    /// Чтение с диска всех версий записи с указанным MFN.
    /// </summary>
    public Record[] ReadAllRecordVersions
        (
            int mfn
        )
    {
        Sure.Positive (mfn);

        var result = new List<Record>();
        var lastVersion = ReadRecord (mfn);
        if (lastVersion is not null)
        {
            result.Add (lastVersion);

            /*

            while (true)
            {
                long offset = lastVersion.PreviousOffset;
                if (offset == 0)
                {
                    break;
                }
                MstRecord64 mstRecord = Mst.ReadRecord(offset);
                MarcRecord previousVersion = mstRecord.DecodeRecord();
                previousVersion.Database = lastVersion.Database;
                previousVersion.Mfn = lastVersion.Mfn;
                result.Add(previousVersion);
                lastVersion = previousVersion;
            }

            */
        }

        return result.ToArray();
    }

    /// <summary>
    /// Чтение ссылок для указанного поискового термина.
    /// </summary>
    public TermLink[] ReadLinks
        (
            string key
        )
    {
        Sure.NotNull (key);

        return InvertedFile.SearchExact (key);
    }

    /// <summary>
    /// Чтение терминов, удовлетворяющий заданным условиям.
    /// </summary>
    public Term[] ReadTerms
        (
            TermParameters parameters
        )
    {
        Sure.VerifyNotNull (parameters);

        var result = InvertedFile.ReadTerms (parameters);

        return result;
    }

    /// <summary>
    /// Простейший поиск.
    /// </summary>
    public int[] SearchSimple
        (
            string key
        )
    {
        Sure.NotNull (key);

        var found = InvertedFile.SearchSimple (key);
        var result = new List<int>();
        foreach (var mfn in found)
        {
            if (!Xrf.ReadRecord (mfn).IsDeleted)
            {
                result.Add (mfn);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Простейший поиск, совмещенный чтением найденных записей.
    /// </summary>
    public Record[] SearchReadSimple
        (
            string key
        )
    {
        Sure.NotNull (key);

        var mfns = InvertedFile.SearchSimple (key);
        var result = new List<Record> (mfns.Length);
        foreach (var mfn in mfns)
        {
            try
            {
                var xrfRecord = Xrf.ReadRecord (mfn);
                if (!xrfRecord.IsDeleted)
                {
                    var mstRecord = Mst.ReadRecord (xrfRecord.Offset);
                    if (!mstRecord.Deleted)
                    {
                        var irbisRecord = mstRecord.DecodeRecord();
                        irbisRecord.Database = Database;
                        result.Add (irbisRecord);
                    }
                }
            }
            catch (Exception exception)
            {
                Magna.Logger.LogError
                    (
                        exception,
                        nameof (MappedAccess64) + "::" + nameof (SearchReadSimple)
                    );
            }
        }

        return result.ToArray();
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        InvertedFile.Dispose();
        InvertedFile = null!;

        Xrf.Dispose();
        Xrf = null!;

        Mst.Dispose();
        Mst = null!;
    }

    #endregion
}
