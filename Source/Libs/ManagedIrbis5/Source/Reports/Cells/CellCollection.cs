﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedParameter.Local

/* CellCollection.cs -- коллекция ячеек отчета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Коллекция ячеек отчета.
    /// </summary>
    public sealed class CellCollection
        : Collection<ReportCell>,
        IHandmadeSerializable,
        IReadOnly<CellCollection>,
        IVerifiable,
        IDisposable
    {
        #region Properties

        /// <summary>
        /// Band.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public ReportBand? Band { get; internal set; }

        /// <summary>
        /// Record.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public IrbisReport? Report { get; internal set; }

        #endregion

        #region Private members

        internal CellCollection SetReport
            (
                IrbisReport? report
            )
        {
            Report = report;

            foreach (var cell in this)
            {
                cell.Report = report;
            }

            return this;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add range of <see cref="Field"/>s.
        /// </summary>
        public void AddRange
            (
                IEnumerable<ReportCell> cells
            )
        {
            ThrowIfReadOnly();

            foreach (var cell in cells)
            {
                Add(cell);
            }
        }

        /// <summary>
        /// Создание клона коллекции.
        /// </summary>
        public CellCollection Clone()
        {
            var result = new CellCollection
            {
                Band = Band,
                Report = Report
            };

            foreach (var cell in this)
            {
                var clone = cell.Clone();
                clone.Report = Report;
                result.Add(clone);
            }

            return result;
        }

        /// <summary>
        /// Find first occurrence of the field with given predicate.
        /// </summary>
        public ReportCell? Find (Predicate<ReportCell> predicate) =>
            this.FirstOrDefault(cell => predicate(cell));

        /// <summary>
        /// Find all occurrences of the field
        /// with given predicate.
        /// </summary>
        public ReportCell[] FindAll (Predicate<ReportCell> predicate) =>
            this.Where(cell => predicate(cell)).ToArray();

        #endregion

        #region Collection<T> members

        /// <inheritdoc cref="Collection{T}.ClearItems" />
        protected override void ClearItems()
        {
            ThrowIfReadOnly();

            foreach (var cell in this)
            {
                cell.Band = null;
                cell.Report = null;
            }

            base.ClearItems();
        }

        /// <inheritdoc cref="Collection{T}.InsertItem" />
        protected override void InsertItem
            (
                int index,
                ReportCell item
            )
        {
            ThrowIfReadOnly();

            item.Band = Band;
            item.Report = Report;

            base.InsertItem(index, item);
        }

        /// <inheritdoc cref="Collection{T}.RemoveItem" />
        protected override void RemoveItem
            (
                int index
            )
        {
            ThrowIfReadOnly();

            if (index >= 0 && index < Count)
            {
                var cell  = this[index];
                if (!ReferenceEquals(cell, null))
                {
                    cell.Band = null;
                    cell.Report = null;
                }
            }

            base.RemoveItem(index);
        }

        /// <inheritdoc cref="Collection{T}.SetItem" />
        protected override void SetItem
            (
                int index,
                ReportCell item
            )
        {
            ThrowIfReadOnly();

            item.Band = Band;
            item.Report = Report;

            base.SetItem(index, item);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ThrowIfReadOnly();

            // TODO implement

            ClearItems();
            //RecordField[] array = reader.ReadArray<RecordField>();
            //AddRange(array);

            Magna.Error
                (
                    "CellCollection::RestoreFromStream: "
                    + "not implemented"
                );

            throw new NotImplementedException();
        }

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
                    "CellCollection::SaveToStream: "
                    + "not implemented"
                );

            throw new NotImplementedException();
        }

        #endregion

        #region IReadOnly<T> members

        internal bool _readOnly;

        /// <inheritdoc cref="IReadOnly{T}.ReadOnly" />
        public bool ReadOnly { get { return _readOnly; } }

        /// <inheritdoc cref="IReadOnly{T}.AsReadOnly" />
        public CellCollection AsReadOnly()
        {
            var result = Clone();
            result.SetReadOnly();

            return result;
        } // method AsReadOnly

        /// <inheritdoc cref="IReadOnly{T}.SetReadOnly" />
        public void SetReadOnly()
        {
            _readOnly = true;
        }

        /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly" />
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                Magna.Error
                    (
                        "CellCollection::ThrowIfReadOnly"
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
            var verifier
                = new Verifier<CellCollection>(this, throwOnError);

            foreach (var cell in this)
            {
                verifier.VerifySubObject(cell, "cell");
            }

            return verifier.Result;
        } // method Verify

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            foreach (var cell in this)
            {
                cell.Dispose();
            }
        } // method Dispose

        #endregion

    } // class CellCollection

} // namespace ManagedIrbis.Reports
