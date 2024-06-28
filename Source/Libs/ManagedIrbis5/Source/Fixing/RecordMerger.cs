// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RecordMerger.cs -- сливает две записи в одну
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using JetBrains.Annotations;

using ManagedIrbis.Fields;
using ManagedIrbis.Providers;

#endregion

namespace ManagedIrbis.Fixing;

/// <summary>
/// Сливает две записи в одну.
/// </summary>
[PublicAPI]
public sealed class RecordMerger
{
    #region Properties

    /// <summary>
    /// Используемое подключение к серверу.
    /// </summary>
    public SyncConnection Connection { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RecordMerger
        (
            SyncConnection connection
        )
    {
        Sure.NotNull (connection);

        Connection = connection;
    }

    #endregion

    #region Private members

    private static bool ContainsExemplar
        (
            Record record,
            Field field
        )
    {
        var number = field.FM ('b');
        foreach (var exemplar in record.EnumerateField (ExemplarInfo.ExemplarTag))
        {
            if (IsMergeableExemplar (exemplar)
                && exemplar.FM ('b').SameString (number))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsMergeableExemplar
        (
            Field field
        )
    {
        var status = field.FM ('a');
        if (!ExemplarInfo.VerifyStatus (status))
        {
            return false;
        }

        var code = char.ToUpperInvariant (status[0]);
        var number = field.FM ('b');
        switch (code)
        {
            case '0':
            case '1':
            case '5':
            case '9':
            case 'p':
                return !string.IsNullOrWhiteSpace (number);
        }

        return false;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Слияние записей.
    /// </summary>
    public bool MergeRecords
        (
            int fromMfn,
            int toMfn
        )
    {
        Sure.Positive (fromMfn);
        Sure.Positive (toMfn);

        var fromRecord = Connection.ReadRecord (fromMfn).ThrowIfNull();
        var toRecord = Connection.ReadRecord (toMfn).ThrowIfNull();

        var needSave = false;
        foreach (var fromField in fromRecord
                     .EnumerateField (ExemplarInfo.ExemplarTag))
        {
            if (IsMergeableExemplar (fromField)
                && !ContainsExemplar (toRecord, fromField))
            {
                toRecord.Add (fromField.Clone());
                needSave = true;
            }
        }

        if (needSave)
        {
            if (!Connection.WriteRecord (toRecord))
            {
                // throw new IrbisException ("Error saving record");
                return false;
            }
        }

        if (!Connection.DeleteRecord (fromMfn))
        {
            // throw new IrbisException ("Error deleting record");
            return false;
        }

        return true;
    }

    #endregion
}
