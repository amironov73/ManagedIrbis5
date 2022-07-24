// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* BandCollection.cs -- коллекция полос отчета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using AM;
using AM.Runtime;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Reports;

/// <summary>
/// Коллекция полос отчета.
/// </summary>
public sealed class BandCollection<T>
    : Collection<T>,
    IHandmadeSerializable,
    IReadOnly<BandCollection<T>>,
    IVerifiable,
    IDisposable
    where T: ReportBand
{
    #region Properties

    /// <summary>
    /// Родительская полоса.
    /// </summary>
    public ReportBand? Parent
    {
        get => _parent;
        internal set => SetParent (value);
    }

    /// <summary>
    /// Отчет, которому принадлежит полоса.
    /// </summary>
    public IrbisReport? Report
    {
        get => _report;
        internal set => SetReport (value);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public BandCollection()
        : this (null, null)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BandCollection
        (
            IrbisReport? report,
            ReportBand? parent
        )
    {
        Report = report;
        Parent = parent;
    }

    #endregion

    #region Private members

    private ReportBand? _parent;

    private IrbisReport? _report;

    internal void SetParent
        (
            ReportBand? parent
        )
    {
        _parent = parent;

        foreach (var band in this)
        {
            band.Parent = parent;
        }
    }

    internal void SetReport
        (
            IrbisReport? report
        )
    {
        _report = report;

        foreach (var band in this)
        {
            band.Report = report;
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Add range of <see cref="Field"/>s.
    /// </summary>
    public void AddRange
        (
            IEnumerable<T> bands
        )
    {
        Sure.NotNull ((object?) bands);

        ThrowIfReadOnly();

        foreach (var band in bands)
        {
            Add (band);
        }
    }

    /// <summary>
    /// Создание клона коллекции.
    /// </summary>
    public BandCollection<T> Clone()
    {
        var result = new BandCollection<T> (Report, Parent);

        foreach (var band in this)
        {
            var clone = (T)band.Clone();
            result.Add (clone);
        }

        return result;
    }

    /// <summary>
    /// Find first occurrence of the field with given predicate.
    /// </summary>
    public ReportBand? Find (Predicate<ReportBand> predicate)
    {
        return this.FirstOrDefault (band => predicate (band));
    }

    /// <summary>
    /// Find all occurrences of the field
    /// with given predicate.
    /// </summary>
    public T[] FindAll (Predicate<ReportBand> predicate)
    {
        return this.Where (band => predicate (band)).ToArray();
    }

    /// <summary>
    /// Render bands.
    /// </summary>
    public void Render
        (
            ReportContext context
        )
    {
        Sure.NotNull (context);

        foreach (var band in this)
        {
            band.Render (context);
        }
    }

    #endregion

    #region Collection<T> members

    /// <inheritdoc cref="Collection{T}.ClearItems" />
    protected override void ClearItems()
    {
        ThrowIfReadOnly();

        foreach (var band in this)
        {
            band.Report = null;
            band.Parent = null;
        }

        base.ClearItems();
    }

    /// <inheritdoc cref="Collection{T}.InsertItem" />
    protected override void InsertItem
        (
            int index,
            T item
        )
    {
        Sure.NonNegative (index);
        Sure.NotNull (item, nameof (item));

        ThrowIfReadOnly();

        if (item.Report is not null)
        {
            throw new IrbisException ("Band already belongs to report");
        }

        item.Report = Report;
        item.Parent = Parent;

        base.InsertItem (index, item);
    }

    /// <inheritdoc cref="Collection{T}.RemoveItem" />
    protected override void RemoveItem
        (
            int index
        )
    {
        Sure.NonNegative (index);

        ThrowIfReadOnly();

        if (index >= 0 && index < Count)
        {
            var band = this[index];
            band.Report = null;
            band.Parent = null;
        }

        base.RemoveItem (index);
    }

    /// <inheritdoc cref="Collection{T}.SetItem" />
    protected override void SetItem
        (
            int index,
            T item
        )
    {
        Sure.NonNegative (index);
        Sure.NotNull (item);

        ThrowIfReadOnly();

        if (item.Report is not null)
        {
            throw new IrbisException ("Band already belongs to report");
        }

        item.Report = Report;
        item.Parent = Parent;

        base.SetItem (index, item);
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        ThrowIfReadOnly();
        ClearItems();

        // TODO implement

        //RecordField[] array = reader.ReadArray<RecordField>();
        //AddRange(array);

        Magna.Logger.LogError
            (
                nameof (BandCollection<T>) + "::" + nameof (RestoreFromStream)
                + ": not implemented"
            );

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        // TODO implement

        //writer.WriteArray(this.ToArray());

        Magna.Logger.LogError
            (
                nameof (BandCollection<T>) + "::" + nameof (SaveToStream)
                + ": not implemented"
            );

        throw new NotImplementedException();
    }

    #endregion

    #region IReadOnly<T> members

    internal bool _readOnly;

    /// <inheritdoc cref="IReadOnly{T}.ReadOnly" />
    public bool ReadOnly => _readOnly;

    /// <inheritdoc cref="IReadOnly{T}.AsReadOnly" />
    public BandCollection<T> AsReadOnly()
    {
        var result = Clone();
        result.SetReadOnly();

        return result;
    }

    /// <inheritdoc cref="IReadOnly{T}.SetReadOnly" />
    public void SetReadOnly() => _readOnly = true;

    /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly" />
    public void ThrowIfReadOnly()
    {
        if (ReadOnly)
        {
            Magna.Logger.LogError
                (
                    nameof (BandCollection<T>) + "::" + nameof (ThrowIfReadOnly)
                );

            throw new ReadOnlyException();
        }
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<BandCollection<T>> (this, throwOnError);

        foreach (var band in this)
        {
            verifier.VerifySubObject (band);
        }

        return verifier.Result;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        foreach (var item in Items)
        {
            item.Dispose();
        }
    }

    #endregion
}
