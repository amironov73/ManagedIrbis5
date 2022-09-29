// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* MasterVisitor64.cs -- простой доступ к мастер-файлу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Direct;

/// <summary>
/// Простой доступ к мастер-файлу ИРБИС64.
/// </summary>
public sealed class MasterVisitor64
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Мастер-файл.
    /// </summary>
    public MstFile64 Mst { get; }

    /// <summary>
    /// Файл кросс-ссылок.
    /// </summary>
    public XrfFile64 Xrf { get; }

    /// <summary>
    /// Путь к базе данных.
    /// </summary>
    public string Database { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MasterVisitor64
        (
            string masterFile
        )
    {
        Sure.NotNullNorEmpty (masterFile);

        const DirectAccessMode mode = DirectAccessMode.ReadOnly;
        var serviceProvider = Magna.Host.Services;

        Database = Path.GetFileNameWithoutExtension (masterFile)
            .ThrowIfNullOrEmpty();
        Mst = new MstFile64
            (
                Path.ChangeExtension (masterFile, ".mst"),
                mode,
                serviceProvider
            );
        Xrf = new XrfFile64
            (
                Path.ChangeExtension (masterFile, ".xrf"),
                mode,
                serviceProvider
            );
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получает реальный максимальный MFN в базе данных.
    /// НЕ СЛЕДУЮЩИЙ MFN, а именно максимальный!
    /// </summary>
    public int GetMaxMfn() => Mst.ControlRecord.NextMfn - 1;

    /// <summary>
    /// Чтение записи с указанным MFN в виде массива байтов.
    /// </summary>
    public byte[]? ReadRecordBytes
        (
            int mfn
        )
    {
        Sure.Positive (mfn);

        XrfRecord64 xrfRecord;
        try
        {
            xrfRecord = Xrf.ReadRecord (mfn);
        }
        catch
        {
            return null;
        }

        if (xrfRecord.Offset == 0)
        {
            return null;
        }

        return Mst.ReadRecordBytes (xrfRecord.Offset);
    }

    /// <summary>
    /// Посещает все записи в мастер-файле.
    /// </summary>
    public bool VisitRecords
        (
            Func<int, byte[]?, bool> function
        )
    {
        Sure.NotNull (function);

        var maxMfn = GetMaxMfn();

        for (var mfn = 1; mfn <= maxMfn; mfn++)
        {
            var bytes = ReadRecordBytes (mfn);
            if (!function (mfn, bytes))
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Mst.Dispose();
        Xrf.Dispose();
    }

    #endregion
}
