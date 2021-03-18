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

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
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
        /// Parent band.
        /// </summary>
        public ReportBand? Parent
        {
            get => _parent;
            internal set => SetParent(value);
        } // property Parent

        /// <summary>
        /// Record.
        /// </summary>
        public IrbisReport? Report
        {
            get => _report;
            internal set => SetReport(value);
        } // property Report

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BandCollection()
            : this (null, null)
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public BandCollection
            (
                IrbisReport? report,
                ReportBand? parent
            )
        {
            Report = report;
            Parent = parent;
        } // constructor

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
        } // method SetParent

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
        } // method SetReport

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
            ThrowIfReadOnly();

            foreach (var band in bands)
            {
                Add(band);
            }
        } // method AddRange

        /// <summary>
        /// Создание клона коллекции.
        /// </summary>
        public BandCollection<T> Clone()
        {
            var result = new BandCollection<T> (Report, Parent);

            foreach (var band in this)
            {
                var clone = (T)band.Clone();
                result.Add(clone);
            }

            return result;
        } // method Clone

        /// <summary>
        /// Find first occurrence of the field with given predicate.
        /// </summary>
        public ReportBand? Find(Predicate<ReportBand> predicate) =>
            this.FirstOrDefault(band => predicate(band));

        /// <summary>
        /// Find all occurrences of the field
        /// with given predicate.
        /// </summary>
        public T[] FindAll(Predicate<ReportBand> predicate) =>
            this.Where(band => predicate(band)).ToArray();

        /// <summary>
        /// Render bands.
        /// </summary>
        public void Render
            (
                ReportContext context
            )
        {
            foreach (var band in this)
            {
                band.Render(context);
            }
        } // method Render

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
        } // method ClearItems

        /// <inheritdoc cref="Collection{T}.InsertItem" />
        protected override void InsertItem
            (
                int index,
                T item
            )
        {
            ThrowIfReadOnly();
            Sure.NotNull(item, nameof(item));

            item.Report = Report;
            item.Parent = Parent;

            base.InsertItem(index, item);
        } // method InsertItem

        /// <inheritdoc cref="Collection{T}.RemoveItem" />
        protected override void RemoveItem
            (
                int index
            )
        {
            ThrowIfReadOnly();

            if (index >= 0 && index < Count)
            {
                var band  = this[index];
                if (band is not null)
                {
                    band.Report = null;
                    band.Parent = null;
                }
            }

            base.RemoveItem(index);
        } // method RemoveItem

        /// <inheritdoc cref="Collection{T}.SetItem" />
        protected override void SetItem
            (
                int index,
                T item
            )
        {
            ThrowIfReadOnly();
            Sure.NotNull(item, nameof(item));

            item.Report = Report;
            item.Parent = Parent;

            base.SetItem(index, item);
        } // method SetItem

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ThrowIfReadOnly();
            ClearItems();

            // TODO implement

            //RecordField[] array = reader.ReadArray<RecordField>();
            //AddRange(array);

            Magna.Error
                (
                    nameof(BandCollection<T>) + "::" + nameof(RestoreFromStream)
                    + ": not implemented"
                );

            throw new NotImplementedException();
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            // TODO implement

            //writer.WriteArray(this.ToArray());

            Magna.Error
                (
                    nameof(BandCollection<T>) + "::" + nameof(SaveToStream)
                    + ": not implemented"
                );

            throw new NotImplementedException();
        } // method SaveToStream

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
        } // method AsReadOnly

        /// <inheritdoc cref="IReadOnly{T}.SetReadOnly" />
        public void SetReadOnly() => _readOnly = true;

        /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly" />
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                Magna.Error
                    (
                        nameof(BandCollection<T>) + "::" + nameof(ThrowIfReadOnly)
                    );

                throw new ReadOnlyException();
            }
        } // method ThrowIfReadOnly

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier
                = new Verifier<BandCollection<T>>(this, throwOnError);

            foreach (var band in this)
            {
                verifier.VerifySubObject(band, "band");
            }

            return verifier.Result;
        } // method Verify

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            foreach (T item in Items)
            {
                item.Dispose();
            }
        } // method Dispose

        #endregion

    } // class BandCollection

} // namespace ManagedIrbis.Reports
