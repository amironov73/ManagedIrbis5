// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RecordCollection.cs -- коллекция записей для главы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Коллекция библиографических записей для главы указателя.
/// </summary>
public sealed class RecordCollection
    : NonNullCollection<Record>,
    IVerifiable
{
    #region Public methods

    /// <summary>
    /// Sort the records.
    /// </summary>
    public void SortRecords()
    {
        var records = ToArray();

        Array.Sort (records, RecordComparer.BySortKey());
        Clear();
        AddRange (records);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<RecordCollection> (this, throwOnError);

        foreach (var record in this)
        {
            verifier.VerifySubObject (record);
        }

        return verifier.Result;
    }

    #endregion
}
